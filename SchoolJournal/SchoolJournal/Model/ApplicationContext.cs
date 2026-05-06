using System.Data.Entity;

namespace SchoolJournal.Model
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext() : base("school_journal")
        {
            Database.SetInitializer(new DatabaseInitializer());
        }

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
            // 1. Отключаем каскадное удаление для связи Студент -> Пользователь
            modelBuilder.Entity<Student>()
                .HasRequired(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .WillCascadeOnDelete(false);

            // 2. Отключаем каскадное удаление для связи Учитель -> Пользователь
            modelBuilder.Entity<Teacher>()
                .HasRequired(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .WillCascadeOnDelete(false);

            // 3. Отключаем каскадное удаление для связи Родитель -> Пользователь
            modelBuilder.Entity<Parent>()
                .HasRequired(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .WillCascadeOnDelete(false);

            // 4. Важно: Логи не должны удаляться каскадом при удалении пользователя
            modelBuilder.Entity<MarkLog>()
                .HasRequired(ml => ml.User)
                .WithMany(u => u.MarkLogs)
                .HasForeignKey(ml => ml.UserId)
                .WillCascadeOnDelete(false);

            // 5. Оценки тоже лучше защитить от каскада со стороны учителя
            modelBuilder.Entity<Mark>()
                .HasRequired(m => m.Teacher)
                .WithMany()
                .HasForeignKey(m => m.TeacherId)
                .WillCascadeOnDelete(false);

            // Настройка связи многие-ко-многим между Учениками и Родителями
            modelBuilder.Entity<Student>()
                .HasMany(s => s.Parents)
                .WithMany(p => p.Students)
                .Map(m =>
                {
                    m.ToTable("StudentParents");
                    m.MapLeftKey("StudentId");
                    m.MapRightKey("ParentId");
                });

            // Настройка связи многие-ко-многим между Учителями и Предметами
            modelBuilder.Entity<Teacher>()
                .HasMany(t => t.Subjects)
                .WithMany(s => s.Teachers)
                .Map(m =>
                {
                    m.ToTable("TeacherSubjects");
                    m.MapLeftKey("TeacherId");
                    m.MapRightKey("SubjectId");
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}