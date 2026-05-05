using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolJournal.Models
{
    public enum UserRole
    {
        Director,
        Teacher,
        Parent,
        Student
    }

    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Username { get; set; }

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; }

        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        public UserRole Role { get; set; }

        public virtual ICollection<MarkLog> MarkLogs { get; set; } = new List<MarkLog>();
    }
}