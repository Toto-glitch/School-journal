using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SchoolJournal.View.Pages
{
    public partial class DiaryPage : Page
    {
        public DiaryPage()
        {
            InitializeComponent();
            Loaded += DiaryPage_Loaded;
        }

        private void DiaryPage_Loaded(object sender, RoutedEventArgs e)
        {
            using (var db = new Model.ApplicationContext())
            {
                StudentCombo.ItemsSource = db.Students.ToList();
                StudentCombo.DisplayMemberPath = "LastName";
                StudentCombo.SelectedValuePath = "Id";
            }
        }

        private void StudentCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StudentCombo.SelectedItem is Model.Student student)
            {
                using (var db = new Model.ApplicationContext())
                {
                    var marks = db.Marks
                        .Where(m => m.StudentId == student.Id)
                        .OrderByDescending(m => m.DateTime) 
                        .ToList();

                    DiaryDataGrid.ItemsSource = marks;
                }
            }
        }
    }
}