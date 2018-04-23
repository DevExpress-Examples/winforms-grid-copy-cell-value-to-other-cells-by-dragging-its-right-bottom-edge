Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.Utils.Paint
Imports DevExpress.Utils

Namespace WindowsApplication1
	Public Class DragCellsValuesHelper

		Private _Helper As SelectedCellsBorderHelper

		Public ReadOnly Property View() As GridView
			Get
				Return _Helper.GridView
			End Get
		End Property

		Private _SourceGridCell As GridCell
		Public Property SourceGridCell() As GridCell
			Get
				Return _SourceGridCell
			End Get
			Set(ByVal value As GridCell)
				_SourceGridCell = value
			End Set
		End Property
		Public Sub New(ByVal selectedCellsHelper As SelectedCellsBorderHelper)
			_Helper = selectedCellsHelper
			InitViewEvents()
		End Sub

		Private Sub InitViewEvents()
			AddHandler View.MouseDown, AddressOf View_MouseDown
			AddHandler View.MouseUp, AddressOf View_MouseUp
			AddHandler View.ShowingEditor, AddressOf View_ShowingEditor
			AddHandler View.SelectionChanged, AddressOf View_SelectionChanged
		End Sub

		Private Sub View_SelectionChanged(ByVal sender As Object, ByVal e As DevExpress.Data.SelectionChangedEventArgs)
			If _Helper.IsCopyMode Then
				UpdateHint()
			End If
		End Sub

		Private Sub UpdateHint()
			ToolTipController.DefaultController.HideHint()
			Dim text As String = View.GetFocusedDisplayText()
			If String.IsNullOrEmpty(text) Then
				Return
			End If
			ToolTipController.DefaultController.ShowHint(text)
		End Sub
		Private Sub View_ShowingEditor(ByVal sender As Object, ByVal e As CancelEventArgs)
			e.Cancel = _Helper.IsCopyMode
		End Sub

		Private Sub View_MouseUp(ByVal sender As Object, ByVal e As MouseEventArgs)
			If _Helper.IsCopyMode Then
				OnCopyFinished()
			End If
			_Helper.IsCopyMode = False
		End Sub

		Private Sub OnCopyFinished()
			ToolTipController.DefaultController.HideHint()
			CopyCellsValues()
		End Sub


		Private Sub CopyCellsValues()
			Dim value As Object = View.GetRowCellValue(SourceGridCell.RowHandle, SourceGridCell.Column)
			Dim selectedCells() As GridCell = View.GetSelectedCells()
			For Each cell As GridCell In selectedCells
				View.SetRowCellValue(cell.RowHandle, cell.Column, value)
			Next cell
		End Sub

		Private Sub View_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs)
			_Helper.IsCopyMode = _Helper.GetDragRect().Contains(e.Location)
			If _Helper.IsCopyMode Then
				OnStartCopy()
			End If
		End Sub

		Private Sub OnStartCopy()
			SourceGridCell = New GridCell(View.FocusedRowHandle, View.FocusedColumn)
		End Sub
	End Class
End Namespace
