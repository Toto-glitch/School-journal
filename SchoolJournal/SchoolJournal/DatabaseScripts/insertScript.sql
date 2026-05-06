USE SchoolJournal;

-- ============================================
-- 1. Пользователи (Users)
-- ============================================
INSERT INTO Users (Username, PasswordHash, PhoneNumber, Email, Role) VALUES
(N'director', N'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', N'+79001000001', N'director@school.ru', 0),
(N'teacher_ivanov', N'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', N'+79001000002', N'ivanov@school.ru', 1),
(N'teacher_petrova', N'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', N'+79001000003', N'petrova@school.ru', 1),
(N'teacher_sidorov', N'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', N'+79001000004', N'sidorov@school.ru', 1),
(N'student_smirnov', N'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', N'+79001000005', N'smirnov@school.ru', 3),
(N'student_kuznetsova', N'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', N'+79001000006', N'kuznetsova@school.ru', 3),
(N'student_popov', N'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', N'+79001000007', N'popov@school.ru', 3),
(N'student_sokolova', N'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', N'+79001000008', N'sokolova@school.ru', 3),
(N'student_volkov', N'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', N'+79001000009', N'volkov@school.ru', 3),
(N'student_morozova', N'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', N'+79001000010', N'morozova@school.ru', 3),
(N'parent_smirnova', N'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', N'+79001000011', N'smirnova_parent@mail.ru', 2),
(N'parent_kuznetsov', N'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', N'+79001000012', N'kuznetsov_parent@mail.ru', 2),
(N'parent_popova', N'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', N'+79001000013', N'popova_parent@mail.ru', 2),
(N'parent_sokolov', N'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', N'+79001000014', N'sokolov_parent@mail.ru', 2);
GO

-- ============================================
-- 2. Группы/Классы (Groups)
-- ============================================
INSERT INTO Groups (Title) VALUES
(N'5А'),
(N'5Б'),
(N'6А'),
(N'6Б'),
(N'7А'),
(N'7Б');
GO

-- ============================================
-- 3. Учителя (Teachers)
-- ============================================
INSERT INTO Teachers (LastName, FirstName, FatherName, UserId) VALUES
(N'Иванов', N'Александр', N'Петрович', 2),
(N'Петрова', N'Елена', N'Сергеевна', 3),
(N'Сидоров', N'Дмитрий', N'Алексеевич', 4);
GO

-- ============================================
-- 4. Студенты (Students)
-- ============================================
INSERT INTO Students (LastName, FirstName, FatherName, GroupId, UserId) VALUES
(N'Смирнов', N'Артем', N'Александрович', 1, 5),
(N'Кузнецова', N'Мария', N'Дмитриевна', 1, 6),
(N'Попов', N'Иван', N'Сергеевич', 2, 7),
(N'Соколова', N'Анна', N'Игоревна', 2, 8),
(N'Волков', N'Максим', N'Андреевич', 3, 9),
(N'Морозова', N'Дарья', N'Владимировна', 3, 10);
GO

-- ============================================
-- 5. Родители (Parents)
-- ============================================
INSERT INTO Parents (LastName, FirstName, FatherName, UserId) VALUES
(N'Смирнова', N'Ольга', N'Викторовна', 11),
(N'Кузнецов', N'Дмитрий', N'Александрович', 12),
(N'Попова', N'Наталья', N'Сергеевна', 13),
(N'Соколов', N'Игорь', N'Петрович', 14);
GO

-- ============================================
-- 6. Предметы (Subjects)
-- ============================================
INSERT INTO Subjects (Title) VALUES
(N'Математика'),
(N'Русский язык'),
(N'Литература'),
(N'Физика'),
(N'Информатика'),
(N'История'),
(N'Биология'),
(N'Химия'),
(N'Английский язык'),
(N'Физкультура');
GO

-- ============================================
-- 7. Связь Student-Parent (StudentParents)
-- ============================================
INSERT INTO StudentParents (StudentId, ParentId) VALUES
(1, 1),  -- Смирнов Артем - Смирнова Ольга
(2, 2),  -- Кузнецова Мария - Кузнецов Дмитрий
(3, 3),  -- Попов Иван - Попова Наталья
(4, 4),  -- Соколова Анна - Соколов Игорь
(1, 2),  -- Смирнов Артем - Кузнецов Дмитрий (второй родитель)
(2, 1);  -- Кузнецова Мария - Смирнова Ольга (опекун)
GO

-- ============================================
-- 8. Связь Teacher-Subject (TeacherSubjects)
-- ============================================
INSERT INTO TeacherSubjects (TeacherId, SubjectId) VALUES
(1, 1),  -- Иванов - Математика
(1, 5),  -- Иванов - Информатика
(2, 2),  -- Петрова - Русский язык
(2, 3),  -- Петрова - Литература
(3, 4),  -- Сидоров - Физика
(3, 1),  -- Сидоров - Математика
(3, 5);  -- Сидоров - Информатика
GO

-- ============================================
-- 9. Связь Subject-Group (SubjectGroups)
-- ============================================
-- Проверяем существование таблицы перед вставкой
INSERT INTO SubjectGroups (Subject_Id, Group_Id) VALUES
(1, 1), (1, 2), (1, 3),  -- Математика во всех классах
(2, 1), (2, 2), (2, 3),  -- Русский язык
(3, 1), (3, 2),           -- Литература только в 5А и 5Б
(4, 1), (4, 2), (4, 3),  -- Физика
(5, 3),                    -- Информатика только в 6А
(6, 1), (6, 2),           -- История
(7, 3),                    -- Биология в 6А
(8, 3),                    -- Химия в 6А
(9, 1), (9, 2), (9, 3),  -- Английский
(10, 1), (10, 2), (10, 3); -- Физкультура
GO

-- ============================================
-- 11. Оценки (Marks)
-- ============================================
INSERT INTO Marks (Value, Date, StudentId, SubjectId, TeacherId) VALUES
-- Оценки для Смирнова Артема (student 1)
(5, '2026-05-01', 1, 1, 1),  -- Математика
(4, '2026-05-02', 1, 2, 2),  -- Русский язык
(5, '2026-05-03', 1, 5, 1),  -- Информатика
(3, '2026-05-04', 1, 4, 3),  -- Физика

-- Оценки для Кузнецовой Марии (student 2)
(4, '2026-05-01', 2, 1, 1),  -- Математика
(5, '2026-05-02', 2, 2, 2),  -- Русский язык
(4, '2026-05-03', 2, 3, 2),  -- Литература
(5, '2026-05-04', 2, 6, 1),  -- История

-- Оценки для Попова Ивана (student 3)
(3, '2026-05-01', 3, 1, 1),  -- Математика
(4, '2026-05-02', 3, 2, 2),  -- Русский язык
(5, '2026-05-03', 3, 4, 3),  -- Физика
(2, '2026-05-04', 3, 1, 1),  -- Математика (пересдача)

-- Оценки для Соколовой Анны (student 4)
(5, '2026-05-01', 4, 2, 2),  -- Русский язык
(5, '2026-05-02', 4, 3, 2),  -- Литература
(4, '2026-05-03', 4, 5, 1),  -- Информатика
(5, '2026-05-04', 4, 6, 3),  -- История

-- Оценки для Волкова Максима (student 5)
(4, '2026-05-01', 5, 1, 1),  -- Математика
(3, '2026-05-02', 5, 4, 3),  -- Физика
(4, '2026-05-03', 5, 5, 1),  -- Информатика
(5, '2026-05-04', 5, 7, 3),  -- Биология

-- Оценки для Морозовой Дарьи (student 6)
(5, '2026-05-01', 6, 1, 1),  -- Математика
(4, '2026-05-02', 6, 4, 3),  -- Физика
(5, '2026-05-03', 6, 7, 3),  -- Биология
(4, '2026-05-04', 6, 8, 3);  -- Химия
GO


-- ============================================
-- ПРОВЕРКА ДАННЫХ
-- ============================================
SELECT 'Users' as TableName, COUNT(*) as Count FROM Users
UNION ALL
SELECT 'Teachers', COUNT(*) FROM Teachers
UNION ALL
SELECT 'Students', COUNT(*) FROM Students
UNION ALL
SELECT 'Parents', COUNT(*) FROM Parents
UNION ALL
SELECT 'Groups', COUNT(*) FROM Groups
UNION ALL
SELECT 'Subjects', COUNT(*) FROM Subjects
UNION ALL
SELECT 'Marks', COUNT(*) FROM Marks
UNION ALL
SELECT 'MarkLogs', COUNT(*) FROM MarkLogs
UNION ALL
SELECT 'StudentParents', COUNT(*) FROM StudentParents
UNION ALL
SELECT 'TeacherSubjects', COUNT(*) FROM TeacherSubjects;