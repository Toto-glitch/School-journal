using SchoolJournal.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SchoolJournal.Service
{
    public class UserService
    {
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
                var existStudent = context.Students.FirstOrDefault(s => s.Id == student.Id);
                if (existStudent == null)
                    throw new Exception("Ученик не найден");

                existStudent.LastName = student.LastName;
                existStudent.FirstName = student.FirstName;
                existStudent.FatherName = student.FatherName;
                existStudent.GroupId = student.GroupId;
                context.SaveChanges();
            }
        }

        public void DeleteStudent(int studentId)
        {
            using (var context = new ApplicationContext())
            {
                var marks = context.Marks.Where(m => m.StudentId == studentId).ToList();
                context.Marks.RemoveRange(marks);
                var student = context.Students.FirstOrDefault(s => s.Id == studentId);
                if (student != null)
                    context.Students.Remove(student);
                context.SaveChanges();
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
                var t = context.Teachers.FirstOrDefault(x => x.Id == teacher.Id);
                if (t == null)
                    throw new Exception("Учитель не найден");

                t.LastName = teacher.LastName;
                t.FirstName = teacher.FirstName;
                t.FatherName = teacher.FatherName;
                context.SaveChanges();
            }
        }

        public void DeleteTeacher(int teacherId)
        {
            using (var context = new ApplicationContext())
            {
                var marks = context.Marks.Where(m => m.TeacherId == teacherId).ToList();
                var markIds = marks.Select(m => m.Id).ToList();
                var logs = context.MarkLogs.Where(ml => markIds.Contains(ml.MarkId)).ToList();
                context.MarkLogs.RemoveRange(logs);
                context.Marks.RemoveRange(marks);
                var teacher = context.Teachers
                    .Include(t => t.Subjects)
                    .FirstOrDefault(t => t.Id == teacherId);

                if (teacher != null)
                {
                    teacher.Subjects.Clear();
                    context.Teachers.Remove(teacher);
                }
                context.SaveChanges();
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
                var p = context.Parents.FirstOrDefault(x => x.Id == parent.Id);
                if (p == null)
                    throw new Exception("Родитель не найден");

                p.LastName = parent.LastName;
                p.FirstName = parent.FirstName;
                p.FatherName = parent.FatherName;
                context.SaveChanges();
            }
        }

        public void AddStudentParent(int studentId, int parentId)
        {
            using (var context = new ApplicationContext())
            {
                var student = context.Students.Include("Parents").FirstOrDefault(s => s.Id == studentId);
                var parent = context.Parents.Find(parentId);
                if (student != null && parent != null)
                {
                    student.Parents.Add(parent);
                    context.SaveChanges();
                }
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
                var u = context.Users.FirstOrDefault(x => x.Id == user.Id);
                if (u == null)
                    throw new Exception("Пользователь не найден");

                u.Username = user.Username;
                u.Email = user.Email;
                u.PhoneNumber = user.PhoneNumber;
                if (!string.IsNullOrWhiteSpace(user.PasswordHash))
                    u.PasswordHash = user.PasswordHash;
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
    }
}