using System.Collections.Generic;

namespace SchoolJournal.Model
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }

        public int? UserId { get; set; }
        public virtual User User { get; set; }

        public int GroupId { get; set; }
        public virtual Group Group { get; set; }

        public virtual ICollection<Parent> Parents { get; set; } = new List<Parent>();
        public virtual ICollection<Mark> Marks { get; set; } = new List<Mark>();
    }
}