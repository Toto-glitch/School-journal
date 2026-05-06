using System.Windows;
using System.Windows.Controls;
using SchoolJournal.Model;
using SchoolJournal.ViewModel;
using System.Linq;

namespace SchoolJournal.View.Pages
{
    public partial class ParentsPage : Page
    {
        private ParentsManagementViewModel ViewModel => DataContext as ParentsManagementViewModel;

        public ParentsPage(User currentUser)
        {
            InitializeComponent();
            DataContext = new ParentsManagementViewModel(Application.Current.MainWindow, currentUser);
        }

        private void StudentsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel == null) return;

            // Удаляем те, которые были сняты
            foreach (Student removed in e.RemovedItems)
                ViewModel.SelectedStudents.Remove(removed);

            // Добавляем выбранные
            foreach (Student added in e.AddedItems)
                if (!ViewModel.SelectedStudents.Contains(added))
                    ViewModel.SelectedStudents.Add(added);
        }
    }
}