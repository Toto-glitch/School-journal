using SchoolJournal.Helper;
using System.Windows;

namespace SchoolJournal.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        private string _username;
        private string _password;
        private string _errorMessage;

        public LoginViewModel(Window win) : base(win) { }

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
            set { _errorMessage = value; OnPropertyChanged(nameof(_errorMessage)); }
        }

        private RelayCommand _loginCommand;
        public RelayCommand LoginCommand => _loginCommand ?? (new RelayCommand(_ => Login()));

        private void Login()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Заполните пустые поля!";
                return;
            }

            var mainWindow = new MainWindow();
            mainWindow.Show();
            _current_window.Close();
        }
    }
}
