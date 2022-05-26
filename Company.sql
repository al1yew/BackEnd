CREATE DATABASE Company

USE Company

CREATE TABLE Employees(Id INT, Name nvarchar(25), SurName nvarchar(25), Position nvarchar(40), Salary INT)

INSERT INTO Employees(Id, Name, SurName, Position, Salary)
Values
(1, N'Vasif', N'Aliyev', N'CEO', 15000),
(2, N'Asif', N'Aliyev', N'Director', 1000),
(4, N'Agasif', N'Mamedov', N'Manager', 5346),
(5, N'Mamed', N'Mamedov', N'CTO', 753),
(6, N'Axmed', N'Mamedov', N'Web Developer', 864),
(7, N'Abulfaz', N'Ismailov', N'Operations and production.', 2346),
(8, N'Qara', N'Qarashov', N'CFO', 678),
(9, N'Musa', N'Axundov', N'CMO', 538),
(10, N'Metleb', N'Aydinov', N'CTO', 6593),
(11, N'Rasim', N'Bəşirov', N'COO', 4608),
(12, N'Qasim', N'Niyaməddinov', N'Vice President', 2345),
(13, N'Asim', N'Nizamioglu', N'President', 1422),
(14, N'Razin', N'Dadashov', N'Executive', 8965),
(15, N'Ismayil', N'Aziyev', N'Marketing Manager', 6438),
(16, N'Cabbar', N'Cabbarov', N'Product Manager', 542),
(17, N'Penax', N'Penaxov', N'Project Manager', 235),
(18, N'Vusal', N'Rasimov', N'Finance Manager', 754),
(19, N'Rasim', N'Vusalov', N'HR', 5624),
(20, N'Aziz', N'Vekilov', N'HR operator', 2434),
(21, N'Terlan', N'Azizov', N'Marketing Specialist', 7543),
(22, N'Telman', N'Terlanov', N'Business analyst', 643)

--Finding average salary
SELECT AVG(Salary) 'Average Salary' FROM Employees

-- Finding who has salary more than average salary in record
SELECT Id, (Name + ' ' + SurName) 'Full Name' FROM Employees WHERE Salary > (SELECT AVG(Salary) FROM Employees)

-- Finding max and min salary 
SELECT Min(Salary) 'Min Salary in rec' FROM Employees

SELECT Max(Salary) 'Max Salary in rec' FROM Employees




--------------------------
INSERT INTO Employees(Id, Name, SurName, Position, Salary) Values(3, 'Shirali', 'Axundov', 'Cleaner', 10)

DELETE FROM Employees WHERE Id=3

