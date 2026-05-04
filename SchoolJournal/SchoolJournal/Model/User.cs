using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolJournal.Model
{
    public enum UserRole
    {
        Director = 1,
        Teacher = 2,
        Parent = 3,
        Student = 4
    }

    [Table("users")]
    public class User
    {
        [Key, Column("user_id")]
        public int Id { get; set; }

        [Required, MaxLength(255), Column("username")]
        public string UserName { get; set; }

        [Required, MaxLength(255), Column("password_hash")]
        public string PasswordHash { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required, Column("role")]
        public UserRole Role { get; set; }

        public virtual Student Student { get; set; }
        public virtual Teacher Teacher { get; set; }
        public virtual Parent Parent { get; set; }

        public virtual ICollection<MarkLog> MarkLogs { get; set; } = new List<MarkLog>();
    }
}