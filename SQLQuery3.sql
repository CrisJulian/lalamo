-- ============================================================
-- AOOP_DB - Finance Tracker Schema
-- Safe to run on existing DB: uses IF NOT EXISTS checks
-- Run this in Visual Studio > SQL Server Object Explorer
-- or via Query window connected to your LocalDB
-- ============================================================

-- ============================================================
-- 1. CATEGORIES (new reference table)
--    Used by Transactions, Budgets, and RecurringPayments
-- ============================================================
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Categories' AND xtype='U')
BEGIN
    CREATE TABLE Categories (
        CategoryId   INT           NOT NULL IDENTITY(1,1) PRIMARY KEY,
        Name         NVARCHAR(50)  NOT NULL UNIQUE,   -- 'Food', 'Transport', etc.
        Color        NVARCHAR(7)   NOT NULL DEFAULT '#CCCCCC', -- hex color
        Icon         NVARCHAR(50)  NULL                -- optional icon name
    );

    -- Seed default categories matching your app screens
    INSERT INTO Categories (Name, Color) VALUES
        ('Food',          '#F5A623'),
        ('Transport',     '#4A90D9'),
        ('Entertainment', '#9B59B6'),
        ('Utilities',     '#F39C12'),
        ('Healthcare',    '#E74C3C'),
        ('Shopping',      '#E91E63'),
        ('Health',        '#27AE60'),
        ('Software',      '#3498DB'),
        ('Income',        '#2ECC71'),
        ('Others',        '#95A5A6');
END
GO

-- ============================================================
-- 2. USERS - add missing Profile screen columns
-- ============================================================
IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME='Users' AND COLUMN_NAME='Phone'
)
    ALTER TABLE Users ADD Phone NVARCHAR(20) NULL;
GO

IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME='Users' AND COLUMN_NAME='Location'
)
    ALTER TABLE Users ADD Location NVARCHAR(100) NULL;
GO

IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME='Users' AND COLUMN_NAME='ProfilePicture'
)
    ALTER TABLE Users ADD ProfilePicture NVARCHAR(500) NULL; -- file path or base64
GO

IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME='Users' AND COLUMN_NAME='AccountType'
)
    ALTER TABLE Users ADD AccountType NVARCHAR(20) NOT NULL DEFAULT 'PERSONAL';
GO

-- ============================================================
-- 3. TRANSACTIONS - add Type and Category columns
-- ============================================================

-- Type: 'Income' or 'Expense'
IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME='Transactions' AND COLUMN_NAME='Type'
)
    ALTER TABLE Transactions ADD [Type] NVARCHAR(10) NOT NULL DEFAULT 'Expense';
GO

-- CategoryId: FK to Categories table
IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME='Transactions' AND COLUMN_NAME='CategoryId'
)
BEGIN
    ALTER TABLE Transactions ADD CategoryId INT NULL;

    ALTER TABLE Transactions
        ADD CONSTRAINT FK_Transactions_Categories
        FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId);
END
GO

-- Add CHECK constraint for Type column
IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
    WHERE CONSTRAINT_NAME = 'CHK_Transactions_Type'
)
    ALTER TABLE Transactions
        ADD CONSTRAINT CHK_Transactions_Type 
        CHECK ([Type] IN ('Income', 'Expense'));
GO

-- ============================================================
-- 4. BUDGETS - add Month/Year for per-month tracking
-- ============================================================

IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME='Budgets' AND COLUMN_NAME='Month'
)
    ALTER TABLE Budgets ADD [Month] INT NOT NULL DEFAULT MONTH(GETDATE());
GO

IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME='Budgets' AND COLUMN_NAME='Year'
)
    ALTER TABLE Budgets ADD [Year] INT NOT NULL DEFAULT YEAR(GETDATE());
GO

-- CategoryId FK on Budgets
IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME='Budgets' AND COLUMN_NAME='CategoryId'
)
BEGIN
    ALTER TABLE Budgets ADD CategoryId INT NULL;

    ALTER TABLE Budgets
        ADD CONSTRAINT FK_Budgets_Categories
        FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId);
END
GO

-- ============================================================
-- 5. RECURRINGPAYMENTS - add Category column
-- ============================================================

IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME='RecurringPayments' AND COLUMN_NAME='CategoryId'
)
BEGIN
    ALTER TABLE RecurringPayments ADD CategoryId INT NULL;

    ALTER TABLE RecurringPayments
        ADD CONSTRAINT FK_RecurringPayments_Categories
        FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId);
END
GO

-- ============================================================
-- 6. DEBTS - add missing display columns
-- ============================================================

-- Remaining is computed (Principal - AmountPaid) but storing it
-- avoids repeated calculation; update via trigger or app logic
IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME='Debts' AND COLUMN_NAME='Notes'
)
    ALTER TABLE Debts ADD Notes NVARCHAR(500) NULL;
GO

-- ============================================================
-- 7. DEBT PAYMENTS LOG (new table)
--    Tracks each individual payment logged via "Log Payment"
-- ============================================================
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='DebtPayments' AND xtype='U')
BEGIN
    CREATE TABLE DebtPayments (
        PaymentId   INT      NOT NULL IDENTITY(1,1) PRIMARY KEY,
        DebtId      INT      NOT NULL,
        Amount      DECIMAL(18,2) NOT NULL,
        PaidOn      DATETIME NOT NULL DEFAULT GETDATE(),
        Notes       NVARCHAR(500) NULL,

        CONSTRAINT FK_DebtPayments_Debts
            FOREIGN KEY (DebtId) REFERENCES Debts(DebtId)
    );
END
GO

-- ============================================================
-- 8. USEFUL VIEWS
-- ============================================================

-- Monthly spending per category for Analytics screen
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

-- Dashboard: total balance per user
CREATE OR ALTER VIEW vw_UserBalance AS
    SELECT
        UserId,
        SUM(CASE WHEN [Type] = 'Income'  THEN Amount ELSE 0 END) AS TotalIncome,
        SUM(CASE WHEN [Type] = 'Expense' THEN Amount ELSE 0 END) AS TotalExpenses,
        SUM(CASE WHEN [Type] = 'Income'  THEN Amount ELSE -Amount END) AS Balance
    FROM Transactions
    GROUP BY UserId;
GO

-- Budget usage: spent vs limit per budget this month
CREATE OR ALTER VIEW vw_BudgetUsage AS
    SELECT
        b.BudgetId,
        b.UserId,
        b.Name,
        b.LimitAmount,
        b.Month,
        b.Year,
        b.CategoryColor,
        ISNULL(SUM(t.Amount), 0)                            AS Spent,
        b.LimitAmount - ISNULL(SUM(t.Amount), 0)           AS Remaining,
        CAST(
            ISNULL(SUM(t.Amount), 0) * 100.0 / NULLIF(b.LimitAmount, 0)
        AS DECIMAL(5,1))                                     AS PercentUsed
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

-- Recurring payments due soon (within 7 days)
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

-- ============================================================
-- DONE
-- All existing tables preserved. New columns added safely.
-- New tables: Categories, DebtPayments
-- New views:  vw_MonthlySpendingByCategory, vw_UserBalance,
--             vw_BudgetUsage, vw_RecurringDueSoon
-- ============================================================
