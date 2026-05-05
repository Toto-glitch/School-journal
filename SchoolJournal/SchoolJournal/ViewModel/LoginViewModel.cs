using SchoolJournal.Helper;
using SchoolJournal.Services;
using System.Windows;

namespace SchoolJournal.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        private AuthService _authService;

        private string _username;
        private string _password;
        private string _errorMessage;

        public LoginViewModel(Window win) : base(win)
        {
            _authService = new AuthService();
        }

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(nameof(Username)); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(nameof(Password)); }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(nameof(ErrorMessage)); }
        }

        private RelayCommand _loginCommand;
        public RelayCommand LoginCommand => _loginCommand ?? (_loginCommand = new RelayCommand(obj => Login()));

        private void Login()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Заполните пустые поля!";
                return;
            }

            var user = _authService.Authenticate(Username, Password);

            if (user != null)
            {
                var mainWin = new MainWindow(user);
                mainWin.Show();
                _current_window.Close();
            }
            else
            {
                ErrorMessage = "Неверный логин или пароль!";
            }
        }
    }
}
