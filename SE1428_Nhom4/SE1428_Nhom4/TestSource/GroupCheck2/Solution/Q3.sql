
SELECT [JobID]
      ,[JobTitle]
      ,[min_salary]
  FROM [dbo].[Jobs] WHERE min_salary >5000 AND JobTitle LIKE '%Manager%' ORDER BY min_salary DESC

