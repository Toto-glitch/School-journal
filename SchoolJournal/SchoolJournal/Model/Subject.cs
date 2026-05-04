using System.Collections.Generic;

namespace SchoolJournal.Model
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
        public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
        public virtual ICollection<Mark> Marks { get; set; } = new List<Mark>();
    }
}