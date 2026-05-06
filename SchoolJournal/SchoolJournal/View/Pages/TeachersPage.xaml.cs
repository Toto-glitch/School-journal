using System.Windows.Controls;
using System.Windows;
using SchoolJournal.Model;
using SchoolJournal.ViewModel;

namespace SchoolJournal.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для TeachersPage.xaml
    /// </summary>
    public partial class TeachersPage : Page
    {
        public TeachersPage(User currentUser)
        {
            InitializeComponent();
            DataContext = new TeachersManagementViewModel(Application.Current.MainWindow, currentUser);
        }
    }
}