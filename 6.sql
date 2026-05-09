-- First delete the failed attempt if it exists
DELETE FROM dbo.Users WHERE Email = 'demo@example.com';

-- Insert with UserId = 99
SET IDENTITY_INSERT dbo.Users ON;

INSERT INTO dbo.Users (UserId, FullName, Email, PasswordHash, AccountType, CreatedAt)
VALUES (99, 'Demo User', 'demo@example.com', 'demo123', 'PERSONAL', GETDATE());

SET IDENTITY_INSERT dbo.Users OFF;