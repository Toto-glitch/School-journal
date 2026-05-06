using SchoolJournal.Model;
using SchoolJournal.Service;
using SchoolJournal.View.Pages;
using System.Windows;
using System.Windows.Controls;

namespace SchoolJournal.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private readonly AuthService _authService;
        private readonly GradeService _gradeService;
        private readonly User _currentUser;

        private string _currentUserName;
        private string _currentUserRole;
        private Frame _mainFrame;

        // Видимость разделов
        private Visibility _dashboardVisibility = Visibility.Visible;
        private Visibility _diaryVisibility = Visibility.Collapsed;
        private Visibility _journalVisibility = Visibility.Collapsed;
        private Visibility _studentsVisibility = Visibility.Collapsed;
        private Visibility _teachersVisibility = Visibility.Collapsed;
        private Visibility _subjectsVisibility = Visibility.Collapsed;
        private Visibility _parentsVisibility = Visibility.Collapsed;

        public MainViewModel(Window win, User user) : base(win)
        {
            _authService = new AuthService();
            _gradeService = new GradeService();
            _currentUser = user;

            CurrentUserName = $"{user.Username} ({GetRoleName(user.Role)})";
            CurrentUserRole = GetRoleName(user.Role);

            SetPermissionsByRole();
        }

        public Frame MainFrame
        {
            get => _mainFrame;
            set => _mainFrame = value;
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

        public Visibility DashboardVisibility
        {
            get => _dashboardVisibility;
            set { _dashboardVisibility = value; OnPropertyChanged(nameof(DashboardVisibility)); }
        }
        public Visibility DiaryVisibility
        {
            get => _diaryVisibility;
            set { _diaryVisibility = value; OnPropertyChanged(nameof(DiaryVisibility)); }
        }
        public Visibility JournalVisibility
        {
            get => _journalVisibility;
            set { _journalVisibility = value; OnPropertyChanged(nameof(JournalVisibility)); }
        }
        public Visibility StudentsVisibility
        {
            get => _studentsVisibility;
            set { _studentsVisibility = value; OnPropertyChanged(nameof(StudentsVisibility)); }
        }
        public Visibility TeachersVisibility
        {
            get => _teachersVisibility;
            set { _teachersVisibility = value; OnPropertyChanged(nameof(TeachersVisibility)); }
        }
        public Visibility SubjectsVisibility
        {
            get => _subjectsVisibility;
            set { _subjectsVisibility = value; OnPropertyChanged(nameof(SubjectsVisibility)); }
        }
        public Visibility ParentsVisibility
        {
            get => _parentsVisibility;
            set { _parentsVisibility = value; OnPropertyChanged(nameof(ParentsVisibility)); }
        }

        private Visibility _groupsVisibility = Visibility.Collapsed;
        public Visibility GroupsVisibility
        {
            get => _groupsVisibility;
            set { _groupsVisibility = value; OnPropertyChanged(); }
        }

        private void SetPermissionsByRole()
        {
            switch (_currentUser.Role)
            {
                case UserRole.Student:
                case UserRole.Parent:
                    DiaryVisibility = Visibility.Visible;
                    break;
                case UserRole.Teacher:
                    JournalVisibility = Visibility.Visible;
                    StudentsVisibility = Visibility.Visible;
                    break;
                case UserRole.Director:
                    DashboardVisibility = Visibility.Collapsed;
                    DiaryVisibility = Visibility.Collapsed;
                    JournalVisibility = Visibility.Collapsed;

                    StudentsVisibility = Visibility.Visible;
                    TeachersVisibility = Visibility.Visible;
                    SubjectsVisibility = Visibility.Visible;
                    ParentsVisibility = Visibility.Visible;
                    GroupsVisibility = Visibility.Visible;
                    break;
            }
        }

        private RelayCommand _changePageCommand;
        public RelayCommand ChangePageCommand =>
            _changePageCommand ?? (_changePageCommand = new RelayCommand(param => ChangePage(param?.ToString())));

        private void ChangePage(string pageName)
        {
            if (MainFrame == null) return;

            // Очищаем историю навигации, чтобы не копились записи
            while (MainFrame.CanGoBack)
                MainFrame.RemoveBackEntry();

            Page page = null;
            switch (pageName)
            {
                case "DashboardPage":
                    page = new DashboardPage();
                    page.DataContext = new DashboardViewModel(_current_window, _currentUser);
                    break;
                case "DiaryPage":
                    page = new DiaryPage();
                    page.DataContext = new DiaryViewModel(_current_window, _currentUser);
                    break;
                case "JournalPage":
                    page = new JournalPage();
                    page.DataContext = new JournalViewModel(_current_window, _currentUser);
                    break;
                case "TeachersPage":
                    page = new TeachersPage(_currentUser);
                    break;
                case "SubjectsPage":
                    page = new SubjectsPage(_currentUser);
                    break;
                case "ParentsPage":
                    page = new ParentsPage(_currentUser);
                    break;
                case "StudentsPage":
                    page = new StudentsPage();
                    page.DataContext = new StudentsManagementViewModel(_current_window, _currentUser);
                    break;
                case "GroupsPage":
                    page = new GroupsPage(_currentUser);
                    break;
                default:
                    return;
            }
            MainFrame.Navigate(page);
        }

        private RelayCommand _logoutCommand;
        public RelayCommand LogoutCommand =>
            _logoutCommand ?? (_logoutCommand = new RelayCommand(obj => Logout()));

        private void Logout()
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            _current_window.Close();
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
    }
}