using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SchoolJournal.Model
{
    public class DatabaseInitializer : CreateDatabaseIfNotExists<ApplicationContext>
    {
        protected override void Seed(ApplicationContext context)
        {
            base.Seed(context);

            // 1. Пользователи
            var users = new List<User>
            {
                new User { Username = "director", PasswordHash = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92", PhoneNumber = "+79001000001", Email = "director@school.ru", Role = UserRole.Director },
                new User { Username = "teacher_ivanov", PasswordHash = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92", PhoneNumber = "+79001000002", Email = "ivanov@school.ru", Role = UserRole.Teacher },
                new User { Username = "teacher_petrova", PasswordHash = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92", PhoneNumber = "+79001000003", Email = "petrova@school.ru", Role = UserRole.Teacher },
                new User { Username = "teacher_sidorov", PasswordHash = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92", PhoneNumber = "+79001000004", Email = "sidorov@school.ru", Role = UserRole.Teacher },
                new User { Username = "student_smirnov", PasswordHash = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92", PhoneNumber = "+79001000005", Email = "smirnov@school.ru", Role = UserRole.Student },
                new User { Username = "student_kuznetsova", PasswordHash = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92", PhoneNumber = "+79001000006", Email = "kuznetsova@school.ru", Role = UserRole.Student },
                new User { Username = "student_popov", PasswordHash = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92", PhoneNumber = "+79001000007", Email = "popov@school.ru", Role = UserRole.Student },
                new User { Username = "student_sokolova", PasswordHash = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92", PhoneNumber = "+79001000008", Email = "sokolova@school.ru", Role = UserRole.Student },
                new User { Username = "student_volkov", PasswordHash = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92", PhoneNumber = "+79001000009", Email = "volkov@school.ru", Role = UserRole.Student },
                new User { Username = "student_morozova", PasswordHash = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92", PhoneNumber = "+79001000010", Email = "morozova@school.ru", Role = UserRole.Student },
                new User { Username = "parent_smirnova", PasswordHash = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92", PhoneNumber = "+79001000011", Email = "smirnova_parent@mail.ru", Role = UserRole.Parent },
                new User { Username = "parent_kuznetsov", PasswordHash = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92", PhoneNumber = "+79001000012", Email = "kuznetsov_parent@mail.ru", Role = UserRole.Parent },
                new User { Username = "parent_popova", PasswordHash = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92", PhoneNumber = "+79001000013", Email = "popova_parent@mail.ru", Role = UserRole.Parent },
                new User { Username = "parent_sokolov", PasswordHash = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92", PhoneNumber = "+79001000014", Email = "sokolov_parent@mail.ru", Role = UserRole.Parent },
            };
            context.Users.AddRange(users);
            context.SaveChanges();

            // 2. Группы
            var groups = new List<Group>
            {
                new Group { Title = "5А" }, new Group { Title = "5Б" },
                new Group { Title = "6А" }, new Group { Title = "6Б" },
                new Group { Title = "7А" }, new Group { Title = "7Б" }
            };
            context.Groups.AddRange(groups);
            context.SaveChanges();

            // 3. Учителя
            var teachers = new List<Teacher>
            {
                new Teacher { LastName = "Иванов", FirstName = "Александр", FatherName = "Петрович", UserId = 2 },
                new Teacher { LastName = "Петрова", FirstName = "Елена", FatherName = "Сергеевна", UserId = 3 },
                new Teacher { LastName = "Сидоров", FirstName = "Дмитрий", FatherName = "Алексеевич", UserId = 4 }
            };
            context.Teachers.AddRange(teachers);
            context.SaveChanges();

            // 4. Студенты
            var students = new List<Student>
            {
                new Student { LastName = "Смирнов", FirstName = "Артем", FatherName = "Александрович", GroupId = 1, UserId = 5 },
                new Student { LastName = "Кузнецова", FirstName = "Мария", FatherName = "Дмитриевна", GroupId = 1, UserId = 6 },
                new Student { LastName = "Попов", FirstName = "Иван", FatherName = "Сергеевич", GroupId = 2, UserId = 7 },
                new Student { LastName = "Соколова", FirstName = "Анна", FatherName = "Игоревна", GroupId = 2, UserId = 8 },
                new Student { LastName = "Волков", FirstName = "Максим", FatherName = "Андреевич", GroupId = 3, UserId = 9 },
                new Student { LastName = "Морозова", FirstName = "Дарья", FatherName = "Владимировна", GroupId = 3, UserId = 10 }
            };
            context.Students.AddRange(students);
            context.SaveChanges();

            // 5. Родители
            var parents = new List<Parent>
            {
                new Parent { LastName = "Смирнова", FirstName = "Ольга", FatherName = "Викторовна", UserId = 11 },
                new Parent { LastName = "Кузнецов", FirstName = "Дмитрий", FatherName = "Александрович", UserId = 12 },
                new Parent { LastName = "Попова", FirstName = "Наталья", FatherName = "Сергеевна", UserId = 13 },
                new Parent { LastName = "Соколов", FirstName = "Игорь", FatherName = "Петрович", UserId = 14 }
            };
            context.Parents.AddRange(parents);
            context.SaveChanges();

            // 6. Предметы
            var subjects = new List<Subject>
            {
                new Subject { Title = "Математика" }, new Subject { Title = "Русский язык" },
                new Subject { Title = "Литература" }, new Subject { Title = "Физика" },
                new Subject { Title = "Информатика" }, new Subject { Title = "История" },
                new Subject { Title = "Биология" }, new Subject { Title = "Химия" },
                new Subject { Title = "Английский язык" }, new Subject { Title = "Физкультура" }
            };
            context.Subjects.AddRange(subjects);
            context.SaveChanges();

            // 7. Связи Student-Parent (many-to-many)
            var student1 = context.Students.First(s => s.Id == 1);
            var student2 = context.Students.First(s => s.Id == 2);
            var student3 = context.Students.First(s => s.Id == 3);
            var student4 = context.Students.First(s => s.Id == 4);
            var parent1 = context.Parents.First(p => p.Id == 1);
            var parent2 = context.Parents.First(p => p.Id == 2);
            var parent3 = context.Parents.First(p => p.Id == 3);
            var parent4 = context.Parents.First(p => p.Id == 4);

            student1.Parents.Add(parent1); student1.Parents.Add(parent2);
            student2.Parents.Add(parent2); student2.Parents.Add(parent1);
            student3.Parents.Add(parent3);
            student4.Parents.Add(parent4);

            // 8. Связи Teacher-Subject
            context.Teachers.Find(1).Subjects.Add(context.Subjects.Find(1)); // Иванов - Математика
            context.Teachers.Find(1).Subjects.Add(context.Subjects.Find(5)); // Иванов - Информатика
            context.Teachers.Find(2).Subjects.Add(context.Subjects.Find(2)); // Петрова - Русский язык
            context.Teachers.Find(2).Subjects.Add(context.Subjects.Find(3)); // Петрова - Литература
            context.Teachers.Find(3).Subjects.Add(context.Subjects.Find(4)); // Сидоров - Физика
            context.Teachers.Find(3).Subjects.Add(context.Subjects.Find(1)); // Сидоров - Математика
            context.Teachers.Find(3).Subjects.Add(context.Subjects.Find(5)); // Сидоров - Информатика

            // 9. Связи Subject-Group (many-to-many)
            AddSubjectToGroups(context, 1, new[] { 1, 2, 3 });   // Математика
            AddSubjectToGroups(context, 2, new[] { 1, 2, 3 });   // Русский язык
            AddSubjectToGroups(context, 3, new[] { 1, 2 });      // Литература
            AddSubjectToGroups(context, 4, new[] { 1, 2, 3 });   // Физика
            AddSubjectToGroups(context, 5, new[] { 3 });         // Информатика
            AddSubjectToGroups(context, 6, new[] { 1, 2 });      // История
            AddSubjectToGroups(context, 7, new[] { 3 });         // Биология
            AddSubjectToGroups(context, 8, new[] { 3 });         // Химия
            AddSubjectToGroups(context, 9, new[] { 1, 2, 3 });   // Английский
            AddSubjectToGroups(context, 10, new[] { 1, 2, 3 });  // Физкультура

            context.SaveChanges();

            // 10. Оценки (Marks)
            var marks = new List<Mark>
            {
                new Mark { Value = 5, Date = DateTime.Parse("2026-05-01"), StudentId = 1, SubjectId = 1, TeacherId = 1 },
                new Mark { Value = 4, Date = DateTime.Parse("2026-05-02"), StudentId = 1, SubjectId = 2, TeacherId = 2 },
                new Mark { Value = 5, Date = DateTime.Parse("2026-05-03"), StudentId = 1, SubjectId = 5, TeacherId = 1 },
                new Mark { Value = 3, Date = DateTime.Parse("2026-05-04"), StudentId = 1, SubjectId = 4, TeacherId = 3 },
                new Mark { Value = 4, Date = DateTime.Parse("2026-05-01"), StudentId = 2, SubjectId = 1, TeacherId = 1 },
                new Mark { Value = 5, Date = DateTime.Parse("2026-05-02"), StudentId = 2, SubjectId = 2, TeacherId = 2 },
                new Mark { Value = 4, Date = DateTime.Parse("2026-05-03"), StudentId = 2, SubjectId = 3, TeacherId = 2 },
                new Mark { Value = 5, Date = DateTime.Parse("2026-05-04"), StudentId = 2, SubjectId = 6, TeacherId = 1 },
                new Mark { Value = 3, Date = DateTime.Parse("2026-05-01"), StudentId = 3, SubjectId = 1, TeacherId = 1 },
                new Mark { Value = 4, Date = DateTime.Parse("2026-05-02"), StudentId = 3, SubjectId = 2, TeacherId = 2 },
                new Mark { Value = 5, Date = DateTime.Parse("2026-05-03"), StudentId = 3, SubjectId = 4, TeacherId = 3 },
                new Mark { Value = 2, Date = DateTime.Parse("2026-05-04"), StudentId = 3, SubjectId = 1, TeacherId = 1 },
                new Mark { Value = 5, Date = DateTime.Parse("2026-05-01"), StudentId = 4, SubjectId = 2, TeacherId = 2 },
                new Mark { Value = 5, Date = DateTime.Parse("2026-05-02"), StudentId = 4, SubjectId = 3, TeacherId = 2 },
                new Mark { Value = 4, Date = DateTime.Parse("2026-05-03"), StudentId = 4, SubjectId = 5, TeacherId = 1 },
                new Mark { Value = 5, Date = DateTime.Parse("2026-05-04"), StudentId = 4, SubjectId = 6, TeacherId = 3 },
                new Mark { Value = 4, Date = DateTime.Parse("2026-05-01"), StudentId = 5, SubjectId = 1, TeacherId = 1 },
                new Mark { Value = 3, Date = DateTime.Parse("2026-05-02"), StudentId = 5, SubjectId = 4, TeacherId = 3 },
                new Mark { Value = 4, Date = DateTime.Parse("2026-05-03"), StudentId = 5, SubjectId = 5, TeacherId = 1 },
                new Mark { Value = 5, Date = DateTime.Parse("2026-05-04"), StudentId = 5, SubjectId = 7, TeacherId = 3 },
                new Mark { Value = 5, Date = DateTime.Parse("2026-05-01"), StudentId = 6, SubjectId = 1, TeacherId = 1 },
                new Mark { Value = 4, Date = DateTime.Parse("2026-05-02"), StudentId = 6, SubjectId = 4, TeacherId = 3 },
                new Mark { Value = 5, Date = DateTime.Parse("2026-05-03"), StudentId = 6, SubjectId = 7, TeacherId = 3 },
                new Mark { Value = 4, Date = DateTime.Parse("2026-05-04"), StudentId = 6, SubjectId = 8, TeacherId = 3 }
            };
            context.Marks.AddRange(marks);
            context.SaveChanges();
        }

        private void AddSubjectToGroups(ApplicationContext context, int subjectId, int[] groupIds)
        {
            var subject = context.Subjects.Find(subjectId);
            foreach (var gid in groupIds)
                subject.Classes.Add(context.Groups.Find(gid));
        }
    }
}