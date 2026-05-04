using System.Collections.Generic;

namespace SchoolJournal.Model
{
    public class Parent
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }

        public int? UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
    }
}