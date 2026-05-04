using System.Windows;
using System.Windows.Controls;
using SchoolJournal.Model;
using System.Linq;

namespace SchoolJournal.View.Pages
{
    public partial class SubjectsPage : Page
    {
        public SubjectsPage()
        {
            InitializeComponent();
            Loaded += SubjectsPage_Loaded;
        }

        private void SubjectsPage_Loaded(object sender, RoutedEventArgs e)
        {
            using (var db = new ApplicationContext())
            {
                DataGridList.ItemsSource = db.Subjects.ToList();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Форма добавления предмета в разработке", "Информация",
                          MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}