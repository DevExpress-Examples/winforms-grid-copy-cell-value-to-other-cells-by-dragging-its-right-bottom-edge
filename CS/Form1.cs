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
    public partial class Form1 : DevExpress.XtraEditors.XtraForm {
        public Form1()
        {
            InitializeComponent();


            WindowState = FormWindowState.Maximized;
            gridView1.OptionsView.ColumnAutoWidth = false;
            gridView1.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDownFocused;
            gridView1.OptionsSelection.MultiSelect = true;
            gridView1.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            new DataSourceHelper(gridView1, 50, 50);
            SelectedCellsBorderHelper selectedCellsHelper = new SelectedCellsBorderHelper(gridView1);
            new DragCellsValuesHelper(selectedCellsHelper);
        }
    }
}