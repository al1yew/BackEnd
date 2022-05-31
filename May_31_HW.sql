CREATE DATABASE Store

USE Store

CREATE TABLE Brands
(
	Id INT CONSTRAINT PK_Brands_Id PRIMARY KEY Identity,
	Name nvarchar(50) NOT NULL CONSTRAINT CK_Brands_Name CHECK(LEN(Name) > 1)
)

INSERT INTO Brands
VALUES
('Apple'),
('Hp'),
('Samsung'),
('Xiaomi'),
('Huawei'),
('Asus'),
('Dell')


CREATE TABLE Notebooks
(
	 Id INT CONSTRAINT PK_Notebooks_Id PRIMARY KEY Identity,
	 Name nvarchar(50) NOT NULL CONSTRAINT CK_Notebooks_Name CHECK(LEN(Name) > 1), 
	 Price Money NOT NULL CONSTRAINT CK_Notebooks_Price CHECK(Price > 100),
	 BrandId INT CONSTRAINT FK_Notebooks_BrandId FOREIGN KEY REFERENCES Brands(Id)
)

INSERT INTO Notebooks
VALUES
('250 G5', 943, 2),
('250 G6', 1158, 2),
('250 G7', 1251, 2),
('Air', 2363, 1),
('Pro 13', 2975, 1),
('Pro 15', 3439, 1),
('ROG', 2928, 6),
('ROG PRO', 3968, 6),
('VIVOBOOK 15', 1536, 6),
('VIVOBOOK 14', 1325, 6),
('Mate X', 1600, 5),
('Mate X PRO', 1900, 5),
('Mate XL PRO', 1864, 5),
('Mate XXL PRO', 1253, 5),
('Mi Notebook Air', 1753, 4),
('Mi Notebook Pro', 2153, 4),
('Lustrous Grey', 4681, 4),
('Galaxy Book', 1874, 3),
('Galaxy Book PRO', 3274, 3),
('Galaxy Book AIR', 2574, 3),
('Galaxy Book AIR PRO', 3367, 3)


CREATE TABLE Phones
(
	Id INT CONSTRAINT PK_Phones_Id PRIMARY KEY Identity,
	Name nvarchar(50) NOT NULL CONSTRAINT CK_Phones_Name CHECK(LEN(Name) > 1), 
	Price Money NOT NULL CONSTRAINT CK_Phones_Price CHECK(Price > 100),
	BrandId INT CONSTRAINT FK_Phones_BrandId FOREIGN KEY REFERENCES Brands(Id)
)

INSERT INTO Phones
VALUES
('13', 2463, 1),
('13 Pro', 3075, 1),
('13 Pro Max', 3339, 1),
('Mate Pad', 1600, 5),
('Mate Xs', 1900, 5),
('Nova 9 SE', 1864, 5),
('P50E', 1853, 5),	
('Poco 5', 1753, 4),
('Poco 4', 2153, 4),
('Poco 6', 4681, 4),
('A11', 275, 3),
('A21', 285, 3),
('A31', 374, 3),
('A41', 467, 3),
('A51', 567, 3),
('A61', 667, 3),
('A71', 767, 3),
('A81', 867, 3),
('A91', 967, 3)



--3) Notebooks Adini, Brandin Adini BrandName kimi ve Qiymetini Cixardan Query.
SELECT Notebooks.Name, Brands.Name as [BrandsName] FROM Notebooks Join Brands on Notebooks.BrandId = Brands.Id

--4) Phones Adini, Brandin Adini BrandName kimi ve Qiymetini Cixardan Query.
SELECT Phones.Name, Brands.Name as [BrandsName] FROM Phones Join Brands on Phones.BrandId = Brands.Id

--5) Brand Adinin Terkibinde Olan Butun Notebooklari Cixardan Query.
Select Brands.Name, Notebooks.Name FROM Brands Join Notebooks On Brands.Id = Notebooks.BrandId

--6) Qiymeti 2000 ve 5000 arasi ve ya 5000 den yuksek Notebooklari Cixardan Query.
Select (Name + ', Price: ' + CONVERT(nvarchar(20), Price)) FROM Notebooks WHERE Price between 2000 and 5000

--7) Qiymeti 1000 ve 1500 arasi ve ya 1500 den yuksek Phonelari Cixardan Query. 
Select (Name + ', Price: ' + CONVERT(nvarchar(20), Price)) FROM Phones WHERE (( Price between 1000 and 1500) or Price > 1500 )

--8) Her Branda Aid Nece dene Notebook Varsa Brandin Adini Ve Yaninda Sayini Cixardan Query.
Select Brands.Name, Count(Brands.Id) From Notebooks Join Brands On Notebooks.BrandId = Brands.Id Group By Brands.Name

--9) Her Branda Aid Nece dene Phone Varsa Brandin Adini Ve Yaninda Sayini Cixardan Query.
Select Brands.Name, Count(Brands.Id) From Phones Join Brands On Phones.BrandId = Brands.Id Group By Brands.Name

--10) Hem Phone Hem de Notebookda Ortaq Olan Name ve BrandId Datalarni Bir Cedvelde Cixardan Query.
SELECT Notebooks.Name FROM Notebooks Join Phones On Phones.Id = Notebooks.BrandId 
--Alinmadi

--11) Phone ve Notebook da Id, Name, Price, ve BrandId Olan Butun Datalari Cixardan Query.
SELECT * FROM Phones Union All SELECT * FROM Notebooks 

--12) Phone ve Notebook da Id, Name, Price, ve Brandin Adini BrandName kimi Olan Butun Datalari Cixardan Query.
SELECT Phones.Id, Brands.Name 'BrandName', Phones.Name 'Model', (CONVERT(nvarchar(200), Phones.Price)+ ' AZN')
FROM Phones Join Brands on Phones.BrandId = Brands.id
Union All 
SELECT Notebooks.Id, Brands.Name 'BrandName', Notebooks.Name 'Model', (CONVERT(nvarchar(200), Notebooks.Price)+ ' AZN')
FROM Notebooks Join Brands on Notebooks.BrandId = Brands.id 

--13) Phone ve Notebook da Id, Name, Price, ve Brandin Adini BrandName kimi Olan Butun Datalarin 
--Icinden Price 1000-den Boyuk Olan Datalari Cixardan Query.

SELECT Phones.Id, Phones.Name, Phones.Price, Brands.Name 'BrandName' FROM Phones 
Join Brands on Phones.BrandId = Brands.Id
WHERE Phones.Price > 1000
Union All 
SELECT Notebooks.Id, Notebooks.Name, Notebooks.Price, Brands.Name 'BrandName' FROM Notebooks 
Join Brands on Notebooks.BrandId = Brands.Id 
WHERE Notebooks.Price > 1000 

--14) Phones Tabelenden Data Cixardacaqsiniz Amma Nece Olacaq Brandin Adi (BrandName kimi), 
--Hemin Brandda Olan Telefonlarin Pricenin Cemi (TotalPrice Kimi) 
--ve Hemin Branda Nece dene Telefon Varsa Sayini (ProductCount Kimi) Olan Datalari Cixardan Query
SELECT Brands.Name 'Brand Name', Sum(Phones.Price) 'Total Price', Count(Brands.Name) 'Product Count' FROM Phones 
Join Brands on Phones.BrandId = Brands.Id 
GROUP BY Brands.Name HAVING COUNT(Brands.Name) > 0

--15) Notebooks Tabelenden Data Cixardacaqsiniz Amma Nece Olacaq Brandin Adi (BrandName kimi), Hemin Brandda Olan Telefonlarin Pricenin Cemi (TotalPrice Kimi) , Hemin Branda Nece dene Telefon Varsa Sayini (ProductCount Kimi) Olacaq ve Sayi 3-ve 3-den Cox Olan Datalari Cixardan Query.Misal
SELECT Brands.Name 'BrandName', Sum(Notebooks.Price) 'TotalPrice', Count(Brands.Name) 'ProductCount' FROM Notebooks 
Join Brands on Notebooks.BrandId = Brands.Id 
GROUP BY Brands.Name HAVING COUNT(Brands.Name) > 0