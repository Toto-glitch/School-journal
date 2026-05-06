using SchoolJournal.Model;
using SchoolJournal.Service;
using System.Collections.ObjectModel;
using System.Windows;

namespace SchoolJournal.ViewModel
{
    public class StudentsViewModel : BaseViewModel
    {
        private readonly GradeService _gradeService;
        private ObservableCollection<Student> _students;

        public StudentsViewModel(Window win, User user) : base(win)
        {
            _gradeService = new GradeService();
            Students = new ObservableCollection<Student>();
            LoadStudents();
        }

        public ObservableCollection<Student> Students
        {
            get => _students;
            set { _students = value; OnPropertyChanged(); }
        }

        private void LoadStudents()
        {
            var all = _gradeService.GetAllStudents();
            Students.Clear();
            foreach (var s in all)
                Students.Add(s);
        }
    }
}