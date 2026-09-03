IF DB_ID('RaceDayDB') IS NULL
BEGIN
    CREATE DATABASE RaceDayDB;
END
GO

USE RaceDayDB;
GO

IF OBJECT_ID('dbo.Results', 'U') IS NOT NULL DROP TABLE dbo.Results;
IF OBJECT_ID('dbo.Enrolments', 'U') IS NOT NULL DROP TABLE dbo.Enrolments;
IF OBJECT_ID('dbo.Categories', 'U') IS NOT NULL DROP TABLE dbo.Categories;
IF OBJECT_ID('dbo.Events', 'U') IS NOT NULL DROP TABLE dbo.Events;
IF OBJECT_ID('dbo.Venues', 'U') IS NOT NULL DROP TABLE dbo.Venues;
IF OBJECT_ID('dbo.Users', 'U') IS NOT NULL DROP TABLE dbo.Users;
GO

CREATE TABLE dbo.Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    Role NVARCHAR(20) NOT NULL DEFAULT 'Participant' CHECK (Role IN ('Organiser', 'Participant')),
    PhoneNumber NVARCHAR(20) NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);

CREATE TABLE dbo.Venues (
    VenueId INT IDENTITY(1,1) PRIMARY KEY,
    VenueName NVARCHAR(150) NOT NULL,
    AddressLine NVARCHAR(200) NULL,
    City NVARCHAR(100) NOT NULL,
    Province NVARCHAR(100) NULL
);

CREATE TABLE dbo.Events (
    EventId INT IDENTITY(1,1) PRIMARY KEY,
    OrganiserId INT NOT NULL,
    VenueId INT NOT NULL,
    EventName NVARCHAR(150) NOT NULL,
    EventType NVARCHAR(20) NOT NULL CHECK (EventType IN ('Running', 'Walking', 'Cycling')),
    EventDate DATE NOT NULL,
    Description NVARCHAR(MAX) NULL,
    RouteInfo NVARCHAR(MAX) NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Events_Users FOREIGN KEY (OrganiserId) REFERENCES dbo.Users(UserId),
    CONSTRAINT FK_Events_Venues FOREIGN KEY (VenueId) REFERENCES dbo.Venues(VenueId)
);

CREATE TABLE dbo.Categories (
    CategoryId INT IDENTITY(1,1) PRIMARY KEY,
    EventId INT NOT NULL,
    CategoryName NVARCHAR(100) NOT NULL,
    DistanceKm DECIMAL(5,2) NOT NULL,
    EntryFee DECIMAL(8,2) NOT NULL DEFAULT 0,
    MaxParticipants INT NULL,
    CONSTRAINT FK_Categories_Events FOREIGN KEY (EventId) REFERENCES dbo.Events(EventId)
);

CREATE TABLE dbo.Enrolments (
    EnrolmentId INT IDENTITY(1,1) PRIMARY KEY,
    ParticipantId INT NOT NULL,
    CategoryId INT NOT NULL,
    EnrolmentDate DATETIME NOT NULL DEFAULT GETDATE(),
    Status NVARCHAR(20) NOT NULL DEFAULT 'Confirmed' CHECK (Status IN ('Pending', 'Confirmed', 'Cancelled')),
    CONSTRAINT FK_Enrolments_Users FOREIGN KEY (ParticipantId) REFERENCES dbo.Users(UserId),
    CONSTRAINT FK_Enrolments_Categories FOREIGN KEY (CategoryId) REFERENCES dbo.Categories(CategoryId),
    CONSTRAINT UQ_Enrolments_Unique UNIQUE (ParticipantId, CategoryId)
);

CREATE TABLE dbo.Results (
    ResultId INT IDENTITY(1,1) PRIMARY KEY,
    EnrolmentId INT NOT NULL UNIQUE,
    FinishTime TIME NULL,
    Position INT NULL,
    CapturedByUserId INT NOT NULL,
    CapturedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Results_Enrolments FOREIGN KEY (EnrolmentId) REFERENCES dbo.Enrolments(EnrolmentId),
    CONSTRAINT FK_Results_Users FOREIGN KEY (CapturedByUserId) REFERENCES dbo.Users(UserId)
);

INSERT INTO dbo.Users (FullName, Email, PasswordHash, Role, PhoneNumber) VALUES
('Sarah Naidoo', 'sarah@raceday.co.za', 'hash1', 'Organiser', '0821234567'),
('Thabo Mokoena', 'thabo@raceday.co.za', 'hash2', 'Organiser', '0837654321'),
('Emma van Wyk', 'emma@example.com', 'hash3', 'Participant', '0845558899'),
('Lucky Dube', 'lucky@example.com', 'hash4', 'Participant', '0839997766');

INSERT INTO dbo.Venues (VenueName, AddressLine, City, Province) VALUES
('Union Buildings', 'Government Ave', 'Pretoria', 'Gauteng'),
('Sea Point Promenade', 'Beach Rd', 'Cape Town', 'Western Cape'),
('Pietermaritzburg', 'City Hall', 'Pietermaritzburg', 'KwaZulu-Natal');

INSERT INTO dbo.Events (OrganiserId, VenueId, EventName, EventType, EventDate, Description) VALUES
(1, 1, 'Pretoria Spring Run', 'Running', '2026-10-10', 'Community run through gardens'),
(1, 2, 'Sea Point Walk', 'Walking', '2026-11-01', 'Scenic coastal walk'),
(2, 3, 'Comrades Cycle', 'Cycling', '2026-11-22', 'Charity cycling event');

INSERT INTO dbo.Categories (EventId, CategoryName, DistanceKm, EntryFee, MaxParticipants) VALUES
(1, '5km Fun Run', 5.00, 50.00, 200),
(1, '10km Challenge', 10.00, 80.00, 150),
(2, '3km Fun Walk', 3.00, 30.00, 300),
(3, '42km Road Cycle', 42.00, 150.00, 100);

INSERT INTO dbo.Enrolments (ParticipantId, CategoryId, Status) VALUES
(3, 1, 'Confirmed'),
(3, 4, 'Confirmed'),
(4, 2, 'Confirmed'),
(4, 3, 'Confirmed');

INSERT INTO dbo.Results (EnrolmentId, FinishTime, Position, CapturedByUserId) VALUES
(1, '00:28:45', 12, 1),
(3, '00:52:10', 5, 1);