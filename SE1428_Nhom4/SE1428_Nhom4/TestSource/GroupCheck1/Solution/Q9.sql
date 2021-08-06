
CREATE TRIGGER Tr1
ON dbo.Products
AFTER INSERT
AS BEGIN
SELECT *
FROM Inserted i
END 
GO
INSERT INTO [dbo].[Products] VALUES
('P0007' ,'Magic Mouse 2' ,'C0001' ,'unit-2' ,1 ,1 ,1 ,'2021-01-01'), 
('P0008' ,'Magic Mouse 2' ,'C0001' ,'unit-2' ,1 ,1 ,1 ,'2021-01-01')
DELETE dbo.Products WHERE ProductId ='P0007'
DELETE dbo.Products WHERE ProductId ='P0008'
DROP TRIGGER Tr1

