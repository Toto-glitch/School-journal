using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SchoolJournal.Model;

namespace SchoolJournal.View.Pages
{
    public partial class ReportsPage : Page
    {
        public ReportsPage()
        {
            InitializeComponent();
            Loaded += ReportsPage_Loaded;
        }

        private void ReportsPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTopStudents();
            LoadSubjectAverage();
        }

        private void LoadTopStudents_Click(object sender, RoutedEventArgs e) => LoadTopStudents();

        private void LoadTopStudents()
        {
            using (var db = new ApplicationContext())
            {
                var query = (from s in db.Students
                             let avg = db.Marks.Where(m => m.StudentId == s.Id).Average(m => (double?)m.Value)
                             orderby avg descending
                             select new
                             {
                                 Name = $"{s.LastName} {s.FirstName}",
                                 Average = avg ?? 0
                             }).Take(3).ToList();

                var result = query.Select((x, index) => new
                {
                    Rank = index + 1,
                    x.Name,
                    x.Average
                }).ToList();

                TopStudentsGrid.ItemsSource = result;
            }
        }

        private void LoadSubjectAverage_Click(object sender, RoutedEventArgs e) => LoadSubjectAverage();

        private void LoadSubjectAverage()
        {
            using (var db = new ApplicationContext())
            {
                var result = (from sub in db.Subjects
                              join m in db.Marks on sub.Id equals m.SubjectId into marksGroup
                              from m in marksGroup.DefaultIfEmpty()
                              group m by new { sub.Name } into g
                              select new
                              {
                                  SubjectName = g.Key.Name,
                                  Average = g.Average(x => (double?)x.Value) ?? 0
                              }).ToList();

                SubjectAverageGrid.ItemsSource = result;
            }
        }
    }
}