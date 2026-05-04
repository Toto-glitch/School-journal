using System.Data.Entity;

namespace SchoolJournal.Model
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext() : base("school_journal") { }

        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Mark> Marks { get; set; }
        public DbSet<MarkLog> MarkLogs { get; set; }
    }
}