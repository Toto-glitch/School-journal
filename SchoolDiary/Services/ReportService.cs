using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SchoolDiary.Models;

namespace SchoolDiary.Services
{
    /// <summary>
    /// Сервис для формирования отчётов и статистики
    /// </summary>
    public class ReportService
    {
        private readonly SchoolDbContext _context;
        private readonly GradeService _gradeService;

        public ReportService(SchoolDbContext context, GradeService gradeService)
        {
            _context = context;
            _gradeService = gradeService;
        }

        /// <summary>
        /// Отчёт: успеваемость по классу
        /// </summary>
        public ClassReport GetClassReport(string className)
        {
            var students = _context.Students
                .Where(s => s.Class == className)
                .Include(s => s.Marks)
                .ToList();

            var report = new ClassReport
            {
                ClassName = className,
                TotalStudents = students.Count,
                Students = new List<StudentReport>()
            };

            foreach (var student in students)
            {
                var avgMark = student.Marks.Any() 
                    ? Math.Round(student.Marks.Average(m => m.Value), 2) 
                    : 0;

                report.Students.Add(new StudentReport
                {
                    StudentId = student.Id,
                    FullName = student.FullName,
                    AverageMark = avgMark,
                    TotalMarks = student.Marks.Count
                });
            }

            report.ClassAverage = report.Students.Any()
                ? Math.Round(report.Students.Average(s => s.AverageMark), 2)
                : 0;

            return report;
        }

        /// <summary>
        /// Отчёт: успеваемость по предмету
        /// </summary>
        public SubjectReport GetSubjectReport(int subjectId)
        {
            var subject = _context.Subjects
                .Include(s => s.Teacher)
                .FirstOrDefault(s => s.Id == subjectId);

            if (subject == null)
                throw new Exception("Предмет не найден");

            var marks = _context.Marks
                .Where(m => m.SubjectId == subjectId)
                .Include(m => m.Student)
                .ToList();

            var report = new SubjectReport
            {
                SubjectId = subjectId,
                SubjectName = subject.Name,
                TeacherName = subject.Teacher?.FullName ?? "Нет учителя",
                TotalMarks = marks.Count,
                AverageMark = marks.Any() 
                    ? Math.Round(marks.Average(m => m.Value), 2) 
                    : 0,
                MarksByValue = new Dictionary<int, int>()
            };

            // Группировка оценок по значению
            for (int i = 1; i <= 5; i++)
            {
                report.MarksByValue[i] = marks.Count(m => m.Value == i);
            }

            return report;
        }

        /// <summary>
        /// Отчёт: топ учеников школы
        /// </summary>
        public List<TopStudentReport> GetTopStudentsReport(int count = 10)
        {
            var students = _context.Students
                .Include(s => s.Marks)
                .Include(s => s.Class)
                .Select(s => new
                {
                    s.Id,
                    s.FullName,
                    s.Class,
                    AverageMark = s.Marks.Any() ? s.Marks.Average(m => m.Value) : 0,
                    TotalMarks = s.Marks.Count
                })
                .OrderByDescending(s => s.AverageMark)
                .Take(count)
                .ToList();

            return students.Select(s => new TopStudentReport
            {
                StudentId = s.Id,
                FullName = s.FullName,
                Class = s.Class,
                AverageMark = Math.Round(s.AverageMark, 2),
                TotalMarks = s.TotalMarks
            }).ToList();
        }

        /// <summary>
        /// Отчёт: активность учителей (количество выставленных оценок)
        /// </summary>
        public List<TeacherActivityReport> GetTeacherActivityReport()
        {
            var teachers = _context.Teachers
                .Include(t => t.Subject)
                .Include(t => t.Marks)
                .Select(t => new
                {
                    t.Id,
                    t.FullName,
                    SubjectName = t.Subject != null ? t.Subject.Name : "Нет предмета",
                    TotalMarks = t.Marks.Count,
                    LastMarkDate = t.Marks.Any() ? t.Marks.Max(m => m.Date) : (DateTime?)null
                })
                .OrderByDescending(t => t.TotalMarks)
                .ToList();

            return teachers.Select(t => new TeacherActivityReport
            {
                TeacherId = t.Id,
                FullName = t.FullName,
                SubjectName = t.SubjectName,
                TotalMarks = t.TotalMarks,
                LastMarkDate = t.LastMarkDate
            }).ToList();
        }

        /// <summary>
        /// Отчёт: журнал оценок по предмету для всех учеников класса
        /// </summary>
        public ClassJournalReport GetClassJournalReport(string className, int subjectId)
        {
            var students = _context.Students
                .Where(s => s.Class == className)
                .OrderBy(s => s.FullName)
                .ToList();

            var subject = _context.Subjects
                .Include(s => s.Teacher)
                .FirstOrDefault(s => s.Id == subjectId);

            var marks = _context.Marks
                .Where(m => m.SubjectId == subjectId && students.Select(s => s.Id).Contains(m.StudentId))
                .Include(m => m.Student)
                .ToList();

            var report = new ClassJournalReport
            {
                ClassName = className,
                SubjectName = subject?.Name ?? "Неизвестный предмет",
                TeacherName = subject?.Teacher?.FullName ?? "Нет учителя",
                Students = new List<StudentJournalEntry>()
            };

            foreach (var student in students)
            {
                var studentMarks = marks.Where(m => m.StudentId == student.Id).ToList();

                report.Students.Add(new StudentJournalEntry
                {
                    StudentId = student.Id,
                    FullName = student.FullName,
                    Marks = studentMarks.Select(m => new MarkEntry
                    {
                        MarkId = m.Id,
                        Value = m.Value,
                        Date = m.Date,
                        Comment = m.Comment
                    }).ToList(),
                    AverageMark = studentMarks.Any()
                        ? Math.Round(studentMarks.Average(m => m.Value), 2)
                        : 0
                });
            }

            return report;
        }

        /// <summary>
        /// Получение всех логов изменений оценок
        /// </summary>
        public List<MarkLog> GetAllMarkLogs(DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = _context.MarkLogs
                .Include(ml => ml.Mark)
                .Include(ml => ml.Teacher)
                .AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(ml => ml.ChangedAt >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(ml => ml.ChangedAt <= toDate.Value);

            return query.OrderByDescending(ml => ml.ChangedAt).ToList();
        }
    }

    #region DTO для отчётов

    public class ClassReport
    {
        public string ClassName { get; set; }
        public int TotalStudents { get; set; }
        public double ClassAverage { get; set; }
        public List<StudentReport> Students { get; set; }
    }

    public class StudentReport
    {
        public int StudentId { get; set; }
        public string FullName { get; set; }
        public double AverageMark { get; set; }
        public int TotalMarks { get; set; }
    }

    public class SubjectReport
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string TeacherName { get; set; }
        public int TotalMarks { get; set; }
        public double AverageMark { get; set; }
        public Dictionary<int, int> MarksByValue { get; set; }
    }

    public class TopStudentReport
    {
        public int StudentId { get; set; }
        public string FullName { get; set; }
        public string Class { get; set; }
        public double AverageMark { get; set; }
        public int TotalMarks { get; set; }
    }

    public class TeacherActivityReport
    {
        public int TeacherId { get; set; }
        public string FullName { get; set; }
        public string SubjectName { get; set; }
        public int TotalMarks { get; set; }
        public DateTime? LastMarkDate { get; set; }
    }

    public class ClassJournalReport
    {
        public string ClassName { get; set; }
        public string SubjectName { get; set; }
        public string TeacherName { get; set; }
        public List<StudentJournalEntry> Students { get; set; }
    }

    public class StudentJournalEntry
    {
        public int StudentId { get; set; }
        public string FullName { get; set; }
        public List<MarkEntry> Marks { get; set; }
        public double AverageMark { get; set; }
    }

    public class MarkEntry
    {
        public int MarkId { get; set; }
        public int Value { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
    }

    #endregion
}
