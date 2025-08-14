Public Class ClsWebGISControl
    Private ReadOnly Sv As Lib_InadaShokai_Web.SimpleWebServerClass
    Private WithEvents Br As Lib_InadaShokai_Web.WebBrowserClass

    Public Class WebGISControlCoordEventsArgs
        Inherits EventArgs
        Public ReadOnly Property Longitude As Decimal
        Public ReadOnly Property Latitude As Decimal
        Public Sub New(Longitude As Decimal, Latitude As Decimal)
            Me.Longitude = Longitude
            Me.Latitude = Latitude
        End Sub
    End Class

    Public Class WebGISControlFeatureEventsArgs
        Inherits EventArgs
        Public ReadOnly Property UIDStr As String
        Public ReadOnly Property ChildUIDStr As String
        Public ReadOnly Property Longitude As Decimal
        Public ReadOnly Property Latitude As Decimal
        Public Sub New(UIDStr As String, ChildUIDStr As String, Longitude As Decimal, Latitude As Decimal)
            Me.UIDStr = UIDStr
            Me.ChildUIDStr = ChildUIDStr
            Me.Longitude = Longitude
            Me.Latitude = Latitude
        End Sub
    End Class

    Public Class WebGISControlRectangleSelectedEventsArgs
        Inherits EventArgs
        Public ReadOnly Property Longitude1 As Decimal
        Public ReadOnly Property Latitude1 As Decimal
        Public ReadOnly Property Longitude2 As Decimal
        Public ReadOnly Property Latitude2 As Decimal
        Public Sub New(Longitude1 As Decimal, Latitude1 As Decimal, Longitude2 As Decimal, Latitude2 As Decimal)
            Me.Longitude1 = Longitude1
            Me.Latitude1 = Latitude1
            Me.Longitude2 = Longitude2
            Me.Latitude2 = Latitude2
        End Sub
    End Class

    Public Event MapClicked(sender As Object, e As WebGISControlCoordEventsArgs)
    Public Event FeatureClicked(sender As Object, e As WebGISControlFeatureEventsArgs)
    Public Event FeatureDragged(sender As Object, e As WebGISControlFeatureEventsArgs)
    Public Event RectangleSelected(sender As Object, e As WebGISControlRectangleSelectedEventsArgs)

    Protected Friend Sub OnMapClicked(e As WebGISControlCoordEventsArgs)
        RaiseEvent MapClicked(Me, e)
    End Sub
    Protected Friend Sub OnFeatureClicked(e As WebGISControlFeatureEventsArgs)
        RaiseEvent FeatureClicked(Me, e)
    End Sub
    Protected Friend Sub OnFeatureDragged(e As WebGISControlFeatureEventsArgs)
        RaiseEvent FeatureDragged(Me, e)
    End Sub

    Protected Friend Sub OnRectangleSelected(e As WebGISControlRectangleSelectedEventsArgs)
        RaiseEvent RectangleSelected(Me, e)
    End Sub

#Region "汎用クラス"
    Public Class ClsFlushList(Of T)
        Inherits List(Of T)

        Public Event FlushRaised(sender As Object, e As EventArgs)

        Protected Friend Sub OnFlushRaised(e As EventArgs)
            RaiseEvent FlushRaised(Me, e)
        End Sub

        Public Sub Flush()
            Call OnFlushRaised(New EventArgs)
        End Sub
    End Class

#End Region

    Public Property SourceFiles(path As String) As Byte()
        Get
            Return Sv.SourceFile(path)
        End Get
        Set(value As Byte())
            Sv.SourceFile(path) = value
        End Set
    End Property

    Public Property SourceFilesAsText(path As String) As String
        Get
            Return System.Text.Encoding.UTF8.GetString(Sv.SourceFile(path))
        End Get
        Set(value As String)
            Sv.SourceFile(path) = System.Text.Encoding.UTF8.GetBytes(value)
        End Set
    End Property

    Public Property SourceFilesAsImage(path As String) As Image
        Get
            Dim conv As New ImageConverter
            Return CType(conv.ConvertFrom(Sv.SourceFile(path)), Image)
        End Get
        Set(value As Image)
            Dim conv As New ImageConverter
            Sv.SourceFile(path) = CType(conv.ConvertTo(value, GetType(Byte())), Byte())
        End Set
    End Property

    Private Const GeoJSONFile As String = "geojson_json.json"

    Public Property GeoJSONFileAsText() As String
        Get
            Return System.Text.Encoding.UTF8.GetString(Sv.SourceFile(GeoJSONFile))
        End Get
        Set(value As String)
            Sv.SourceFile(GeoJSONFile) = System.Text.Encoding.UTF8.GetBytes(value)
        End Set
    End Property

    Private WithEvents GeoJSONList As New ClsFlushList(Of String)
    Public ReadOnly Property GeoJSONAsList() As ClsFlushList(Of String)
        Get
            Return GeoJSONList
        End Get
    End Property

    Private Sub ClsFlushList_FlushRaised(sender As Object, e As EventArgs) Handles GeoJSONList.FlushRaised
        Dim this As ClsFlushList(Of String) = CType(sender, ClsFlushList(Of String))

        Dim json As String = ""
        If this.Count > 0 Then
            json = "{ ""type"": ""FeatureCollection"", ""features"": ["
            For Each item In this
                If item IsNot Nothing Then
                    If Not IsDBNull(item) Then
                        json &= item & ","
                    End If
                End If
            Next
            json = json.TrimEnd(","c) & "] }"
        End If
        Sv.SourceFile(GeoJSONFile) = System.Text.Encoding.UTF8.GetBytes(json)

    End Sub

    ''' <summary>
    ''' WebGIS連携のコントロールクラス
    ''' </summary>
    ''' <param name="browser">フォームに貼り付けたWebView2のオブジェクト</param>
    ''' <remarks>WebGISの機能を提供するクラス</remarks>
    ''' <example>Dim ctl As New ClsWebGISControl(Me.WebView21)</example>
    Public Sub New(browser As Microsoft.Web.WebView2.WinForms.WebView2)
        Sv = New Lib_InadaShokai_Web.SimpleWebServerClass()
        Br = New Lib_InadaShokai_Web.WebBrowserClass(browser)
    End Sub

    ''' <summary>
    ''' ブラウザ画面を開始
    ''' </summary>
    ''' <param name="SourceFiles">ブラウザに登録するファイル</param>
    ''' <param name="StartPagePath">最初にブラウザが表示するページ</param>
    Public Sub StartServer(StartPagePath As String, Optional SourceFiles As SortedList(Of String, Byte()) = Nothing)
        If SourceFiles IsNot Nothing Then
            For Each item As KeyValuePair(Of String, Byte()) In SourceFiles
                Sv.SourceFile(item.Key) = item.Value
            Next
        End If
        Sv.SetStartPath(StartPagePath)
        Sv.ServerStart()
    End Sub

    ''' <summary>
    ''' ブラウザ画面を終了
    ''' </summary>
    Public Sub StopServer()
        Sv.ServerStop()
    End Sub

    Private Sub Br_InitializationCompleted(e As Lib_InadaShokai_Web.WebBrowserClass.InitializationCompletedEventArgs) Handles Br.InitializationCompleted
        Br.Navigate(Sv.WebStartURL)
    End Sub

    Public Class ArgJS
        Public Class ParameterItem
            Public Name As String
            Public Value As String
        End Class

        Public [Type] As String
        Public Command As String
        Public Parameters As List(Of ParameterItem)
    End Class

    Private Sub Br_BrowserMessageReceived(e As Lib_InadaShokai_Web.WebBrowserClass.BrowserMessageReceivedEventArgs) Handles Br.BrowserMessageReceived
        Try
            Dim jsonObj As List(Of ArgJS) = Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of ArgJS))(e.Message)
            If jsonObj IsNot Nothing Then
                For Each jsonEach As ArgJS In jsonObj
                    If jsonEach.Type = "EventMessage" Then
                        Call ProcessMessage(jsonEach)
                    End If
                Next
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub ProcessMessage(arg As ArgJS)
        Select Case arg.Command
            Case "MapClicked"
                Dim Lat As Decimal = 0
                Dim Lon As Decimal = 0
                For Each item As ArgJS.ParameterItem In arg.Parameters
                    Select Case LCase(item.Name)
                        Case "longitude", "lon"
                            Lon = Decimal.Parse(item.Value)
                        Case "latitude", "lat"
                            Lat = Decimal.Parse(item.Value)
                    End Select
                Next
                Call OnMapClicked(New WebGISControlCoordEventsArgs(Lon, Lat))

            Case "FeatureClicked"
                Dim Uid As String = ""
                Dim childUid As String = ""
                Dim Lat As Decimal = 0
                Dim Lon As Decimal = 0
                For Each item As ArgJS.ParameterItem In arg.Parameters
                    Select Case LCase(item.Name)
                        Case "uid"
                            Uid = item.Value
                        Case "childuid"
                            childUid = item.Value
                        Case "longitude", "lon"
                            Lon = Decimal.Parse(item.Value)
                        Case "latitude", "lat"
                            Lat = Decimal.Parse(item.Value)
                    End Select
                Next
                Call OnFeatureClicked(New WebGISControlFeatureEventsArgs(Uid, childUid, Lon, Lat))

            Case "FeatureDragged"
                Dim Uid As String = ""
                Dim childUid As String = ""
                Dim Lat As Decimal = 0
                Dim Lon As Decimal = 0
                For Each item As ArgJS.ParameterItem In arg.Parameters
                    Select Case LCase(item.Name)
                        Case "uid"
                            Uid = item.Value
                        Case "childuid"
                            childUid = item.Value
                        Case "longitude", "lon"
                            Lon = Decimal.Parse(item.Value)
                        Case "latitude", "lat"
                            Lat = Decimal.Parse(item.Value)
                    End Select
                Next
                Call OnFeatureDragged(New WebGISControlFeatureEventsArgs(Uid, childUid, Lon, Lat))

            Case "ZoomInfoChanged"
                PrvZoomInfo = New STCZoomInfo With {
                    .zoom = 0,
                    .center = New List(Of Decimal) From {0, 0},
                    .extent = New List(Of Decimal) From {0, 0, 0, 0}
                }
                For Each item As ArgJS.ParameterItem In arg.Parameters
                    Select Case LCase(item.Name)
                        Case "zoom"
                            PrvZoomInfo.zoom = Decimal.Parse(item.Value)

                        Case "center"
                            Dim centerValues As String() = item.Value.Split(","c)
                            If centerValues.Length = 2 Then
                                PrvZoomInfo.center = New List(Of Decimal) From {
                                    Decimal.Parse(centerValues(0)),
                                    Decimal.Parse(centerValues(1))
                                }
                            End If

                        Case "extent"
                            Dim extentValues As String() = item.Value.Split(","c)
                            If extentValues.Length = 4 Then
                                PrvZoomInfo.extent = New List(Of Decimal) From {
                                    Decimal.Parse(extentValues(0)),
                                    Decimal.Parse(extentValues(1)),
                                    Decimal.Parse(extentValues(2)),
                                    Decimal.Parse(extentValues(3))
                                }
                            End If

                    End Select
                Next

            Case "RectangleSelected"
                Dim tempExtent As List(Of Decimal) = Nothing
                For Each item As ArgJS.ParameterItem In arg.Parameters
                    Select Case LCase(item.Name)
                        Case "extent"
                            Dim extentValues As String() = item.Value.Split(","c)
                            If extentValues.Length = 4 Then
                                tempExtent = New List(Of Decimal) From {
                                    Decimal.Parse(extentValues(0)),
                                    Decimal.Parse(extentValues(1)),
                                    Decimal.Parse(extentValues(2)),
                                    Decimal.Parse(extentValues(3))
                                }
                            End If

                    End Select
                Next
                If tempExtent Is Nothing OrElse tempExtent.Count <> 4 Then
                    Throw New Exception("RectangleSelected event requires a valid extent with 4 values.")
                End If
                Call OnRectangleSelected(New WebGISControlRectangleSelectedEventsArgs(tempExtent(0), tempExtent(1), tempExtent(2), tempExtent(3)))

        End Select
    End Sub

    Public Async Sub AddFeatureOnMap(UIDStr As String, FeatureJSON As String, FeatureStyleJSON As String)
        Dim command As String = "insertFeatureOnMap('" & UIDStr & "','" & FeatureJSON & "', '" & FeatureStyleJSON & "' );"
        Await Br.ExecuteCommandAsync(command)
    End Sub

    Public Async Sub AddFeatureOnMap(UIDStr As String, FeatureJSON As String)
        Dim command As String = "insertFeatureOnMap('" & UIDStr & "','" & FeatureJSON & "', null );"
        Await Br.ExecuteCommandAsync(command)
    End Sub

    Public Async Sub AddMarkerOnMap(UIDStr As String, Longitude As Decimal, Latitude As Decimal, MarkerJSON As String)
        Dim command As String = "insertMarkerOnMap('" & UIDStr & "'," & Longitude.ToString() & "," & Latitude.ToString() & ", '" & MarkerJSON & "' )"
        Await Br.ExecuteCommandAsync(command)
    End Sub

    Public Async Sub AddMarkerOnMap(UIDStr As String, Longitude As Decimal, Latitude As Decimal)
        Dim command As String = "insertMarkerOnMap('" & UIDStr & "'," & Longitude.ToString() & "," & Latitude.ToString() & ", null )"
        Await Br.ExecuteCommandAsync(command)
    End Sub

    Public Async Sub RemoveMarkerOnMap(UIDStr As String)
        Dim command As String = "removeMarkerOnMap('" & UIDStr & "')"
        Await Br.ExecuteCommandAsync(command)
    End Sub

    Public Async Sub RemoveAllMarkerOnMap()
        Dim command As String = "removeAllMarkerOnMap()"
        Await Br.ExecuteCommandAsync(command)
    End Sub

    Public Structure STCZoomInfo
        Public zoom As Decimal
        Public center As List(Of Decimal) ' [Longitude, Latitude]
        Public extent As List(Of Decimal) ' [Longitude1, Latitude1, Longitude2, Latitude2]
    End Structure

    Private PrvZoomInfo As STCZoomInfo

    Public Function GetZoomInfo() As STCZoomInfo
        Return PrvZoomInfo
    End Function

    Public Async Sub SetZoomCircle(Longitude As Decimal, Latitude As Decimal, Radius As Decimal)
        Dim command As String = "setZoomInfo({argstr})"
        Dim arg As String = String.Format("""center"": [{0}, {1}], ""radius"": {2}",
            Longitude.ToString(), Latitude.ToString(), Radius.ToString())
        Await Br.ExecuteCommandAsync(command.Replace("argstr", arg))
    End Sub

    Public Async Sub SetZoomRectangle(Fence As List(Of Decimal))
        Dim command As String = "setZoomInfo({argstr})"
        Dim arg As String = String.Format("""fence"": [{0}, {1}, {2}, {3}]",
            Fence(0).ToString(), Fence(1).ToString(), Fence(2).ToString(), Fence(3).ToString())
        Await Br.ExecuteCommandAsync(command.Replace("argstr", arg))
    End Sub

    Public Async Sub SetZoomInfo(Longitude As Decimal, Latitude As Decimal, zoom As Decimal)
        Dim command As String = "setZoomInfo({argstr})"
        Dim arg As String = String.Format("""center"": [{0}, {1}], ""zoom"": {2}",
            Longitude.ToString(), Latitude.ToString(), zoom.ToString())
        Await Br.ExecuteCommandAsync(command.Replace("argstr", arg))
    End Sub

    Public Async Sub DoSelectionMode(modeOn As Boolean)
        Dim command As String = "setSelectionRectangleInteraction(argstr)"
        Dim arg As String = String.Format("{0}", IIf(Of String)(modeOn, "true", "false"))
        Await Br.ExecuteCommandAsync(command.Replace("argstr", arg))
    End Sub

    Public Enum EnumBaseLayer As Integer
        e国土地理院地図 = 1
        eOpenStreetMap = 2
        e国土地理院航空写真 = 3
        eその他 = 0
    End Enum

    Private _baselayerkind As EnumBaseLayer = EnumBaseLayer.e国土地理院地図

    Public Async Sub ChangeBaseLayer(baselayer As EnumBaseLayer)
        Dim command As String = "changeBaseLayer(argstr)"
        Dim arg As String = CType(baselayer, Integer).ToString()
        Await Br.ExecuteCommandAsync(command.Replace("argstr", arg))
        _baselayerkind = baselayer
    End Sub

    Public Function GetBaseLayerKind() As EnumBaseLayer
        Return _baselayerkind
    End Function

    Public Sub RedrawMap()
        SetInitialBox(InitialBox:=Me.GetZoomInfo().extent)
        SetInitialBaseLayer(InitialBaseLayer:=Me.GetBaseLayerKind)
        Br.Reload()
    End Sub

    Public Sub RedrawMap(InitialBox As List(Of Decimal))
        SetInitialBox(InitialBox:=InitialBox)
        SetInitialBaseLayer(InitialBaseLayer:=Me.GetBaseLayerKind)
        Br.Reload()
    End Sub

    Public Sub SetInitialBox(Optional InitialBox As List(Of Decimal) = Nothing)
        Dim script As String = ""
        If InitialBox IsNot Nothing AndAlso InitialBox.Count = 4 AndAlso
            Not (InitialBox(0) = 0 And InitialBox(1) = 0 And InitialBox(2) = 0 And InitialBox(3) = 0) Then
            script = String.Format("var constantsInitialBox = [{0}, {1}, {2}, {3}];",
                 InitialBox(0), InitialBox(1), InitialBox(2), InitialBox(3))
        End If
        SourceFilesAsText("constantsBox.js") = script
    End Sub

    Public Sub SetInitialBaseLayer(Optional InitialBaseLayer As EnumBaseLayer = EnumBaseLayer.eその他)
        Dim script As String = ""
        If InitialBaseLayer <> EnumBaseLayer.eその他 Then
            script = String.Format("var constantsInitialBaseLayer = {0};", CType(InitialBaseLayer, Integer).ToString())
        End If
        SourceFilesAsText("constantsBaseLayer.js") = script
    End Sub
End Class
