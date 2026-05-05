using SchoolJournal.Model;
using SchoolJournal.Service;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Data.Entity;

namespace SchoolJournal.ViewModel
{
    public class MarkDto { public DateTime Date { get; set; } public string Subject { get; set; } public int Value { get; set; } }
    public class SubjectAvgDto { public string Subject { get; set; } public double Avg { get; set; } }

    public class DiaryViewModel : BaseViewModel
    {
        private readonly GradeService _gradeService;
        private readonly User _currentUser;

        public ObservableCollection<MarkDto> Marks { get; set; } = new ObservableCollection<MarkDto>();
        public ObservableCollection<SubjectAvgDto> Averages { get; set; } = new ObservableCollection<SubjectAvgDto>();
        public string StudentFullName { get; set; }

        public RelayCommand LoadCommand { get; }

        public DiaryViewModel(Window win, User currentUser) : base(win)
        {
            _gradeService = new GradeService();
            _currentUser = currentUser;
            LoadCommand = new RelayCommand(_ => LoadDiary(), _ => true);
            LoadDiary();
        }

        private void LoadDiary()
        {
            int? studentId = GetTargetStudentId();
            if (!studentId.HasValue) return;

            using (var db = new ApplicationContext())
            {
                var student = db.Students.FirstOrDefault(s => s.Id == studentId.Value);
                StudentFullName = $"{student.LastName} {student.FirstName} {student.FatherName}".Trim();
                OnPropertyChanged(nameof(StudentFullName));
            }

            var marks = _gradeService.GetStudentMarks(studentId.Value);
            Marks.Clear();
            foreach (var m in marks)
                Marks.Add(new MarkDto { Date = m.Date, Subject = m.Subject.Title, Value = m.Value });

            var avgs = _gradeService.GetAverageMarks(studentId.Value);
            Averages.Clear();
            foreach (var a in avgs)
                Averages.Add(new SubjectAvgDto { Subject = a.Key, Avg = a.Value });

            OnPropertyChanged(nameof(Marks));
            OnPropertyChanged(nameof(Averages));
        }

        private int? GetTargetStudentId()
        {
            using (var db = new ApplicationContext())
            {
                if (_currentUser.Role == UserRole.Student)
                {
                    return db.Students.FirstOrDefault(s => s.UserId == _currentUser.Id)?.Id;
                }
                else if (_currentUser.Role == UserRole.Parent)
                {
                    // Для родителя берем первого привязанного ребенка (позже можно добавить ComboBox выбора)
                    var parent = db.Parents.Include(p => p.Students).FirstOrDefault(p => p.UserId == _currentUser.Id);
                    return parent?.Students.FirstOrDefault()?.Id;
                }
            }
            return null;
        }
    }
}