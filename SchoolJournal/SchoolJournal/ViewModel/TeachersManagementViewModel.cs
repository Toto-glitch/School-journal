using SchoolJournal.Model;
using SchoolJournal.Service;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace SchoolJournal.ViewModel
{
    public class TeachersManagementViewModel : BaseViewModel
    {
        private readonly AbsoluteService _absoluteService;
        private ObservableCollection<Teacher> _teachers;
        private Teacher _selectedTeacher;
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

        public TeachersManagementViewModel(Window win, User currentUser) : base(win)
        {
            _absoluteService = new AbsoluteService();
            _isDirector = currentUser.Role == UserRole.Director;
            Teachers = new ObservableCollection<Teacher>();
            LoadTeachers();
        }

        public ObservableCollection<Teacher> Teachers
        {
            get => _teachers;
            set { _teachers = value; OnPropertyChanged(); }
        }

        public Teacher SelectedTeacher
        {
            get => _selectedTeacher;
            set { _selectedTeacher = value; OnPropertyChanged(); }
        }

        public bool IsDirector => _isDirector;

        public string LastName { get => _lastName; set { _lastName = value; OnPropertyChanged(); } }
        public string FirstName { get => _firstName; set { _firstName = value; OnPropertyChanged(); } }
        public string FatherName { get => _fatherName; set { _fatherName = value; OnPropertyChanged(); } }
        public string Username { get => _username; set { _username = value; OnPropertyChanged(); } }
        public string Password { get => _password; set { _password = value; OnPropertyChanged(); } }
        public string Email { get => _email; set { _email = value; OnPropertyChanged(); } }
        public string PhoneNumber { get => _phoneNumber; set { _phoneNumber = value; OnPropertyChanged(); } }

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

        private void LoadTeachers()
        {
            var all = _absoluteService.GetAllTeachers();
            Teachers.Clear();
            foreach (var t in all)
                Teachers.Add(t);
        }

        private RelayCommand _addTeacherCommand;
        public RelayCommand AddTeacherCommand => _addTeacherCommand ?? (_addTeacherCommand = new RelayCommand(
            obj => OpenAddDialog(),
            obj => _isDirector));

        private void OpenAddDialog()
        {
            LastName = "";
            FirstName = "";
            FatherName = "";
            Username = "";
            Password = "";
            Email = "";
            PhoneNumber = "";
            IsEditing = false;
            IsDialogOpen = true;
        }

        private RelayCommand _editTeacherCommand;
        public RelayCommand EditTeacherCommand => _editTeacherCommand ?? (_editTeacherCommand = new RelayCommand(
            obj => StartEdit(),
            obj => _isDirector && SelectedTeacher != null));

        private void StartEdit()
        {
            if (SelectedTeacher == null) return;

            LastName = SelectedTeacher.LastName;
            FirstName = SelectedTeacher.FirstName;
            FatherName = SelectedTeacher.FatherName;

            if (SelectedTeacher.User != null)
            {
                Username = SelectedTeacher.User.Username;
                Email = SelectedTeacher.User.Email;
                PhoneNumber = SelectedTeacher.User.PhoneNumber;
            }
            Password = "";

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
                if (IsEditing && SelectedTeacher != null)
                {
                    SelectedTeacher.LastName = LastName;
                    SelectedTeacher.FirstName = FirstName;
                    SelectedTeacher.FatherName = FatherName;
                    _absoluteService.UpdateTeacher(SelectedTeacher);

                    if (SelectedTeacher.User != null)
                    {
                        SelectedTeacher.User.Username = Username;
                        SelectedTeacher.User.Email = Email;
                        SelectedTeacher.User.PhoneNumber = PhoneNumber;
                        if (!string.IsNullOrWhiteSpace(Password))
                            SelectedTeacher.User.PasswordHash = PasswordHelper.HashPassword(Password);
                        _absoluteService.UpdateUser(SelectedTeacher.User);
                    }
                }
                else
                {
                    var user = new User
                    {
                        Username = string.IsNullOrWhiteSpace(Username) ? $"{LastName}_{FirstName}" : Username,
                        PasswordHash = PasswordHelper.HashPassword(string.IsNullOrWhiteSpace(Password) ? "default123" : Password),
                        Email = Email ?? "",
                        PhoneNumber = PhoneNumber ?? "",
                        Role = UserRole.Teacher
                    };
                    _absoluteService.AddUser(user);

                    var teacher = new Teacher
                    {
                        LastName = LastName,
                        FirstName = FirstName,
                        FatherName = FatherName,
                        UserId = user.Id
                    };
                    _absoluteService.AddTeacher(teacher);
                }

                LoadTeachers();
                IsDialogOpen = false;
                MessageBox.Show("Данные сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private RelayCommand _deleteTeacherCommand;
        public RelayCommand DeleteTeacherCommand => _deleteTeacherCommand ?? (_deleteTeacherCommand = new RelayCommand(
            obj => DeleteTeacher(),
            obj => _isDirector && SelectedTeacher != null));

        private void DeleteTeacher()
        {
            if (SelectedTeacher == null) return;

            var result = MessageBox.Show(
                $"Вы уверены, что хотите удалить учителя {SelectedTeacher.LastName} {SelectedTeacher.FirstName}?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _absoluteService.DeleteTeacher(SelectedTeacher.Id);
                    if (SelectedTeacher.User != null)
                        _absoluteService.DeleteUser(SelectedTeacher.User.Id);
                    LoadTeachers();
                    MessageBox.Show("Учитель удалён.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private RelayCommand _cancelCommand;
        public RelayCommand CancelCommand => _cancelCommand ?? (_cancelCommand = new RelayCommand(
            obj => { IsDialogOpen = false; }));
    }
}