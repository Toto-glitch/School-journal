using SchoolJournal.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolJournal.Model
{
    public class Group
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(10)]
        public string Title { get; set; }

        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
        public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();
    }
}