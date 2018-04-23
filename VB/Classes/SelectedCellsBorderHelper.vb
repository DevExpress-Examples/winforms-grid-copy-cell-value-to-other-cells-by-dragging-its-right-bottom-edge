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

Namespace WindowsApplication1
	Public Class SelectedCellsBorderHelper

		Public Sub New(ByVal gridView As GridView)
			_GridView = gridView
			AddHandler gridView.GridControl.Paint, AddressOf GridControl_Paint
		End Sub

		Private _IsCopyMode As Boolean
		Private _GridView As GridView
		Public Property GridView() As GridView
			Get
				Return _GridView
			End Get
			Set(ByVal value As GridView)
				_GridView = value
			End Set
		End Property


		Private _GridControl As GridControl
		Public ReadOnly Property GridControl() As GridControl
			Get
				Return _GridView.GridControl
			End Get
		End Property

		Public Property IsCopyMode() As Boolean
			Get
				Return _IsCopyMode
			End Get
			Set(ByVal value As Boolean)
				_IsCopyMode = value
				OnCopyModeChanged()
			End Set
		End Property

		Private _paint As New XPaint()

		Private ReadOnly Property Paint() As XPaint
			Get
				Return _paint
			End Get
		End Property
		Private Sub GridControl_Paint(ByVal sender As Object, ByVal e As PaintEventArgs)
			If IsCopyMode Then
				DrawCopyBorder(e)
			Else
				DrawRegularBorder(e)
			End If
		End Sub

		Private _DragRectSize As Integer = 6
		Private lineWidth As Integer = 3


		Private Function GetSelectionBounds() As Rectangle
			Dim width As Integer = 0
			Dim height As Integer = 0
			Dim rTop As Rectangle = Rectangle.Empty
			Dim shouldReturn As Boolean = False
			Dim view As GridView = TryCast(GridControl.FocusedView, GridView)
			Dim info As GridViewInfo = TryCast(view.GetViewInfo(), GridViewInfo)
			Dim gridCells() As GridCell = view.GetSelectedCells()
			If gridCells.Length = 0 Then
				shouldReturn = True
				Return Rectangle.Empty
			End If
			Dim hb As Brush = Brushes.Black
			Dim visibleColl As New List(Of GridCellInfo)()
			For Each row As GridRowInfo In info.RowsInfo
				Dim coll As GridCellInfoCollection = (TryCast(row, GridDataRowInfo)).Cells
				For Each cell As GridCellInfo In coll
					visibleColl.Add(cell)
				Next cell

			Next row
			Dim collection As New List(Of GridCellInfo)()
			For Each cell As GridCell In gridCells
				For Each cellInfo As GridCellInfo In visibleColl
					If cellInfo.RowInfo IsNot Nothing AndAlso cellInfo.ColumnInfo IsNot Nothing Then
						If cell.RowHandle = cellInfo.RowHandle AndAlso cell.Column Is cellInfo.Column Then
							collection.Add(cellInfo)
						End If
					End If
				Next cellInfo
			Next cell
			If collection.Count = 0 Then
				shouldReturn = True
				Return Rectangle.Empty
			End If
			rTop = GetCellRect(view, collection(0).RowHandle, collection(0).Column)
			Dim rBottom As Rectangle = GetCellRect(view, collection(collection.Count - 1).RowHandle, collection(collection.Count - 1).Column)
			If rTop.Y > rBottom.Y Then
				height = rTop.Y - rBottom.Bottom
			Else
				height = rBottom.Bottom - rTop.Y
			End If

			If rTop.X <= rBottom.X Then
				width = rBottom.Right - rTop.X
			Else
				width = rTop.X - rBottom.Right
			End If
			Return New Rectangle(rTop.X, rTop.Y, width, height)
		End Function

		Private Sub DrawCopyBorder(ByVal e As PaintEventArgs)
			Dim rect As Rectangle = GetSelectionBounds()
			Paint.DrawFocusRectangle(e.Graphics, rect, Color.Black, Color.White)
		End Sub

		Private Sub DrawRegularBorder(ByVal e As PaintEventArgs)
			Dim rect As Rectangle = GetSelectionBounds()
			e.Graphics.DrawRectangle(New Pen(Brushes.Black, lineWidth), rect)
			If GridView.GetSelectedCells().Length = 1 Then
				DrawDragImage(e, rect)
			End If
		End Sub

		Public Function GetDragRect() As Rectangle
			Return GetDragRect(GetSelectionBounds())
		End Function

		Private Function GetDragRect(ByVal rect As Rectangle) As Rectangle
			Return New Rectangle(rect.Right - _DragRectSize, rect.Bottom - _DragRectSize, _DragRectSize, _DragRectSize)
		End Function
		Private Sub DrawDragImage(ByVal e As PaintEventArgs, ByVal rect As Rectangle)
			e.Graphics.FillRectangle(Brushes.Black, GetDragRect(rect))
		End Sub
		Private Function GetCellRect(ByVal view As GridView, ByVal rowHandle As Integer, ByVal column As GridColumn) As Rectangle
			Dim info As GridViewInfo = CType(view.GetViewInfo(), GridViewInfo)
			Dim cell As GridCellInfo = info.GetGridCellInfo(rowHandle, column.AbsoluteIndex)
			If cell IsNot Nothing Then
				Return cell.Bounds
			End If
			Return Rectangle.Empty
		End Function
		Private Sub OnCopyModeChanged()
			GridView.InvalidateRow(GridView.FocusedRowHandle)
		End Sub
	End Class
End Namespace