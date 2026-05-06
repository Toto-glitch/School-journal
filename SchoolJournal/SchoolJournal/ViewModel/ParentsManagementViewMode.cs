using SchoolJournal.Model;
using SchoolJournal.Service;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace SchoolJournal.ViewModel
{
    public class ParentsManagementViewModel : BaseViewModel
    {
        private readonly GradeService _gradeService;
        private ObservableCollection<Parent> _parents;
        private Parent _selectedParent;
        private bool _isDirector;
        private bool _isEditing;
        private bool _isDialogOpen;
        private string _lastName, _firstName, _fatherName, _username, _password, _email, _phoneNumber;

        public ParentsManagementViewModel(Window win, User currentUser) : base(win)
        {
            _gradeService = new GradeService();
            _isDirector = currentUser.Role == UserRole.Director;
            Parents = new ObservableCollection<Parent>();
            LoadData();
        }

        public ObservableCollection<Parent> Parents { get => _parents; set { _parents = value; OnPropertyChanged(); } }
        public Parent SelectedParent { get => _selectedParent; set { _selectedParent = value; OnPropertyChanged(); } }
        public bool IsDirector => _isDirector;

        public string LastName { get => _lastName; set { _lastName = value; OnPropertyChanged(); } }
        public string FirstName { get => _firstName; set { _firstName = value; OnPropertyChanged(); } }
        public string FatherName { get => _fatherName; set { _fatherName = value; OnPropertyChanged(); } }
        public string Username { get => _username; set { _username = value; OnPropertyChanged(); } }
        public string Password { get => _password; set { _password = value; OnPropertyChanged(); } }
        public string Email { get => _email; set { _email = value; OnPropertyChanged(); } }
        public string PhoneNumber { get => _phoneNumber; set { _phoneNumber = value; OnPropertyChanged(); } }

        public bool IsEditing { get => _isEditing; set { _isEditing = value; OnPropertyChanged(); } }
        public bool IsDialogOpen { get => _isDialogOpen; set { _isDialogOpen = value; OnPropertyChanged(); } }

        private void LoadData()
        {
            var all = _gradeService.GetAllParents();
            Parents.Clear();
            foreach (var p in all) Parents.Add(p);
        }

        private RelayCommand _addCommand;
        public RelayCommand AddParentCommand => _addCommand ?? (_addCommand = new RelayCommand(
            obj => OpenAdd(), obj => _isDirector));

        private void OpenAdd()
        {
            ClearForm();
            IsEditing = false;
            IsDialogOpen = true;
        }

        private RelayCommand _editCommand;
        public RelayCommand EditParentCommand => _editCommand ?? (_editCommand = new RelayCommand(
            obj => StartEdit(), obj => _isDirector && SelectedParent != null));

        private void StartEdit()
        {
            if (SelectedParent == null) return;
            LastName = SelectedParent.LastName;
            FirstName = SelectedParent.FirstName;
            FatherName = SelectedParent.FatherName;
            if (SelectedParent.User != null)
            {
                Username = SelectedParent.User.Username;
                Email = SelectedParent.User.Email;
                PhoneNumber = SelectedParent.User.PhoneNumber;
            }
            IsEditing = true;
            IsDialogOpen = true;
        }

        private RelayCommand _saveCommand;
        public RelayCommand SaveCommand => _saveCommand ?? (_saveCommand = new RelayCommand(
            obj => Save(), obj => _isDirector));

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(LastName) || string.IsNullOrWhiteSpace(FirstName))
            {
                MessageBox.Show("Фамилия и имя обязательны!");
                return;
            }

            try
            {
                if (IsEditing && SelectedParent != null)
                {
                    SelectedParent.LastName = LastName;
                    SelectedParent.FirstName = FirstName;
                    SelectedParent.FatherName = FatherName;
                    _gradeService.UpdateParent(SelectedParent);
                    if (SelectedParent.User != null)
                    {
                        SelectedParent.User.Username = Username;
                        SelectedParent.User.Email = Email;
                        SelectedParent.User.PhoneNumber = PhoneNumber;
                        if (!string.IsNullOrWhiteSpace(Password))
                            SelectedParent.User.PasswordHash = HashPassword(Password);
                        _gradeService.UpdateUser(SelectedParent.User);
                    }
                }
                else
                {
                    var user = new User
                    {
                        Username = Username ?? $"{LastName}_{FirstName}",
                        PasswordHash = HashPassword(Password ?? "default123"),
                        Email = Email ?? "",
                        PhoneNumber = PhoneNumber ?? "",
                        Role = UserRole.Parent
                    };
                    _gradeService.AddUser(user);
                    var parent = new Parent
                    {
                        LastName = LastName,
                        FirstName = FirstName,
                        FatherName = FatherName,
                        UserId = user.Id
                    };
                    _gradeService.AddParent(parent);
                }

                LoadData();
                IsDialogOpen = false;
                MessageBox.Show("Сохранено!");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private RelayCommand _deleteCommand;
        public RelayCommand DeleteParentCommand => _deleteCommand ?? (_deleteCommand = new RelayCommand(
            obj => Delete(), obj => _isDirector && SelectedParent != null));

        private void Delete()
        {
            if (SelectedParent == null) return;
            var ask = MessageBox.Show($"Удалить родителя {SelectedParent.LastName}?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (ask == MessageBoxResult.Yes)
            {
                try
                {
                    int uid = SelectedParent.UserId;
                    _gradeService.DeleteParent(SelectedParent.Id);
                    _gradeService.DeleteUser(uid);
                    LoadData();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private RelayCommand _cancelCommand;
        public RelayCommand CancelCommand => _cancelCommand ?? (_cancelCommand = new RelayCommand(
            obj => { IsDialogOpen = false; }));

        private void ClearForm()
        {
            LastName = FirstName = FatherName = Username = Password = Email = PhoneNumber = "";
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                var sb = new System.Text.StringBuilder();
                foreach (var b in bytes) sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }
    }
}