Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraGrid.Views.Grid

Namespace WindowsApplication1
	Public Class DataSourceHelper

		Private Function CreateTable(ByVal columnCount As Integer, ByVal rowCount As Integer) As DataTable
			Dim tbl As New DataTable()
			For i As Integer = 0 To columnCount - 1
				tbl.Columns.Add(String.Format("Column{0}", i), GetType(String))
			Next i
			For i As Integer = 0 To rowCount - 1
				tbl.Rows.Add(New Object() { })
			Next i
			Return tbl
		End Function


		Public Sub New(ByVal gridView As GridView, ByVal columnCount As Integer, ByVal rowCount As Integer)
			gridView.GridControl.DataSource = CreateTable(columnCount, rowCount)
		End Sub
	End Class
End Namespace
