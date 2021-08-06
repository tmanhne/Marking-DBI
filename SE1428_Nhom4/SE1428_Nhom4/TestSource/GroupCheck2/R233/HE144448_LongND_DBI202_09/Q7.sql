
SELECT [EmployeeID]
      ,[FirstName]
      ,[LastName]
      ,[Salary]
      ,Employees.DepartmentID
	  ,DepartmentName
	  ,CASE
	  WHEN B.NumberOfSubordinates = B.NumberOfSubordinates THEN B.NumberOfSubordinates
	  ELSE '0'
	  END AS number

  FROM [dbo].[Employees] 
  LEFT  JOIN (select  ManagerID, count(ManagerID) as NumberOfSubordinates from dbo.Employees group by ManagerID) B
  ON b.ManagerID = Employees.EmployeeID 
  JOIN dbo.Departments ON Departments.DepartmentID = Employees.DepartmentID
  WHERE NumberOfSubordinates>0 or Salary>10000
  ORDER BY B.NumberOfSubordinates DESC , LastName ASC

