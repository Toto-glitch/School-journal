USE SchoolJournal;
GO

-- Создаем таблицу MarkLog если она еще не существует
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MarkLogs]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[MarkLogs](
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [MarkId] INT NOT NULL,
        [OldValue] INT NULL,
        [NewValue] INT NULL,
        [ChangeDate] DATETIME NOT NULL DEFAULT GETDATE(),
        [UserId] INT NOT NULL,
        [Action] NVARCHAR(100) NOT NULL
    )
END
GO

-- Создаем триггер для INSERT (добавление новой оценки)
CREATE OR ALTER TRIGGER [dbo].[TR_Mark_Insert_Log]
ON [dbo].[Marks]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [dbo].[MarkLogs] ([MarkId], [OldValue], [NewValue], [ChangeDate], [UserId], [Action])
    SELECT
        i.Id AS MarkId,
        NULL AS OldValue,
        i.Value AS NewValue,
        GETDATE() AS ChangeDate,
        i.TeacherId AS UserId,
        N'Добавление оценки (триггер)' AS Action
    FROM inserted i;
END
GO

-- Создаем триггер для UPDATE (изменение оценки)
CREATE OR ALTER TRIGGER [dbo].[TR_Mark_Update_Log]
ON [dbo].[Marks]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF UPDATE(Value)
    BEGIN
        INSERT INTO [dbo].[MarkLogs] ([MarkId], [OldValue], [NewValue], [ChangeDate], [UserId], [Action])
        SELECT
            i.Id AS MarkId,
            d.Value AS OldValue,
            i.Value AS NewValue,
            GETDATE() AS ChangeDate,
            i.TeacherId AS UserId,
            N'Изменение оценки (триггер)' AS Action
        FROM inserted i
        INNER JOIN deleted d ON i.Id = d.Id;
    END
END
GO

-- Создаем триггер для DELETE (удаление оценки)
CREATE OR ALTER TRIGGER [dbo].[TR_Mark_Delete_Log]
ON [dbo].[Marks]
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [dbo].[MarkLogs] ([MarkId], [OldValue], [NewValue], [ChangeDate], [UserId], [Action])
    SELECT
        d.Id AS MarkId,
        d.Value AS OldValue,
        NULL AS NewValue,
        GETDATE() AS ChangeDate,
        d.TeacherId AS UserId,
        N'Удаление оценки (триггер)' AS Action
    FROM deleted d;
END
GO

PRINT 'Триггеры для логирования оценок успешно созданы!';
GO