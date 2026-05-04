using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SchoolJournal.View.Pages
{
    public partial class DashboardPage : Page
    {
        public DashboardPage()
        {
            InitializeComponent();
            Loaded += DashboardPage_Loaded;
        }

        private void DashboardPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadStatistics();
            LoadRecentMarks();
        }

        private void LoadStatistics()
        {
            using (var db = new Model.ApplicationContext())
            {
                StudentsCount.Text = db.Students.Count().ToString();
                TeachersCount.Text = db.Teachers.Count().ToString();
                MarksCount.Text = db.Marks.Count().ToString();
                ParentsCount.Text = db.Parents.Count().ToString();
            }
        }

        private void LoadRecentMarks()
        {
            using (var db = new Model.ApplicationContext())
            {

                var recentMarks = (from m in db.Marks
                                   join s in db.Students on m.StudentId equals s.Id
                                   join sub in db.Subjects on m.SubjectId equals sub.Id
                                   orderby m.DateTime descending
                                   select new
                                   {
                                       StudentName = $"{s.LastName} {s.FirstName}",
                                       Subject = sub.Name,
                                       Grade = m.Value,
                                       Date = m.DateTime 
                                   }).Take(10).ToList();

                RecentMarksGrid.ItemsSource = recentMarks;
            }
        }

        private void AddMark_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Функция добавления оценки", "Информация",
                          MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}