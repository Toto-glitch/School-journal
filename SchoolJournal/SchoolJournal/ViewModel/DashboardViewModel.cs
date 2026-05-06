using SchoolJournal.Model;
using SchoolJournal.Service;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace SchoolJournal.ViewModel
{
    public class DashboardViewModel : BaseViewModel
    {
        private readonly AuthService _authService;
        private readonly AbsoluteService _absoluteService;
        private readonly User _currentUser;

        private string _welcomeMessage;
        private ObservableCollection<Student> _children;
        private Student _selectedChild;
        private double _studentAverageMark;
        private ObservableCollection<Mark> _recentMarks;
        private ObservableCollection<Subject> _teacherSubjects;
        private ObservableCollection<SubjectAverage> _subjectAverages;   

        public DashboardViewModel(Window win, User user) : base(win)
        {
            _authService = new AuthService();
            _absoluteService = new AbsoluteService();
            _currentUser = user;

            Children = new ObservableCollection<Student>();
            RecentMarks = new ObservableCollection<Mark>();
            TeacherSubjects = new ObservableCollection<Subject>();
            SubjectAverages = new ObservableCollection<SubjectAverage>();

            LoadDashboardData();
        }

        public string WelcomeMessage
        {
            get => _welcomeMessage;
            set { _welcomeMessage = value; OnPropertyChanged(nameof(WelcomeMessage)); }
        }

        public ObservableCollection<Student> Children
        {
            get => _children;
            set { _children = value; OnPropertyChanged(nameof(Children)); }
        }

        public Student SelectedChild
        {
            get => _selectedChild;
            set
            {
                _selectedChild = value;
                OnPropertyChanged(nameof(SelectedChild));
                if (value != null)
                    LoadStudentDashboard(value.Id);
            }
        }

        public double StudentAverageMark
        {
            get => _studentAverageMark;
            set { _studentAverageMark = value; OnPropertyChanged(nameof(StudentAverageMark)); }
        }

        public ObservableCollection<Mark> RecentMarks
        {
            get => _recentMarks;
            set { _recentMarks = value; OnPropertyChanged(nameof(RecentMarks)); }
        }

        public ObservableCollection<Subject> TeacherSubjects
        {
            get => _teacherSubjects;
            set { _teacherSubjects = value; OnPropertyChanged(nameof(TeacherSubjects)); }
        }

        public ObservableCollection<SubjectAverage> SubjectAverages
        {
            get => _subjectAverages;
            set { _subjectAverages = value; OnPropertyChanged(nameof(SubjectAverages)); }
        }

        public bool IsParent => _currentUser.Role == UserRole.Parent;
        public bool IsStudent => _currentUser.Role == UserRole.Student;
        public bool IsTeacher => _currentUser.Role == UserRole.Teacher;
        public bool IsStudentOrParent => IsStudent || IsParent;

        private void LoadDashboardData()
        {
            WelcomeMessage = $"Добро пожаловать, {_currentUser.Username}!";

            if (IsParent)
            {
                var parent = _authService.GetParentByUserId(_currentUser.Id);
                if (parent?.Students != null)
                {
                    foreach (var student in parent.Students)
                        Children.Add(student);

                    if (Children.Any())
                        SelectedChild = Children.First();
                }
            }
            else if (IsStudent)
            {
                var student = _authService.GetStudentByUserId(_currentUser.Id);
                if (student != null)
                    LoadStudentDashboard(student.Id);
            }
            else if (IsTeacher)
            {
                var teacher = _authService.GetTeacherByUserId(_currentUser.Id);
                if (teacher?.Subjects != null)
                {
                    TeacherSubjects.Clear();
                    foreach (var subj in teacher.Subjects.OrderBy(s => s.Title))
                        TeacherSubjects.Add(subj);
                }
            }
        }

        private void LoadStudentDashboard(int studentId)
        {
            var marks = _absoluteService.GetStudentMarks(studentId);
            RecentMarks.Clear();
            foreach (var m in marks.Take(10))
                RecentMarks.Add(m);

            StudentAverageMark = _absoluteService.GetOverallAverageMark(studentId);

            var subjectAvgs = marks
                .GroupBy(m => m.Subject)
                .Select(g => new SubjectAverage
                {
                    Title = g.Key.Title,
                    Average = Math.Round(g.Average(m => m.Value), 2)
                })
                .OrderBy(s => s.Title)
                .ToList();

            SubjectAverages.Clear();
            foreach (var sa in subjectAvgs)
                SubjectAverages.Add(sa);
        }
    }

    // Вспомогательный класс для отображения
    public class SubjectAverage
    {
        public string Title { get; set; }
        public double Average { get; set; }
    }
}