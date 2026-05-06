using SchoolJournal.Model;
using SchoolJournal.Service;
using System.Collections.ObjectModel;
using System.Windows;

namespace SchoolJournal.ViewModel
{
    public class JournalViewModel : BaseViewModel
    {
        private GradeService _gradeService;
        private AuthService _authService;
        private Teacher _currentTeacher;

        private ObservableCollection<Subject> _subjects;
        private ObservableCollection<Student> _students;
        private Subject _selectedSubject;
        private Student _selectedStudent;
        private int _selectedMarkValue;
        private Mark _selectedMark;
        private string _statusMessage;

        public JournalViewModel(Window win, User user) : base(win)
        {
            _gradeService = new GradeService();
            _authService = new AuthService();

            _subjects = new ObservableCollection<Subject>();
            _students = new ObservableCollection<Student>();
            _selectedMarkValue = 5;

            LoadTeacherData(user);
        }

        public ObservableCollection<Subject> Subjects
        {
            get => _subjects;
            set { _subjects = value; OnPropertyChanged(nameof(Subjects)); }
        }

        public ObservableCollection<Student> Students
        {
            get => _students;
            set { _students = value; OnPropertyChanged(nameof(Students)); }
        }

        public Subject SelectedSubject
        {
            get => _selectedSubject;
            set
            {
                _selectedSubject = value;
                OnPropertyChanged(nameof(SelectedSubject));
                if (value != null)
                    LoadStudentsForSubject();
            }
        }

        public Student SelectedStudent
        {
            get => _selectedStudent;
            set { _selectedStudent = value; OnPropertyChanged(nameof(SelectedStudent)); }
        }

        public int SelectedMarkValue
        {
            get => _selectedMarkValue;
            set { _selectedMarkValue = value; OnPropertyChanged(nameof(SelectedMarkValue)); }
        }

        public Mark SelectedMark
        {
            get => _selectedMark;
            set { _selectedMark = value; OnPropertyChanged(nameof(SelectedMark)); }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set { _statusMessage = value; OnPropertyChanged(nameof(StatusMessage)); }
        }

        private void LoadTeacherData(User user)
        {
            _currentTeacher = _authService.GetTeacherByUserId(user.Id);
            if (_currentTeacher != null)
            {
                var subjects = _gradeService.GetTeacherSubjects(_currentTeacher.Id);
                foreach (var subject in subjects)
                    Subjects.Add(subject);

                StatusMessage = $"Преподаватель: {_currentTeacher.LastName} {_currentTeacher.FirstName}";
            }
        }

        private void LoadStudentsForSubject()
        {
            Students.Clear();
            var students = _gradeService.GetStudentsBySubject(_selectedSubject.Id);
            foreach (var student in students)
                Students.Add(student);
        }

        private RelayCommand _addMarkCommand;
        public RelayCommand AddMarkCommand => _addMarkCommand ?? (_addMarkCommand = new RelayCommand(
            obj => AddMark(),
            obj => _selectedStudent != null && _selectedSubject != null));

        private void AddMark()
        {
            try
            {
                _gradeService.AddMark(_selectedStudent.Id, _selectedSubject.Id, _currentTeacher.Id, _selectedMarkValue);
                StatusMessage = "Оценка успешно добавлена!";
                OnPropertyChanged(nameof(Students)); // Обновить отображение
            }
            catch (System.Exception ex)
            {
                StatusMessage = $"Ошибка: {ex.Message}";
            }
        }

        private RelayCommand _updateMarkCommand;
        public RelayCommand UpdateMarkCommand => _updateMarkCommand ?? (_updateMarkCommand = new RelayCommand(
            obj => UpdateMark((int)obj),
            obj => _selectedMark != null));

        private void UpdateMark(int markId)
        {
            try
            {
                _gradeService.UpdateMark(markId, _selectedMarkValue, _currentTeacher.Id);
                StatusMessage = "Оценка обновлена!";
            }
            catch (System.Exception ex)
            {
                StatusMessage = $"Ошибка: {ex.Message}";
            }
        }

        private RelayCommand _deleteMarkCommand;
        public RelayCommand DeleteMarkCommand => _deleteMarkCommand ?? (_deleteMarkCommand = new RelayCommand(
            obj => DeleteMark((int)obj),
            obj => _selectedMark != null));

        private void DeleteMark(int markId)
        {
            try
            {
                var result = MessageBox.Show("Вы уверены, что хотите удалить оценку?",
                    "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _gradeService.DeleteMark(markId, _currentTeacher.Id);
                    StatusMessage = "Оценка удалена!";
                }
            }
            catch (System.Exception ex)
            {
                StatusMessage = $"Ошибка: {ex.Message}";
            }
        }
    }
}