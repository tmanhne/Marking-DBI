
SELECT [EmployeeID]
      ,[FirstName]
      ,[LastName]
      ,[Salary]
	  ,DepartmentName
	  ,StateProvince
	  ,CountryID
  FROM [dbo].[Employees], dbo.Departments , dbo.Locations WHERE Departments.DepartmentID =Employees.DepartmentID AND Departments.LocationID = Locations.LocationID AND StateProvince ='Washington' AND Salary>3000
