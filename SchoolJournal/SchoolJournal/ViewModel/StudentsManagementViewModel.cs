using SchoolJournal.Model;
using SchoolJournal.Service;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace SchoolJournal.ViewModel
{
    public class StudentsManagementViewModel : BaseViewModel
    {
        private readonly GradeService _gradeService;
        private ObservableCollection<Student> _students;
        private Student _selectedStudent;
        private bool _isDirector;
        private ObservableCollection<Group> _groups;

        // Поля для добавления/редактирования
        private string _lastName;
        private string _firstName;
        private string _fatherName;
        private Group _selectedGroup;
        private string _username;
        private string _password;
        private string _email;
        private string _phoneNumber;
        private bool _isEditing;
        private bool _isAddDialogOpen;

        public StudentsManagementViewModel(Window win, User currentUser) : base(win)
        {
            _gradeService = new GradeService();
            _isDirector = currentUser.Role == UserRole.Director;
            Students = new ObservableCollection<Student>();
            Groups = new ObservableCollection<Group>();
            LoadData();
        }

        public ObservableCollection<Student> Students
        {
            get => _students;
            set { _students = value; OnPropertyChanged(); }
        }

        public Student SelectedStudent
        {
            get => _selectedStudent;
            set { _selectedStudent = value; OnPropertyChanged(); }
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

        public string LastName
        {
            get => _lastName;
            set { _lastName = value; OnPropertyChanged(); }
        }

        public string FirstName
        {
            get => _firstName;
            set { _firstName = value; OnPropertyChanged(); }
        }

        public string FatherName
        {
            get => _fatherName;
            set { _fatherName = value; OnPropertyChanged(); }
        }

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set { _phoneNumber = value; OnPropertyChanged(); }
        }

        public bool IsEditing
        {
            get => _isEditing;
            set { _isEditing = value; OnPropertyChanged(); }
        }

        public bool IsAddDialogOpen
        {
            get => _isAddDialogOpen;
            set { _isAddDialogOpen = value; OnPropertyChanged(); }
        }

        private void LoadData()
        {
            var allStudents = _gradeService.GetAllStudents();
            Students.Clear();
            foreach (var s in allStudents)
                Students.Add(s);

            var allGroups = _gradeService.GetAllGroups();
            Groups.Clear();
            foreach (var g in allGroups)
                Groups.Add(g);
        }

        private RelayCommand _addStudentCommand;
        public RelayCommand AddStudentCommand => _addStudentCommand ?? (_addStudentCommand = new RelayCommand(
            obj => AddStudent(),
            obj => _isDirector));

        private void AddStudent()
        {
            if (string.IsNullOrWhiteSpace(LastName) || string.IsNullOrWhiteSpace(FirstName))
            {
                MessageBox.Show("Фамилия и имя обязательны!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (SelectedGroup == null)
            {
                MessageBox.Show("Выберите группу!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var user = new User
                {
                    Username = Username ?? $"{LastName}_{FirstName}",
                    PasswordHash = HashPassword(Password ?? "default123"),
                    Email = Email ?? "",
                    PhoneNumber = PhoneNumber ?? "",
                    Role = UserRole.Student
                };
                _gradeService.AddUser(user);

                var student = new Student
                {
                    LastName = LastName,
                    FirstName = FirstName,
                    FatherName = FatherName ?? "",
                    GroupId = SelectedGroup.Id,
                    UserId = user.Id
                };
                _gradeService.AddStudent(student);

                LoadData();
                ClearForm();
                IsAddDialogOpen = false;
                MessageBox.Show("Ученик успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private RelayCommand _editStudentCommand;
        public RelayCommand EditStudentCommand => _editStudentCommand ?? (_editStudentCommand = new RelayCommand(
            obj => StartEdit(),
            obj => _isDirector && SelectedStudent != null));

        private void StartEdit()
        {
            if (SelectedStudent == null) return;

            LastName = SelectedStudent.LastName;
            FirstName = SelectedStudent.FirstName;
            FatherName = SelectedStudent.FatherName;
            SelectedGroup = Groups.FirstOrDefault(g => g.Id == SelectedStudent.GroupId);

            if (SelectedStudent.User != null)
            {
                Username = SelectedStudent.User.Username;
                Email = SelectedStudent.User.Email;
                PhoneNumber = SelectedStudent.User.PhoneNumber;
            }

            IsEditing = true;
            IsAddDialogOpen = true;
        }

        private RelayCommand _saveEditCommand;
        public RelayCommand SaveEditCommand => _saveEditCommand ?? (_saveEditCommand = new RelayCommand(
            obj => SaveEdit(),
            obj => _isDirector && IsEditing));

        private void SaveEdit()
        {
            if (SelectedStudent == null || SelectedGroup == null) return;

            try
            {
                SelectedStudent.LastName = LastName;
                SelectedStudent.FirstName = FirstName;
                SelectedStudent.FatherName = FatherName ?? "";
                SelectedStudent.GroupId = SelectedGroup.Id;
                _gradeService.UpdateStudent(SelectedStudent);

                if (SelectedStudent.User != null)
                {
                    SelectedStudent.User.Username = Username;
                    SelectedStudent.User.Email = Email;
                    SelectedStudent.User.PhoneNumber = PhoneNumber;
                    if (!string.IsNullOrWhiteSpace(Password))
                        SelectedStudent.User.PasswordHash = HashPassword(Password);
                    _gradeService.UpdateUser(SelectedStudent.User);
                }

                LoadData();
                ClearForm();
                IsEditing = false;
                IsAddDialogOpen = false;
                MessageBox.Show("Данные ученика обновлены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private RelayCommand _deleteStudentCommand;
        public RelayCommand DeleteStudentCommand => _deleteStudentCommand ?? (_deleteStudentCommand = new RelayCommand(
            obj => DeleteStudent(),
            obj => _isDirector && SelectedStudent != null));

        private void DeleteStudent()
        {
            if (SelectedStudent == null) return;

            var result = MessageBox.Show(
                $"Вы уверены, что хотите удалить ученика {SelectedStudent.LastName} {SelectedStudent.FirstName}?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    int userId = SelectedStudent.UserId;
                    _gradeService.DeleteStudent(SelectedStudent.Id);
                    _gradeService.DeleteUser(userId);
                    LoadData();
                    MessageBox.Show("Ученик удалён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private RelayCommand _cancelCommand;
        public RelayCommand CancelCommand => _cancelCommand ?? (_cancelCommand = new RelayCommand(
            obj =>
            {
                ClearForm();
                IsEditing = false;
                IsAddDialogOpen = false;
            }));

        private void ClearForm()
        {
            LastName = "";
            FirstName = "";
            FatherName = "";
            SelectedGroup = null;
            Username = "";
            Password = "";
            Email = "";
            PhoneNumber = "";
        }

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