CREATE DATABASE COMPANY1
--created and used base
USE	COMPANY1


--first create deaprtments - one side
CREATE TABLE Departments
(
	Id INT NOT NULL CONSTRAINT CK_Department_Id PRIMARY KEY Identity, 
	Name nvarchar(40) NOT NULL CONSTRAINT CK_DepartmentName CHECK(LEN(Name) > 2) 
)


--then create Employees - Many side
CREATE TABLE Employees
(
	Id INT Identity NOT NULL CONSTRAINT CK_Employee_Id PRIMARY KEY , 
	FullName nvarchar(25) NOT NULL CONSTRAINT CK_Employee_FullName CHECK(LEN(FUllName) > 3), 
	DepartmentId INT CONSTRAINT CK_Department_Id_Employees FOREIGN KEY REFERENCES Departments(Id), 
	Salary INT CONSTRAINT CK_Salary CHECK(Salary > 0), 
	Email nvarchar(50) NOT NULL Unique CONSTRAINT CK_Email CHECK(Email LIKE '%@%')
)

--herenin oz id-si primary keydi, unique dir ve identity dir. Amma her employeenin oz departmentId-si var deye, 
--DepartmentId Foreign keydir, cunki onu Bashqa tabledan goturur. bu halda relation yaradilir, ve sehv id yazanda qebul etmir.

--added to Departments table, id is primary key 
INSERT INTO Departments
Values
('Marketing'),
('Management'),
('AUDIT'),
('Finance')


--added to Employees table, id is primary key 
INSERT INTO Employees(FullName, DepartmentId, Email, Salary)
Values
(N'Vasif Aliyev', 1, 'nese1@mail.ru', 15000),
(N'Asif Aliyev', 1, 'nese2@mail.ru', 1000),
(N'Agasif Mamedov', 1, 'nese3@mail.ru', 5346),
(N'Mamed Mamedov', 3, 'nese4@mail.ru', 753),
(N'Axmed Mamedov', 3, 'nese5@mail.ru', 864),
(N'Abulfaz Ismailov', 3, 'nese6@mail.ru', 2346),
(N'Qara Qarashov', 3, 'nese7@mail.ru', 678),
(N'Musa Axundov', 3, 'nese8@mail.ru', 538),
(N'Metleb Aydinov', 2, 'nese9@mail.ru', 6593),
(N'Rasim BəşirovC', 2, 'nese10@mail.ru', 4608),
(N'Qasim Niyaməddinov', 2, 'nese11@mail.ru', 2345),
(N'Asim Nizamioglu', 2, 'nese12@mail.ru', 1422),
(N'Razin Dadashov', 2, 'nese13@mail.ru', 8965),
(N'Ismayil Aziyev', 1, 'nese14@mail.ru', 6438)


--can be added to table due to id between 1 and 4
INSERT INTO Employees(FullName, DepartmentId, Email, Salary)
Values
(N'Aga Agayev', 3, 'nese15@mail.ru', 648)


--does not work because of primary and foreign key, id 7 is not appropriate!
INSERT INTO Employees(FullName, DepartmentId, Email, Salary)
Values
(N'Aga Agayev', 7, 'nese15@mail.ru', 648)





--To delete if does not work
DROP TABLE Employees

DROP TABLE Departments