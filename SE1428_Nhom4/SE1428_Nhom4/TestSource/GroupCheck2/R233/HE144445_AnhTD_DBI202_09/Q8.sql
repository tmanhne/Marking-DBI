
CREATE PROCEDURE pr1(@id NVARCHAR(20),@ax INT OUTPUT)
AS
BEGIN
SELECT @ax = COUNT(*)
FROM [dbo].[Departments],dbo.Locations WHERE Departments.LocationID = Locations.LocationID AND CountryID =@id
END


