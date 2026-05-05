using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolJournal.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required][MaxLength(255)] public string LastName { get; set; }
        [Required][MaxLength(255)] public string FirstName { get; set; }
        [MaxLength(255)] public string FatherName { get; set; }

        public int GroupId { get; set; }
        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public virtual ICollection<Parent> Parents { get; set; } = new List<Parent>();
        public virtual ICollection<Mark> Marks { get; set; } = new List<Mark>();
    }
}