CREATE DATABASE Library

USE Library

CREATE TABLE Authors
(
	Id INT CONSTRAINT PK_Authors_Id PRIMARY KEY Identity,
	Name nvarchar(255) CONSTRAINT CK_Authors_Name CHECK(LEN(Name) > 1),
	SurName nvarchar(255) CONSTRAINT CK_Authors_SurName CHECK(LEN(SurName) > 1)
)

CREATE TABLE Books
(
	Id INT CONSTRAINT PK_Books_Id PRIMARY KEY Identity,
	Name nvarchar(100) CONSTRAINT CK_Books_Name CHECK(LEN(Name) BETWEEN 2 AND 100),
	PageCount INT CONSTRAINT CK_Books_PageCount CHECK(PageCount >= 10),
	AuthorId INT CONSTRAINT FK_Books_AuthorId FOREIGN KEY REFERENCES Authors(Id)
)

INSERT INTO Authors 
VALUES
('Stephen', 'King'),
('J.K.', 'Rowling'),
('Amy', 'Tan'),
('Khaled', 'Hosseini'),
('Tana', 'French')

INSERT INTO Books 
VALUES
('It', 543, 1),
('The Shining', 524, 1),
('Misery', 629, 1),
('Carrie', 785, 1),
('Harry Potter', 683, 2),
('Harry Potter 2', 755, 2),
('Harry Potter 3', 531, 2),
('Harry Potter 4', 624, 2),
('Harry Potter 5', 764, 2),
('The Joy Luck Club', 311, 3),
('The Kitchen Gods Wife', 855, 3),
('The Hundred Secret Senses', 345, 3),
('The Bonesetters Daughter', 286, 3),
('The Valley of Amazement', 717, 3),
('THE KITE RUNNER', 863, 4),
('And the Mountains Echoed', 433, 4),
('A Thousand Splendid Suns', 866, 4),
('Sea Prayer', 865, 4),
('The Kite Runner', 326, 4),
('The Searcher', 289, 5),
('The Likeness', 429, 5),
('Faithful Place', 678, 5),
('The Secret Place', 435, 5)

--Id, Name, PageCount ve AuthorFullName columnlarinin valuelarini qaytaran bir view yaradin

CREATE VIEW usv_BooksAndAuthors
AS
SELECT b.Id [Book Id], b.Name [Book Name], b.PageCount [Pages], (a.Name + a.SurName) [Author Full Name] FROM Books b
Join Authors a On b.AuthorId = a.Id

Select * FROM usv_BooksAndAuthors --ishledi 

--Gonderilmis axtaris deyeri name ve ya authorFullNamelerinde olan procedure,
--Book-larin Id, Name, PageCount, AuthorFullName columnlari seklinde gosteren procedure yazin

CREATE PROCEDURE usp_GetIdNamePagesAuthorName 
@Name nvarchar(100),
@AuthorName nvarchar(100)
AS
BEGIN
	SELECT b.[Book Id], b.[Book Name], b.Pages, b.[Author Full Name] FROM usv_BooksAndAuthors b 
	WHERE b.[Book Name] LIKE '%' + @Name + '%' AND b.[Author Full Name] LIKE '%' + @AuthorName + '%'
END

exec usp_GetIdNamePagesAuthorName 'Searcher',  'Tana' --it works



--insert, update ve deleti ucun (her biri ucun ayrica) procedure yaradin

--Insert Book
CREATE PROCEDURE usp_InsertInTableBooks
@Name nvarchar(255),
@PageCount INT,
@AuthorId INT
AS
BEGIN
	INSERT INTO Books
	VALUES
	(@Name, @PageCount, @AuthorId)
END

exec usp_InsertInTableBooks'Kitab Oxumuram', 500, 2


--Insert Author
CREATE PROCEDURE usp_InsertInTableAuthors
@Name nvarchar(100),
@SurName nvarchar(255)
AS
BEGIN
	INSERT INTO Authors
	VALUES
	(@Name, @SurName)
END

exec usp_InsertInTableAuthors 'Ismail', 'Rza'

--Delete Author
CREATE PROCEDURE usp_DeleteAuthorFromAuthorTable
@Name nvarchar(100),
@SurName nvarchar(255)
AS
BEGIN
	DELETE FROM Authors WHERE Name = @Name OR SurName = @SurName
END

exec usp_DeleteAuthorFromAuthorTable 'Mamed', 'Mamedov'



--Delete Book
CREATE PROCEDURE usp_DeleteBookFromBooksTable
@Name nvarchar(255)
AS
BEGIN
	DELETE FROM Books WHERE Name = @Name
END

exec usp_DeleteAuthorFromBooksTable 'Carrie'

--Update Book
CREATE PROCEDURE usp_UpdateBook
@Name nvarchar(255),
@Id INT
AS
BEGIN
	UPDATE Books
	SET Name = @Name WHERE Id = @Id;
END

exec usp_UpdateBook 'Oxumuram El Cekin', 3

--Update Author
CREATE PROCEDURE usp_UpdateAuthor
@Name nvarchar(100),
@SurName nvarchar(255),
@Id INT
AS
BEGIN
	UPDATE Authors
	SET Name = @Name, SurName = @SurName
	WHERE Id = @Id;
END

exec usp_UpdateAuthor'Isi', 'Isiyev', 3

--Authors-larin Id,FullName,BooksCount,MaxPageCount seklinde qaytaran view yaradirsiniz Id-author id-si, FullName - Name ve Surname birlesmesi, BooksCount 
--Hemin authorun elaqeli oldugu kitablarin sayi, MaxPageCount - hemin authorun elaqeli oldugu kitablarin icerisindeki max pagecount deyeri
--bismillah edek yazaq


Create View usv_GetAuthorsAndBooks
AS
Select 
a.Id, 
(a.Name + ' ' + a.SurName) [Full Name], 
MAX(PageCount) [MaxPageCount],
COUNT(a.Name) [BooksCount]
FROM Authors a
JOIN Books b 
On b.AuthorId = a.Id GROUP by a.Name, a.Id, a.SurName

--niye bu mennen group by isteyir 
