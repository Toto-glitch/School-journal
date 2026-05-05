using SchoolJournal.Model;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SchoolJournal.Services
{
    public class AuthService
    {
        public User Authenticate(string username, string password)
        {
            string passwordHash = HashPassword(password);

            using (var context = new ApplicationContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Username == username && u.PasswordHash == passwordHash);
                return user;
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
