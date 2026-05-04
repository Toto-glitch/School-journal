using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SchoolJournal
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                ShowError("Пожалуйста, введите логин и пароль");
                return;
            }

            if (ValidateUser(login, password))
            {
                var mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                ShowError("Неверный логин или пароль");
            }
        }

        private bool ValidateUser(string login, string password)
        {
            using (var db = new Model.ApplicationContext())
            {
                var user = db.Users.FirstOrDefault(u => u.UserName == login && u.PasswordHash == password);
                return user != null;
            }
        }

        private void ShowError(string message)
        {
            ErrorMessage.Text = message;
            ErrorMessage.Visibility = Visibility.Visible;
        }
    }
}