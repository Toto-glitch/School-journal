using SchoolJournal.Model;
using SchoolJournal.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace SchoolJournal.View.Pages
{
    public partial class DiaryPage : Page
    {
        public DiaryPage(User currentUser)
        {
            InitializeComponent();
            DataContext = new DiaryViewModel(Application.Current.MainWindow, currentUser);
        }
    }
}