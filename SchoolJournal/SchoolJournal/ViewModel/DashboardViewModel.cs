using SchoolJournal.Model;
using SchoolJournal.Service;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace SchoolJournal.ViewModel
{
    public class DashboardViewModel : BaseViewModel
    {
        private readonly AuthService _authService;
        private readonly GradeService _gradeService;
        private readonly User _currentUser;

        private string _welcomeMessage;
        private ObservableCollection<Student> _children;
        private Student _selectedChild;
        private double _studentAverageMark;
        private ObservableCollection<Mark> _recentMarks;

        public DashboardViewModel(Window win, User user) : base(win)
        {
            _authService = new AuthService();
            _gradeService = new GradeService();
            _currentUser = user;

            Children = new ObservableCollection<Student>();
            RecentMarks = new ObservableCollection<Mark>();

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

        public bool IsParent => _currentUser.Role == UserRole.Parent;
        public bool IsStudent => _currentUser.Role == UserRole.Student;

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
        }

        private void LoadStudentDashboard(int studentId)
        {
            RecentMarks.Clear();
            var marks = _gradeService.GetStudentMarks(studentId).Take(10);
            foreach (var mark in marks)
                RecentMarks.Add(mark);

            StudentAverageMark = _gradeService.GetOverallAverageMark(studentId);
        }
    }
}