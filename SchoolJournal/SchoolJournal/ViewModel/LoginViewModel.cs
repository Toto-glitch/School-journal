using System.Windows.Input;
using SchoolJournal.Model;
using SchoolJournal.Services;

namespace SchoolJournal.ViewModel
{
    /// <summary>
    /// ViewModel для окна входа
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        private readonly AuthService _authService;
        private string _username;
        private string _password;
        private string _errorMessage;
        private bool _isErrorVisible;

        public LoginViewModel()
        {
            _authService = new AuthService();
            LoginCommand = new RelayCommand(ExecuteLogin, CanExecuteLogin);
        }

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public bool IsErrorVisible
        {
            get => _isErrorVisible;
            set => SetProperty(ref _isErrorVisible, value);
        }

        public ICommand LoginCommand { get; }

        private bool CanExecuteLogin()
        {
            return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
        }

        private void ExecuteLogin()
        {
            // Здесь будет логика аутентификации через AuthService
            // Пока заглушка для демонстрации MVVM
            if (Username == "admin" && Password == "admin")
            {
                // Успешный вход
                var mainWindow = new MainWindow();
                mainWindow.Show();
                
                // Закрыть окно входа
                foreach (var window in App.Current.Windows)
                {
                    if (window is LoginWindow loginWindow)
                    {
                        loginWindow.Close();
                    }
                }
            }
            else
            {
                ErrorMessage = "Неверный логин или пароль";
                IsErrorVisible = true;
            }
        }
    }
}
