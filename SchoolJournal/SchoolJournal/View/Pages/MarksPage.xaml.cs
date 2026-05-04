using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SchoolJournal.Model;

namespace SchoolJournal.View.Pages
{
    public partial class MarksPage : Page
    {
        public MarksPage()
        {
            InitializeComponent();
            Loaded += MarksPage_Loaded;
        }

        private void MarksPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadMarks();
        }

        private void LoadMarks()
        {
            using (var db = new ApplicationContext())
            {
                var marks = (from m in db.Marks
                             join s in db.Students on m.StudentId equals s.Id
                             join sub in db.Subjects on m.SubjectId equals sub.Id
                             join t in db.Teachers on m.TeacherId equals t.Id
                             select new
                             {
                                 StudentName = $"{s.LastName} {s.FirstName}",
                                 SubjectName = sub.Name,
                                 Value = m.Value,
                                 DateTime = m.DateTime,
                                 TeacherName = $"{t.LastName} {t.FirstName}"
                             }).ToList();

                DataGridList.ItemsSource = marks;
            }
        }

        private void AddMark_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Форма выставления оценки в разработке", "Информация",
                          MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}