using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using SchoolDiary.Models;

namespace SchoolDiary.Services
{
    /// <summary>
    /// Сервис аутентификации и авторизации пользователей
    /// </summary>
    public class AuthService
    {
        private readonly SchoolDbContext _context;

        public AuthService(SchoolDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Аутентификация пользователя по логину и паролю
        /// </summary>
        public User Authenticate(string login, string password)
        {
            string passwordHash = HashPassword(password);

            return _context.Users
                .FirstOrDefault(u => u.Login == login && u.PasswordHash == passwordHash);
        }

        /// <summary>
        /// Проверка прав доступа для роли
        /// </summary>
        public bool HasPermission(User user, string requiredRole)
        {
            if (user == null) return false;
            return user.Role.Equals(requiredRole, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Получение учителя по пользователю
        /// </summary>
        public Teacher GetTeacherByUser(User user)
        {
            if (user == null || user.Role != "Teacher") return null;
            return _context.Teachers.FirstOrDefault(t => t.UserId == user.Id);
        }

        /// <summary>
        /// Получение родителя по пользователю
        /// </summary>
        public Parent GetParentByUser(User user)
        {
            if (user == null || user.Role != "Parent") return null;
            return _context.Parents.FirstOrDefault(p => p.UserId == user.Id);
        }

        /// <summary>
        /// Получение ученика по пользователю
        /// </summary>
        public Student GetStudentByUser(User user)
        {
            if (user == null || user.Role != "Student") return null;
            return _context.Students.FirstOrDefault(s => s.UserId == user.Id);
        }

        /// <summary>
        /// Хэширование пароля (SHA256)
        /// </summary>
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// Создание пользователя с заданной ролью
        /// </summary>
        public User CreateUser(string login, string password, string role)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Login == login);
            if (existingUser != null)
                throw new Exception("Пользователь с таким логином уже существует");

            var user = new User
            {
                Login = login,
                PasswordHash = HashPassword(password),
                Role = role,
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }
    }
}
