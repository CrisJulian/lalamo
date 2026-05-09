-- Add the plain Category text column your app actually uses
IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME='Transactions' AND COLUMN_NAME='Category'
)
    ALTER TABLE Transactions ADD Category NVARCHAR(100) NULL;
GO

-- Add demo user
IF NOT EXISTS (SELECT * FROM dbo.Users WHERE UserId = 99)
BEGIN
    SET IDENTITY_INSERT dbo.Users ON;

    INSERT INTO dbo.Users (UserId, FullName, Email, PasswordHash, AccountType, CreatedAt)
    VALUES (99, 'Demo User', 'demo@example.com', 'demo123', 'PERSONAL', GETDATE());

    SET IDENTITY_INSERT dbo.Users OFF;
END
GO