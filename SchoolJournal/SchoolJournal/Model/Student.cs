using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolJournal.Model
{
    [Table("students")]
    public class Student
    {
        [Key, Column("user_id")]
        public int Id { get; set; }

        [Required, MaxLength(255), Column("first_name")]
        public string FirstName { get; set; }

        [Required, MaxLength(255), Column("last_name")]
        public string LastName { get; set; }

        [MaxLength(255), Column("patronymic")]
        public string Patronymic { get; set; }

        public virtual User User { get; set; }
        public virtual Group Group { get; set; }

        public virtual ICollection<Parent> Parents { get; set; } = new List<Parent>();
        public virtual ICollection<Mark> Marks { get; set; } = new List<Mark>();
    }
}