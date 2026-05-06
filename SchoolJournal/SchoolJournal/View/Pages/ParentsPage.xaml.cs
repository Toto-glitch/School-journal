using System.Windows;
using System.Windows.Controls;
using SchoolJournal.Model;
using SchoolJournal.ViewModel;

namespace SchoolJournal.View.Pages
{
    public partial class ParentsPage : Page
    {
        public ParentsPage(User currentUser)
        {
            InitializeComponent();
            DataContext = new ParentsManagementViewModel(Application.Current.MainWindow, currentUser);
        }
    }
}