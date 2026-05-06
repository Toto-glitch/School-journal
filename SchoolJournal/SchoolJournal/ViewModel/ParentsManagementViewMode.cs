using SchoolJournal.Model;
using SchoolJournal.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace SchoolJournal.ViewModel
{
    public class ParentsManagementViewModel : BaseViewModel
    {
        private readonly AbsoluteService _absoluteService;
        private ObservableCollection<Parent> _parents;
        private Parent _selectedParent;
        private bool _isDirector;

        private string _lastName;
        private string _firstName;
        private string _fatherName;
        private string _username;
        private string _password;
        private string _email;
        private string _phoneNumber;
        private bool _isEditing;
        private bool _isDialogOpen;

        private ObservableCollection<Student> _allStudents;
        private ObservableCollection<Student> _selectedStudents;

        public ParentsManagementViewModel(Window win, User currentUser) : base(win)
        {
            _absoluteService = new AbsoluteService();
            _isDirector = currentUser.Role == UserRole.Director;
            Parents = new ObservableCollection<Parent>();
            AllStudents = new ObservableCollection<Student>();
            SelectedStudents = new ObservableCollection<Student>();
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

        public ObservableCollection<Student> AllStudents
        {
            get => _allStudents;
            set { _allStudents = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Student> SelectedStudents
        {
            get => _selectedStudents;
            set { _selectedStudents = value; OnPropertyChanged(); }
        }

        private void LoadData()
        {
            var allParents = _absoluteService.GetAllParents();
            Parents.Clear();
            foreach (var p in allParents) Parents.Add(p);

            var allStudents = _absoluteService.GetAllStudents();
            AllStudents.Clear();
            foreach (var s in allStudents) AllStudents.Add(s);
        }

        private RelayCommand _addParentCommand;
        public RelayCommand AddParentCommand => _addParentCommand ?? (_addParentCommand = new RelayCommand(
            obj => OpenAddDialog(),
            obj => _isDirector));

        private void OpenAddDialog()
        {
            ClearForm();
            IsEditing = false;
            IsDialogOpen = true;
        }

        private RelayCommand _editParentCommand;
        public RelayCommand EditParentCommand => _editParentCommand ?? (_editParentCommand = new RelayCommand(
            obj => StartEdit(),
            obj => _isDirector && SelectedParent != null));

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
            Password = "";

            SelectedStudents = new ObservableCollection<Student>(SelectedParent.Students);

            IsEditing = true;
            IsDialogOpen = true;
        }

        private RelayCommand _saveCommand;
        public RelayCommand SaveCommand => _saveCommand ?? (_saveCommand = new RelayCommand(
            obj => Save(),
            obj => _isDirector));

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(LastName) || string.IsNullOrWhiteSpace(FirstName))
            {
                MessageBox.Show("Фамилия и имя обязательны!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (IsEditing && SelectedParent != null)
                {
                    SelectedParent.LastName = LastName;
                    SelectedParent.FirstName = FirstName;
                    SelectedParent.FatherName = FatherName;
                    _absoluteService.UpdateParent(SelectedParent);

                    if (SelectedParent.User != null)
                    {
                        SelectedParent.User.Username = Username;
                        SelectedParent.User.Email = Email;
                        SelectedParent.User.PhoneNumber = PhoneNumber;
                        if (!string.IsNullOrWhiteSpace(Password))
                            SelectedParent.User.PasswordHash = PasswordHelper.HashPassword(Password);
                        _absoluteService.UpdateUser(SelectedParent.User);
                    }

                    UpdateParentStudents(SelectedParent.Id, SelectedStudents.Select(s => s.Id).ToList());
                }
                else
                {
                    var user = new User
                    {
                        Username = string.IsNullOrWhiteSpace(Username) ? $"{LastName}_{FirstName}" : Username,
                        PasswordHash = PasswordHelper.HashPassword(string.IsNullOrWhiteSpace(Password) ? "default123" : Password),
                        Email = Email ?? "",
                        PhoneNumber = PhoneNumber ?? "",
                        Role = UserRole.Parent
                    };
                    _absoluteService.AddUser(user);

                    var parent = new Parent
                    {
                        LastName = LastName,
                        FirstName = FirstName,
                        FatherName = FatherName,
                        UserId = user.Id
                    };
                    _absoluteService.AddParent(parent);

                    UpdateParentStudents(parent.Id, SelectedStudents.Select(s => s.Id).ToList());
                }

                LoadData();
                IsDialogOpen = false;
                MessageBox.Show("Данные сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private RelayCommand _deleteParentCommand;
        public RelayCommand DeleteParentCommand => _deleteParentCommand ?? (_deleteParentCommand = new RelayCommand(
            obj => DeleteParent(),
            obj => _isDirector && SelectedParent != null));

        private void DeleteParent()
        {
            if (SelectedParent == null) return;
            var ask = MessageBox.Show($"Удалить родителя {SelectedParent.LastName}?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (ask == MessageBoxResult.Yes)
            {
                try
                {
                    int uid = SelectedParent.UserId;
                    _absoluteService.DeleteParent(SelectedParent.Id);
                    _absoluteService.DeleteUser(uid);
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
            SelectedStudents.Clear();
        }

        private void UpdateParentStudents(int parentId, List<int> newStudentIds)
        {
            using (var context = new ApplicationContext())
            {
                var parent = context.Parents.Include("Students").FirstOrDefault(p => p.Id == parentId);
                if (parent == null) return;

                parent.Students.Clear();
                context.SaveChanges();

                foreach (var studentId in newStudentIds)
                {
                    var student = context.Students.Find(studentId);
                    if (student != null)
                        parent.Students.Add(student);
                }
                context.SaveChanges();
            }
        }
    }
}