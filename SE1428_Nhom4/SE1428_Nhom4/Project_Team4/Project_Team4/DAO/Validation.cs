using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Project_Team3.DAO
{
    class Validation
    {
        public static bool checkData(DataTable tbl1, DataTable tbl2)
        {
            if (tbl1 == null || tbl2 == null) return false;
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
    }
}
