
SELECT dbo.Locations.LocationID
      ,[StreetAddress]
      ,[City]
      ,[StateProvince]
      ,[CountryID]
	  ,COUNT(dbo.Departments.LocationID)AS NumberOfDepartments
  FROM [dbo].[Locations] LEFT JOIN dbo.Departments
  ON  Departments.LocationID=Locations.LocationID
  GROUP BY dbo.Locations.LocationID
	  ,[StreetAddress]
      ,[City]
      ,[StateProvince]
      ,[CountryID]
	ORDER BY NumberOfDepartments DESC,Locations.LocationID ASC

