using SchoolJournal.Model;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SchoolJournal.Service
{
    public class AuthService
    {
        public User Authenticate(string username, string password)
        {
            string passwordHash = HashPassword(password);

            using (var context = new ApplicationContext())
            {
                var user = context.Users
                    .FirstOrDefault(u => u.Username == username && u.PasswordHash == passwordHash);

                return user;
            }
        }

        public User GetUserById(int userId)
        {
            using (var context = new ApplicationContext())
            {
                return context.Users
                    .Include(u => u.MarkLogs)
                    .FirstOrDefault(u => u.Id == userId);
            }
        }

        public Teacher GetTeacherByUserId(int userId)
        {
            using (var context = new ApplicationContext())
            {
                return context.Teachers
                    .Include(t => t.Subjects)
                    .FirstOrDefault(t => t.UserId == userId);
            }
        }

        public Student GetStudentByUserId(int userId)
        {
            using (var context = new ApplicationContext())
            {
                return context.Students
                    .Include(s => s.Group)
                    .Include(s => s.Marks.Select(m => m.Subject))
                    .Include(s => s.Marks.Select(m => m.Teacher))
                    .Include(s => s.Parents)
                    .FirstOrDefault(s => s.UserId == userId);
            }
        }

        public Parent GetParentByUserId(int userId)
        {
            using (var context = new ApplicationContext())
            {
                return context.Parents
                    .Include(p => p.Students.Select(s => s.Group))
                    .Include(p => p.Students.Select(s => s.Marks.Select(m => m.Subject)))
                    .Include(p => p.Students.Select(s => s.Marks.Select(m => m.Teacher)))
                    .FirstOrDefault(p => p.UserId == userId);
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}