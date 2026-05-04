using System.Collections.Generic;

namespace SchoolJournal.Model
{
    public class Teacher
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }

        public int? UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();
        public virtual ICollection<Mark> Marks { get; set; } = new List<Mark>();
    }
}