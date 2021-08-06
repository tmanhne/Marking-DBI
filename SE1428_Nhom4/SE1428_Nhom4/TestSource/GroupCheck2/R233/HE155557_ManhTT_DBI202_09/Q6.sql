
SELECT TOP 1 Jobs.JobID
      ,[JobTitle]
	  ,COUNT(Employees.JobID) AS NumberOfEmployees
  FROM [dbo].[Jobs],dbo.Employees WHERE Employees.JobID=Jobs.JobID
  GROUP BY Jobs.JobID
      ,[JobTitle]
      ,[min_salary]
	  ORDER BY NumberOfEmployees DESC

