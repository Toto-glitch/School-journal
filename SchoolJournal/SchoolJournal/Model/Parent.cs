using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolJournal.Model
{
    [Table("parents")]
    public class Parent
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
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
    }
}