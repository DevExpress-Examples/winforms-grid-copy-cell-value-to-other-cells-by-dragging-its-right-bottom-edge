using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.Utils.Paint;
using DevExpress.Utils;

namespace WindowsApplication1
{
    public class DragCellsValuesHelper
    {

        private SelectedCellsBorderHelper _Helper;

        public GridView View
        {
            get { return _Helper.GridView; }
        }

        private GridCell _SourceGridCell;
        public GridCell SourceGridCell
        {
            get { return _SourceGridCell; }
            set { _SourceGridCell = value; }
        }
        public DragCellsValuesHelper(SelectedCellsBorderHelper selectedCellsHelper)
        {
            _Helper = selectedCellsHelper;
            InitViewEvents();
        }

        private void InitViewEvents()
        {
            View.MouseDown += new MouseEventHandler(View_MouseDown);
            View.MouseUp += new MouseEventHandler(View_MouseUp);
            View.ShowingEditor += new CancelEventHandler(View_ShowingEditor);
            View.SelectionChanged += new DevExpress.Data.SelectionChangedEventHandler(View_SelectionChanged);
        }

        void View_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            if (_Helper.IsCopyMode)
                UpdateHint();
        }

        private void UpdateHint()
        {
            ToolTipController.DefaultController.HideHint();
            string text = View.GetFocusedDisplayText();
            if (string.IsNullOrEmpty(text))
                return;
            ToolTipController.DefaultController.ShowHint(text);
        }
        void View_ShowingEditor(object sender, CancelEventArgs e)
        {
            e.Cancel = _Helper.IsCopyMode;
        }

        void View_MouseUp(object sender, MouseEventArgs e)
        {
            if (_Helper.IsCopyMode)
                OnCopyFinished();
            _Helper.IsCopyMode = false;
        }

        private void OnCopyFinished()
        {
            ToolTipController.DefaultController.HideHint();
            CopyCellsValues();
        }


        private void CopyCellsValues()
        {
            object value = View.GetRowCellValue(SourceGridCell.RowHandle, SourceGridCell.Column);
            GridCell[] selectedCells = View.GetSelectedCells();
            foreach (GridCell cell in selectedCells)
            {
                View.SetRowCellValue(cell.RowHandle, cell.Column, value);
            }
        }

        void View_MouseDown(object sender, MouseEventArgs e)
        {
            _Helper.IsCopyMode = _Helper.GetDragRect().Contains(e.Location);
            if (_Helper.IsCopyMode)
                OnStartCopy();
        }

        private void OnStartCopy()
        {
            SourceGridCell = new GridCell(View.FocusedRowHandle, View.FocusedColumn);
        }
    }
}
