
CREATE PROCEDURE pr1(@cate NVARCHAR(20),@x INT OUTPUT)
AS
BEGIN
SELECT @x = COUNT(dbo.Products.ProductId)
FROM dbo.Products WHERE CategoryId= @cate
END



