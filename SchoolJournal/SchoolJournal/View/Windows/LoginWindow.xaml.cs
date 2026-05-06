using System.Windows;
using SchoolJournal.ViewModel;

namespace SchoolJournal
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();

            DataContext = new LoginViewModel(this);
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as LoginViewModel;
            viewModel.Password = PasswordBox.Password;
        }
    }
}