using SchoolJournal.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SchoolJournal.Service
{
    public class GradeService
    {
        public List<Mark> GetStudentMarks(int studentId)
        {
            using (var db = new ApplicationContext())
            {
                return db.Marks
                    .Include(m => m.Subject)
                    .Where(m => m.StudentId == studentId)
                    .OrderBy(m => m.Date)
                    .ToList();
            }
        }

        public Dictionary<string, double> GetAverageMarks(int studentId)
        {
            using (var db = new ApplicationContext())
            {
                return db.Marks
                    .Include(m => m.Subject)
                    .Where(m => m.StudentId == studentId)
                    .GroupBy(m => m.Subject.Title)
                    .ToDictionary(g => g.Key, g => Math.Round(g.Average(m => m.Value), 2));
            }
        }
    }
}