-- ============================================================
-- AOOP_DB - FULL FRESH INSTALL SCRIPT
-- ============================================================

-- 1. USERS
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
BEGIN
    CREATE TABLE Users (
        UserId         INT            NOT NULL IDENTITY(1,1) PRIMARY KEY,
        FullName       NVARCHAR(100)  NULL,
        Email          NVARCHAR(100)  NOT NULL UNIQUE,
        PasswordHash   NVARCHAR(256)  NOT NULL,
        Phone          NVARCHAR(20)   NULL,
        Location       NVARCHAR(100)  NULL,
        AccountType    NVARCHAR(20)   NOT NULL DEFAULT 'PERSONAL',
        ProfilePicture NVARCHAR(500) NULL,
        CreatedAt      DATETIME       NOT NULL DEFAULT GETDATE()
    );
END
GO

-- 2. CATEGORIES
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Categories' AND xtype='U')
BEGIN
    CREATE TABLE Categories (
        CategoryId  INT          NOT NULL IDENTITY(1,1) PRIMARY KEY,
        Name        NVARCHAR(50) NOT NULL UNIQUE,
        Color       NVARCHAR(7)  NOT NULL DEFAULT '#CCCCCC',
        Icon        NVARCHAR(50) NULL
    );

    INSERT INTO Categories (Name, Color) VALUES
        ('Food',          '#F5A623'),
        ('Transport',     '#4A90D9'),
        ('Entertainment', '#9B59B6'),
        ('Utilities',     '#F39C12'),
        ('Healthcare',     '#E74C3C'),
        ('Shopping',      '#E91E63'),
        ('Health',        '#27AE60'),
        ('Software',      '#3498DB'),
        ('Income',        '#2ECC71'),
        ('Others',        '#95A5A6');
END
GO

-- 3. TRANSACTIONS
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Transactions' AND xtype='U')
BEGIN
    CREATE TABLE Transactions (
        TransactionId  INT            NOT NULL IDENTITY(1,1) PRIMARY KEY,
        UserId         INT            NOT NULL,
        BudgetId       INT            NULL,
        CategoryId     INT            NULL,
        Amount         DECIMAL(18,2)  NOT NULL,
        [Type]         NVARCHAR(10)   NOT NULL DEFAULT 'Expense',
        Description    NVARCHAR(200)  NULL,
        [Date]         DATETIME       NOT NULL DEFAULT GETDATE(),

        CONSTRAINT FK_Transactions_Users
            FOREIGN KEY (UserId) REFERENCES Users(UserId),
        CONSTRAINT FK_Transactions_Categories
            FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId),
        CONSTRAINT CHK_Transactions_Type
            CHECK ([Type] IN ('Income', 'Expense'))
    );
END
GO

-- 4. BUDGETS
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Budgets' AND xtype='U')
BEGIN
    CREATE TABLE Budgets (
        BudgetId       INT            NOT NULL IDENTITY(1,1) PRIMARY KEY,
        UserId         INT            NOT NULL,
        CategoryId     INT            NULL,
        Name           NVARCHAR(100)  NOT NULL,
        LimitAmount    DECIMAL(18,2)  NOT NULL,
        Spent          DECIMAL(18,2)  NOT NULL DEFAULT 0,
        CategoryColor NVARCHAR(7)    NULL DEFAULT '#000000',
        Month          INT            NOT NULL DEFAULT MONTH(GETDATE()),
        Year           INT            NOT NULL DEFAULT YEAR(GETDATE()),
        CreatedAt      DATETIME       NULL DEFAULT GETDATE(),

        CONSTRAINT FK_Budgets_Users
            FOREIGN KEY (UserId) REFERENCES Users(UserId),
        CONSTRAINT FK_Budgets_Categories
            FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId)
    );
END
GO

-- 5. SAVINGS GOALS
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='SavingsGoals' AND xtype='U')
BEGIN
    CREATE TABLE SavingsGoals (
        GoalId     INT            NOT NULL IDENTITY(1,1) PRIMARY KEY,
        UserId     INT            NOT NULL,
        Name       NVARCHAR(100)  NOT NULL,
        Target     DECIMAL(18,2)  NOT NULL,
        Saved      DECIMAL(18,2)  NOT NULL DEFAULT 0,
        GoalColor  NVARCHAR(7)    NULL DEFAULT '#000000',
        CreatedAt  DATETIME       NULL DEFAULT GETDATE(),

        CONSTRAINT FK_SavingsGoals_Users
            FOREIGN KEY (UserId) REFERENCES Users(UserId)
    );
END
GO

-- 6. DEBTS
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Debts' AND xtype='U')
BEGIN
    CREATE TABLE Debts (
        DebtId         INT            NOT NULL IDENTITY(1,1) PRIMARY KEY,
        UserId         INT            NOT NULL,
        Name           NVARCHAR(100)  NOT NULL,
        Principal      DECIMAL(18,2)  NOT NULL,
        InterestRate   DECIMAL(18,2)  NOT NULL,
        MonthlyPayment DECIMAL(18,2)  NOT NULL,
        AmountPaid     DECIMAL(18,2)  NOT NULL DEFAULT 0,
        DueDate        DATETIME       NOT NULL,
        PaidThisMonth  BIT            NOT NULL DEFAULT 0,
        Notes          NVARCHAR(500)  NULL,

        CONSTRAINT FK_Debts_Users
            FOREIGN KEY (UserId) REFERENCES Users(UserId)
    );
END
GO

-- 7. DEBT PAYMENTS LOG
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='DebtPayments' AND xtype='U')
BEGIN
    CREATE TABLE DebtPayments (
        PaymentId  INT            NOT NULL IDENTITY(1,1) PRIMARY KEY,
        DebtId     INT            NOT NULL,
        Amount     DECIMAL(18,2)  NOT NULL,
        PaidOn     DATETIME       NOT NULL DEFAULT GETDATE(),
        Notes      NVARCHAR(500)  NULL,

        CONSTRAINT FK_DebtPayments_Debts
            FOREIGN KEY (DebtId) REFERENCES Debts(DebtId)
    );
END
GO

-- 8. RECURRING PAYMENTS
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='RecurringPayments' AND xtype='U')
BEGIN
    CREATE TABLE RecurringPayments (
        PaymentId    INT            NOT NULL IDENTITY(1,1) PRIMARY KEY,
        UserId       INT            NOT NULL,
        CategoryId   INT            NULL,
        PaymentName  NVARCHAR(100)  NOT NULL,
        Amount       DECIMAL(18,2)  NOT NULL,
        FrequencyDays INT           NOT NULL,
        NextDueDate  DATETIME       NOT NULL,
        IsActive     BIT            NOT NULL DEFAULT 1,

        CONSTRAINT FK_RecurringPayments_Users
            FOREIGN KEY (UserId) REFERENCES Users(UserId),
        CONSTRAINT FK_RecurringPayments_Categories
            FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId)
    );
END
GO

-- 9. VIEWS
CREATE OR ALTER VIEW vw_MonthlySpendingByCategory AS
    SELECT
        t.UserId,
        c.Name          AS Category,
        c.Color         AS CategoryColor,
        MONTH(t.Date)   AS [Month],
        YEAR(t.Date)    AS [Year],
        SUM(t.Amount)   AS TotalSpent
    FROM Transactions t
    LEFT JOIN Categories c ON t.CategoryId = c.CategoryId
    WHERE t.[Type] = 'Expense'
    GROUP BY t.UserId, c.Name, c.Color, MONTH(t.Date), YEAR(t.Date);
GO

CREATE OR ALTER VIEW vw_UserBalance AS
    SELECT
        UserId,
        SUM(CASE WHEN [Type] = 'Income'  THEN Amount ELSE 0 END) AS TotalIncome,
        SUM(CASE WHEN [Type] = 'Expense' THEN Amount ELSE 0 END) AS TotalExpenses,
        SUM(CASE WHEN [Type] = 'Income'  THEN Amount ELSE -Amount END) AS Balance
    FROM Transactions
    GROUP BY UserId;
GO

CREATE OR ALTER VIEW vw_BudgetUsage AS
    SELECT
        b.BudgetId,
        b.UserId,
        b.Name,
        b.LimitAmount,
        b.Month,
        b.Year,
        b.CategoryColor,
        ISNULL(SUM(t.Amount), 0)                                     AS Spent,
        b.LimitAmount - ISNULL(SUM(t.Amount), 0)                    AS Remaining,
        CAST(
            ISNULL(SUM(t.Amount), 0) * 100.0 / NULLIF(b.LimitAmount, 0)
        AS DECIMAL(5,1))                                              AS PercentUsed
    FROM Budgets b
    LEFT JOIN Transactions t
        ON  t.UserId     = b.UserId
        AND t.CategoryId = b.CategoryId
        AND MONTH(t.Date) = b.Month
        AND YEAR(t.Date)  = b.Year
        AND t.[Type]     = 'Expense'
    GROUP BY b.BudgetId, b.UserId, b.Name, b.LimitAmount,
             b.Month, b.Year, b.CategoryColor;
GO

CREATE OR ALTER VIEW vw_RecurringDueSoon AS
    SELECT
        rp.*,
        c.Name AS CategoryName,
        DATEDIFF(DAY, GETDATE(), NextDueDate) AS DaysUntilDue
    FROM RecurringPayments rp
    LEFT JOIN Categories c ON rp.CategoryId = c.CategoryId
    WHERE IsActive = 1
      AND NextDueDate <= DATEADD(DAY, 7, GETDATE());
GO

PRINT 'AOOP_DB setup complete! All tables and views created.';