Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraGrid.Views.Grid

Namespace WindowsApplication1
	Partial Public Class Form1
		Inherits DevExpress.XtraEditors.XtraForm

		Public Sub New()
			InitializeComponent()


			WindowState = FormWindowState.Maximized
			gridView1.OptionsView.ColumnAutoWidth = False
			gridView1.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDownFocused
			gridView1.OptionsSelection.MultiSelect = True
			gridView1.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect
			Dim tempVar As New DataSourceHelper(gridView1, 50, 50)
			Dim selectedCellsHelper As New SelectedCellsBorderHelper(gridView1)
			Dim tempVar2 As New DragCellsValuesHelper(selectedCellsHelper)
		End Sub
	End Class
End Namespace