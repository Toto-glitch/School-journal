using SchoolJournal.Model;
using SchoolJournal.Service;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace SchoolJournal.ViewModel
{
    public class DiaryViewModel : BaseViewModel
    {
        private readonly AuthService _authService;
        private readonly AbsoluteService _absoluteService;
        private readonly User _currentUser;

        private ObservableCollection<Student> _children;
        private Student _selectedChild;
        private ObservableCollection<Subject> _subjects;
        private Subject _selectedSubject;
        private ObservableCollection<Mark> _marks;
        private double _averageMark;
        private string _statusMessage;

        public DiaryViewModel(Window win, User user) : base(win)
        {
            _authService = new AuthService();
            _absoluteService = new AbsoluteService();
            _currentUser = user;

            Children = new ObservableCollection<Student>();
            Subjects = new ObservableCollection<Subject>();
            Marks = new ObservableCollection<Mark>();

            LoadInitialData();
        }

        public bool IsParent => _currentUser.Role == UserRole.Parent;

        public ObservableCollection<Student> Children
        {
            get => _children;
            set { _children = value; OnPropertyChanged(nameof(Children)); }
        }

        public Student SelectedChild
        {
            get => _selectedChild;
            set
            {
                _selectedChild = value;
                OnPropertyChanged(nameof(SelectedChild));
                if (value != null)
                    LoadStudentData();
            }
        }

        public ObservableCollection<Subject> Subjects
        {
            get => _subjects;
            set { _subjects = value; OnPropertyChanged(nameof(Subjects)); }
        }

        public Subject SelectedSubject
        {
            get => _selectedSubject;
            set
            {
                _selectedSubject = value;
                OnPropertyChanged(nameof(SelectedSubject));
                if (value != null)
                    LoadMarksForSubject();
            }
        }

        public ObservableCollection<Mark> Marks
        {
            get => _marks;
            set { _marks = value; OnPropertyChanged(nameof(Marks)); }
        }

        public double AverageMark
        {
            get => _averageMark;
            set { _averageMark = value; OnPropertyChanged(nameof(AverageMark)); }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set { _statusMessage = value; OnPropertyChanged(nameof(StatusMessage)); }
        }

        private void LoadInitialData()
        {
            if (IsParent)
            {
                var parent = _authService.GetParentByUserId(_currentUser.Id);
                if (parent?.Students != null)
                {
                    foreach (var student in parent.Students)
                        Children.Add(student);

                    if (Children.Any())
                        SelectedChild = Children.First();
                }
            }
            else if (_currentUser.Role == UserRole.Student)
            {
                var student = _authService.GetStudentByUserId(_currentUser.Id);
                if (student != null)
                {
                    Children.Add(student);
                    SelectedChild = student;
                }
            }
        }

        private void LoadStudentData()
        {
            if (SelectedChild == null) return;

            Subjects.Clear();
            Marks.Clear();

            var allSubjects = _absoluteService.GetAllSubjects();
            foreach (var subj in allSubjects)
            {
                Subjects.Add(subj);
            }

            AverageMark = _absoluteService.GetOverallAverageMark(SelectedChild.Id);
            StatusMessage = $"Ученик: {SelectedChild.LastName} {SelectedChild.FirstName} | Группа: {SelectedChild.Group?.Title ?? "Не назначена"}";
        }

        private void LoadMarksForSubject()
        {
            if (SelectedChild == null || SelectedSubject == null) return;

            Marks.Clear();
            var marks = _absoluteService.GetStudentMarksBySubject(SelectedChild.Id, SelectedSubject.Id);
            foreach (var m in marks)
                Marks.Add(m);

            AverageMark = _absoluteService.GetAverageMarkBySubject(SelectedChild.Id, SelectedSubject.Id);
        }
    }
}