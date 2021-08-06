using Project_Team3.DAO;
using Project_Team4.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Project_Team4
{
    public partial class FrmProject : Form
    {
        public FrmProject()
        {
            InitializeComponent();
            pgRun.Minimum = 0;
            pgRun.Maximum = 100;
        }

        public static bool checkData(DataTable tbl1, DataTable tbl2)
        {
            int r1 = tbl1.Rows.Count;
            int c1 = tbl1.Columns.Count;
            int r2 = tbl2.Rows.Count;
            int c2 = tbl2.Columns.Count;

            if (r1 != r2 || c1 != c2)
                return false;
            int count = 0;

            for (int i = 0; i < r1; i++) {
                for (int j = 0; j < r1; j++) {
                    int countt = count;
                    for (int c = 0; c < c1; c++) {
                        if (Equals(tbl1.Rows[i][c], tbl2.Rows[j][c])) {
                            count++;
                        }
                    }
                    if ((count - countt) == c1) break;
                    else count = countt;
                }
            }
            if ((count / c1) == r1)
                return true;
            else
                return false;

        }
        public static bool checkSort(DataTable tbl1, DataTable tbl2)
        {
            int r1 = tbl1.Rows.Count;
            int c1 = tbl1.Columns.Count;
            int r2 = tbl2.Rows.Count;
            int c2 = tbl2.Columns.Count;

            for (int i = 0; i < r1; i++) {
                for (int c = 0; c < c1; c++) {
                    if (!Equals(tbl1.Rows[i][c], tbl2.Rows[i][c]))
                        return false;
                }
            }
            return true;
        }
        public string getFolderPath()
        {
            var fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            if (result == DialogResult.OK && !string.IsNullOrEmpty(fbd.SelectedPath)) {
                string path = fbd.SelectedPath;
                return path;
            }
            return "";
        }

        static string procedureData = "";
        static string procedereTest = "";
        static string triggerTest = "";
        static string triggerData = "";
        static string procedureErorr = "";
        static string triggerErorr = "";

        public static DataTable[] getAnswerFile(string directory, string dbName)
        {
            DataTable[] dtAnswer = new DataTable[10];
            string[] sfiles = Directory.GetFiles(directory);
            foreach (string f in sfiles) {
                //check the format of Question(Q1.sql-Q10.sql)
                int index = -1;
                string[] info = f.Split('\\');
                string[] sample = { "Q1.sql", "Q2.sql", "Q3.sql", "Q4.sql", "Q5.sql", "Q6.sql", "Q7.sql", "Q8.sql", "Q9.sql", "Q10.sql" };
                string ques = info[info.Length - 1];
                foreach (string s in sample) {
                    if (ques.Equals(s)) {
                        //get sql script in QX.sql
                        string script = File.ReadAllText(@f);
                        //put data of QX to data Array index X-1 (Q1.sql to dtAnswer[0])
                        //Q10 (last situation)
                        if (!int.TryParse(ques.Substring(ques.Length - 6, 2), out index)) {
                            //Q1-9
                            int.TryParse(ques.Substring(ques.Length - 5, 1), out index);
                        }
                        try {
                            if (index < 8 || index > 9) {
                                dtAnswer[index - 1] = Database.GetBySQL(script, dbName);
                            }
                            if (index == 8) {
                                int a = 0;
                                try { a = Database.ExecuteNonSQL(script, dbName); }
                                catch { }
                                dtAnswer[index - 1] = Database.GetBySQL(procedereTest, dbName);

                                //MessageBox.Show(script+ "\n" + a + "|" + dtAnswer[index-1].Rows.Count);
                            }
                            if (index == 9) {
                                int a = 0;
                                try { a = Database.ExecuteNonSQL(script, dbName); }
                                catch { }
                                dtAnswer[index - 1] = Database.GetBySQL(triggerTest, dbName);
                            }
                        }
                        catch (Exception ex) {
                            //MessageBox.Show("Error to load file Q" + (index) + "\n" + ex.Message);
                            if (index == 8) procedureErorr = ex.Message;
                            if (index == 9) triggerErorr = ex.Message;
                        }
                    }
                }
            }
            return dtAnswer;
        }
        public static DataTable[] getSolutionFile(string directory, string dbName)
        {
            DataTable[] dtSolution = new DataTable[10];
            string[] sfiles = Directory.GetFiles(directory);
            // put data of Solution to data Array
            foreach (string f in sfiles) {
                string script = File.ReadAllText(@f);
                int index = -1;
                //put data of QX   to data Array index X-1
                //Q10 (last situation)
                if (!int.TryParse(f.Substring(f.Length - 6, 2), out index)) {
                    //Q1-9
                    int.TryParse(f.Substring(f.Length - 5, 1), out index);
                }
                try {
                    if (index < 8 || index > 9) {
                        dtSolution[index - 1] = Database.GetBySQL(script, dbName);
                    }
                    if (index == 8) {
                        string[] info = script.Split(new[] { "GO" },
                            StringSplitOptions.None);
                        procedureData = info[0];
                        procedereTest = info[1];
                        int a = 0;
                        try { a = Database.ExecuteNonSQL(procedureData, dbName); }
                        catch { }

                        dtSolution[index - 1] = Database.GetBySQL(procedereTest, dbName);
                    }
                    if (index == 9) {
                        string[] info = script.Split(new[] { "GO" },
                            StringSplitOptions.None);
                        triggerData = info[0];
                        triggerTest = info[1];

                        int a = 0;
                        try { a = Database.ExecuteNonSQL(triggerData, dbName); }
                        catch { }
                        dtSolution[index - 1] = Database.GetBySQL(triggerTest, dbName);
                    }
                }
                catch (Exception) {
                    return null;
                }
            }
            return dtSolution;
        }

        public static Student getMarkedStudent(DataTable[] dtsAnswer, DataTable[] dtSolution, string studentID, string studentName, string examPaperCode)
        {
            string mesage = "";
            double mark = 0;
            for (int i = 0; i < 10; i++) {
                mesage += "[QN=" + (i + 1) + "]";
                //student file available
                if (dtsAnswer[i] == null) {
                    mesage += " Empty.";
                    if (i == 7) mesage += "\n" + procedureErorr;
                    if (i == 8) mesage += "\n" + triggerErorr;
                }
                else {
                    //each question mark
                    double qMark = 0;
                    if (Validation.checkData(dtsAnswer[i], dtSolution[i])) {
                        mesage += "\t - Check Data: Passed => + 0,5";
                        qMark += 0.5;
                        if (Validation.checkSort(dtsAnswer[i], dtSolution[i])) {
                            mesage += "\t - Check Sort: Passed => + 0,5";
                            qMark += 0.5;
                        }
                        else {
                            mesage += "\t - Check Sort: Not pass => + 0";
                        }
                    }
                    else {
                        mesage += "\t - Check Data: Not pass => + 0\t Stop checking";
                    }
                    mesage += "\t=> Mark = " + qMark;
                    mark += qMark;
                }
                mesage += "\n";
            }
            return new Student(studentID, studentName, examPaperCode, mark, mesage);
        }

        DataTable[] dtSAnswer = new DataTable[10];
        DataTable[] dtSolution = new DataTable[10];

        private void putDataToDB(List<Student> list, string tableName)
        {
            if (list.Count != 0) {
                foreach (Student s in list) {
                    try {
                        Student.AddStudent(s, tableName);
                    }
                    catch (SqlException ex) {
                        MessageBox.Show(ex.Message);
                    }
                }
                label7.Text = "Done!";
                pgRun.Value = 100;
                //MessageBox.Show("Marked " + list.Count + " students! Data is put to Database \"DBI202_PE_Result\"");
            }
            else {
                MessageBox.Show("Not found any data! Check your input folder");
            }
        }


        public void LoadDataGridView()
        {
            dgvAll.DataSource = Database.GetAllTable();
            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            {
                btn.HeaderText = "Action";
                btn.Name = "btnExport";
                btn.Text = "Export Excel";
                btn.UseColumnTextForButtonValue = true;
                dgvAll.Columns.Add(btn);
            }
        }

        private void FrmProject_Load(object sender, EventArgs e)
        {
            //create DB TB
            Database.CreateDB();
            LoadDataGridView();
            dgvAll.AllowUserToAddRows = false;
            cboName.DataSource = Database.GetNameDatabase();
            cboName.DisplayMember = "name";
            cboName.ValueMember = "name";
            txtAnswer.ReadOnly = true;
            txtSolution.ReadOnly = true;
            rtxtMarked.ReadOnly = true;
            btnExcel.Enabled = false;
        }

        // export excel
        public void ExportExcel(string tableName)
        {
            using (SaveFileDialog sfd = new SaveFileDialog(){ FileName = tableName,  Filter = "Excel File|*.xlsx"}) {
                if (sfd.ShowDialog() == DialogResult.OK) {
                    try {
                        Microsoft.Office.Interop.Excel.ApplicationClass ExcelApp
                            = new Microsoft.Office.Interop.Excel.ApplicationClass();
                        ExcelApp.Application.Workbooks.Add(Type.Missing);
                        DataTable dtMainSQLData = Student.GetResult(tableName);
                        DataColumnCollection dcCollection = dtMainSQLData.Columns;
                        for (int i = 1; i < dtMainSQLData.Rows.Count + 1; i++) {
                            for (int j = 1; j < dtMainSQLData.Columns.Count + 1; j++) {
                                if (i == 1) {
                                    ExcelApp.Cells[i, j] = dcCollection[j - 1].ToString();
                                    ExcelApp.Cells[i + 1, j] = dtMainSQLData.Rows[i - 1][j - 1].ToString();
                                }
                                else
                                    ExcelApp.Cells[i + 1, j] = dtMainSQLData.Rows[i - 1][j - 1].ToString();
                            }
                        }
                        ExcelApp.ActiveWorkbook.SaveCopyAs(sfd.FileName);
                        ExcelApp.ActiveWorkbook.Saved = true;
                        ExcelApp.Quit();
                        MessageBox.Show("Data Exported Successfully into Excel File",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Process.Start(sfd.FileName);
                    }
                    catch (Exception) {
                        //MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            string[] arr = txtAnswer.Text.Split('\\');
            ExportExcel(arr[arr.Length - 1]);
        }

        private void btnAnswer_Click(object sender, EventArgs e)
        {
            txtAnswer.Text = getFolderPath();
        }

        private void btnSolution_Click(object sender, EventArgs e)
        {
            txtSolution.Text = getFolderPath();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            btnExcel.Enabled = false;
            if (txtAnswer.Text.Equals("") || txtSolution.Text.Equals("")) {
                MessageBox.Show("Please fill all fields", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else {
                rtxtMarked.Text = "";
                label7.Text = "";
                pgRun.Value = 0;
                List<Student> list = new List<Student>();
                // get all Student Folders
                string[] studentAnswer = Directory.GetDirectories(txtAnswer.Text);

                //get all Files from Solution Folder
                dtSolution = getSolutionFile(txtSolution.Text,
                    cboName.SelectedValue.ToString());
                if (dtSolution == null) {
                    MessageBox.Show("Check your solution path or database name", "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else {
                    // select each Student Folder
                    foreach (string file in studentAnswer) {
                        //E:\FPT University\2021 Summer\PRN292\New folder\StudentAnswer\HExxxxxx_Name_DBI202_xx
                        // RollID_Name_DBI202_09
                        string[] filePath = file.Split('\\');
                        string studentID = "";
                        string studentName = "";
                        string examPaperCode = "";
                        try {
                            string[] studentInfo = filePath[filePath.Length - 1].Split('_');
                            studentID = studentInfo[0];
                            var regex = @"^[a-zA-Z]{2}\d{6}$";
                            if (!Regex.IsMatch(studentID, regex)) throw new Exception();
                            studentName = studentInfo[1];
                            examPaperCode = studentInfo[3];

                            //get all Files from Student Folder
                            dtSAnswer = getAnswerFile(file,
                                cboName.SelectedValue.ToString());

                            rtxtMarked.Text += studentID + "\n";

                            //Marking
                            Student student = getMarkedStudent(dtSAnswer,
                                dtSolution, studentID, studentName, examPaperCode);
                            list.Add(student);

                            pgRun.Value += 100 / studentAnswer.Length - 1;
                        }
                        catch (Exception) {
                            //rtxtMarked.Text += "Wrong format file name found!\n";
                            //MessageBox.Show(ex.Message);
                        }
                    }
                    if(list.Count > 0) {
                        // studentanswer-rôm202
                        string[] arr = txtAnswer.Text.Split('\\');
                        Database.CreateTable(arr[arr.Length - 1]);

                        btnExcel.Enabled = true;
                        //put data to DB
                        putDataToDB(list, arr[arr.Length - 1]);
                        dgvAll.Columns.Remove("btnExport");
                        LoadDataGridView();
                    } else {
                        MessageBox.Show("Check Room Folders Path!!", "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void dgvAll_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = this.dgvAll.Rows[e.RowIndex];
            if (dgvAll.Columns[e.ColumnIndex].Name == "btnExport") {
                string tableName = row.Cells[1].Value.ToString();
                if (MessageBox.Show("Do you want export excel " + tableName + "?", "Comfirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                    ExportExcel(tableName);
                }
            }
        }
    }
}
