using System;
using System.Data.Entity;
using System.Linq;
using SchoolDiary.Models;

namespace SchoolDiary.Models
{
    /// <summary>
    /// Контекст базы данных для системы электронного дневника
    /// </summary>
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext() 
            : base("name=SchoolDiaryConnection")
        {
            // Отключаем инициализатор по умолчанию, используем свой
            Database.SetInitializer<SchoolDbContext>(null);
        }

        // Таблицы
        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Mark> Marks { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<MarkLog> MarkLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка связи Student - Parent (1:1)
            modelBuilder.Entity<Student>()
                .HasOptional(s => s.Parent)
                .WithRequired(p => p.Student);

            // Настройка связи Teacher - Subject (1:1)
            modelBuilder.Entity<Teacher>()
                .HasOptional(t => t.Subject)
                .WithRequired(s => s.Teacher);

            // Настройка связи User - Teacher/Parent/Student (1:1)
            modelBuilder.Entity<User>()
                .HasOptional(u => u.Teacher)
                .WithRequired(t => t.User);

            modelBuilder.Entity<User>()
                .HasOptional(u => u.Parent)
                .WithRequired(p => p.User);

            modelBuilder.Entity<User>()
                .HasOptional(u => u.Student)
                .WithRequired(s => s.User);

            // Настройка связи Mark - Student (M:1)
            modelBuilder.Entity<Mark>()
                .HasRequired(m => m.Student)
                .WithMany(s => s.Marks)
                .HasForeignKey(m => m.StudentId)
                .WillCascadeOnDelete(false);

            // Настройка связи Mark - Subject (M:1)
            modelBuilder.Entity<Mark>()
                .HasRequired(m => m.Subject)
                .WithMany(s => s.Marks)
                .HasForeignKey(m => m.SubjectId)
                .WillCascadeOnDelete(false);

            // Настройка связи Mark - Teacher (M:1)
            modelBuilder.Entity<Mark>()
                .HasRequired(m => m.Teacher)
                .WithMany(t => t.Marks)
                .HasForeignKey(m => m.TeacherId)
                .WillCascadeOnDelete(false);

            // Настройка связи MarkLog - Mark (M:1)
            modelBuilder.Entity<MarkLog>()
                .HasRequired(ml => ml.Mark)
                .WithMany(m => m.Logs)
                .HasForeignKey(ml => ml.MarkId)
                .WillCascadeOnDelete(false);

            // Настройка связи MarkLog - Teacher (M:1)
            modelBuilder.Entity<MarkLog>()
                .HasRequired(ml => ml.Teacher)
                .WithMany()
                .HasForeignKey(ml => ml.TeacherId)
                .WillCascadeOnDelete(false);
        }
    }
}
