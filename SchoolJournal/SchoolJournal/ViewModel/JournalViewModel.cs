using SchoolJournal.Model;
using SchoolJournal.Service;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace SchoolJournal.ViewModel
{
    public class JournalViewModel : BaseViewModel
    {
        private readonly AbsoluteService _absoluteService;
        private readonly AuthService _authService;
        private Teacher _currentTeacher;

        private ObservableCollection<Subject> _subjects;
        private ObservableCollection<Student> _students;
        private ObservableCollection<Mark> _marks;
        private ObservableCollection<MarkLog> _logs;
        private Subject _selectedSubject;
        private Student _selectedStudent;
        private Mark _selectedMark;
        private int _selectedMarkValue;
        private string _statusMessage;

        public JournalViewModel(Window win, User user) : base(win)
        {
            _absoluteService = new AbsoluteService();
            _authService = new AuthService();

            _subjects = new ObservableCollection<Subject>();
            _students = new ObservableCollection<Student>();
            _marks = new ObservableCollection<Mark>();
            _logs = new ObservableCollection<MarkLog>();
            _selectedMarkValue = 5;

            LoadTeacherData(user);
        }

        public ObservableCollection<Subject> Subjects
        {
            get => _subjects;
            set { _subjects = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Student> Students
        {
            get => _students;
            set { _students = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Mark> Marks
        {
            get => _marks;
            set { _marks = value; OnPropertyChanged(); }
        }

        public ObservableCollection<MarkLog> Logs
        {
            get => _logs;
            set { _logs = value; OnPropertyChanged(); }
        }

        public Subject SelectedSubject
        {
            get => _selectedSubject;
            set
            {
                _selectedSubject = value;
                OnPropertyChanged();
                if (value != null)
                {
                    LoadStudentsForSubject();
                    LoadLogs();
                }
            }
        }

        public Student SelectedStudent
        {
            get => _selectedStudent;
            set
            {
                _selectedStudent = value;
                OnPropertyChanged();
                if (value != null && SelectedSubject != null)
                    LoadMarksForStudent();
            }
        }

        public Mark SelectedMark
        {
            get => _selectedMark;
            set { _selectedMark = value; OnPropertyChanged(); }
        }

        public int SelectedMarkValue
        {
            get => _selectedMarkValue;
            set { _selectedMarkValue = value; OnPropertyChanged(); }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set { _statusMessage = value; OnPropertyChanged(); }
        }

        private void LoadTeacherData(User user)
        {
            _currentTeacher = _authService.GetTeacherByUserId(user.Id);
            if (_currentTeacher != null)
            {
                var subjects = _absoluteService.GetTeacherSubjects(_currentTeacher.Id);
                Subjects.Clear();
                foreach (var s in subjects)
                    Subjects.Add(s);

                StatusMessage = $"Преподаватель: {_currentTeacher.LastName} {_currentTeacher.FirstName}";
            }
            else
            {
                StatusMessage = "Ошибка загрузки данных преподавателя.";
            }
        }

        private void LoadStudentsForSubject()
        {
            Students.Clear();
            Marks.Clear();
            if (_selectedSubject == null) return;

            var students = _absoluteService.GetStudentsBySubject(_selectedSubject.Id);
            foreach (var s in students.OrderBy(s => s.LastName))
                Students.Add(s);
        }

        private void LoadMarksForStudent()
        {
            Marks.Clear();
            if (_selectedStudent == null || _selectedSubject == null) return;

            var marks = _absoluteService.GetStudentMarksBySubject(_selectedStudent.Id, _selectedSubject.Id);
            foreach (var m in marks)
                Marks.Add(m);
        }

        private void LoadLogs()
        {
            Logs.Clear();
            if (_currentTeacher == null) return;

            var logs = _absoluteService.GetTeacherMarkLogs(_currentTeacher.Id, 30);
            foreach (var l in logs)
                Logs.Add(l);
        }

        private RelayCommand _addMarkCommand;
        public RelayCommand AddMarkCommand => _addMarkCommand ?? (_addMarkCommand = new RelayCommand(
            obj => AddMark(),
            obj => SelectedStudent != null && SelectedSubject != null));

        private void AddMark()
        {
            try
            {
                _absoluteService.AddMark(SelectedStudent.Id, SelectedSubject.Id, _currentTeacher.Id, SelectedMarkValue);
                StatusMessage = "Оценка добавлена.";
                RefreshAfterChange();
            }
            catch (System.Exception ex)
            {
                StatusMessage = $"Ошибка: {ex.Message}";
            }
        }

        private RelayCommand _updateMarkCommand;
        public RelayCommand UpdateMarkCommand => _updateMarkCommand ?? (_updateMarkCommand = new RelayCommand(
            obj => UpdateMark((int)obj),
            obj => SelectedMark != null));

        private void UpdateMark(int markId)
        {
            try
            {
                _absoluteService.UpdateMark(markId, SelectedMarkValue, _currentTeacher.Id);
                StatusMessage = "Оценка изменена.";
                RefreshAfterChange();
            }
            catch (System.Exception ex)
            {
                StatusMessage = $"Ошибка: {ex.Message}";
            }
        }

        private RelayCommand _deleteMarkCommand;
        public RelayCommand DeleteMarkCommand => _deleteMarkCommand ?? (_deleteMarkCommand = new RelayCommand(
            obj => DeleteMark((int)obj),
            obj => SelectedMark != null));

        private void DeleteMark(int markId)
        {
            var result = MessageBox.Show("Вы уверены, что хотите удалить оценку?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _absoluteService.DeleteMark(markId, _currentTeacher.Id);
                    StatusMessage = "Оценка удалена.";
                    RefreshAfterChange();
                }
                catch (System.Exception ex)
                {
                    StatusMessage = $"Ошибка: {ex.Message}";
                }
            }
        }

        private void RefreshAfterChange()
        {
            LoadMarksForStudent();
            LoadLogs();
        }
    }
}