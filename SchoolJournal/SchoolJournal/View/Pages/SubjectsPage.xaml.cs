using System.Windows;
using System.Windows.Controls;
using SchoolJournal.Model;
using SchoolJournal.ViewModel;

namespace SchoolJournal.View.Pages
{
    public partial class SubjectsPage : Page
    {
        public SubjectsPage(User currentUser)
        {
            InitializeComponent();
            DataContext = new SubjectsManagementViewModel(Application.Current.MainWindow, currentUser);
        }
    }
}