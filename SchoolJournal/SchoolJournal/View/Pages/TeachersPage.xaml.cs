using System.Windows;
using System.Windows.Controls;
using SchoolJournal.Model;
using System.Linq;

namespace SchoolJournal.View.Pages
{
    public partial class TeachersPage : Page
    {
        public TeachersPage()
        {
            InitializeComponent();
            Loaded += TeachersPage_Loaded;
        }

        private void TeachersPage_Loaded(object sender, RoutedEventArgs e)
        {
            using (var db = new ApplicationContext())
            {
                DataGridList.ItemsSource = db.Teachers.ToList();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Форма добавления учителя в разработке", "Информация",
                          MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}