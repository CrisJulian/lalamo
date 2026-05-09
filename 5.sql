SET IDENTITY_INSERT dbo.Users ON;

INSERT INTO dbo.Users (UserId, FullName, Email, PasswordHash, AccountType, CreatedAt)
VALUES (0, 'Demo User', 'demo@example.com', 'demo123', 'PERSONAL', GETDATE());

SET IDENTITY_INSERT dbo.Users OFF;