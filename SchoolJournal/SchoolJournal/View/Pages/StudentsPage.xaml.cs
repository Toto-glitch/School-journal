using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SchoolJournal.Model;

namespace SchoolJournal.View.Pages
{
    public partial class StudentsPage : Page
    {
        public StudentsPage()
        {
            InitializeComponent();
            Loaded += Page_Loaded;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using (var db = new ApplicationContext())
            {
                DataGridList.ItemsSource = db.Students.ToList();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Здесь откроется форма добавления нового ученика");
        }
    }
}