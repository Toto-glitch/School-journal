using SchoolJournal.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SchoolJournal.Service
{
    public class SubjectGroupService
    {
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

        public List<Subject> GetAllSubjects()
        {
            using (var context = new ApplicationContext())
            {
                return context.Subjects
                    .Include(s => s.Classes)
                    .Include(s => s.Teachers)
                    .ToList();
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
                var s = context.Subjects.FirstOrDefault(x => x.Id == subject.Id);
                if (s == null)
                    throw new Exception("Предмет не найден");

                s.Title = subject.Title;
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

        public List<Group> GetAllGroups()
        {
            using (var context = new ApplicationContext())
            {
                return context.Groups
                    .Include(g => g.Students)
                    .ToList();
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
                var g = context.Groups.FirstOrDefault(x => x.Id == group.Id);
                if (g == null)
                    throw new Exception("Группа не найдена");

                g.Title = group.Title;
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