CREATE DATABASE Market 

USE Market

CREATE TABLE Products(Id INT, Name nvarchar(50), Price INT)

ALTER TABLE Products ADD Brand nvarchar(30)

INSERT INTO Products (Id, Name, Price, Brand)
VALUES
(1, 'Cola', 3.2, 'Hansisa_Brend1'),
(2, 'Fanta', 2, 'Hansisa_Brend2'),
(3, 'Sprite', 3, 'Hansisa_Brend3'),
(4, 'Bread', 2.2, 'Hansisa_Brend4'),
(5, 'Butter', 4.2, 'Hansisa_Brend5'),
(6, 'Tea', 5.8, 'Hansisa_Brend6'),
(7, 'Cookies', 1.2, 'Hansisa_Brend7'),
(8, 'Cheese', 3.7, 'Hansisa_Brend8'),
(9, 'Cucumber', 4.2, 'Hansisa_Brend9'),
(10, 'Tomato', 32, 'Hansisa_Brend10'),
(11, 'Apple', 22, 'Hansisa_Brend11'),
(12, 'Pear', 122, 'Hansisa_Brend12'),
(13, 'Melon', 37, 'Hansisa_Brend13'),
(14, 'Strawberry', 32.1, 'Hansisa_Brend14'),
(15, 'Raspberry', 3.24, 'Hansisa_Brend15'),
(16, 'Chocolate', 3.4, 'Hansisa_Brend156'),
(17, 'Kiwi', 15, 'Hansisa_Brend17'),
(18, 'Grapefruit', 14, 'Hansisa_Brend18'),
(19, 'Eraser', 0.8, 'Hansisa_Brend19'),
(20, 'Pen', 0.7, 'Hansisa_Brend20'),
(21, 'Pepsi', 3, 'Hansisa_Brend21'),
(22, 'Pineapple', 5, 'Hansisa_Brend22'),
(23, 'Mamed', 6, 'Hansisa_Brend23'),
(24, 'Axmed', 8, 'Hansisa_Brend24'),
(25, 'Juice', 10, 'Hansisa_Brend25')

INSERT INTO Products (Id, Name, Price, Brand)
VALUES
(26, 'Coca-Cola', 1, 'Hansisa_Brend26')

DELETE FROM Products WHERE Id>15


--10 dan baha olanlar
SELECT * FROM Products WHERE Price > 10

-- average price
Select AVG(Price) FROM Products

--average price-dan boyuk olanlar
SELECT * FROM Products WHERE Price  > (SELECT AVG(Price) FROM Products)

-- average price-dan kicik giymetler
SELECT * FROM Products WHERE Price  < (SELECT AVG(Price) FROM Products)

-- axirinci taskin axirinci setiri
SELECT (Name + ' - ' + Brand + ';' + ' Price: ' + CONVERT(NVARCHAR(20), Price) +' azn') 'Product Information' FROM Products WHERE LEN(Brand) > 5

--maragli 100 balliq variant :DD
SELECT (Name + ' - ' + SUBSTRING(Brand, CHARINDEX('_',Brand), LEN(Brand)) + ';' + ' Price: ' + CONVERT(NVARCHAR(20), Price) +' azn') 'Product Information' FROM Products WHERE LEN(Brand) > 5

--bilirem hamsini getirir amma brand adlari fikirleshmek cetin oldu )))