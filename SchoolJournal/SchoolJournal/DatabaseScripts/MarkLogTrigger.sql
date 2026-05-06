USE SchoolJournal;
GO

-- ============================================
-- Триггер для INSERT (добавление новой оценки)
-- ============================================
CREATE OR ALTER TRIGGER [dbo].[TR_Mark_Insert_Log]
ON [dbo].[Marks]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [dbo].[MarkLogs] ([MarkId], [OldValue], [NewValue], [ChangeDate], [UserId], [Action])
    SELECT
        i.[Id] AS MarkId,
        NULL AS OldValue,
        i.[Value] AS NewValue,
        GETDATE() AS ChangeDate,
        t.[UserId] AS UserId,  -- Берем UserId из таблицы Teachers
        N'Добавление оценки' AS Action
    FROM inserted i
    INNER JOIN [dbo].[Teachers] t ON i.[TeacherId] = t.[Id];
END
GO

-- ============================================
-- Триггер для UPDATE (изменение оценки)
-- ============================================
CREATE OR ALTER TRIGGER [dbo].[TR_Mark_Update_Log]
ON [dbo].[Marks]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Проверяем, что изменилось именно поле Value
    IF UPDATE(Value)
    BEGIN
        INSERT INTO [dbo].[MarkLogs] ([MarkId], [OldValue], [NewValue], [ChangeDate], [UserId], [Action])
        SELECT
            i.[Id] AS MarkId,
            d.[Value] AS OldValue,
            i.[Value] AS NewValue,
            GETDATE() AS ChangeDate,
            t.[UserId] AS UserId,  -- Берем UserId из таблицы Teachers
            N'Изменение оценки' AS Action
        FROM inserted i
        INNER JOIN deleted d ON i.[Id] = d.[Id]
        INNER JOIN [dbo].[Teachers] t ON i.[TeacherId] = t.[Id]
        WHERE i.[Value] <> d.[Value];  -- Только если значение действительно изменилось
    END
END
GO

PRINT 'Triggers created!';
GO