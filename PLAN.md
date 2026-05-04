# План разработки системы электронного дневника школьника

## Этап 1: Базовая структура проекта ✅ (ВЫПОЛНЕНО)

### Созданные файлы:

#### Модели данных (Models/)
- `User.cs` - пользователи системы (авторизация)
- `Student.cs` - ученики
- `Subject.cs` - предметы
- `Mark.cs` - оценки
- `Teacher.cs` - учителя
- `Parent.cs` - родители
- `MarkLog.cs` - лог изменений оценок
- `SchoolDbContext.cs` - контекст Entity Framework

#### Сервисы (Services/)
- `AuthService.cs` - аутентификация и авторизация
- `GradeService.cs` - работа с оценками (процедура выставления, триггер логирования)
- `StudentService.cs` - CRUD операции с данными учеников
- `ReportService.cs` - отчёты и статистика

#### Данные (Data/)
- `DbInitializer.cs` - инициализация и наполнение БД тестовыми данными

#### Конфигурация
- `App.xaml` / `App.xaml.cs` - точка входа приложения
- `App.config` - конфигурация с connection string
- `SchoolDiary.csproj` - файл проекта
- `packages.config` - NuGet пакеты

---

## Этап 2: ViewModel (СЛЕДУЮЩИЙ ШАГ)

Необходимо создать следующие ViewModel классы в папке `ViewModels/`:

### 2.1 LoginViewModel.cs
- Свойства: Login, Password, ErrorMessage
- Команда: LoginCommand
- Методы: Authenticate(), NavigateToMainView()

### 2.2 BaseViewModel.cs ✅ (СОЗДАНО)
- Реализация INotifyPropertyChanged
- Метод OnPropertyChanged()

### 2.3 StudentDiaryViewModel.cs
- Свойства: CurrentStudent, Marks, Subjects, AverageMark
- Методы: LoadStudentData(), CalculateAverage()

### 2.4 TeacherGradeViewModel.cs
- Свойства: SelectedStudent, SelectedSubject, GradeValue, Comment
- Команды: SetGradeCommand, UpdateGradeCommand
- Методы: ValidateTeacherSubject(), SaveGrade()

### 2.5 ParentViewModel.cs
- Свойства: ChildStudent, ChildMarks, ChildAverage
- Методы: LoadChildData()

### 2.6 DirectorViewModel.cs
- Свойства: AllStudents, AllTeachers, Reports
- Методы: GetTopStudents(), GetTeacherActivity(), GetLogs()

---

## Этап 3: Представления (Views)

### 3.1 LoginView.xaml
- Форма авторизации
- Поля: логин, пароль
- Кнопка: Войти

### 3.2 MainWindow.xaml
- Главное окно с навигацией
- Menu или TabControl для переключения между разделами

### 3.3 StudentDiaryView.xaml
- DataGrid со списком оценок
- Группировка по предметам
- Отображение среднего балла

### 3.4 TeacherGradeView.xaml
- ComboBox для выбора ученика
- ComboBox для выбора предмета (автоматически свой)
- Ввод оценки (1-5)
- Поле для комментария
- Кнопки: Сохранить, Изменить, Удалить

### 3.5 ParentView.xaml
- Информация о ребёнке
- Список оценок
- Средний балл

### 3.6 DirectorView.xaml
- Вкладки:
  - Все ученики
  - Все учителя
  - Отчёты
  - Логи изменений

---

## Этап 4: SQL Script (опционально)

Создать файл `Database/` с SQL скриптами:

### 4.1 Trigger_MarkLog.sql
```sql
CREATE TRIGGER trg_MarkLog_Insert
ON Marks
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    -- Логирование изменений
END
```

### 4.2 Procedure_SetGrade.sql
```sql
CREATE PROCEDURE sp_SetGrade
    @StudentId INT,
    @SubjectId INT,
    @Value INT,
    @TeacherId INT,
    @Comment NVARCHAR(200)
AS
BEGIN
    -- Проверки и вставка
END
```

### 4.3 View_TopStudents.sql
```sql
CREATE VIEW vw_TopStudents AS
SELECT TOP 3 ...
```

---

## Этап 5: Тестирование и документация

### 5.1 Unit тесты (опционально)
- Tests/ проект с NUnit или xUnit
- Тесты для сервисов

### 5.2 Документация
- README.md ✅ (СОЗДАНО)
- Инструкция пользователя
- Схема базы данных (ER-диаграмма)

---

## Соответствие требованиям варианта 9

| Требование | Реализация | Статус |
|------------|------------|--------|
| **Логическая структура** | | |
| Students, Subjects, Marks, Teachers, Parents, Users | Модели в Models/ | ✅ |
| **Ограничения** | | |
| Учитель выставляет только по своему предмету | GradeService.SetGrade() | ✅ |
| Оценки 1-5 | [Range] атрибут + валидация | ✅ |
| Родители видят только своего ребёнка | AuthService.GetParentByUser() | ✅ |
| **Объем данных** | | |
| 10 учеников | DbInitializer.Seed() | ✅ |
| 5 учителей | DbInitializer.Seed() | ✅ |
| 50 оценок | DbInitializer.CreateMarks() | ✅ |
| 10 родителей | DbInitializer.Seed() | ✅ |
| **Обработка данных** | | |
| Триггер: лог при изменении | MarkLog + LogMarkChange() | ✅ |
| Процедура: выставление оценки | GradeService.SetGrade() | ✅ |
| Запрос: средний балл | GradeService.GetAverageMarkBySubject() | ✅ |
| Подзапрос: топ-3 ученика | GradeService.GetTop3Students() | ✅ |
| **Пользователи и права** | | |
| Директор: все данные | DirectorViewModel | ⏳ |
| Учителя: выставление оценок | TeacherGradeViewModel | ⏳ |
| Родители: просмотр ребёнка | ParentViewModel | ⏳ |
| Учащиеся: просмотр дневника | StudentDiaryViewModel | ⏳ |
| **Формы** | | |
| Авторизация | LoginView.xaml | ⏳ |
| Дневник ученика | StudentDiaryView.xaml | ⏳ |
| Выставление оценки | TeacherGradeView.xaml | ⏳ |
| Журнал | DirectorView.xaml | ⏳ |
| **Дополнительно** | | |
| Гибкая навигация | MainWindow с Menu/TabControl | ⏳ |

---

## Следующие шаги

1. **Создать ViewModel классы** для каждой роли
2. **Создать XAML представления** (окна)
3. **Реализовать навигацию** между окнами
4. **Протестировать** функционал
5. **Добавить стилизацию** (WPF ресурсы, темы)

---

## Примечания

- Используется **Entity Framework 6** (не Core) для соответствия требованию ".NET Framework"
- **MVVM паттерн** для разделения логики и UI
- **Code First** подход для создания БД
- **SQL Server LocalDB** для простоты развёртывания
