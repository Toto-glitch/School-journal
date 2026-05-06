using SchoolJournal.Model;
using SchoolJournal.Service;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace SchoolJournal.ViewModel
{
    public class GroupsManagementViewModel : BaseViewModel
    {
        private readonly AbsoluteService _absoluteService;
        private ObservableCollection<Group> _groups;
        private Group _selectedGroup;
        private bool _isDirector;
        private bool _isEditing;
        private bool _isDialogOpen;
        private string _title;

        public GroupsManagementViewModel(Window win, User currentUser) : base(win)
        {
            _absoluteService = new AbsoluteService();
            _isDirector = currentUser.Role == UserRole.Director;
            Groups = new ObservableCollection<Group>();
            LoadData();
        }

        public ObservableCollection<Group> Groups
        {
            get => _groups;
            set { _groups = value; OnPropertyChanged(); }
        }

        public Group SelectedGroup
        {
            get => _selectedGroup;
            set { _selectedGroup = value; OnPropertyChanged(); }
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
            var all = _absoluteService.GetAllGroups();
            Groups.Clear();
            foreach (var g in all)
                Groups.Add(g);
        }

        private RelayCommand _addGroupCommand;
        public RelayCommand AddGroupCommand => _addGroupCommand ?? (_addGroupCommand = new RelayCommand(
            obj => OpenAddDialog(),
            obj => _isDirector));

        private void OpenAddDialog()
        {
            Title = "";
            IsEditing = false;
            IsDialogOpen = true;
        }

        private RelayCommand _editGroupCommand;
        public RelayCommand EditGroupCommand => _editGroupCommand ?? (_editGroupCommand = new RelayCommand(
            obj => StartEdit(),
            obj => _isDirector && SelectedGroup != null));

        private void StartEdit()
        {
            if (SelectedGroup == null) return;
            Title = SelectedGroup.Title;
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
                MessageBox.Show("Название группы обязательно!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (IsEditing && SelectedGroup != null)
                {
                    SelectedGroup.Title = Title;
                    _absoluteService.UpdateGroup(SelectedGroup);
                }
                else
                {
                    var group = new Group { Title = Title };
                    _absoluteService.AddGroup(group);
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

        private RelayCommand _deleteGroupCommand;
        public RelayCommand DeleteGroupCommand => _deleteGroupCommand ?? (_deleteGroupCommand = new RelayCommand(
            obj => DeleteGroup(),
            obj => _isDirector && SelectedGroup != null));

        private void DeleteGroup()
        {
            if (SelectedGroup == null) return;
            var ask = MessageBox.Show($"Удалить группу «{SelectedGroup.Title}»?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (ask == MessageBoxResult.Yes)
            {
                try
                {
                    _absoluteService.DeleteGroup(SelectedGroup.Id);
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