using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SchoolDiary.Models;

namespace SchoolDiary.Services
{
    /// <summary>
    /// Сервис для работы с данными учеников
    /// </summary>
    public class StudentService
    {
        private readonly SchoolDbContext _context;

        public StudentService(SchoolDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получение всех учеников
        /// </summary>
        public List<Student> GetAllStudents()
        {
            return _context.Students
                .Include(s => s.Parent)
                .Include(s => s.Marks)
                .ToList();
        }

        /// <summary>
        /// Получение ученика по ID
        /// </summary>
        public Student GetStudentById(int studentId)
        {
            return _context.Students
                .Include(s => s.Parent)
                .Include(s => s.Marks.Select(m => m.Subject))
                .Include(s => s.Marks.Select(m => m.Teacher))
                .FirstOrDefault(s => s.Id == studentId);
        }

        /// <summary>
        /// Получение ученика по родителю
        /// </summary>
        public Student GetStudentByParentId(int parentId)
        {
            return _context.Students
                .Include(s => s.Marks.Select(m => m.Subject))
                .Include(s => s.Marks.Select(m => m.Teacher))
                .FirstOrDefault(s => s.ParentId == parentId);
        }

        /// <summary>
        /// Получение всех предметов
        /// </summary>
        public List<Subject> GetAllSubjects()
        {
            return _context.Subjects
                .Include(s => s.Teacher)
                .ToList();
        }

        /// <summary>
        /// Получение предмета по ID
        /// </summary>
        public Subject GetSubjectById(int subjectId)
        {
            return _context.Subjects
                .Include(s => s.Teacher)
                .FirstOrDefault(s => s.Id == subjectId);
        }

        /// <summary>
        /// Получение всех учителей
        /// </summary>
        public List<Teacher> GetAllTeachers()
        {
            return _context.Teachers
                .Include(t => t.Subject)
                .ToList();
        }

        /// <summary>
        /// Получение учителя по ID
        /// </summary>
        public Teacher GetTeacherById(int teacherId)
        {
            return _context.Teachers
                .Include(t => t.Subject)
                .FirstOrDefault(t => t.Id == teacherId);
        }

        /// <summary>
        /// Получение всех родителей
        /// </summary>
        public List<Parent> GetAllParents()
        {
            return _context.Parents
                .Include(p => p.Student)
                .ToList();
        }

        /// <summary>
        /// Получение родителя по ID
        /// </summary>
        public Parent GetParentById(int parentId)
        {
            return _context.Parents
                .Include(p => p.Student)
                .FirstOrDefault(p => p.Id == parentId);
        }

        /// <summary>
        /// Добавление ученика
        /// </summary>
        public Student AddStudent(string fullName, DateTime birthDate, string className, int? parentId = null)
        {
            var student = new Student
            {
                FullName = fullName,
                BirthDate = birthDate,
                Class = className,
                ParentId = parentId,
                CreatedAt = DateTime.Now
            };

            _context.Students.Add(student);
            _context.SaveChanges();

            return student;
        }

        /// <summary>
        /// Обновление ученика
        /// </summary>
        public Student UpdateStudent(int studentId, string fullName, string className)
        {
            var student = _context.Students.FirstOrDefault(s => s.Id == studentId);
            if (student == null)
                throw new Exception("Ученик не найден");

            student.FullName = fullName;
            student.Class = className;

            _context.SaveChanges();

            return student;
        }

        /// <summary>
        /// Удаление ученика
        /// </summary>
        public void DeleteStudent(int studentId)
        {
            var student = _context.Students.FirstOrDefault(s => s.Id == studentId);
            if (student == null)
                throw new Exception("Ученик не найден");

            _context.Students.Remove(student);
            _context.SaveChanges();
        }
    }
}
