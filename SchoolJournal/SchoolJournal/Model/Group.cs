using System.Collections.Generic;

namespace SchoolJournal.Model
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
        public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();
    }
}