using SchoolJournal.Model;
using System.Collections.Generic;

namespace SchoolJournal.Service
{
    public class AbsoluteService
    {
        private readonly MarkService _markService;
        private readonly UserService _userService;
        private readonly SubjectGroupService _subjectGroupService;

        public AbsoluteService()
        {   
            _markService = new MarkService();
            _userService = new UserService();
            _subjectGroupService = new SubjectGroupService();
        }

        public void AddMark(int studentId, int subjectId, int teacherId, int value)
            => _markService.AddMark(studentId, subjectId, teacherId, value);

        public void UpdateMark(int markId, int newValue, int teacherId)
            => _markService.UpdateMark(markId, newValue, teacherId);

        public void DeleteMark(int markId, int teacherId)
            => _markService.DeleteMark(markId, teacherId);

        public List<MarkLog> GetTeacherMarkLogs(int teacherId, int count = 50)
            => _markService.GetTeacherMarkLogs(teacherId, count);

        public List<MarkLog> GetAllMarkLogs(int count = 100)
            => _markService.GetAllMarkLogs(count);

        public List<Mark> GetStudentMarksBySubject(int studentId, int subjectId)
            => _markService.GetStudentMarksBySubject(studentId, subjectId);

        public List<Mark> GetStudentMarks(int studentId)
            => _markService.GetStudentMarks(studentId);

        public double GetAverageMarkBySubject(int studentId, int subjectId)
            => _markService.GetAverageMarkBySubject(studentId, subjectId);

        public double GetOverallAverageMark(int studentId)
            => _markService.GetOverallAverageMark(studentId);

        public List<Subject> GetTeacherSubjects(int teacherId)
            => _subjectGroupService.GetTeacherSubjects(teacherId);

        public List<Student> GetStudentsBySubject(int subjectId)
            => _subjectGroupService.GetStudentsBySubject(subjectId);

        public List<Subject> GetAllSubjects()
            => _subjectGroupService.GetAllSubjects();

        public void AddSubject(Subject subject)
            => _subjectGroupService.AddSubject(subject);

        public void UpdateSubject(Subject subject)
            => _subjectGroupService.UpdateSubject(subject);

        public void DeleteSubject(int subjectId)
            => _subjectGroupService.DeleteSubject(subjectId);

        public List<Group> GetAllGroups()
            => _subjectGroupService.GetAllGroups();

        public void AddGroup(Group group)
            => _subjectGroupService.AddGroup(group);

        public void UpdateGroup(Group group)
            => _subjectGroupService.UpdateGroup(group);

        public void DeleteGroup(int groupId)
            => _subjectGroupService.DeleteGroup(groupId);

        public List<Student> GetAllStudents()
            => _userService.GetAllStudents();

        public void AddStudent(Student student)
            => _userService.AddStudent(student);

        public void UpdateStudent(Student student)
            => _userService.UpdateStudent(student);

        public void DeleteStudent(int studentId)
            => _userService.DeleteStudent(studentId);

        public List<Teacher> GetAllTeachers()
            => _userService.GetAllTeachers();

        public void AddTeacher(Teacher teacher)
            => _userService.AddTeacher(teacher);

        public void UpdateTeacher(Teacher teacher)
            => _userService.UpdateTeacher(teacher);

        public void DeleteTeacher(int teacherId)
            => _userService.DeleteTeacher(teacherId);

        public List<Parent> GetAllParents()
            => _userService.GetAllParents();

        public void AddParent(Parent parent)
            => _userService.AddParent(parent);

        public void UpdateParent(Parent parent)
            => _userService.UpdateParent(parent);

        public void AddStudentParent(int studentId, int parentId)
            => _userService.AddStudentParent(studentId, parentId);

        public void DeleteParent(int parentId)
            => _userService.DeleteParent(parentId);

        public void AddUser(User user)
            => _userService.AddUser(user);

        public void UpdateUser(User user)
            => _userService.UpdateUser(user);

        public void DeleteUser(int userId)
            => _userService.DeleteUser(userId);
    }
}