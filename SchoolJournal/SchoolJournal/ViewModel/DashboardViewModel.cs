using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using SchoolJournal.Model;

namespace SchoolJournal.ViewModel
{
    /// <summary>
    /// ViewModel для главной панели управления (Dashboard)
    /// </summary>
    public class DashboardViewModel : ViewModelBase
    {
        private int _studentsCount;
        private int _teachersCount;
        private int _marksCount;
        private int _parentsCount;
        private ObservableCollection<MarkInfo> _recentMarks;

        public DashboardViewModel()
        {
            // Инициализация данных (в реальном приложении - загрузка из БД)
            LoadData();
            
            AddGradeCommand = new RelayCommand(ExecuteAddGrade);
            NewRecordCommand = new RelayCommand(ExecuteNewRecord);
            ReportCommand = new RelayCommand(ExecuteReport);
        }

        public int StudentsCount
        {
            get => _studentsCount;
            set => SetProperty(ref _studentsCount, value);
        }

        public int TeachersCount
        {
            get => _teachersCount;
            set => SetProperty(ref _teachersCount, value);
        }

        public int MarksCount
        {
            get => _marksCount;
            set => SetProperty(ref _marksCount, value);
        }

        public int ParentsCount
        {
            get => _parentsCount;
            set => SetProperty(ref _parentsCount, value);
        }

        public ObservableCollection<MarkInfo> RecentMarks
        {
            get => _recentMarks;
            set => SetProperty(ref _recentMarks, value);
        }

        public ICommand AddGradeCommand { get; }
        public ICommand NewRecordCommand { get; }
        public ICommand ReportCommand { get; }

        private void LoadData()
        {
            // Заглушка для демонстрации - в реальности данные загружаются из БД
            StudentsCount = 10;
            TeachersCount = 5;
            MarksCount = 50;
            ParentsCount = 10;

            RecentMarks = new ObservableCollection<MarkInfo>
            {
                new MarkInfo { StudentName = "Иванов Иван", Subject = "Математика", Grade = "5", Date = DateTime.Now },
                new MarkInfo { StudentName = "Петров Петр", Subject = "Физика", Grade = "4", Date = DateTime.Now.AddDays(-1) },
                new MarkInfo { StudentName = "Сидоров Сидор", Subject = "Химия", Grade = "3", Date = DateTime.Now.AddDays(-2) }
            };
        }

        private void ExecuteAddGrade()
        {
            // Логика добавления оценки
        }

        private void ExecuteNewRecord()
        {
            // Логика создания новой записи
        }

        private void ExecuteReport()
        {
            // Логика формирования отчета
        }
    }

    /// <summary>
    /// Модель для отображения информации об оценке в DataGrid
    /// </summary>
    public class MarkInfo
    {
        public string StudentName { get; set; }
        public string Subject { get; set; }
        public string Grade { get; set; }
        public DateTime Date { get; set; }
    }
}
