<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmMain
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.DGV地点リスト = New System.Windows.Forms.DataGridView()
        Me.UID_DataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.POINTLABEL_DataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LONGITUDE_DataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LATITUDE_DataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.COMMENT_DataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DS_MyAppDataSet1 = New OpenLayers_VBApp_skelton.DS_MyAppDataSet()
        Me.PAN地点編集 = New System.Windows.Forms.Panel()
        Me.TBXコメント = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TBXUID = New System.Windows.Forms.TextBox()
        Me.LAB状態表示 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.BTN参照点データ登録編集 = New System.Windows.Forms.Button()
        Me.TBX地点名 = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.TBX緯度 = New System.Windows.Forms.TextBox()
        Me.TBX経度 = New System.Windows.Forms.TextBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.WebView21 = New Microsoft.Web.WebView2.WinForms.WebView2()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        CType(Me.DGV地点リスト, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DS_MyAppDataSet1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PAN地点編集.SuspendLayout()
        CType(Me.WebView21, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Margin = New System.Windows.Forms.Padding(4)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.SplitContainer2)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.WebView21)
        Me.SplitContainer1.Size = New System.Drawing.Size(861, 708)
        Me.SplitContainer1.SplitterDistance = 300
        Me.SplitContainer1.SplitterWidth = 6
        Me.SplitContainer1.TabIndex = 0
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer2.Margin = New System.Windows.Forms.Padding(4)
        Me.SplitContainer2.Name = "SplitContainer2"
        Me.SplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.DGV地点リスト)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.PAN地点編集)
        Me.SplitContainer2.Size = New System.Drawing.Size(300, 708)
        Me.SplitContainer2.SplitterDistance = 467
        Me.SplitContainer2.SplitterWidth = 5
        Me.SplitContainer2.TabIndex = 11
        '
        'DGV地点リスト
        '
        Me.DGV地点リスト.AllowUserToAddRows = False
        Me.DGV地点リスト.AllowUserToOrderColumns = True
        Me.DGV地点リスト.AutoGenerateColumns = False
        Me.DGV地点リスト.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DGV地点リスト.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.UID_DataGridViewTextBoxColumn, Me.POINTLABEL_DataGridViewTextBoxColumn, Me.LONGITUDE_DataGridViewTextBoxColumn, Me.LATITUDE_DataGridViewTextBoxColumn, Me.COMMENT_DataGridViewTextBoxColumn})
        Me.DGV地点リスト.DataMember = "T_REFERENCEPOINT"
        Me.DGV地点リスト.DataSource = Me.DS_MyAppDataSet1
        Me.DGV地点リスト.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DGV地点リスト.Location = New System.Drawing.Point(0, 0)
        Me.DGV地点リスト.Margin = New System.Windows.Forms.Padding(4)
        Me.DGV地点リスト.MultiSelect = False
        Me.DGV地点リスト.Name = "DGV地点リスト"
        Me.DGV地点リスト.ReadOnly = True
        Me.DGV地点リスト.RowTemplate.Height = 21
        Me.DGV地点リスト.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DGV地点リスト.Size = New System.Drawing.Size(300, 467)
        Me.DGV地点リスト.TabIndex = 0
        '
        'UID_DataGridViewTextBoxColumn
        '
        Me.UID_DataGridViewTextBoxColumn.DataPropertyName = "uid"
        Me.UID_DataGridViewTextBoxColumn.HeaderText = "uid"
        Me.UID_DataGridViewTextBoxColumn.Name = "UID_DataGridViewTextBoxColumn"
        Me.UID_DataGridViewTextBoxColumn.ReadOnly = True
        Me.UID_DataGridViewTextBoxColumn.Visible = False
        Me.UID_DataGridViewTextBoxColumn.Width = 51
        '
        'POINTLABEL_DataGridViewTextBoxColumn
        '
        Me.POINTLABEL_DataGridViewTextBoxColumn.DataPropertyName = "pointLabel"
        Me.POINTLABEL_DataGridViewTextBoxColumn.HeaderText = "地点名"
        Me.POINTLABEL_DataGridViewTextBoxColumn.Name = "POINTLABEL_DataGridViewTextBoxColumn"
        Me.POINTLABEL_DataGridViewTextBoxColumn.ReadOnly = True
        Me.POINTLABEL_DataGridViewTextBoxColumn.Width = 80
        '
        'LONGITUDE_DataGridViewTextBoxColumn
        '
        Me.LONGITUDE_DataGridViewTextBoxColumn.DataPropertyName = "longitude"
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle1.Format = "0.000000"
        Me.LONGITUDE_DataGridViewTextBoxColumn.DefaultCellStyle = DataGridViewCellStyle1
        Me.LONGITUDE_DataGridViewTextBoxColumn.HeaderText = "経度"
        Me.LONGITUDE_DataGridViewTextBoxColumn.Name = "LONGITUDE_DataGridViewTextBoxColumn"
        Me.LONGITUDE_DataGridViewTextBoxColumn.ReadOnly = True
        Me.LONGITUDE_DataGridViewTextBoxColumn.Width = 64
        '
        'LATITUDE_DataGridViewTextBoxColumn
        '
        Me.LATITUDE_DataGridViewTextBoxColumn.DataPropertyName = "latitude"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle2.Format = "0.000000"
        Me.LATITUDE_DataGridViewTextBoxColumn.DefaultCellStyle = DataGridViewCellStyle2
        Me.LATITUDE_DataGridViewTextBoxColumn.HeaderText = "緯度"
        Me.LATITUDE_DataGridViewTextBoxColumn.Name = "LATITUDE_DataGridViewTextBoxColumn"
        Me.LATITUDE_DataGridViewTextBoxColumn.ReadOnly = True
        Me.LATITUDE_DataGridViewTextBoxColumn.Width = 64
        '
        'COMMENT_DataGridViewTextBoxColumn
        '
        Me.COMMENT_DataGridViewTextBoxColumn.DataPropertyName = "comment"
        Me.COMMENT_DataGridViewTextBoxColumn.HeaderText = "コメント"
        Me.COMMENT_DataGridViewTextBoxColumn.Name = "COMMENT_DataGridViewTextBoxColumn"
        Me.COMMENT_DataGridViewTextBoxColumn.ReadOnly = True
        Me.COMMENT_DataGridViewTextBoxColumn.Width = 77
        '
        'DS_MyAppDataSet1
        '
        Me.DS_MyAppDataSet1.DataSetName = "DS_MyAppDataSet"
        Me.DS_MyAppDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'PAN地点編集
        '
        Me.PAN地点編集.AutoScroll = True
        Me.PAN地点編集.AutoSize = True
        Me.PAN地点編集.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.PAN地点編集.Controls.Add(Me.TBXコメント)
        Me.PAN地点編集.Controls.Add(Me.Label2)
        Me.PAN地点編集.Controls.Add(Me.TBXUID)
        Me.PAN地点編集.Controls.Add(Me.LAB状態表示)
        Me.PAN地点編集.Controls.Add(Me.Label1)
        Me.PAN地点編集.Controls.Add(Me.BTN参照点データ登録編集)
        Me.PAN地点編集.Controls.Add(Me.TBX地点名)
        Me.PAN地点編集.Controls.Add(Me.Label15)
        Me.PAN地点編集.Controls.Add(Me.TBX緯度)
        Me.PAN地点編集.Controls.Add(Me.TBX経度)
        Me.PAN地点編集.Controls.Add(Me.Label17)
        Me.PAN地点編集.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PAN地点編集.Location = New System.Drawing.Point(0, 0)
        Me.PAN地点編集.Margin = New System.Windows.Forms.Padding(4)
        Me.PAN地点編集.Name = "PAN地点編集"
        Me.PAN地点編集.Size = New System.Drawing.Size(300, 236)
        Me.PAN地点編集.TabIndex = 0
        '
        'TBXコメント
        '
        Me.TBXコメント.Location = New System.Drawing.Point(83, 100)
        Me.TBXコメント.Margin = New System.Windows.Forms.Padding(4)
        Me.TBXコメント.Multiline = True
        Me.TBXコメント.Name = "TBXコメント"
        Me.TBXコメント.Size = New System.Drawing.Size(194, 75)
        Me.TBXコメント.TabIndex = 9
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(18, 103)
        Me.Label2.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(52, 16)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "コメント"
        '
        'TBXUID
        '
        Me.TBXUID.Location = New System.Drawing.Point(148, 13)
        Me.TBXUID.Margin = New System.Windows.Forms.Padding(4)
        Me.TBXUID.Name = "TBXUID"
        Me.TBXUID.Size = New System.Drawing.Size(42, 23)
        Me.TBXUID.TabIndex = 2
        Me.TBXUID.Visible = False
        '
        'LAB状態表示
        '
        Me.LAB状態表示.AutoSize = True
        Me.LAB状態表示.Location = New System.Drawing.Point(236, 9)
        Me.LAB状態表示.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.LAB状態表示.Name = "LAB状態表示"
        Me.LAB状態表示.Size = New System.Drawing.Size(39, 16)
        Me.LAB状態表示.TabIndex = 1
        Me.LAB状態表示.Text = "新規"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 9)
        Me.Label1.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(111, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "地点登録・編集"
        '
        'BTN参照点データ登録編集
        '
        Me.BTN参照点データ登録編集.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.BTN参照点データ登録編集.Location = New System.Drawing.Point(21, 184)
        Me.BTN参照点データ登録編集.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.BTN参照点データ登録編集.Name = "BTN参照点データ登録編集"
        Me.BTN参照点データ登録編集.Size = New System.Drawing.Size(256, 35)
        Me.BTN参照点データ登録編集.TabIndex = 10
        Me.BTN参照点データ登録編集.Text = "登録"
        Me.BTN参照点データ登録編集.UseVisualStyleBackColor = True
        '
        'TBX地点名
        '
        Me.TBX地点名.Location = New System.Drawing.Point(83, 38)
        Me.TBX地点名.Margin = New System.Windows.Forms.Padding(4)
        Me.TBX地点名.Name = "TBX地点名"
        Me.TBX地点名.Size = New System.Drawing.Size(194, 23)
        Me.TBX地点名.TabIndex = 4
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(18, 41)
        Me.Label15.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(55, 16)
        Me.Label15.TabIndex = 3
        Me.Label15.Text = "地点名"
        '
        'TBX緯度
        '
        Me.TBX緯度.Location = New System.Drawing.Point(184, 69)
        Me.TBX緯度.Margin = New System.Windows.Forms.Padding(4)
        Me.TBX緯度.Name = "TBX緯度"
        Me.TBX緯度.Size = New System.Drawing.Size(93, 23)
        Me.TBX緯度.TabIndex = 7
        Me.TBX緯度.Text = "###.######"
        '
        'TBX経度
        '
        Me.TBX経度.Location = New System.Drawing.Point(83, 69)
        Me.TBX経度.Margin = New System.Windows.Forms.Padding(4)
        Me.TBX経度.Name = "TBX経度"
        Me.TBX経度.Size = New System.Drawing.Size(93, 23)
        Me.TBX経度.TabIndex = 6
        Me.TBX経度.Text = "###.######"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(18, 72)
        Me.Label17.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(39, 16)
        Me.Label17.TabIndex = 5
        Me.Label17.Text = "座標"
        '
        'WebView21
        '
        Me.WebView21.AllowExternalDrop = True
        Me.WebView21.CreationProperties = Nothing
        Me.WebView21.DefaultBackgroundColor = System.Drawing.Color.White
        Me.WebView21.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WebView21.Location = New System.Drawing.Point(0, 0)
        Me.WebView21.Margin = New System.Windows.Forms.Padding(4)
        Me.WebView21.Name = "WebView21"
        Me.WebView21.Size = New System.Drawing.Size(555, 708)
        Me.WebView21.TabIndex = 0
        Me.WebView21.ZoomFactor = 1.0R
        '
        'FrmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(861, 708)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "FrmMain"
        Me.Text = "地点の登録編集"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        Me.SplitContainer2.Panel2.PerformLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer2.ResumeLayout(False)
        CType(Me.DGV地点リスト, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DS_MyAppDataSet1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PAN地点編集.ResumeLayout(False)
        Me.PAN地点編集.PerformLayout()
        CType(Me.WebView21, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents WebView21 As Microsoft.Web.WebView2.WinForms.WebView2
    Friend WithEvents PAN地点編集 As Panel
    Friend WithEvents BTN参照点データ登録編集 As Button
    Friend WithEvents TBX地点名 As TextBox
    Friend WithEvents Label15 As Label
    Friend WithEvents TBX緯度 As TextBox
    Friend WithEvents TBX経度 As TextBox
    Friend WithEvents Label17 As Label
    Friend WithEvents SplitContainer2 As SplitContainer
    Friend WithEvents DGV地点リスト As DataGridView
    Friend WithEvents Label1 As Label
    Friend WithEvents LAB状態表示 As Label
    Friend WithEvents TBXUID As TextBox
    Friend WithEvents TBXコメント As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents DS_MyAppDataSet1 As DS_MyAppDataSet
    Friend WithEvents UID_DataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents POINTLABEL_DataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents LONGITUDE_DataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents LATITUDE_DataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents COMMENT_DataGridViewTextBoxColumn As DataGridViewTextBoxColumn
End Class
