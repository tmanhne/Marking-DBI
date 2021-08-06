
CREATE TRIGGER Tr1 ON dbo.Employees
    AFTER INSERT
AS
    BEGIN
        SELECT  i.EmployeeID ,
                i.FirstName ,
                i.LastName ,
                CASE WHEN i.DepartmentID = i.DepartmentID THEN i.DepartmentID
                     ELSE NULL
                END AS DepartmentID ,
                CASE WHEN DepartmentName = DepartmentName THEN DepartmentName
                     ELSE 'NULL'
                END AS DepartmentName
        FROM    Inserted i
                LEFT JOIN dbo.Departments ON i.DepartmentID = Departments.DepartmentID
    END 
GO

INSERT  INTO [dbo].[Employees]
        ( [EmployeeID], [FirstName], [LastName], [Email], [JobID], [DepartmentID] )
VALUES  ( 1003, 'Alain', 'Bouchher', 'alain.boucher@gmail.com', 'SH_CLERK', 50 )
			,
        ( 1004, 'Muriel', 'Visani', 'muriel.visani@gmail.com', 'SA_REP', NULL )
DELETE  dbo.Employees
WHERE   EmployeeID = 1003
DELETE  dbo.Employees
WHERE   EmployeeID = 1004
DROP TRIGGER Tr1