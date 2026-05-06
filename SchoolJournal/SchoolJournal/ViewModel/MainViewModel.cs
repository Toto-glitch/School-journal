using SchoolJournal.Model;
using SchoolJournal.Service;
using System.Collections.ObjectModel;
using System.Windows;

namespace SchoolJournal.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private AuthService _authService;
        private GradeService _gradeService;
        private User _currentUser;
        private string _currentUserName;
        private string _currentUserRole;
        private int _selectedMenuItemIndex;
        private object _currentPageContent;

        // Коллекции данных
        private ObservableCollection<MarkLog> _recentLogs;
        private ObservableCollection<Subject> _teacherSubjects;

        public MainViewModel(Window win, User user) : base(win)
        {
            _authService = new AuthService();
            _gradeService = new GradeService();
            _currentUser = user;

            CurrentUserName = $"{user.Username} ({GetRoleName(user.Role)})";
            CurrentUserRole = GetRoleName(user.Role);

            _recentLogs = new ObservableCollection<MarkLog>();
            _teacherSubjects = new ObservableCollection<Subject>();

            LoadUserData();
        }

        public User CurrentUser => _currentUser;

        public string CurrentUserName
        {
            get => _currentUserName;
            set { _currentUserName = value; OnPropertyChanged(nameof(CurrentUserName)); }
        }

        public string CurrentUserRole
        {
            get => _currentUserRole;
            set { _currentUserRole = value; OnPropertyChanged(nameof(CurrentUserRole)); }
        }

        public int SelectedMenuItemIndex
        {
            get => _selectedMenuItemIndex;
            set
            {
                _selectedMenuItemIndex = value;
                OnPropertyChanged(nameof(SelectedMenuItemIndex));
                OnMenuItemSelected(value);
            }
        }

        public object CurrentPageContent
        {
            get => _currentPageContent;
            set { _currentPageContent = value; OnPropertyChanged(nameof(CurrentPageContent)); }
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

        private RelayCommand _logoutCommand;
        public RelayCommand LogoutCommand => _logoutCommand ?? (_logoutCommand = new RelayCommand(obj => Logout()));

        private void LoadUserData()
        {
            // Загружаем логи в зависимости от роли пользователя
            if (_currentUser.Role == UserRole.Teacher)
            {
                var teacher = _authService.GetTeacherByUserId(_currentUser.Id);
                if (teacher != null)
                {
                    var logs = _gradeService.GetTeacherMarkLogs(teacher.Id, 20);
                    foreach (var log in logs)
                    {
                        RecentLogs.Add(log);
                    }

                    var subjects = _gradeService.GetTeacherSubjects(teacher.Id);
                    foreach (var subject in subjects)
                    {
                        TeacherSubjects.Add(subject);
                    }
                }
            }
            else if (_currentUser.Role == UserRole.Director)
            {
                var logs = _gradeService.GetAllMarkLogs(50);
                foreach (var log in logs)
                {
                    RecentLogs.Add(log);
                }
            }
        }

        private void OnMenuItemSelected(int index)
        {
            // Логика переключения страниц будет реализована в MainWindow
            // Здесь можно добавить загрузку данных для выбранной страницы
        }

        private string GetRoleName(UserRole role)
        {
            switch (role)
            {
                case UserRole.Director: return "Директор";
                case UserRole.Teacher: return "Преподаватель";
                case UserRole.Parent: return "Родитель";
                case UserRole.Student: return "Учащийся";
                default: return "Неизвестно";
            }
        }

        private void Logout()
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            _current_window.Close();
        }
    }
}