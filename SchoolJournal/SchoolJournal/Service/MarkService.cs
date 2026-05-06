using SchoolJournal.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SchoolJournal.Service
{
    public class MarkService
    {
        public void AddMark(int studentId, int subjectId, int teacherId, int value)
        {
            using (var context = new ApplicationContext())
            {
                var mark = new Mark
                {
                    StudentId = studentId,
                    SubjectId = subjectId,
                    TeacherId = teacherId,
                    Value = value,
                    Date = DateTime.Now
                };

                context.Marks.Add(mark);
                context.SaveChanges();

                // Логирование действия
                var log = new MarkLog
                {
                    MarkId = mark.Id,
                    OldValue = 0,
                    NewValue = value,
                    ChangeDate = DateTime.Now,
                    UserId = teacherId,
                    Action = "Добавление оценки"
                };

                context.MarkLogs.Add(log);
                context.SaveChanges();
            }
        }

        public void UpdateMark(int markId, int newValue, int teacherId)
        {
            using (var context = new ApplicationContext())
            {
                var mark = context.Marks.FirstOrDefault(m => m.Id == markId);
                if (mark == null)
                    throw new Exception("Оценка не найдена");

                int oldValue = mark.Value;
                mark.Value = newValue;

                var log = new MarkLog
                {
                    MarkId = markId,
                    OldValue = oldValue,
                    NewValue = newValue,
                    ChangeDate = DateTime.Now,
                    UserId = teacherId,
                    Action = "Изменение оценки"
                };

                context.MarkLogs.Add(log);
                context.SaveChanges();
            }
        }

        public void DeleteMark(int markId, int teacherId)
        {
            using (var context = new ApplicationContext())
            {
                var mark = context.Marks.FirstOrDefault(m => m.Id == markId);
                if (mark == null)
                    throw new Exception("Оценка не найдена");

                context.Marks.Remove(mark);
                context.SaveChanges();
            }
        }

        public List<MarkLog> GetTeacherMarkLogs(int teacherId, int count = 50)
        {
            using (var context = new ApplicationContext())
            {
                return context.MarkLogs
                    .Include(ml => ml.Mark)
                    .Include(ml => ml.Mark.Student)
                    .Include(ml => ml.Mark.Subject)
                    .Where(ml => ml.UserId == teacherId)
                    .OrderByDescending(ml => ml.ChangeDate)
                    .Take(count)
                    .ToList();
            }
        }

        public List<MarkLog> GetAllMarkLogs(int count = 100)
        {
            using (var context = new ApplicationContext())
            {
                return context.MarkLogs
                    .Include(ml => ml.Mark)
                    .Include(ml => ml.Mark.Student)
                    .Include(ml => ml.Mark.Subject)
                    .Include(ml => ml.User)
                    .OrderByDescending(ml => ml.ChangeDate)
                    .Take(count)
                    .ToList();
            }
        }

        public List<Mark> GetStudentMarksBySubject(int studentId, int subjectId)
        {
            using (var context = new ApplicationContext())
            {
                return context.Marks
                    .Include(m => m.Subject)
                    .Include(m => m.Teacher)
                    .Where(m => m.StudentId == studentId && m.SubjectId == subjectId)
                    .OrderByDescending(m => m.Date)
                    .ToList();
            }
        }

        public List<Mark> GetStudentMarks(int studentId)
        {
            using (var context = new ApplicationContext())
            {
                return context.Marks
                    .Include(m => m.Subject)
                    .Include(m => m.Teacher)
                    .Where(m => m.StudentId == studentId)
                    .OrderByDescending(m => m.Date)
                    .ToList();
            }
        }

        public double GetAverageMarkBySubject(int studentId, int subjectId)
        {
            using (var context = new ApplicationContext())
            {
                var marks = context.Marks
                    .Where(m => m.StudentId == studentId && m.SubjectId == subjectId)
                    .Select(m => m.Value)
                    .ToList();

                if (marks.Count == 0)
                    return 0;

                return Math.Round(marks.Average(), 2);
            }
        }

        public double GetOverallAverageMark(int studentId)
        {
            using (var context = new ApplicationContext())
            {
                var marks = context.Marks
                    .Where(m => m.StudentId == studentId)
                    .Select(m => m.Value)
                    .ToList();

                if (marks.Count == 0)
                    return 0;

                return Math.Round(marks.Average(), 2);
            }
        }
    }
}