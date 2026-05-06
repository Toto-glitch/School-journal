using System.Windows;
using SchoolJournal.Model;
using SchoolJournal.ViewModel;

namespace SchoolJournal.View.Windows
{
    public partial class MainWindow : Window
    {
        public MainWindow(User user)
        {
            InitializeComponent();
            var viewModel = new MainViewModel(this, user);
            DataContext = viewModel;
            viewModel.MainFrame = MainFrame; // Передаём Frame в ViewModel
        }
    }
}