Module AppCommons
    Public Function IIf(Of T)(decision As Boolean, v1 As T, v2 As T) As T
        If decision Then
            Return v1
        Else
            Return v2
        End If
    End Function

    Public Sub SkipEvent(Of T)(item As T, job As Action(Of T))
        Dim tempItem As T = item
#Disable Warning IDE0059 ' 値の不必要な代入
        item = Nothing
#Enable Warning IDE0059 ' 値の不必要な代入
        job(tempItem)
#Disable Warning IDE0059 ' 値の不必要な代入
        item = tempItem
        tempItem = Nothing
#Enable Warning IDE0059 ' 値の不必要な代入
    End Sub

    Public Sub SetTBXWithoutEvent(item As TextBox, value As String)
        SkipEvent(Of TextBox)(item, Sub(tb) tb.Text = value)
    End Sub

End Module

Namespace AppCommonsClass
    Public Class ClsAddModifyDeleteTableArray(Of T As DataTable)
        Private WithEvents PrvBasetable As T
        Private PrvDeleteTable As T

        Public Sub New(ByVal basetable As T)
            Me.ResetBase(basetable)
        End Sub

        Public Sub ResetBase(ByVal basetable As T)
            PrvBasetable = basetable
            PrvDeleteTable = CType(Activator.CreateInstance(GetType(T)), T)
        End Sub

        Public Sub AcceptChanges()
            PrvBasetable.AcceptChanges()
            PrvDeleteTable.Clear()
        End Sub

        Public ReadOnly Property Base As T
            Get
                Return PrvBasetable
            End Get
        End Property

        Public ReadOnly Property Deleted As T
            Get
                Return PrvDeleteTable
            End Get
        End Property

        Public ReadOnly Property Inserted As T
            Get
                Dim PrvInsertedTable As T = CType(Activator.CreateInstance(GetType(T)), T)
                For Each row In PrvBasetable.Select("", "", DataViewRowState.Added)
                    Dim newrow = PrvInsertedTable.NewRow
                    newrow.ItemArray = row.ItemArray
                    PrvInsertedTable.Rows.Add(newrow)
                Next
                Return PrvInsertedTable
            End Get
        End Property

        Public ReadOnly Property Updated As T
            Get
                Dim PrvUpdatedTable As T = CType(Activator.CreateInstance(GetType(T)), T)
                For Each row In PrvBasetable.Select("", "", DataViewRowState.ModifiedCurrent)
                    Dim newrow = PrvUpdatedTable.NewRow
                    newrow.ItemArray = row.ItemArray
                    PrvUpdatedTable.Rows.Add(newrow)
                Next
                Return PrvUpdatedTable
            End Get
        End Property

        Private Sub PrvBasetable_RowDeleting(sender As Object, e As DataRowChangeEventArgs) Handles PrvBasetable.RowDeleting
            Dim newrow = PrvDeleteTable.NewRow
            newrow.ItemArray = e.Row.ItemArray
            PrvDeleteTable.Rows.Add(newrow)
        End Sub

        Private Sub PrvBasetable_TableCleared(sender As Object, e As DataTableClearEventArgs) Handles PrvBasetable.TableCleared
            PrvDeleteTable.Clear()
        End Sub
    End Class

    Public Class ClsBackendDB_SAMPLE(Of T1 As DataTable, T2 As DataTable)
        Private ReadOnly PrvTbl As T1
        Private ReadOnly PrvTbl_KeyCol() As String

        Public Sub New()
            PrvTbl = CType(Activator.CreateInstance(GetType(T1)), T1)
            ReDim PrvTbl_KeyCol(0 To PrvTbl.PrimaryKey.Length - 1)
            For i As Integer = 0 To PrvTbl.PrimaryKey.Count - 1
                PrvTbl_KeyCol(i) = PrvTbl.PrimaryKey(i).ColumnName
            Next
        End Sub

        Public Sub ReadBackendDB(dt As T2)
            dt.Clear()
            For i As Integer = 0 To PrvTbl.Rows.Count - 1
                Dim distrow = PrvTbl.Rows(i)

                Dim targetrow = dt.NewRow()

                targetrow.ItemArray = distrow.ItemArray

                dt.Rows.Add(targetrow)
            Next
        End Sub

        Private Function MakeKeys(r As DataRow) As Object()
            Dim ans(0 To PrvTbl_KeyCol.Length - 1) As Object
            For i As Integer = 0 To PrvTbl_KeyCol.Length - 1
                ans(i) = r(PrvTbl_KeyCol(i))
            Next
            Return ans
        End Function

        Public Sub WriteBackendDB(amda As ClsAddModifyDeleteTableArray(Of T2))

            'Delete
            For i As Integer = 0 To amda.Deleted.Rows.Count - 1
                Dim k() As Object = MakeKeys(amda.Deleted.Rows(i))

                Dim tmprow = PrvTbl.Rows.Find(k)
                tmprow?.Delete()
            Next

            'Update
            For i As Integer = 0 To amda.Updated.Rows.Count - 1
                Dim distrow = amda.Updated.Rows(i)
                Dim k() As Object = MakeKeys(distrow)

                Dim row = PrvTbl.Rows.Find(k)
                row.ItemArray = distrow.ItemArray
            Next

            'Insert
            For i As Integer = 0 To amda.Inserted.Rows.Count - 1
                Dim distrow = amda.Inserted.Rows(i)

                Dim targetrow = PrvTbl.NewRow
                targetrow.ItemArray = distrow.ItemArray

                PrvTbl.Rows.Add(targetrow)
            Next

            PrvTbl.AcceptChanges()
        End Sub

    End Class

End Namespace

