using SchoolJournal.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using SchoolJournal.Service;

namespace SchoolJournal.ViewModel
{
    public class DiaryViewModel : BaseViewModel
    {
        private AuthService _authService;
        private GradeService _gradeService;
        private User _currentUser;
        private Student _currentStudent;

        private ObservableCollection<Subject> _subjects;
        private ObservableCollection<Mark> _marks;
        private Subject _selectedSubject;
        private double _averageMark;
        private string _statusMessage;

        public DiaryViewModel(Window win, User user) : base(win)
        {
            _authService = new AuthService();
            _gradeService = new GradeService();
            _currentUser = user;

            _subjects = new ObservableCollection<Subject>();
            _marks = new ObservableCollection<Mark>();

            LoadStudentData();
        }

        public ObservableCollection<Subject> Subjects
        {
            get => _subjects;
            set { _subjects = value; OnPropertyChanged(nameof(Subjects)); }
        }

        public ObservableCollection<Mark> Marks
        {
            get => _marks;
            set { _marks = value; OnPropertyChanged(nameof(Marks)); }
        }

        public Subject SelectedSubject
        {
            get => _selectedSubject;
            set
            {
                _selectedSubject = value;
                OnPropertyChanged(nameof(SelectedSubject));
                if (value != null)
                    LoadMarksForSubject();
            }
        }

        public double AverageMark
        {
            get => _averageMark;
            set { _averageMark = value; OnPropertyChanged(nameof(AverageMark)); }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set { _statusMessage = value; OnPropertyChanged(nameof(StatusMessage)); }
        }

        private void LoadStudentData()
        {
            // Получаем данные студента в зависимости от роли пользователя
            if (_currentUser.Role == UserRole.Student)
            {
                _currentStudent = _authService.GetStudentByUserId(_currentUser.Id);
            }
            else if (_currentUser.Role == UserRole.Parent)
            {
                // Для родителя нужно выбрать ребенка (здесь берем первого)
                var parent = _authService.GetParentByUserId(_currentUser.Id);
                if (parent?.Students.Any() == true)
                {
                    _currentStudent = parent.Students.First();
                }
            }

            if (_currentStudent != null)
            {
                // Загружаем предметы (все предметы школы или предметы группы студента)
                var allSubjects = _gradeService.GetAllSubjects();
                foreach (var subject in allSubjects)
                {
                    Subjects.Add(subject);
                }

                // Загружаем все оценки студента
                var allMarks = _gradeService.GetStudentMarks(_currentStudent.Id);
                foreach (var mark in allMarks)
                {
                    Marks.Add(mark);
                }

                // Вычисляем средний балл
                AverageMark = _gradeService.GetOverallAverageMark(_currentStudent.Id);

                StatusMessage = $"Ученик: {_currentStudent.LastName} {_currentStudent.FirstName}, Группа: {_currentStudent.Group?.Title}";
            }
            else
            {
                StatusMessage = "Данные ученика не найдены";
            }
        }

        private void LoadMarksForSubject()
        {
            if (_currentStudent == null || _selectedSubject == null)
                return;

            Marks.Clear();
            var marks = _gradeService.GetStudentMarksBySubject(_currentStudent.Id, _selectedSubject.Id);
            foreach (var mark in marks)
            {
                Marks.Add(mark);
            }

            AverageMark = _gradeService.GetAverageMarkBySubject(_currentStudent.Id, _selectedSubject.Id);
        }

        private RelayCommand _refreshCommand;
        public RelayCommand RefreshCommand => _refreshCommand ?? (_refreshCommand = new RelayCommand(obj => Refresh()));

        private void Refresh()
        {
            Marks.Clear();
            Subjects.Clear();
            LoadStudentData();
        }
    }
}