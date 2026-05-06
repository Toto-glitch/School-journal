using SchoolJournal.Model;
using SchoolJournal.Service;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace SchoolJournal.ViewModel
{
    public class TeachersManagementViewModel : BaseViewModel
    {
        private readonly GradeService _gradeService;
        private ObservableCollection<Teacher> _teachers;
        private Teacher _selectedTeacher;
        private bool _isDirector;

        // Поля формы
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
            _gradeService = new GradeService();
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
            var all = _gradeService.GetAllTeachers();
            Teachers.Clear();
            foreach (var t in all)
                Teachers.Add(t);
        }

        // ========== Команда открытия диалога для добавления ==========
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

        // ========== Команда редактирования ==========
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
            Password = ""; // пароль при редактировании не показываем

            IsEditing = true;
            IsDialogOpen = true;
        }

        // ========== Команда сохранения (добавление или обновление) ==========
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
                    // Обновление
                    SelectedTeacher.LastName = LastName;
                    SelectedTeacher.FirstName = FirstName;
                    SelectedTeacher.FatherName = FatherName;
                    _gradeService.UpdateTeacher(SelectedTeacher);

                    if (SelectedTeacher.User != null)
                    {
                        SelectedTeacher.User.Username = Username;
                        SelectedTeacher.User.Email = Email;
                        SelectedTeacher.User.PhoneNumber = PhoneNumber;
                        if (!string.IsNullOrWhiteSpace(Password))
                            SelectedTeacher.User.PasswordHash = HashPassword(Password);
                        _gradeService.UpdateUser(SelectedTeacher.User);
                    }
                }
                else
                {
                    // Добавление
                    var user = new User
                    {
                        Username = string.IsNullOrWhiteSpace(Username) ? $"{LastName}_{FirstName}" : Username,
                        PasswordHash = HashPassword(string.IsNullOrWhiteSpace(Password) ? "default123" : Password),
                        Email = Email ?? "",
                        PhoneNumber = PhoneNumber ?? "",
                        Role = UserRole.Teacher
                    };
                    _gradeService.AddUser(user);

                    var teacher = new Teacher
                    {
                        LastName = LastName,
                        FirstName = FirstName,
                        FatherName = FatherName,
                        UserId = user.Id
                    };
                    _gradeService.AddTeacher(teacher);
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

        // ========== Команда удаления ==========
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
                    // Используем безопасный метод удаления
                    _gradeService.DeleteTeacher(SelectedTeacher.Id);
                    if (SelectedTeacher.User != null)
                        _gradeService.DeleteUser(SelectedTeacher.User.Id);
                    LoadTeachers();
                    MessageBox.Show("Учитель удалён.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // ========== Отмена ==========
        private RelayCommand _cancelCommand;
        public RelayCommand CancelCommand => _cancelCommand ?? (_cancelCommand = new RelayCommand(
            obj => { IsDialogOpen = false; }));

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                var builder = new System.Text.StringBuilder();
                foreach (var b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }
    }
}