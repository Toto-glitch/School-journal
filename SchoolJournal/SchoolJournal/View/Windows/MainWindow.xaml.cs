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
            DataContext = new MainViewModel(this, user);
        }
    }
}