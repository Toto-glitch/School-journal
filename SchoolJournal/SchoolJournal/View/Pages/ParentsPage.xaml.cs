using System.Windows;
using System.Windows.Controls;
using SchoolJournal.Model;
using System.Linq;

namespace SchoolJournal.View.Pages
{
    public partial class ParentsPage : Page
    {
        public ParentsPage()
        {
            InitializeComponent();
            Loaded += ParentsPage_Loaded;
        }

        private void ParentsPage_Loaded(object sender, RoutedEventArgs e)
        {
            using (var db = new ApplicationContext())
            {
                DataGridList.ItemsSource = db.Parents.ToList();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Форма добавления родителя в разработке", "Информация",
                          MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}