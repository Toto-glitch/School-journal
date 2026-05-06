using SchoolJournal.Model;
using SchoolJournal.Service;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace SchoolJournal.ViewModel
{
    public class SubjectsManagementViewModel : BaseViewModel
    {
        private readonly GradeService _gradeService;
        private ObservableCollection<Subject> _subjects;
        private Subject _selectedSubject;
        private bool _isDirector;
        private bool _isEditing;
        private bool _isDialogOpen;
        private string _title;

        public SubjectsManagementViewModel(Window win, User currentUser) : base(win)
        {
            _gradeService = new GradeService();
            _isDirector = currentUser.Role == UserRole.Director;
            Subjects = new ObservableCollection<Subject>();
            LoadData();
        }

        public ObservableCollection<Subject> Subjects
        {
            get => _subjects;
            set { _subjects = value; OnPropertyChanged(); }
        }

        public Subject SelectedSubject
        {
            get => _selectedSubject;
            set { _selectedSubject = value; OnPropertyChanged(); }
        }

        public bool IsDirector => _isDirector;

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }

        public bool IsEditing
        {
            get => _isEditing;
            set { _isEditing = value; OnPropertyChanged(); }
        }

        public bool IsDialogOpen
        {
            get => _isDialogOpen;
            set { _isDialogOpen = value; OnPropertyChanged(); }
        }

        private void LoadData()
        {
            var all = _gradeService.GetAllSubjects();
            Subjects.Clear();
            foreach (var s in all)
                Subjects.Add(s);
        }

        private RelayCommand _addSubjectCommand;
        public RelayCommand AddSubjectCommand => _addSubjectCommand ?? (_addSubjectCommand = new RelayCommand(
            obj => OpenAddDialog(),
            obj => _isDirector));

        private void OpenAddDialog()
        {
            Title = "";
            IsEditing = false;
            IsDialogOpen = true;
        }

        private RelayCommand _editSubjectCommand;
        public RelayCommand EditSubjectCommand => _editSubjectCommand ?? (_editSubjectCommand = new RelayCommand(
            obj => StartEdit(),
            obj => _isDirector && SelectedSubject != null));

        private void StartEdit()
        {
            if (SelectedSubject == null) return;
            Title = SelectedSubject.Title;
            IsEditing = true;
            IsDialogOpen = true;
        }

        private RelayCommand _saveCommand;
        public RelayCommand SaveCommand => _saveCommand ?? (_saveCommand = new RelayCommand(
            obj => Save(),
            obj => _isDirector));

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                MessageBox.Show("Название предмета обязательно!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (IsEditing && SelectedSubject != null)
                {
                    SelectedSubject.Title = Title;
                    _gradeService.UpdateSubject(SelectedSubject);
                }
                else
                {
                    var subject = new Subject { Title = Title };
                    _gradeService.AddSubject(subject);
                }

                LoadData();
                IsDialogOpen = false;
                MessageBox.Show("Сохранено!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private RelayCommand _deleteSubjectCommand;
        public RelayCommand DeleteSubjectCommand => _deleteSubjectCommand ?? (_deleteSubjectCommand = new RelayCommand(
            obj => DeleteSubject(),
            obj => _isDirector && SelectedSubject != null));

        private void DeleteSubject()
        {
            if (SelectedSubject == null) return;
            var ask = MessageBox.Show($"Удалить предмет «{SelectedSubject.Title}»?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (ask == MessageBoxResult.Yes)
            {
                try
                {
                    _gradeService.DeleteSubject(SelectedSubject.Id);
                    LoadData();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private RelayCommand _cancelCommand;
        public RelayCommand CancelCommand => _cancelCommand ?? (_cancelCommand = new RelayCommand(
            obj => { IsDialogOpen = false; }));
    }
}