/* ============================================================
   RACEDAY DATABASE - PART 1
   SQL SERVER DATABASE SCRIPT
   ============================================================ */

-- 1. CREATE DATABASE
IF DB_ID('RaceDay') IS NULL
BEGIN
    CREATE DATABASE RaceDay;
END
GO

USE RaceDay;
GO


/* ============================================================
   2. REMOVE EXISTING TABLES
   ============================================================ */

IF OBJECT_ID('dbo.Results', 'U') IS NOT NULL
    DROP TABLE dbo.Results;
GO

IF OBJECT_ID('dbo.Enrolments', 'U') IS NOT NULL
    DROP TABLE dbo.Enrolments;
GO

IF OBJECT_ID('dbo.Categories', 'U') IS NOT NULL
    DROP TABLE dbo.Categories;
GO

IF OBJECT_ID('dbo.Events', 'U') IS NOT NULL
    DROP TABLE dbo.Events;
GO

IF OBJECT_ID('dbo.Profile', 'U') IS NOT NULL
    DROP TABLE dbo.Profile;
GO

IF OBJECT_ID('dbo.Users', 'U') IS NOT NULL
    DROP TABLE dbo.Users;
GO


/* ============================================================
   3. USERS TABLE
   ============================================================ */

CREATE TABLE dbo.Users
(
    UserID INT IDENTITY(1,1) NOT NULL,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    Role NVARCHAR(20) NOT NULL,
    CreatedAt DATETIME2 NOT NULL
        CONSTRAINT DF_Users_CreatedAt DEFAULT SYSDATETIME(),

    CONSTRAINT PK_Users PRIMARY KEY (UserID),

    CONSTRAINT UQ_Users_Email UNIQUE (Email),

    CONSTRAINT CK_Users_Role
        CHECK (Role IN ('Organiser', 'Participant'))
);
GO


/* ============================================================
   4. PROFILE TABLE
   ============================================================ */

CREATE TABLE dbo.Profile
(
    ProfileID INT IDENTITY(1,1) NOT NULL,
    UserID INT NOT NULL,
    PhoneNumber NVARCHAR(20) NOT NULL,
    DateOfBirth DATE NOT NULL,
    EmergencyContactName NVARCHAR(100) NOT NULL,
    EmergencyContactPhone NVARCHAR(20) NOT NULL,

    CONSTRAINT PK_Profile PRIMARY KEY (ProfileID),

    CONSTRAINT UQ_Profile_UserID UNIQUE (UserID),

    CONSTRAINT FK_Profile_Users
        FOREIGN KEY (UserID)
        REFERENCES dbo.Users(UserID)
);
GO


/* ============================================================
   5. EVENTS TABLE
   ============================================================ */

CREATE TABLE dbo.Events
(
    EventID INT IDENTITY(1,1) NOT NULL,
    UserID INT NOT NULL,
    EventName NVARCHAR(150) NOT NULL,
    Description NVARCHAR(500) NOT NULL,
    EventDate DATE NOT NULL,
    StartTime TIME NOT NULL,
    Location NVARCHAR(200) NOT NULL,
    DistanceKm DECIMAL(5,2) NOT NULL,
    EventType NVARCHAR(20) NOT NULL,
    Status NVARCHAR(30) NOT NULL,
    CreatedAt DATETIME2 NOT NULL
        CONSTRAINT DF_Events_CreatedAt DEFAULT SYSDATETIME(),

    CONSTRAINT PK_Events PRIMARY KEY (EventID),

    CONSTRAINT FK_Events_Users
        FOREIGN KEY (UserID)
        REFERENCES dbo.Users(UserID),

    CONSTRAINT CK_Events_Distance
        CHECK (DistanceKm > 0),

    CONSTRAINT CK_Events_Type
        CHECK (EventType IN ('Run', 'Walk', 'Cycle')),

    CONSTRAINT CK_Events_Status
        CHECK (Status IN ('Upcoming', 'Open', 'Closed', 'Completed'))
);
GO


/* ============================================================
   6. CATEGORIES TABLE
   ============================================================ */

CREATE TABLE dbo.Categories
(
    CategoryID INT IDENTITY(1,1) NOT NULL,
    EventID INT NOT NULL,
    CategoryName NVARCHAR(100) NOT NULL,
    DistanceKm DECIMAL(5,2) NOT NULL,
    MaximumParticipants INT NOT NULL,
    EntryFee DECIMAL(10,2) NOT NULL,

    CONSTRAINT PK_Categories PRIMARY KEY (CategoryID),

    CONSTRAINT FK_Categories_Events
        FOREIGN KEY (EventID)
        REFERENCES dbo.Events(EventID),

    CONSTRAINT CK_Categories_Distance
        CHECK (DistanceKm > 0),

    CONSTRAINT CK_Categories_MaxParticipants
        CHECK (MaximumParticipants > 0),

    CONSTRAINT CK_Categories_EntryFee
        CHECK (EntryFee >= 0)
);
GO


/* ============================================================
   7. ENROLMENTS TABLE
   ============================================================ */

CREATE TABLE dbo.Enrolments
(
    EnrolmentID INT IDENTITY(1,1) NOT NULL,
    ProfileID INT NOT NULL,
    CategoryID INT NOT NULL,
    EnrolmentDate DATETIME2 NOT NULL
        CONSTRAINT DF_Enrolments_Date DEFAULT SYSDATETIME(),
    Status NVARCHAR(30) NOT NULL,

    CONSTRAINT PK_Enrolments PRIMARY KEY (EnrolmentID),

    CONSTRAINT FK_Enrolments_Profile
        FOREIGN KEY (ProfileID)
        REFERENCES dbo.Profile(ProfileID),

    CONSTRAINT FK_Enrolments_Categories
        FOREIGN KEY (CategoryID)
        REFERENCES dbo.Categories(CategoryID),

    CONSTRAINT UQ_Enrolments_Profile_Category
        UNIQUE (ProfileID, CategoryID),

    CONSTRAINT CK_Enrolments_Status
        CHECK (Status IN ('Pending', 'Confirmed', 'Cancelled'))
);
GO


/* ============================================================
   8. RESULTS TABLE
   ============================================================ */

CREATE TABLE dbo.Results
(
    ResultID INT IDENTITY(1,1) NOT NULL,
    EnrolmentID INT NOT NULL,
    FinishTime TIME NOT NULL,
    Position INT NOT NULL,
    ResultStatus NVARCHAR(30) NOT NULL,
    RecordedAt DATETIME2 NOT NULL
        CONSTRAINT DF_Results_RecordedAt DEFAULT SYSDATETIME(),

    CONSTRAINT PK_Results PRIMARY KEY (ResultID),

    CONSTRAINT UQ_Results_EnrolmentID UNIQUE (EnrolmentID),

    CONSTRAINT FK_Results_Enrolments
        FOREIGN KEY (EnrolmentID)
        REFERENCES dbo.Enrolments(EnrolmentID),

    CONSTRAINT CK_Results_Position
        CHECK (Position > 0),

    CONSTRAINT CK_Results_Status
        CHECK (ResultStatus IN ('Finished', 'DNF', 'DNS'))
);
GO


/* ============================================================
   9. INSERT USERS
   ============================================================ */

INSERT INTO dbo.Users
(
    FirstName,
    LastName,
    Email,
    PasswordHash,
    Role
)
VALUES
(
    'Thabo',
    'Mokoena',
    'thabo@raceday.co.za',
    'PasswordHash_Thabo_123',
    'Organiser'
),
(
    'Lerato',
    'Dlamini',
    'lerato@raceday.co.za',
    'PasswordHash_Lerato_123',
    'Organiser'
),
(
    'Karabo',
    'Baloi',
    'karabo@example.com',
    'PasswordHash_Karabo_123',
    'Participant'
),
(
    'Sipho',
    'Nkosi',
    'sipho@example.com',
    'PasswordHash_Sipho_123',
    'Participant'
);
GO


/* ============================================================
   10. INSERT PROFILES
   ============================================================ */

INSERT INTO dbo.Profile
(
    UserID,
    PhoneNumber,
    DateOfBirth,
    EmergencyContactName,
    EmergencyContactPhone
)
VALUES
(
    1,
    '0711111111',
    '1985-04-12',
    'Mpho Mokoena',
    '0722222222'
),
(
    2,
    '0733333333',
    '1988-09-20',
    'Nomsa Dlamini',
    '0744444444'
),
(
    3,
    '0755555555',
    '2001-06-15',
    'Molefe Baloi',
    '0766666666'
),
(
    4,
    '0777777777',
    '1999-11-03',
    'Zanele Nkosi',
    '0788888888'
);
GO


/* ============================================================
   11. INSERT EVENTS
   ============================================================ */

INSERT INTO dbo.Events
(
    UserID,
    EventName,
    Description,
    EventDate,
    StartTime,
    Location,
    DistanceKm,
    EventType,
    Status
)
VALUES
(
    1,
    'Johannesburg City Run',
    'A road running event through Johannesburg.',
    '2026-10-10',
    '07:00',
    'Johannesburg',
    10.00,
    'Run',
    'Open'
),
(
    2,
    'Cape Town Charity Walk',
    'A community charity walk along the Cape Town coastline.',
    '2026-11-14',
    '08:00',
    'Cape Town',
    5.00,
    'Walk',
    'Upcoming'
),
(
    1,
    'Pretoria Cycle Challenge',
    'A cycling challenge through Pretoria.',
    '2026-12-05',
    '06:30',
    'Pretoria',
    25.00,
    'Cycle',
    'Upcoming'
);
GO


/* ============================================================
   12. INSERT CATEGORIES
   ============================================================ */

INSERT INTO dbo.Categories
(
    EventID,
    CategoryName,
    DistanceKm,
    MaximumParticipants,
    EntryFee
)
VALUES
(
    1,
    '10km Open',
    10.00,
    500,
    150.00
),
(
    1,
    '10km Junior',
    10.00,
    200,
    100.00
),
(
    2,
    '5km Family Walk',
    5.00,
    300,
    80.00
),
(
    2,
    '5km Open Walk',
    5.00,
    400,
    100.00
),
(
    3,
    '25km Open Cycle',
    25.00,
    250,
    250.00
),
(
    3,
    '25km Junior Cycle',
    25.00,
    100,
    150.00
);
GO


/* ============================================================
   13. INSERT ENROLMENTS
   ============================================================ */

INSERT INTO dbo.Enrolments
(
    ProfileID,
    CategoryID,
    Status
)
VALUES
(
    3,
    1,
    'Confirmed'
),
(
    4,
    1,
    'Confirmed'
),
(
    3,
    3,
    'Confirmed'
),
(
    4,
    4,
    'Confirmed'
),
(
    3,
    5,
    'Confirmed'
);
GO


/* ============================================================
   14. INSERT RESULTS
   ============================================================ */

INSERT INTO dbo.Results
(
    EnrolmentID,
    FinishTime,
    Position,
    ResultStatus
)
VALUES
(
    1,
    '00:52:35',
    1,
    'Finished'
),
(
    2,
    '00:58:20',
    2,
    'Finished'
),
(
    3,
    '01:31:45',
    1,
    'Finished'
);
GO


/* ============================================================
   15. VERIFY DATABASE
   ============================================================ */

SELECT * FROM dbo.Users;
SELECT * FROM dbo.Profile;
SELECT * FROM dbo.Events;
SELECT * FROM dbo.Categories;
SELECT * FROM dbo.Enrolments;
SELECT * FROM dbo.Results;
GO