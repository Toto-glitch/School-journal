using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using SchoolDiary.Models;

namespace SchoolDiary.Services
{
    /// <summary>
    /// Сервис для работы с оценками (выставление, изменение, логирование)
    /// Реализует процедуру выставления оценки и триггер логирования
    /// </summary>
    public class GradeService
    {
        private readonly SchoolDbContext _context;
        private readonly AuthService _authService;

        public GradeService(SchoolDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        /// <summary>
        /// Процедура выставления оценки
        /// Ограничение: учитель может выставлять оценки только по своему предмету
        /// Ограничение: оценки только в диапазоне от 1 до 5
        /// </summary>
        public Mark SetGrade(int studentId, int subjectId, int value, int teacherId, string comment = null)
        {
            // Проверка диапазона оценок
            if (value < 1 || value > 5)
                throw new ArgumentOutOfRangeException(nameof(value), "Оценка должна быть от 1 до 5");

            // Получаем учителя
            var teacher = _context.Teachers.FirstOrDefault(t => t.Id == teacherId);
            if (teacher == null)
                throw new Exception("Учитель не найден");

            // Проверка: учитель может выставлять оценки только по своему предмету
            if (teacher.SubjectId != subjectId)
                throw new Exception("Учитель может выставлять оценки только по своему предмету");

            // Проверяем существование ученика
            var student = _context.Students.FirstOrDefault(s => s.Id == studentId);
            if (student == null)
                throw new Exception("Ученик не найден");

            // Проверяем существование предмета
            var subject = _context.Subjects.FirstOrDefault(s => s.Id == subjectId);
            if (subject == null)
                throw new Exception("Предмет не найден");

            // Создаём новую оценку
            var mark = new Mark
            {
                StudentId = studentId,
                SubjectId = subjectId,
                TeacherId = teacherId,
                Value = value,
                Comment = comment,
                Date = DateTime.Now,
                CreatedAt = DateTime.Now
            };

            _context.Marks.Add(mark);
            _context.SaveChanges();

            // Логирование (имитация триггера)
            LogMarkChange(mark, 0, value, "Insert", teacherId, comment);

            return mark;
        }

        /// <summary>
        /// Процедура изменения оценки
        /// </summary>
        public Mark UpdateGrade(int markId, int newValue, int teacherId, string comment = null)
        {
            // Проверка диапазона оценок
            if (newValue < 1 || newValue > 5)
                throw new ArgumentOutOfRangeException(nameof(newValue), "Оценка должна быть от 1 до 5");

            var mark = _context.Marks
                .Include(m => m.Subject)
                .FirstOrDefault(m => m.Id == markId);

            if (mark == null)
                throw new Exception("Оценка не найдена");

            // Проверка: учитель может изменять оценки только по своему предмету
            if (mark.Subject.TeacherId != teacherId)
                throw new Exception("Учитель может изменять оценки только по своему предмету");

            int oldValue = mark.Value;

            // Обновляем оценку
            mark.Value = newValue;
            mark.Comment = comment ?? mark.Comment;
            mark.UpdatedAt = DateTime.Now;

            _context.SaveChanges();

            // Логирование (имитация триггера)
            LogMarkChange(mark, oldValue, newValue, "Update", teacherId, comment);

            return mark;
        }

        /// <summary>
        /// Удаление оценки
        /// </summary>
        public void DeleteGrade(int markId, int teacherId)
        {
            var mark = _context.Marks
                .Include(m => m.Subject)
                .FirstOrDefault(m => m.Id == markId);

            if (mark == null)
                throw new Exception("Оценка не найдена");

            // Проверка: учитель может удалять оценки только по своему предмету
            if (mark.Subject.TeacherId != teacherId)
                throw new Exception("Учитель может удалять оценки только по своему предмету");

            int oldValue = mark.Value;

            // Логирование перед удалением
            LogMarkChange(mark, oldValue, 0, "Delete", teacherId, "Удаление оценки");

            _context.Marks.Remove(mark);
            _context.SaveChanges();
        }

        /// <summary>
        /// Логирование изменений оценки (имитация триггера БД)
        /// </summary>
        private void LogMarkChange(Mark mark, int oldValue, int newValue, string action, int teacherId, string comment)
        {
            var log = new MarkLog
            {
                MarkId = mark.Id,
                OldValue = oldValue,
                NewValue = newValue,
                Action = action,
                TeacherId = teacherId,
                Comment = comment,
                ChangedAt = DateTime.Now
            };

            _context.MarkLogs.Add(log);
            _context.SaveChanges();
        }

        /// <summary>
        /// Запрос: средний балл по предмету
        /// </summary>
        public double GetAverageMarkBySubject(int subjectId)
        {
            var marks = _context.Marks
                .Where(m => m.SubjectId == subjectId)
                .ToList();

            if (!marks.Any())
                return 0;

            return Math.Round(marks.Average(m => m.Value), 2);
        }

        /// <summary>
        /// Запрос: средний балл ученика по всем предметам
        /// </summary>
        public double GetAverageMarkByStudent(int studentId)
        {
            var marks = _context.Marks
                .Where(m => m.StudentId == studentId)
                .ToList();

            if (!marks.Any())
                return 0;

            return Math.Round(marks.Average(m => m.Value), 2);
        }

        /// <summary>
        /// Подзапрос: топ-3 лучших ученика по среднему баллу
        /// </summary>
        public List<Student> GetTop3Students()
        {
            var topStudents = _context.Students
                .Select(s => new
                {
                    Student = s,
                    AverageMark = s.Marks.Any() ? s.Marks.Average(m => m.Value) : 0
                })
                .OrderByDescending(x => x.AverageMark)
                .Take(3)
                .Select(x => x.Student)
                .ToList();

            return topStudents;
        }

        /// <summary>
        /// Получение всех оценок ученика
        /// </summary>
        public List<Mark> GetMarksByStudent(int studentId)
        {
            return _context.Marks
                .Include(m => m.Subject)
                .Include(m => m.Teacher)
                .Where(m => m.StudentId == studentId)
                .OrderByDescending(m => m.Date)
                .ToList();
        }

        /// <summary>
        /// Получение оценок ученика по предмету
        /// </summary>
        public List<Mark> GetMarksByStudentAndSubject(int studentId, int subjectId)
        {
            return _context.Marks
                .Include(m => m.Subject)
                .Include(m => m.Teacher)
                .Where(m => m.StudentId == studentId && m.SubjectId == subjectId)
                .OrderByDescending(m => m.Date)
                .ToList();
        }

        /// <summary>
        /// Получение всех оценок по предмету
        /// </summary>
        public List<Mark> GetMarksBySubject(int subjectId)
        {
            return _context.Marks
                .Include(m => m.Student)
                .Include(m => m.Teacher)
                .Where(m => m.SubjectId == subjectId)
                .OrderByDescending(m => m.Date)
                .ToList();
        }

        /// <summary>
        /// Получение логов изменений для оценки
        /// </summary>
        public List<MarkLog> GetMarkLogs(int markId)
        {
            return _context.MarkLogs
                .Include(ml => ml.Teacher)
                .Where(ml => ml.MarkId == markId)
                .OrderByDescending(ml => ml.ChangedAt)
                .ToList();
        }
    }
}
