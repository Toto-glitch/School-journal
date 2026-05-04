using System;
using System.Data.Entity;
using System.Linq;
using SchoolDiary.Models;

namespace SchoolDiary.Data
{
    /// <summary>
    /// Инициализатор базы данных с тестовыми данными
    /// Объем данных: 10 учеников, 5 учителей, 50 оценок, 10 родителей
    /// </summary>
    public class DbInitializer : DropCreateDatabaseIfModelChanges<SchoolDbContext>
    {
        protected override void Seed(SchoolDbContext context)
        {
            // Создание пользователей и учителей (5 учителей)
            var teachers = new[]
            {
                CreateTeacher(context, "Иванов Иван Иванович", "Учитель математики", "Математика", "teacher1", "pass1"),
                CreateTeacher(context, "Петрова Анна Сергеевна", "Учитель русского языка", "Русский язык", "teacher2", "pass2"),
                CreateTeacher(context, "Сидоров Петр Александрович", "Учитель физики", "Физика", "teacher3", "pass3"),
                CreateTeacher(context, "Кузнецова Елена Владимировна", "Учитель истории", "История", "teacher4", "pass4"),
                CreateTeacher(context, "Смирнов Дмитрий Николаевич", "Учитель информатики", "Информатика", "teacher5", "pass5")
            };

            // Создание предметов
            var subjects = context.Subjects.ToList();

            // Создание учеников (10 учеников) и родителей (10 родителей)
            var students = new[]
            {
                CreateStudentWithParent(context, "Алексеев Алексей Алексеевич", new DateTime(2008, 5, 15), "9А", "Алексеев Алексей Петрович", "+79001111101", "parent1", "pass1"),
                CreateStudentWithParent(context, "Борисова Бориса Борисовна", new DateTime(2008, 6, 20), "9А", "Борисова Мария Ивановна", "+79001111102", "parent2", "pass2"),
                CreateStudentWithParent(context, "Васильев Василий Васильевич", new DateTime(2008, 7, 10), "9А", "Васильев Василий Петрович", "+79001111103", "parent3", "pass3"),
                CreateStudentWithParent(context, "Григорьева Григория Григорьевна", new DateTime(2008, 8, 5), "9Б", "Григорьева Елена Сергеевна", "+79001111104", "parent4", "pass4"),
                CreateStudentWithParent(context, "Дмитриев Дмитрий Дмитриевич", new DateTime(2008, 9, 12), "9Б", "Дмитриев Дмитрий Иванович", "+79001111105", "parent5", "pass5"),
                CreateStudentWithParent(context, "Егорова Егор Егорович", new DateTime(2008, 10, 18), "9Б", "Егорова Ольга Петровна", "+79001111106", "parent6", "pass6"),
                CreateStudentWithParent(context, "Жуков Жука Жукович", new DateTime(2008, 11, 25), "10А", "Жукова Анна Владимировна", "+79001111107", "parent7", "pass7"),
                CreateStudentWithParent(context, "Зайцев Зайка Зайцевич", new DateTime(2008, 12, 30), "10А", "Зайцева Наталья Игоревна", "+79001111108", "parent8", "pass8"),
                CreateStudentWithParent(context, "Иванова Иванка Ивановна", new DateTime(2009, 1, 8), "10А", "Иванов Иван Ильич", "+79001111109", "parent9", "pass9"),
                CreateStudentWithParent(context, "Козлов Козел Козлович", new DateTime(2009, 2, 14), "10Б", "Козлова Светлана Михайловна", "+79001111110", "parent10", "pass10")
            };

            // Создание директора
            CreateUser(context, "director", "directorpass", "Director");

            // Создание оценок (50 оценок)
            CreateMarks(context, students, subjects, teachers);

            context.SaveChanges();

            base.Seed(context);
        }

        private Teacher CreateTeacher(SchoolDbContext context, string fullName, string position, string subjectName, string login, string password)
        {
            var user = new User
            {
                Login = login,
                PasswordHash = HashPassword(password),
                Role = "Teacher",
                CreatedAt = DateTime.Now
            };
            context.Users.Add(user);
            context.SaveChanges();

            var subject = new Subject
            {
                Name = subjectName,
                CreatedAt = DateTime.Now
            };
            context.Subjects.Add(subject);
            context.SaveChanges();

            var teacher = new Teacher
            {
                FullName = fullName,
                Position = position,
                UserId = user.Id,
                SubjectId = subject.Id,
                CreatedAt = DateTime.Now
            };
            context.Teachers.Add(teacher);
            context.SaveChanges();

            // Обновляем связь предмета с учителем
            subject.TeacherId = teacher.Id;
            context.SaveChanges();

            return teacher;
        }

        private Student CreateStudentWithParent(SchoolDbContext context, string fullName, DateTime birthDate, string className, 
            string parentFullName, string phone, string parentLogin, string parentPassword)
        {
            // Создаём родителя
            var parentUser = new User
            {
                Login = parentLogin,
                PasswordHash = HashPassword(parentPassword),
                Role = "Parent",
                CreatedAt = DateTime.Now
            };
            context.Parents.Add(new Parent 
            { 
                FullName = parentFullName, 
                Phone = phone,
                User = parentUser,
                CreatedAt = DateTime.Now
            });
            context.SaveChanges();

            var parent = context.Parents.FirstOrDefault(p => p.User.Login == parentLogin);

            // Создаём ученика
            var student = new Student
            {
                FullName = fullName,
                BirthDate = birthDate,
                Class = className,
                ParentId = parent.Id,
                CreatedAt = DateTime.Now
            };
            context.Students.Add(student);
            context.SaveChanges();

            // Обновляем связь родителя со студентом (1:1)
            parent.StudentId = student.Id;
            context.SaveChanges();

            return student;
        }

        private void CreateUser(SchoolDbContext context, string login, string password, string role)
        {
            var user = new User
            {
                Login = login,
                PasswordHash = HashPassword(password),
                Role = role,
                CreatedAt = DateTime.Now
            };
            context.Users.Add(user);
            context.SaveChanges();
        }

        private void CreateMarks(SchoolDbContext context, Student[] students, System.Collections.Generic.List<Subject> subjects, Teacher[] teachers)
        {
            var random = new Random(42); // Фиксированный seed для воспроизводимости
            int marksCreated = 0;

            // Распределяем оценки по ученикам и предметам
            foreach (var student in students)
            {
                foreach (var subject in subjects)
                {
                    // Каждый ученик получает 1-2 оценки по каждому предмету
                    int marksCount = random.Next(1, 3);
                    
                    for (int i = 0; i < marksCount && marksCreated < 50; i++)
                    {
                        var mark = new Mark
                        {
                            StudentId = student.Id,
                            SubjectId = subject.Id,
                            TeacherId = subject.TeacherId.Value,
                            Value = random.Next(3, 6), // Оценки от 3 до 5 (хорошие для демонстрации)
                            Date = DateTime.Now.AddDays(-random.Next(0, 30)),
                            Comment = GetRandomComment(random),
                            CreatedAt = DateTime.Now
                        };
                        context.Marks.Add(mark);
                        marksCreated++;
                    }
                }
            }

            context.SaveChanges();
        }

        private string GetRandomComment(Random random)
        {
            var comments = new[]
            {
                "Хорошая работа",
                "Можно лучше",
                "Отлично!",
                "Недостаточно проработано",
                "Превосходно",
                "Есть ошибки",
                "Молодец",
                "Требуется повторение"
            };
            return comments[random.Next(comments.Length)];
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                var builder = new System.Text.StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
