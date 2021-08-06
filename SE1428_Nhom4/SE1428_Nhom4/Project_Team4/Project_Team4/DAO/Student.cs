using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Project_Team4.DAO
{
    public class Student
    {
        private string studentID;
        private string studentName;
        private string examPaperCode;
        private double mark;
        private string markingDetail;

        public string StudentID { get => studentID; set => studentID = value; }
        public string StudentName { get => studentName; set => studentName = value; }
        public string ExamPaperCode { get => examPaperCode; set => examPaperCode = value; }
        public double Mark { get => mark; set => mark = value; }
        public string MarkingDetail { get => markingDetail; set => markingDetail = value; }

        public Student(string studentID, string studentName, string examPaperCode, double mark, string markingDetail)
        {
            this.StudentID = studentID;
            this.StudentName = studentName;
            this.ExamPaperCode = examPaperCode;
            this.Mark = mark;
            this.MarkingDetail = markingDetail;
        }

        public static int AddStudent(Student student, string tableName)
        {
            string sql = "INSERT INTO [dbo].[" + tableName + "] VALUES" +
                " (@studentID ,@studentName ,@examPaperCode ,@mark ,@markingDetail)";
            SqlParameter[] param = new SqlParameter[] {
                new SqlParameter("@studentID", SqlDbType.VarChar),
                new SqlParameter("@studentName", SqlDbType.NVarChar),
                new SqlParameter("@examPaperCode", SqlDbType.VarChar),
                new SqlParameter("@mark", SqlDbType.Float),
                new SqlParameter("@markingDetail", SqlDbType.NVarChar)
            };
            // gan gia tri cho cac phan tu trong mang
            param[0].Value = student.StudentID;
            param[1].Value = student.StudentName;
            param[2].Value = student.ExamPaperCode;
            param[3].Value = student.Mark;
            param[4].Value = student.MarkingDetail;

            return Database.ExecuteSQL(sql, param);
        }

        public static System.Data.DataTable GetResult(string table)
        {
            string sql = "SELECT * FROM [dbo].[" + table + "]";
            return Database.GetBySQL(sql, "DBI202_PE_Result");
        }

        
    }
}
