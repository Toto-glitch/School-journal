using System.Windows;
using System.Windows.Controls;
using SchoolJournal.Model;
using SchoolJournal.ViewModel;

namespace SchoolJournal.View.Pages
{
    public partial class GroupsPage : Page
    {
        public GroupsPage(User currentUser)
        {
            InitializeComponent();
            DataContext = new GroupsManagementViewModel(Application.Current.MainWindow, currentUser);
        }
    }
}