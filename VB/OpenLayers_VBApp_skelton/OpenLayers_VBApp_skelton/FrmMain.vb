Imports System.ComponentModel

Public Class FrmMain

    Private Structure StcModuleVar
        Public Controller As ClsWebGISControl
        Public AMDArr As AppCommonsClass.ClsAddModifyDeleteTableArray(Of DS_MyAppDataSet.T_REFERENCEPOINTDataTable)
        Public Sample_BackendDB As AppCommonsClass.ClsBackendDB_SAMPLE(Of DS_MyAppDataSet.BACKENDDB_T_REFERENCEPOINTDataTable, DS_MyAppDataSet.T_REFERENCEPOINTDataTable)
    End Structure
    Private mdl As StcModuleVar

    Private WithEvents Item地点編集 As New Cls地点編集Item

    Public MapInitialKind As ClsWebGISControl.EnumBaseLayer
    Public MapInitialExtent As New List(Of Decimal)

    Private Sub FrmMain_Load(sender As Object, e As EventArgs) Handles Me.Load
        mdl.Controller = New ClsWebGISControl(Me.WebView21)
        AddHandler mdl.Controller.MapClicked, AddressOf ProcessMapClick
        AddHandler mdl.Controller.FeatureClicked, AddressOf ProcessFeatureClick
        AddHandler mdl.Controller.FeatureDragged, AddressOf ProcessFeatureDragged

        mdl.Controller.SetInitialBaseLayer(Me.MapInitialKind)
        mdl.Controller.SetInitialBox(Me.MapInitialExtent)

        mdl.Controller.SourceFilesAsText("index_html.html") = My.Resources.index_html
        mdl.Controller.SourceFilesAsText("style_css.css") = My.Resources.style_css
        mdl.Controller.SourceFilesAsText("createmap_js.js") = My.Resources.createmap_js
        mdl.Controller.SourceFilesAsText("drawitems_js.js") = My.Resources.drawitems_js
        mdl.Controller.SourceFilesAsText("detectclick_js.js") = My.Resources.detectclick_js
        mdl.Controller.SourceFilesAsText("zoommap_js.js") = My.Resources.zoommap_js
        mdl.Controller.SourceFilesAsImage("marker.png") = My.Resources.marker

        mdl.Controller.StartServer("index_html.html")

        mdl.AMDArr = New AppCommonsClass.ClsAddModifyDeleteTableArray(Of DS_MyAppDataSet.T_REFERENCEPOINTDataTable)(Me.DS_MyAppDataSet1.T_REFERENCEPOINT)

        mdl.Sample_BackendDB = New AppCommonsClass.ClsBackendDB_SAMPLE(Of DS_MyAppDataSet.BACKENDDB_T_REFERENCEPOINTDataTable, DS_MyAppDataSet.T_REFERENCEPOINTDataTable)

        Call SplitterInitialize()

        Call Item地点編集.Initialize(Me.TBXUID, Me.TBX地点名, Me.TBX経度, Me.TBX緯度, Me.TBXコメント)

        Call Sub地点リスト初期化()

    End Sub

    Private Sub FrmMain_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        mdl.Controller.StopServer()
    End Sub

#Region "WebGIS-DataGridView連携"

    Private Sub ProcessMapClick(sender As Object, e As ClsWebGISControl.WebGISControlCoordEventsArgs)
        Dim UIDstr As String = Guid.NewGuid.ToString()
        If CreateNewMarkerInfo(UIDStr:=UIDstr, Longitude:=e.Longitude, Latitude:=e.Latitude) Then
            mdl.Controller.AddMarkerOnMap(UIDstr, e.Longitude, e.Latitude)
        End If
    End Sub

    Private Sub ProcessFeatureClick(sender As Object, e As ClsWebGISControl.WebGISControlFeatureEventsArgs)
        Call SelectMarkerInfo(UIDStr:=e.UIDStr)
    End Sub

    Private Sub ProcessFeatureDragged(sender As Object, e As ClsWebGISControl.WebGISControlFeatureEventsArgs)
        Call UpdateMarkerInfo(UIDStr:=e.UIDStr, e.Longitude, e.Latitude)
    End Sub

    Private Sub Item地点編集_RowTypeChanged(sender As Object, e As Cls地点編集Item.RowTypeEventArgs) Handles Item地点編集.RowTypeChanged
        Call SubDisplayMarkerType(e.RowType, e.IsValidated)
    End Sub

    Private Sub SubDisplayMarkerType(markerType As Cls地点編集Item.EnumRowType, isValidRow As Boolean)
        Select Case markerType
            Case Cls地点編集Item.EnumRowType.eNewRow
                Me.LAB状態表示.Visible = True
                Me.LAB状態表示.ForeColor = Color.Black
                Me.LAB状態表示.BackColor = Color.GreenYellow
                Me.LAB状態表示.Text = "新規"
                Me.BTN参照点データ登録編集.Text = "新規登録"
                Me.BTN参照点データ登録編集.Enabled = True

            Case Cls地点編集Item.EnumRowType.eExistRow
                Me.LAB状態表示.Visible = True
                Me.LAB状態表示.ForeColor = Color.White
                Me.LAB状態表示.BackColor = Color.OrangeRed
                Me.LAB状態表示.Text = "更新"
                Me.BTN参照点データ登録編集.Text = "更新登録"
                Me.BTN参照点データ登録編集.Enabled = True

            Case Cls地点編集Item.EnumRowType.eNullRow
                Me.LAB状態表示.Visible = False
                Me.LAB状態表示.ForeColor = Color.Black
                Me.LAB状態表示.BackColor = Color.Transparent
                Me.LAB状態表示.Text = "(---)"
                Me.BTN参照点データ登録編集.Text = "--登--録--"
                Me.BTN参照点データ登録編集.Enabled = True

            Case Else 'EnumMarkerStatus.eそのほか
                Me.LAB状態表示.Visible = True
                Me.LAB状態表示.ForeColor = Color.Black
                Me.LAB状態表示.BackColor = Color.Transparent
                Me.LAB状態表示.Text = "その他"
                Me.BTN参照点データ登録編集.Text = "その他"
                Me.BTN参照点データ登録編集.Enabled = False

        End Select

    End Sub

    '新規マーカー情報を作成する
    Private Function CreateNewMarkerInfo(UIDStr As String, Longitude As Decimal, Latitude As Decimal) As Boolean
        Return Fnc新規地点を地点編集へ(UIDStr, Longitude, Latitude)
    End Function

    Private Function UpdateMarkerInfo(UIDStr As String, Longitude As Decimal, Latitude As Decimal) As Boolean
        Return Fnc地点編集の座標変更(UIDStr, Longitude, Latitude)
    End Function

    Private Sub SelectMarkerInfo(UIDStr As String)
        If Item地点編集.IsEditing Then
            If Item地点編集.Prop_UID = UIDStr Then
                '編集中の地点を選択
                Exit Sub
            Else
                'それ以外のマーカーを選択
                Throw New Exception("不明なマーカーを選択しました")
            End If
        Else
            '編集中でないのにマーカーを選択
            Throw New Exception("不明なマーカーを選択しました")
        End If

        'DGVの選択をクリア
        Me.DGV地点リスト.ClearSelection()

        Dim rows = Me.DGV地点リスト.Rows.Cast(Of DataGridViewRow).Where(Function(row) row.Cells("UID").Equals(UIDStr))
        If rows.Count > 0 Then
            Dim srcrow = FncDGVRowからDataRow取得(Of DS_MyAppDataSet.T_REFERENCEPOINTRow)(rows(0))

            Item地点編集.SetValues(Cls地点編集Item.EnumRowType.eExistRow, srcrow)

            '該当行をDGVで選択状態に
            For Each row As DataGridViewRow In rows
                row.Selected = True
            Next
        End If
    End Sub

#Region "イベント"

    Private Sub BTN参照点データ登録編集_Click(sender As Object, e As EventArgs) Handles BTN参照点データ登録編集.Click
        Call Sub編集地点登録()
    End Sub

    Private Sub DGV地点リスト_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGV地点リスト.CellDoubleClick
        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Exit Sub

        Dim row As DS_MyAppDataSet.T_REFERENCEPOINTRow
        row = FncDGVからDataRow取得(Of DS_MyAppDataSet.T_REFERENCEPOINTRow)(Me.DGV地点リスト, e.RowIndex)

        Call Fnc既存地点を地点編集へ(row)

    End Sub

    Private Sub DGV地点リスト_UserDeletingRow(sender As Object, e As DataGridViewRowCancelEventArgs) Handles DGV地点リスト.UserDeletingRow
        If Fnc地点削除確認() = False Then
            e.Cancel = True
        End If
    End Sub

    Private Sub DGV地点リスト_UserDeletedRow(sender As Object, e As DataGridViewRowEventArgs) Handles DGV地点リスト.UserDeletedRow
        Call Sub既存地点を削除()
    End Sub

#End Region

#Region "WebGIS関連データ操作"

    Private Function Fnc新規地点を地点編集へ(UIDStr As String, Longitude As Decimal, Latitude As Decimal) As Boolean
        If Fnc地点編集に変更あり() Then
            If MsgBox("編集中の地点があります" & vbCrLf & "中断して別の地点を編集しますか？", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                Return False
            End If
            Item地点編集.Clear()
        End If

        'DataGridViewの選択状態は解除
        Me.DGV地点リスト.ClearSelection()

        mdl.Controller.RemoveAllMarkerOnMap()

        '新規表示
        Item地点編集.SetValues(Cls地点編集Item.EnumRowType.eNewRow, UIDStr, "", Longitude, Latitude, "")

        mdl.Controller.AddMarkerOnMap(UIDStr, Longitude, Latitude)
        mdl.Controller.SetZoomInfo(Longitude, Latitude, 15)

        Return True
    End Function

    Private Function Fnc地点編集の座標変更(UIDStr As String, Longitude As Decimal, Latitude As Decimal) As Boolean
        If Item地点編集.IsEditing = False Then
            Throw New Exception("編集中のデータが失われました")
        End If

        If Item地点編集.Prop_UID <> UIDStr Then
            Throw New Exception("UIDが合致しない")
        End If

        Item地点編集.SetCoordsValues(Longitude, Latitude)
        Return True
    End Function

    Private Function FncDGVRowからDataRow取得(Of X As DataRow)(dgvRow As DataGridViewRow) As X
        Dim item = dgvRow.DataBoundItem
        If item Is Nothing Then
            Return Nothing
        End If
        Dim view = CType(item, DataRowView)
        If view Is Nothing Then
            Return Nothing
        End If
        Return CType(view.Row, X)
    End Function

    Private Function FncDGVからDataRow取得(Of X As DataRow)(DGV As DataGridView, rowindex As Integer) As X
        If rowindex < 0 Or rowindex > DGV.RowCount - 1 Then
            Throw New Exception("行番号が範囲外です")
        End If

        Dim DGVrow As DataGridViewRow = DGV.Rows(rowindex)
        Return FncDGVRowからDataRow取得(Of X)(DGVrow)
    End Function

    Private Function Fnc既存地点を地点編集へ(targetrow As DS_MyAppDataSet.T_REFERENCEPOINTRow) As Boolean

        If Fnc地点編集に変更あり() Then
            If MsgBox("編集中の地点があります" & vbCrLf & "中断して別の地点を編集しますか？", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                Return False
            End If
            Item地点編集.Clear()
        End If

        mdl.Controller.RemoveAllMarkerOnMap()

        Item地点編集.SetValues(Cls地点編集Item.EnumRowType.eExistRow, targetrow)

        mdl.Controller.AddMarkerOnMap(targetrow.uid, targetrow.LONGITUDE, targetrow.LATITUDE)
        mdl.Controller.SetZoomInfo(targetrow.LONGITUDE, targetrow.LATITUDE, 15)

        Return True

    End Function

    Private Function Fnc地点編集に変更あり() As Boolean
        Dim ans As Boolean = False
        If Item地点編集.IsEditing Then
            If Item地点編集.IsUpdate Then
                ans = True
            End If
        End If
        Return ans
    End Function

    Private Function Fnc地点削除確認(Optional pointlabel As String = "") As Boolean
        Dim ans As Boolean = False

        Dim msg As String
        If pointlabel = "" Then
            msg = "指定行の地点データを参照点データから削除しますか？"
        Else
            msg = "地点データ [ " & pointlabel & " ] を参照点データから削除しますか？"
        End If
        If MsgBox(msg, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            ans = True
        End If

        Return ans
    End Function

    Private Sub Sub既存地点を削除()
        For Each row As DS_MyAppDataSet.T_REFERENCEPOINTRow In mdl.AMDArr.Deleted
            Dim uid_remove As String = row.uid
            mdl.Controller.RemoveMarkerOnMap(UIDStr:=uid_remove)
            If Item地点編集.IsEditing Then
                If Item地点編集.Prop_UID = uid_remove Then
                    Item地点編集.Clear()
                End If
            End If
        Next

        ''TODO: DBなどへの書込み
        mdl.Sample_BackendDB.WriteBackendDB(mdl.AMDArr)

        mdl.AMDArr.AcceptChanges()

    End Sub

    Private Sub Sub編集地点登録()
        If Item地点編集.IsEditing = False Then
            Exit Sub
        End If
        If Item地点編集.IsValid = False Then
            MsgBox("情報が不足しているため登録できません。")
            Exit Sub
        End If
        Select Case Item地点編集.RowType
            Case Cls地点編集Item.EnumRowType.eNewRow
                Item地点編集.SetRowState(Cls地点編集Item.EnumRowState.eAddedRow)
            Case Cls地点編集Item.EnumRowType.eExistRow
                Item地点編集.SetRowState(Cls地点編集Item.EnumRowState.eModifiedRow)
            Case Cls地点編集Item.EnumRowType.eNullRow
                Item地点編集.SetRowState(Cls地点編集Item.EnumRowState.eDetachedRow)
        End Select

        ''TODO: DBなどへの書込み
        Dim tmpAMDarr As New AppCommonsClass.ClsAddModifyDeleteTableArray(Of DS_MyAppDataSet.T_REFERENCEPOINTDataTable)(Item地点編集.AsTable)
        mdl.Sample_BackendDB.WriteBackendDB(tmpAMDarr)

        mdl.Controller.RemoveAllMarkerOnMap()

        Call Sub地点リスト初期化()

    End Sub

    Private Sub Sub地点リスト初期化()
        '画面の初期化
        mdl.AMDArr.Base.Clear()
        Item地点編集.Clear()

        ''TODO: DBなどからの読込
        mdl.Sample_BackendDB.ReadBackendDB(Me.DS_MyAppDataSet1.T_REFERENCEPOINT)

        mdl.AMDArr.AcceptChanges()
        Me.DGV地点リスト.Sort(Me.POINTLABEL_DataGridViewTextBoxColumn, ListSortDirection.Ascending)
        Me.DGV地点リスト.Refresh()

    End Sub

#End Region

#End Region

#Region "画面分離位置調整"

    Private Sub FrmPointEntry_ResizeBegin(sender As Object, e As EventArgs) Handles Me.ResizeBegin
        Call SplitterStore()
    End Sub

    Private Sub FrmPointEntry_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        Call SplitterRestore()
    End Sub

    Dim distPanel1Width As Integer
    Dim distPanel2Width As Integer

    Private Sub SplitterInitialize()
        Me.SplitContainer1.SplitterDistance = 300
        Me.SplitContainer2.SplitterDistance = Me.SplitContainer2.Size.Height - 236
        Call SplitterStore()
    End Sub

    Private Sub SplitterStore()
        distPanel1Width = Me.SplitContainer1.SplitterDistance
        distPanel2Width = Me.SplitContainer2.Height - Me.SplitContainer2.SplitterDistance
    End Sub

    Private Sub SplitterRestore()
        Me.SplitContainer1.SplitterDistance = distPanel1Width
        Me.SplitContainer2.SplitterDistance = Me.SplitContainer2.Height - distPanel2Width
    End Sub

#End Region

End Class

''' <summary>
''' テキスト入力欄を連動するようにクラス化
''' </summary>
Friend Class Cls地点編集Item
    Private WithEvents Prv_UIDTBX As TextBox
    Private WithEvents Prv_POINTLABELTBX As TextBox
    Private WithEvents Prv_LONTBX As TextBox
    Private WithEvents Prv_LATTBX As TextBox
    Private WithEvents Prv_COMMENTTBX As TextBox
    Private WithEvents Prv_Table As New DS_MyAppDataSet.T_REFERENCEPOINTDataTable

    Public Enum EnumRowState
        eAddedRow = DataRowState.Added
        eModifiedRow = DataRowState.Modified
        eDeletedRow = DataRowState.Deleted
        eUnchangedRow = DataRowState.Unchanged
        eDetachedRow = DataRowState.Detached
        eNullRow = 0
    End Enum

    Public Enum EnumRowType
        eNewRow = 1
        eExistRow = 2
        eNullRow = 0
    End Enum

    Private __RowType As EnumRowType = EnumRowType.eNullRow

    Public Property RowType As EnumRowType
        Get
            Return __RowType
        End Get
        Private Set(value As EnumRowType)
            If __RowType <> value Then
                __RowType = value
                Call OnRowTypeChanged(New RowTypeEventArgs(__RowType, FncIsValidatedRow()))
            End If
        End Set
    End Property

#Region "イベント発生処理"
    Public Event RowStateChanged(sender As Object, e As RowStateEventArgs)

    Public Class RowStateEventArgs
        Inherits EventArgs

        Public ReadOnly Property RowState As EnumRowState
        Public ReadOnly Property IsValidated As Boolean

        Public Sub New(RowState As EnumRowState, IsValidated As Boolean)
            Me.RowState = RowState
            Me.IsValidated = IsValidated
        End Sub

    End Class

    Protected Friend Sub OnRowStateChanged(e As RowStateEventArgs)

        RaiseEvent RowStateChanged(Me, e)
    End Sub

    Public Event RowTypeChanged(sender As Object, e As RowTypeEventArgs)

    Public Class RowTypeEventArgs
        Inherits EventArgs
        Public ReadOnly Property RowType As EnumRowType
        Public ReadOnly Property IsValidated As Boolean
        Public Sub New(RowType As EnumRowType, IsValidated As Boolean)
            Me.RowType = RowType
            Me.IsValidated = IsValidated
        End Sub
    End Class

    Protected Friend Sub OnRowTypeChanged(e As RowTypeEventArgs)
        RaiseEvent RowTypeChanged(Me, e)
    End Sub

#End Region

#Region "内部コントロールのイベント処理"
    Private Sub Prv_Table_RowChanged(sender As Object, e As DataRowChangeEventArgs) Handles Prv_Table.RowChanged
        Select Case e.Action
            Case DataRowAction.Add
                Call OnRowStateChanged(New RowStateEventArgs(Cls地点編集Item.EnumRowState.eAddedRow, FncIsValidatedRow()))

            Case DataRowAction.Change
                Call OnRowStateChanged(New RowStateEventArgs(Cls地点編集Item.EnumRowState.eModifiedRow, FncIsValidatedRow()))

            Case DataRowAction.Delete
                Call OnRowStateChanged(New RowStateEventArgs(Cls地点編集Item.EnumRowState.eDeletedRow, FncIsValidatedRow()))

            Case Else

        End Select

    End Sub

    Private Function FncIsValidatedRow() As Boolean
        If Prv_Table.Count = 0 Then
            Return False
        End If
        If Prv_Table.Item(0).IsuidNull Then
            Return False
        End If
        If Prv_Table.Item(0).IsuidNull = True OrElse Prv_Table.Item(0).uid = "" Then
            Return False
        End If
        If Prv_Table.Item(0).IsPOINTLABELNull = True OrElse Prv_Table.Item(0).POINTLABEL = "" Then
            Return False
        End If
        If Prv_Table.Item(0).IsLONGITUDENull OrElse Prv_Table.Item(0).IsLATITUDENull Then
            Return False
        End If
        Return True
    End Function
#End Region

    Public Sub Initialize(UIDTextbox As TextBox, POINTLABELTextbox As TextBox, LongitudeTextbox As TextBox, LatitudeTextbox As TextBox, CommentTextBox As TextBox)
        Prv_UIDTBX = UIDTextbox
        Prv_POINTLABELTBX = POINTLABELTextbox
        Prv_LONTBX = LongitudeTextbox
        Prv_LATTBX = LatitudeTextbox
        Prv_COMMENTTBX = CommentTextBox
    End Sub

    Public Sub SetRowState(state As EnumRowState)
        Prv_Table.Item(0).AcceptChanges()
        Select Case state
            Case EnumRowState.eAddedRow
                Prv_Table.Item(0).SetAdded()
            Case EnumRowState.eModifiedRow
                Prv_Table.Item(0).SetModified()
            Case EnumRowState.eDeletedRow
                Prv_Table.Item(0).Delete()
            Case EnumRowState.eUnchangedRow
                'nothing
            Case Else
                Throw New Exception("未知の行ステートです。")
        End Select
    End Sub

    Public Function AsTable() As DS_MyAppDataSet.T_REFERENCEPOINTDataTable
        Return Prv_Table
    End Function

    Public Sub FlushRowToDisplay(Optional withEvent As Boolean = False)
        If withEvent Then
            Me.Prv_UIDTBX.Text = Prv_Table.Item(0).uid
            Me.Prv_POINTLABELTBX.Text = Prv_Table.Item(0).POINTLABEL
            Me.Prv_LONTBX.Text = IIf(Of String)(Prv_Table.Item(0).IsLONGITUDENull, "", Prv_Table.Item(0).LONGITUDE.ToString("###.#####0"))
            Me.Prv_LATTBX.Text = IIf(Of String)(Prv_Table.Item(0).IsLATITUDENull, "", Prv_Table.Item(0).LATITUDE.ToString("###.#####0"))
            Me.Prv_COMMENTTBX.Text = Prv_Table.Item(0).comment
        Else
            SetTBXWithoutEvent(Me.Prv_UIDTBX, Prv_Table.Item(0).uid)
            SetTBXWithoutEvent(Me.Prv_POINTLABELTBX, Prv_Table.Item(0).POINTLABEL)
            SetTBXWithoutEvent(Me.Prv_LONTBX, IIf(Of String)(Prv_Table.Item(0).IsLONGITUDENull,
                                    "", Prv_Table.Item(0).LONGITUDE.ToString("###.#####0")))
            SetTBXWithoutEvent(Me.Prv_LATTBX, IIf(Of String)(Prv_Table.Item(0).IsLATITUDENull,
                                    "", Prv_Table.Item(0).LATITUDE.ToString("###.#####0")))
            SetTBXWithoutEvent(Me.Prv_COMMENTTBX, Prv_Table.Item(0).comment)
        End If
    End Sub

    Public Sub AcceptRowChanges()
        Prv_Table.AcceptChanges()
    End Sub

    Public Sub SetValues(rowType As EnumRowType, UID As String, PointLabel As String, Longitude As Decimal?, Latitude As Decimal?, Comment As String)
        Prop_UID = UID
        Prop_PointLabel = PointLabel
        Prop_Longitude = Longitude
        Prop_Latitude = Latitude
        Prop_Comment = Comment
        Me.RowType = rowType
        Call FlushRowToDisplay()
    End Sub

    Public Sub SetValues(rowType As EnumRowType, row As DS_MyAppDataSet.T_REFERENCEPOINTRow)
        Prop_UID = row.uid
        Prop_PointLabel = row.POINTLABEL
        Prop_Longitude = row.LONGITUDE
        Prop_Latitude = row.LATITUDE
        Prop_Comment = row.comment
        Me.RowType = rowType
        Call FlushRowToDisplay()
    End Sub

    Public Sub SetCoordsValues(Longitude As Decimal?, Latitude As Decimal?)
        Prop_Longitude = Longitude
        Prop_Latitude = Latitude
        Call FlushRowToDisplay()
    End Sub

    Public Property Prop_UID(Optional withEvent As Boolean = True) As String
        Get
            Return Prv_Table.Item(0).uid
        End Get
        Private Set(value As String)
            Prv_Table.Item(0).uid = value
            If withEvent Then
                Me.Prv_UIDTBX.Text = value
            Else
                SetTBXWithoutEvent(Me.Prv_UIDTBX, value)
            End If
        End Set
    End Property

    Public Property Prop_PointLabel(Optional withEvent As Boolean = True) As String
        Get
            Return Prv_Table.Item(0).pointlabel
        End Get
        Private Set(value As String)
            Prv_Table.Item(0).pointlabel = value
            If withEvent Then
                Me.Prv_POINTLABELTBX.Text = value
            Else
                SetTBXWithoutEvent(Me.Prv_POINTLABELTBX, value)
            End If
        End Set
    End Property

    Public Property Prop_Longitude(Optional withEvent As Boolean = True) As Decimal?
        Get
            Return IIf(Of Decimal?)(Prv_Table.Item(0).IslongitudeNull, Nothing, Prv_Table.Item(0).longitude)
        End Get
        Private Set(value As Decimal?)
            If value.HasValue Then
                Prv_Table.Item(0).longitude = value.Value
                If withEvent Then
                    Me.Prv_LONTBX.Text = value.Value.ToString("###.#####0")
                Else
                    SetTBXWithoutEvent(Me.Prv_LONTBX, value.Value.ToString("###.#####0"))
                End If
            Else
                Prv_Table.Item(0).SetlongitudeNull()
                If withEvent Then
                    Me.Prv_LONTBX.Text = ""
                Else
                    SetTBXWithoutEvent(Me.Prv_LONTBX, "")
                End If
            End If
        End Set
    End Property

    Public Property Prop_Latitude(Optional withEvent As Boolean = True) As Decimal?
        Get
            Return IIf(Of Decimal?)(Prv_Table.Item(0).IslatitudeNull, Nothing, Prv_Table.Item(0).latitude)
        End Get
        Private Set(value As Decimal?)
            If value.HasValue Then
                Prv_Table.Item(0).latitude = value.Value
                If withEvent Then
                    Me.Prv_LATTBX.Text = value.Value.ToString("###.#####0")
                Else
                    SetTBXWithoutEvent(Me.Prv_LATTBX, value.Value.ToString("###.#####0"))
                End If
            Else
                Prv_Table.Item(0).SetlatitudeNull()
                If withEvent Then
                    Me.Prv_LATTBX.Text = ""
                Else
                    SetTBXWithoutEvent(Me.Prv_LATTBX, "")
                End If
            End If
        End Set
    End Property

    Public Property Prop_Comment(Optional withEvent As Boolean = True) As String
        Get
            Return Prv_Table.Item(0).comment
        End Get
        Private Set(value As String)
            Prv_Table.Item(0).comment = value
            If withEvent Then
                Me.Prv_COMMENTTBX.Text = value
            Else
                SetTBXWithoutEvent(Me.Prv_COMMENTTBX, value)
            End If
        End Set
    End Property

    Public ReadOnly Property IsEditing() As Boolean
        Get
            Return Me.RowType = EnumRowType.eExistRow OrElse Me.RowType = EnumRowType.eNewRow
        End Get
    End Property

    Public ReadOnly Property IsUpdate() As Boolean
        Get
            Return Me.Prv_Table.Item(0).RowState = DataRowState.Added OrElse
               Prv_Table.Item(0).RowState = DataRowState.Modified
        End Get
    End Property

    Public Function IsValid() As Boolean
        Return FncIsValidatedRow()
    End Function

    Public Sub Clear()
        Prv_UIDTBX.Text = ""
        Prv_POINTLABELTBX.Text = ""
        Prv_LONTBX.Text = ""
        Prv_LATTBX.Text = ""
        Prv_COMMENTTBX.Text = ""

        Prv_Table.Clear()
        Prv_Table.AddT_REFERENCEPOINTRow(Prv_Table.NewT_REFERENCEPOINTRow())
        Prv_Table.Item(0).AcceptChanges()
        Me.RowType = EnumRowType.eNullRow
    End Sub

#Region "テキストボックスイベント処理"
    Private Sub Prv_LONTBX_Validating(sender As Object, e As CancelEventArgs) Handles Prv_LONTBX.Validating
        Dim this As TextBox = CType(sender, TextBox)
        Dim val As Decimal
        If this.Text <> "" And Decimal.TryParse(this.Text, val) = False Then
            e.Cancel = True
            MsgBox("経度の値が不正です" & vbCrLf & "数値を入力してください")
            this.Focus()
        End If
    End Sub

    Private Sub Prv_LATTBX_Validating(sender As Object, e As CancelEventArgs) Handles Prv_LATTBX.Validating
        Dim this As TextBox = CType(sender, TextBox)
        Dim val As Decimal
        If this.Text <> "" And Decimal.TryParse(this.Text, val) = False Then
            e.Cancel = True
            MsgBox("緯度の値が不正です" & vbCrLf & "数値を入力してください")
            this.Focus()
        End If
    End Sub

    Private Sub Prv_UIDTBX_Validated(sender As Object, e As EventArgs) Handles Prv_UIDTBX.Validated
        Prv_Table.Item(0).uid = Me.Prv_UIDTBX.Text
    End Sub

    Private Sub Prv_POINTLABELTBX_Validated(sender As Object, e As EventArgs) Handles Prv_POINTLABELTBX.Validated
        Prv_Table.Item(0).pointlabel = Me.Prv_POINTLABELTBX.Text
    End Sub

    Private Sub Prv_LONTBX_Validated(sender As Object, e As EventArgs) Handles Prv_LONTBX.Validated
        If Me.Prv_LONTBX.Text <> "" Then
            Prv_Table.Item(0).longitude = Decimal.Parse(Me.Prv_LONTBX.Text)
        Else
            Prv_Table.Item(0).SetlongitudeNull()
        End If
    End Sub

    Private Sub Prv_LATTBX_Validated(sender As Object, e As EventArgs) Handles Prv_LATTBX.Validated
        If Me.Prv_LATTBX.Text <> "" Then
            Prv_Table.Item(0).latitude = Decimal.Parse(Me.Prv_LATTBX.Text)
        Else
            Prv_Table.Item(0).SetlatitudeNull()
        End If
    End Sub

    Private Sub Prv_COMMENTTBX_Validated(sender As Object, e As EventArgs) Handles Prv_COMMENTTBX.Validated
        Prv_Table.Item(0).comment = Me.Prv_COMMENTTBX.Text
    End Sub

#End Region

End Class
