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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // При удалении пользователя логи не удаляем
            modelBuilder.Entity<MarkLog>()
                .HasRequired(ml => ml.User)
                .WithMany(u => u.MarkLogs)
                .HasForeignKey(ml => ml.UserId)
                .WillCascadeOnDelete(false);

            // При удалении оценки, логи удаляются
            modelBuilder.Entity<MarkLog>()
                .HasRequired(ml => ml.Mark)
                .WithMany(m => m.Logs)
                .HasForeignKey(ml => ml.MarkId)
                .WillCascadeOnDelete(true);

            // При удалении учащегося, оценки тоже удаляются
            modelBuilder.Entity<Mark>()
                .HasRequired(m => m.Student)
                .WithMany(s => s.Marks)
                .HasForeignKey(m => m.StudentId)
                .WillCascadeOnDelete(true);

            // При удалении учителя, оценки не удаляем
            modelBuilder.Entity<Mark>()
                .HasRequired(m => m.Teacher)
                .WithMany(t => t.Marks)
                .HasForeignKey(m => m.TeacherId)
                .WillCascadeOnDelete(false);

            // При удалении учащегося, мы не удаляем его учетную запись
            modelBuilder.Entity<Student>()
                .HasOptional(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .WillCascadeOnDelete(false);

            // То же самое и с преподавателями
            modelBuilder.Entity<Teacher>()
                .HasOptional(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .WillCascadeOnDelete(false);

            // И с родителями учащихся
            modelBuilder.Entity<Parent>()
                .HasOptional(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .WillCascadeOnDelete(false);
        }
    }
}