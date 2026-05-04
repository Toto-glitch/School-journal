using System;
using System.Collections.Generic;

namespace SchoolJournal.Model
{
    public enum UserRole
    {
        Director = 1,
        Teacher = 2,
        Parent = 3,
        Student = 4
    }

    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public UserRole Role { get; set; }

        public virtual ICollection<MarkLog> MarkLogs { get; set; } = new List<MarkLog>();
    }
}