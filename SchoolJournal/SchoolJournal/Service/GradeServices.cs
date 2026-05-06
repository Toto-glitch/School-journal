using SchoolJournal.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SchoolJournal.Service
{
    public class GradeService
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

                context.SaveChanges();

                // Логирование действия
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

                int oldValue = mark.Value;

                context.Marks.Remove(mark);
                context.SaveChanges();

                // Логирование действия
                var log = new MarkLog
                {
                    MarkId = markId,
                    OldValue = oldValue,
                    NewValue = 0,
                    ChangeDate = DateTime.Now,
                    UserId = teacherId,
                    Action = "Удаление оценки"
                };

                context.MarkLogs.Add(log);
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

        public List<Subject> GetTeacherSubjects(int teacherId)
        {
            using (var context = new ApplicationContext())
            {
                var teacher = context.Teachers
                    .Include(t => t.Subjects)
                    .FirstOrDefault(t => t.Id == teacherId);

                return teacher?.Subjects.ToList() ?? new List<Subject>();
            }
        }

        public List<Student> GetStudentsBySubject(int subjectId)
        {
            using (var context = new ApplicationContext())
            {
                var subject = context.Subjects
                    .Include(s => s.Classes)
                    .Include(s => s.Classes.Select(c => c.Students))
                    .FirstOrDefault(s => s.Id == subjectId);

                var students = new List<Student>();
                if (subject?.Classes != null)
                {
                    foreach (var group in subject.Classes)
                    {
                        students.AddRange(group.Students);
                    }
                }

                return students.Distinct().ToList();
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

        public List<Student> GetAllStudents()
        {
            using (var context = new ApplicationContext())
            {
                return context.Students
                    .Include(s => s.Group)
                    .Include(s => s.User)
                    .ToList();
            }
        }

        public List<Teacher> GetAllTeachers()
        {
            using (var context = new ApplicationContext())
            {
                return context.Teachers
                    .Include(t => t.User)
                    .Include(t => t.Subjects)
                    .ToList();
            }
        }

        public List<Subject> GetAllSubjects()
        {
            using (var context = new ApplicationContext())
            {
                return context.Subjects.ToList();
            }
        }

        public List<Group> GetAllGroups()
        {
            using (var context = new ApplicationContext())
            {
                return context.Groups.ToList();
            }
        }

        public List<Parent> GetAllParents()
        {
            using (var context = new ApplicationContext())
            {
                return context.Parents
                    .Include(p => p.User)
                    .Include(p => p.Students)
                    .ToList();
            }
        }

        public void AddStudent(Student student)
        {
            using (var context = new ApplicationContext())
            {
                context.Students.Add(student);
                context.SaveChanges();
            }
        }

        public void UpdateStudent(Student student)
        {
            using (var context = new ApplicationContext())
            {
                context.Entry(student).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteStudent(int studentId)
        {
            using (var context = new ApplicationContext())
            {
                var student = context.Students.FirstOrDefault(s => s.Id == studentId);
                if (student != null)
                {
                    context.Students.Remove(student);
                    context.SaveChanges();
                }
            }
        }

        public void AddTeacher(Teacher teacher)
        {
            using (var context = new ApplicationContext())
            {
                context.Teachers.Add(teacher);
                context.SaveChanges();
            }
        }

        public void UpdateTeacher(Teacher teacher)
        {
            using (var context = new ApplicationContext())
            {
                context.Entry(teacher).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteTeacher(int teacherId)
        {
            using (var context = new ApplicationContext())
            {
                var teacher = context.Teachers.FirstOrDefault(t => t.Id == teacherId);
                if (teacher != null)
                {
                    context.Teachers.Remove(teacher);
                    context.SaveChanges();
                }
            }
        }

        public void AddSubject(Subject subject)
        {
            using (var context = new ApplicationContext())
            {
                context.Subjects.Add(subject);
                context.SaveChanges();
            }
        }

        public void UpdateSubject(Subject subject)
        {
            using (var context = new ApplicationContext())
            {
                context.Entry(subject).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteSubject(int subjectId)
        {
            using (var context = new ApplicationContext())
            {
                var subject = context.Subjects.FirstOrDefault(s => s.Id == subjectId);
                if (subject != null)
                {
                    context.Subjects.Remove(subject);
                    context.SaveChanges();
                }
            }
        }

        public void AddParent(Parent parent)
        {
            using (var context = new ApplicationContext())
            {
                context.Parents.Add(parent);
                context.SaveChanges();
            }
        }

        public void UpdateParent(Parent parent)
        {
            using (var context = new ApplicationContext())
            {
                context.Entry(parent).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteParent(int parentId)
        {
            using (var context = new ApplicationContext())
            {
                var parent = context.Parents.FirstOrDefault(p => p.Id == parentId);
                if (parent != null)
                {
                    context.Parents.Remove(parent);
                    context.SaveChanges();
                }
            }
        }

        public void AddUser(User user)
        {
            using (var context = new ApplicationContext())
            {
                context.Users.Add(user);
                context.SaveChanges();
            }
        }

        public void UpdateUser(User user)
        {
            using (var context = new ApplicationContext())
            {
                context.Entry(user).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteUser(int userId)
        {
            using (var context = new ApplicationContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Id == userId);
                if (user != null)
                {
                    context.Users.Remove(user);
                    context.SaveChanges();
                }
            }
        }

        public void AddGroup(Group group)
        {
            using (var context = new ApplicationContext())
            {
                context.Groups.Add(group);
                context.SaveChanges();
            }
        }

        public void UpdateGroup(Group group)
        {
            using (var context = new ApplicationContext())
            {
                context.Entry(group).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteGroup(int groupId)
        {
            using (var context = new ApplicationContext())
            {
                var group = context.Groups.FirstOrDefault(g => g.Id == groupId);
                if (group != null)
                {
                    context.Groups.Remove(group);
                    context.SaveChanges();
                }
            }
        }
    }
}