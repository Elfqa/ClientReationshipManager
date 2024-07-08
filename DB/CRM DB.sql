CREATE DATABASE ClientRelationshipManager;
GO
USE ClientRelationshipManager;


CREATE TABLE Advisors
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100),
    Email NVARCHAR(255) UNIQUE,
    Password NVARCHAR(255)
);

CREATE TABLE Clients
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(100),
	LastName NVARCHAR(100)
);

CREATE TABLE ClientAdvisorRelationships
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
	AdvisorId INT FOREIGN KEY REFERENCES Advisors(Id) NOT NULL,
    ClientId INT FOREIGN KEY REFERENCES Clients(Id) NOT NULL,
	LastUpdate DATETIME NOT NULL,
);

CREATE TABLE Contacts
(
    Id INT PRIMARY KEY IDENTITY(1,1),
    Description NVARCHAR(MAX) ,
    CreationDate DATETIME NOT NULL,
	LastUpdate DATETIME NOT NULL,
    ScheduledDate DATETIME NULL,
    StartDate DATETIME NULL,
	EndDate DATETIME NULL,
    Status INT NOT NULL,		--scheduled, completed
	--Type INT NOT NULL,			--meeting, telephone
    AdvisorId INT FOREIGN KEY REFERENCES Advisors(Id) NOT NULL,
    ClientId INT FOREIGN KEY REFERENCES Clients(Id) NOT NULL,
);



