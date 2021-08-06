USE master;
GO

CREATE DATABASE ProductDB;
GO

USE ProductDB;
GO

CREATE TABLE Categories
(
    CategoryId CHAR(5) PRIMARY KEY,
    CategoryName NVARCHAR(20),
    Description nText
)

GO
CREATE TABLE Products
(
    ProductId CHAR(5) PRIMARY KEY,
    ProductName NVARCHAR(50),
    CategoryId CHAR(5) FOREIGN KEY REFERENCES Categories(CategoryId),
    Unit NVARCHAR(30),
    Price INT,
    Quantity int,
    Discontinued BIT,
    CreateDate DATE
)

GO

INSERT INTO Categories VALUES('C0001','Mouse','Magic mouse - Apple')
INSERT INTO Categories VALUES('C0002','Keyboard','Bluetooth keyboard')
INSERT INTO Categories VALUES('C0004','RAM','Main memory')
INSERT INTO Categories VALUES('C0005','HDD','Hard Disk Drive')

INSERT INTO Products VALUES('P0001','Magic Mouse 2', 'C0001', 'unit-2',100,10,0,'2021-01-01')
INSERT INTO Products VALUES('P0002','Magic Mouse 2', 'C0001', 'unit-3',80,15,1,'2021-01-15')
INSERT INTO Products VALUES('P0003','Mitsumi', 'C0002', 'unit-5',30,20,0,'2021-02-02')