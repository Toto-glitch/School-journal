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
        private readonly AbsoluteService _absoluteService;
        private ObservableCollection<Student> _students;
        private Student _selectedStudent;
        private bool _isDirector;
        private ObservableCollection<Group> _groups;
        private ObservableCollection<Parent> _parents;

        private string _lastName;
        private string _firstName;
        private string _fatherName;
        private Group _selectedGroup;
        private Parent _selectedParent;
        private string _username;
        private string _password;
        private string _email;
        private string _phoneNumber;
        private bool _isEditing;
        private bool _isDialogOpen;

        public StudentsManagementViewModel(Window win, User currentUser) : base(win)
        {
            _absoluteService = new AbsoluteService();
            _isDirector = currentUser.Role == UserRole.Director;
            Students = new ObservableCollection<Student>();
            Groups = new ObservableCollection<Group>();
            Parents = new ObservableCollection<Parent>();
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

        public ObservableCollection<Parent> Parents
        {
            get => _parents;
            set { _parents = value; OnPropertyChanged(); }
        }

        public Group SelectedGroup
        {
            get => _selectedGroup;
            set { _selectedGroup = value; OnPropertyChanged(); }
        }

        public Parent SelectedParent
        {
            get => _selectedParent;
            set { _selectedParent = value; OnPropertyChanged(); }
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

        private void LoadData()
        {
            var allStudents = _absoluteService.GetAllStudents();
            Students.Clear();
            foreach (var s in allStudents)
                Students.Add(s);

            var allGroups = _absoluteService.GetAllGroups();
            Groups.Clear();
            foreach (var g in allGroups)
                Groups.Add(g);

            var allParents = _absoluteService.GetAllParents();
            Parents.Clear();
            foreach (var p in allParents)
                Parents.Add(p);
        }

        private RelayCommand _addStudentCommand;
        public RelayCommand AddStudentCommand => _addStudentCommand ?? (_addStudentCommand = new RelayCommand(
            obj => OpenAddDialog(),
            obj => _isDirector));

        private void OpenAddDialog()
        {
            LastName = "";
            FirstName = "";
            FatherName = "";
            SelectedGroup = null;
            SelectedParent = null;
            Username = "";
            Password = "";
            Email = "";
            PhoneNumber = "";
            IsEditing = false;
            IsDialogOpen = true;
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

            Student studentWithParents = null;
            using (var context = new ApplicationContext())
            {
                studentWithParents = context.Students
                    .Include("Parents")
                    .FirstOrDefault(s => s.Id == SelectedStudent.Id);
            }

            if (studentWithParents?.Parents != null && studentWithParents.Parents.Any())
                SelectedParent = Parents.FirstOrDefault(p => p.Id == studentWithParents.Parents.First().Id);
            else
                SelectedParent = null;

            if (SelectedStudent.User != null)
            {
                Username = SelectedStudent.User.Username;
                Email = SelectedStudent.User.Email;
                PhoneNumber = SelectedStudent.User.PhoneNumber;
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

            if (SelectedGroup == null)
            {
                MessageBox.Show("Выберите группу!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (SelectedParent == null)
            {
                MessageBox.Show("Необходимо выбрать родителя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (IsEditing && SelectedStudent != null)
                {
                    SelectedStudent.LastName = LastName;
                    SelectedStudent.FirstName = FirstName;
                    SelectedStudent.FatherName = FatherName;
                    SelectedStudent.GroupId = SelectedGroup.Id;
                    _absoluteService.UpdateStudent(SelectedStudent);

                    if (SelectedStudent.User != null)
                    {
                        SelectedStudent.User.Username = Username;
                        SelectedStudent.User.Email = Email;
                        SelectedStudent.User.PhoneNumber = PhoneNumber;
                        if (!string.IsNullOrWhiteSpace(Password))
                            SelectedStudent.User.PasswordHash = PasswordHelper.HashPassword(Password);
                        _absoluteService.UpdateUser(SelectedStudent.User);
                    }

                    using (var context = new ApplicationContext())
                    {
                        var stud = context.Students.Include("Parents").FirstOrDefault(s => s.Id == SelectedStudent.Id);
                        if (stud != null)
                        {
                            stud.Parents.Clear();
                            var parent = context.Parents.Find(SelectedParent.Id);
                            if (parent != null)
                                stud.Parents.Add(parent);
                            context.SaveChanges();
                        }
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
                        Role = UserRole.Student
                    };
                    _absoluteService.AddUser(user);

                    var student = new Student
                    {
                        LastName = LastName,
                        FirstName = FirstName,
                        FatherName = FatherName,
                        GroupId = SelectedGroup.Id,
                        UserId = user.Id
                    };
                    _absoluteService.AddStudent(student);
                    _absoluteService.AddStudentParent(student.Id, SelectedParent.Id);
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

        private RelayCommand _deleteStudentCommand;
        public RelayCommand DeleteStudentCommand => _deleteStudentCommand ?? (_deleteStudentCommand = new RelayCommand(
            obj => DeleteStudent(),
            obj => _isDirector && SelectedStudent != null));

        private void DeleteStudent()
        {
            if (SelectedStudent == null) return;

            var result = MessageBox.Show(
                $"Вы уверены, что хотите удалить ученика {SelectedStudent.LastName} {SelectedStudent.FirstName}?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _absoluteService.DeleteStudent(SelectedStudent.Id);
                    if (SelectedStudent.User != null)
                        _absoluteService.DeleteUser(SelectedStudent.User.Id);
                    LoadData();
                    MessageBox.Show("Ученик удалён.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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