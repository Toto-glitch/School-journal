using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SchoolJournal.Model;

namespace SchoolJournal.View.Pages
{
    public partial class JournalPage : Page
    {
        public JournalPage()
        {
            InitializeComponent();
            Loaded += JournalPage_Loaded;
        }

        private void JournalPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadFilters();
        }

        private void LoadFilters()
        {
            using (var db = new ApplicationContext())
            {
                SubjectCombo.ItemsSource = db.Subjects.ToList();
                SubjectCombo.DisplayMemberPath = "Name";
                SubjectCombo.SelectedValuePath = "Id";

                GroupCombo.ItemsSource = db.Groups.ToList();
                GroupCombo.DisplayMemberPath = "Name"; 
                GroupCombo.SelectedValuePath = "Id";
            }
        }

        private void RefreshJournal_Click(object sender, RoutedEventArgs e)
        {
            int subjectId = SubjectCombo.SelectedItem is Model.Subject s ? s.Id : 0;
            int groupId = GroupCombo.SelectedItem is Model.Group g ? g.Id : 0;

            if (subjectId == 0 || groupId == 0) return;

            using (var db = new ApplicationContext())
            {
                var students = (from st in db.Students
                                where st.GroupId == groupId
                                select new
                                {
                                    StudentId = st.Id,
                                    StudentName = $"{st.LastName} {st.FirstName}",
                                    Average = db.Marks
                                        .Where(m => m.StudentId == st.Id && m.SubjectId == subjectId)
                                        .Average(m => (double?)m.Value) ?? 0,
                                    MarkCount = db.Marks
                                        .Count(m => m.StudentId == st.Id && m.SubjectId == subjectId)
                                }).ToList();

                JournalDataGrid.ItemsSource = students;
            }
        }

        private void SetMark_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Открытие окна выставления оценки для выбранного ученика");
        }

        private void JournalDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }
    }
}