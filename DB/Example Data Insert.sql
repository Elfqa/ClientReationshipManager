-- Wstawienie przyk³adowych danych do tabeli Advisors
INSERT INTO Advisors (Name, Email, Password) VALUES 
('John Smith', 'john.smith@example.com', 'password123'),
('Jane Doe', 'jane.doe@example.com', 'password456'),
('Emily Johnson', 'emily.johnson@example.com', 'password789'),
('Michael Brown', 'michael.brown@example.com', 'password101'),
('Sarah Davis', 'sarah.davis@example.com', 'password202');

-- Wstawienie przyk³adowych danych do tabeli Clients
INSERT INTO Clients (FirstName, LastName) VALUES 
('James', 'Wilson'),
('Linda', 'Taylor'),
('Robert', 'Martinez'),
('Patricia', 'Hernandez'),
('David', 'Lopez'),
('Barbara', 'Gonzales'),
('William', 'Anderson'),
('Elizabeth', 'Thomas'),
('Richard', 'Jackson'),
('Jennifer', 'White'),
('Joseph', 'Harris'),
('Maria', 'Martin'),
('Thomas', 'Thompson'),
('Susan', 'Garcia'),
('Charles', 'Martinez'),
('Margaret', 'Clark'),
('Christopher', 'Rodriguez'),
('Dorothy', 'Lewis'),
('Daniel', 'Lee'),
('Lisa', 'Walker'),
('Matthew', 'Hall'),
('Nancy', 'Allen'),
('Anthony', 'Young'),
('Betty', 'King'),
('Mark', 'Wright'),
('Sandra', 'Scott'),
('Paul', 'Torres'),
('Kimberly', 'Nguyen'),
('Steven', 'Hill'),
('Donna', 'Flores'),
('Andrew', 'Green'),
('Carol', 'Adams'),
('Joshua', 'Nelson'),
('Michelle', 'Baker'),
('Kenneth', 'Perez'),
('Emily', 'Carter'),
('George', 'Mitchell'),
('Amanda', 'Roberts'),
('Kevin', 'Turner'),
('Helen', 'Phillips'),
('Brian', 'Campbell'),
('Deborah', 'Parker'),
('Edward', 'Evans'),
('Jessica', 'Edwards'),
('Ronald', 'Collins'),
('Laura', 'Stewart'),
('Timothy', 'Sanchez'),
('Sarah', 'Morris'),
('Jason', 'Rogers'),
('Karen', 'Reed');


-- Przydzielenie relacji Client-Advisor
DECLARE @advisorCount INT = (SELECT COUNT(*) FROM Advisors);
DECLARE @clientId INT = 1;
DECLARE @advisorId INT;
WHILE @clientId <= 50
BEGIN
    SET @advisorId = ((@clientId - 1) % @advisorCount) + 1; -- Równomierne przypisanie doradców
    INSERT INTO ClientAdvisorRelationships (AdvisorId, ClientId, LastUpdate) VALUES 
    (@advisorId, @clientId, GETDATE());
    SET @clientId = @clientId + 1;
END;

-- Wstawienie przyk³adowych danych do tabeli Contacts
DECLARE @i INT = 1;
DECLARE @relationshipId INT;
DECLARE @status INT;
DECLARE @creationDate DATETIME;
DECLARE @lastUpdate DATETIME;
DECLARE @startDate DATETIME;
DECLARE @endDate DATETIME;
DECLARE @scheduledDate DATETIME;
DECLARE @duration INT;

WHILE @i <= 100
BEGIN
    SET @relationshipId = (SELECT TOP 1 Id FROM ClientAdvisorRelationships ORDER BY NEWID());
    SET @status = CASE WHEN @i % 2 = 0 THEN 0 ELSE 1 END; -- 0 = scheduled, 1 = completed

    -- Ustawienie dat
    SET @creationDate = DATEADD(DAY, -@i, GETDATE());
    SET @lastUpdate = DATEADD(DAY, 0, GETDATE());
    
    IF @status = 0
    BEGIN
        -- scheduled
        SET @scheduledDate = DATEADD(DAY, @i, GETDATE());
        
        INSERT INTO Contacts (Description, CreationDate, LastUpdate, ScheduledDate, StartDate, EndDate, Status, AdvisorId, ClientId) 
        SELECT 
            'Contact Description ' + CAST(@i AS NVARCHAR(MAX)),
            @creationDate,
            @lastUpdate,
            @scheduledDate,
            NULL,
            NULL,
            @status,
            AdvisorId,
            ClientId
        FROM ClientAdvisorRelationships
        WHERE Id = @relationshipId;
    END
    ELSE
    BEGIN
        -- completed
        SET @startDate = DATEADD(DAY, -@i, GETDATE());
        SET @duration = ABS(CHECKSUM(NEWID()) % 180) + 1; -- Duration between 1 minute and 3 hours
        SET @endDate = DATEADD(MINUTE, @duration, @startDate);
        
        INSERT INTO Contacts (Description, CreationDate, LastUpdate, ScheduledDate, StartDate, EndDate, Status, AdvisorId, ClientId) 
        SELECT 
            'Contact Description ' + CAST(@i AS NVARCHAR(MAX)),
            @creationDate,
            @lastUpdate,
            NULL,
            @startDate,
            @endDate,
            @status,
            AdvisorId,
            ClientId
        FROM ClientAdvisorRelationships
        WHERE Id = @relationshipId;
    END

    SET @i = @i + 1;
END;


/*
Advisors Table:
The Advisors table stores information about the advisors in the system.

Id: Primary key, auto-incremented (INT).
Name: The name of the advisor (NVARCHAR(100)).
Email: The email address of the advisor, which must be unique (NVARCHAR(255)).
Password: The password for the advisor's account (NVARCHAR(255)).

Clients Table:
The Clients table stores information about the clients.

Id: Primary key, auto-incremented (INT).
FirstName: The first name of the client (NVARCHAR(100)).
LastName: The last name of the client (NVARCHAR(100)).
ClientAdvisorRelationships Table:

The ClientAdvisorRelationships table maps the relationships between clients and advisors. Each client is assigned to one advisor.

Id: Primary key, auto-incremented (INT).
AdvisorId: Foreign key referencing the Advisors table, indicating the advisor assigned to the client (INT).
ClientId: Foreign key referencing the Clients table, indicating the client assigned to the advisor (INT).
LastUpdate: The date and time when the relationship was last updated (DATETIME).
Summary of Relationships:
Each advisor can have multiple clients.
Each client is assigned to one advisor.
The ClientAdvisorRelationships table ensures a one-to-many relationship between advisors and clients.

The Contacts table is designed to store information about interactions between clients and advisors. Here is an overview of its structure and the rules applied to the dates:

Table Structure:
Id: Primary key, auto-incremented.
Description: A detailed description of the contact (NVARCHAR(MAX)).
CreationDate: The date and time when the contact was created (DATETIME).
LastUpdate: The date and time when the contact was last updated (DATETIME).
ScheduledDate: The date and time when a scheduled contact is planned to occur (DATETIME, nullable).
StartDate: The date and time when the contact actually started (DATETIME, nullable).
EndDate: The date and time when the contact ended (DATETIME, nullable).
Status: An integer indicating the status of the contact (0 = scheduled, 1 = completed).
AdvisorId: Foreign key referencing the Advisors table (INT).
ClientId: Foreign key referencing the Clients table (INT).
Date Rules:
CreationDate <= LastUpdate:
The creation date of the contact must be earlier than or equal to the last update date.
StartDate < EndDate:
The start date must be earlier than the end date for completed contacts.
StartDate <= CreationDate:
The start date must be earlier than or equal to the creation date for completed contacts.
ScheduledDate > LastUpdate:
The scheduled date must be later than the last update date for scheduled contacts.
StartDate and EndDate on the Same Day:
For completed contacts, the StartDate and EndDate must be on the same day, with EndDate being 1 minute to 3 hours after StartDate.




Data Integrity and Constraints:
Advisors Table: Ensures unique email addresses for each advisor.
Clients Table: Stores basic client information.
ClientAdvisorRelationships Table: Ensures valid references to both advisors and clients, maintaining the integrity of the client-advisor assignments.
Contacts Table: Connects clients and advisors through interactions, following specific date rules to maintain consistency.
This structure supports a client relationship management system, allowing for the organization and tracking of advisors, clients, and their interactions.



*/