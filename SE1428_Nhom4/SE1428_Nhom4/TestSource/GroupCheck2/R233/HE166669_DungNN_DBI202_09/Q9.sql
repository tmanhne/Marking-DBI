
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
