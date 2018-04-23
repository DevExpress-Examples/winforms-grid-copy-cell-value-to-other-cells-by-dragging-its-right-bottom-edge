using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;

namespace WindowsApplication1
{
    public class DataSourceHelper
    {

        private DataTable CreateTable(int columnCount, int rowCount)
        {
            DataTable tbl = new DataTable();
            for (int i = 0; i < columnCount; i++)
                tbl.Columns.Add(String.Format("Column{0}", i), typeof(string));
            for (int i = 0; i < rowCount; i++)
                tbl.Rows.Add(new object[] { });
            return tbl;
        }


        public DataSourceHelper(GridView gridView, int columnCount, int rowCount)
        {
            gridView.GridControl.DataSource = CreateTable(columnCount, rowCount);
        }
    }
}
