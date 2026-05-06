using SchoolJournal.Model;
using SchoolJournal.Service;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace SchoolJournal.ViewModel
{
    public class DashboardViewModel : BaseViewModel
    {
        private AuthService _authService;
        private GradeService _gradeService;
        private User _currentUser;

        private string _welcomeMessage;
        private ObservableCollection<MarkLog> _recentLogs;
        private ObservableCollection<Subject> _teacherSubjects;
        private double _studentAverageMark;
        private int _studentsCount;
        private int _teachersCount;

        public DashboardViewModel(Window win, User user) : base(win)
        {
            _authService = new AuthService();
            _gradeService = new GradeService();
            _currentUser = user;

            _recentLogs = new ObservableCollection<MarkLog>();
            _teacherSubjects = new ObservableCollection<Subject>();

            LoadDashboardData();
        }

        public string WelcomeMessage
        {
            get => _welcomeMessage;
            set { _welcomeMessage = value; OnPropertyChanged(nameof(WelcomeMessage)); }
        }

        public ObservableCollection<MarkLog> RecentLogs
        {
            get => _recentLogs;
            set { _recentLogs = value; OnPropertyChanged(nameof(RecentLogs)); }
        }

        public ObservableCollection<Subject> TeacherSubjects
        {
            get => _teacherSubjects;
            set { _teacherSubjects = value; OnPropertyChanged(nameof(TeacherSubjects)); }
        }

        public double StudentAverageMark
        {
            get => _studentAverageMark;
            set { _studentAverageMark = value; OnPropertyChanged(nameof(StudentAverageMark)); }
        }

        public int StudentsCount
        {
            get => _studentsCount;
            set { _studentsCount = value; OnPropertyChanged(nameof(StudentsCount)); }
        }

        public int TeachersCount
        {
            get => _teachersCount;
            set { _teachersCount = value; OnPropertyChanged(nameof(TeachersCount)); }
        }

        private void LoadDashboardData()
        {
            WelcomeMessage = $"Добро пожаловать, {_currentUser.Username}!";

            switch (_currentUser.Role)
            {
                case UserRole.Director:
                    LoadDirectorDashboard();
                    break;
                case UserRole.Teacher:
                    LoadTeacherDashboard();
                    break;
                case UserRole.Student:
                    LoadStudentDashboard();
                    break;
                case UserRole.Parent:
                    LoadParentDashboard();
                    break;
            }
        }

        private void LoadDirectorDashboard()
        {
            // Статистика школы
            StudentsCount = _gradeService.GetAllStudents().Count;
            TeachersCount = _gradeService.GetAllTeachers().Count;

            // Последние логи всех действий
            var logs = _gradeService.GetAllMarkLogs(20);
            foreach (var log in logs)
                RecentLogs.Add(log);
        }

        private void LoadTeacherDashboard()
        {
            var teacher = _authService.GetTeacherByUserId(_currentUser.Id);
            if (teacher != null)
            {
                var subjects = _gradeService.GetTeacherSubjects(teacher.Id);
                foreach (var subject in subjects)
                    TeacherSubjects.Add(subject);

                // Последние действия учителя
                var logs = _gradeService.GetTeacherMarkLogs(teacher.Id, 15);
                foreach (var log in logs)
                    RecentLogs.Add(log);
            }
        }

        private void LoadStudentDashboard()
        {
            var student = _authService.GetStudentByUserId(_currentUser.Id);
            if (student != null)
            {
                StudentAverageMark = _gradeService.GetOverallAverageMark(student.Id);

                // Последние оценки
                var marks = _gradeService.GetStudentMarks(student.Id);
                foreach (var mark in marks.Take(10))
                {
                    RecentLogs.Add(new MarkLog
                    {
                        Action = $"Оценка по предмету {mark.Subject.Title}",
                        NewValue = mark.Value,
                        ChangeDate = mark.Date
                    });
                }
            }
        }

        private void LoadParentDashboard()
        {
            var parent = _authService.GetParentByUserId(_currentUser.Id);
            if (parent?.Students.Any() == true)
            {
                var student = parent.Students.First();
                StudentAverageMark = _gradeService.GetOverallAverageMark(student.Id);
                WelcomeMessage = $"Дети: {string.Join(", ", parent.Students.Select(s => s.LastName + " " + s.FirstName))}";
            }
        }
    }
}