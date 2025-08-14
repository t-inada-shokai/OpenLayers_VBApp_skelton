Imports System.Net
Imports System.Net.Sockets

Public Class SimpleWebServerClass

    '' Webサーバで表示するファイルを登録する
    Private Class ClsFileItem
        Public FileBody As Byte()
        Public ContentType As String
        Public ContentEncoding As String
    End Class

    Private SrcFile As New SortedList(Of String, ClsFileItem)
    Private StartPath As String

    Public Shared Function GetRandomUnusedPort() As Integer
        Dim listener As New TcpListener(IPAddress.Loopback, 0)
        listener.Start()
        Dim port As Integer = CType(listener.LocalEndpoint, IPEndPoint).Port
        listener.Stop()
        Return port
    End Function

    Private Function ToPath(path As String) As String
        Return "/"c & path.TrimStart("/"c)
    End Function

    Private Sub CreateNewFile(adpath As String, body As Byte())
        Dim contenttype As String
        Select Case LCase(adpath.Split("."c).Last)
            Case "html", "htm", "shtml"
                contenttype = "text/html"
            Case "css"
                contenttype = "text/css"
            Case "xml"
                contenttype = "text/xml"
            Case "gif"
                contenttype = "image/ gif"
            Case "jpeg", "jpg"
                contenttype = "image/jpeg"
            Case "js"
                contenttype = "text/javascript"
            Case "atom"
                contenttype = "application/atom+xml"
            Case "rss"
                contenttype = "application/rss+xml"
            Case "mml"
                contenttype = "text/mathml"
            Case "txt"
                contenttype = "text/plain"
            Case "jad"
                contenttype = "text/vnd.sun.j2me.app-descriptor"
            Case "wml"
                contenttype = "text/vnd.wap.wml"
            Case "htc"
                contenttype = "text/x-component"
            Case "png"
                contenttype = "image/png"
            Case "svg", "svgz"
                contenttype = "image/svg+xml"
            Case "tif", "tiff"
                contenttype = "image/tiff"
            Case "wbmp"
                contenttype = "image/vnd.wap.wbmp"
            Case "webp"
                contenttype = "image/webp"
            Case "ico"
                contenttype = "image/x-icon"
            Case "jng"
                contenttype = "image/x-jng"
            Case "bmp"
                contenttype = "image/x-ms-bmp"
            Case "woff"
                contenttype = "font/woff"
            Case "woff2"
                contenttype = "font/woff2"
            Case "jar", "war", "ear"
                contenttype = "application/java-archive"
            Case "json"
                contenttype = "application/json"
            Case "hqx"
                contenttype = "application/mac-binhex40"
            Case "doc"
                contenttype = "application/msword"
            Case "pdf"
                contenttype = "application/pdf"
            Case "ps", "eps", "ai"
                contenttype = "application/postscript"
            Case "rtf"
                contenttype = "application/rtf"
            Case "m3u8"
                contenttype = "application/vnd.apple.mpegurl"
            Case "kml"
                contenttype = "application/vnd.google-earth.kml+xml"
            Case "kmz"
                contenttype = "application/vnd.google-earth.kmz"
            Case "xls"
                contenttype = "application/vnd.ms-excel"
            Case "eot"
                contenttype = "application/vnd.ms-fontobject"
            Case "ppt"
                contenttype = "application/vnd.ms-powerpoint"
            Case "odg"
                contenttype = "application/vnd.oasis.opendocument.graphics"
            Case "odp"
                contenttype = "application/vnd.oasis.opendocument.presentation"
            Case Else
                contenttype = "application/octet-stream"
        End Select

        Dim value = New ClsFileItem With {
            .FileBody = body,
            .ContentType = contenttype
        }
        If value.ContentType.StartsWith("text/") Then
            value.ContentEncoding = "utf-8"
        End If
        SrcFile.Add(adpath, value)
    End Sub

    Public Property SourceFile(path As String) As Byte()
        Get
            If Not SrcFile.ContainsKey(ToPath(path)) Then
                Return {}
            End If
            Return SrcFile.Item(ToPath(path)).FileBody
        End Get
        Set(value As Byte())
            If SrcFile.ContainsKey(ToPath(path)) Then
                SrcFile.Item(ToPath(path)).FileBody = value
            Else
                CreateNewFile(ToPath(path), value)
            End If
        End Set
    End Property

    Public Property ContentType(path As String) As String
        Get
            If Not SrcFile.ContainsKey(ToPath(path)) Then
                Return String.Empty
            End If
            Return SrcFile.Item(ToPath(path)).ContentType
        End Get
        Set(value As String)
            If SrcFile.ContainsKey(ToPath(path)) Then
                SrcFile.Item(ToPath(path)).ContentType = value
            Else
                Throw New Exception("Pathが見つかりません")
            End If
        End Set
    End Property

    Public Property ContentEncoding(path As String) As String
        Get
            If Not SrcFile.ContainsKey(ToPath(path)) Then
                Return String.Empty
            End If
            Return SrcFile.Item(ToPath(path)).ContentEncoding
        End Get
        Set(value As String)
            If SrcFile.ContainsKey(ToPath(path)) Then
                SrcFile.Item(ToPath(path)).ContentEncoding = value
            Else
                Throw New Exception("Pathが見つかりません")
            End If
        End Set
    End Property

    Public Sub SetStartPath(path As String)
        If SrcFile.ContainsKey(ToPath(path)) Then
            StartPath = ToPath(path)
        Else
            Throw New Exception("未登録のパスを指定しました")
        End If
    End Sub

    Public ReadOnly Property WebStartURL As String
        Get
            Return PortPrefix.TrimEnd("/"c) & StartPath
        End Get
    End Property

    Private ServerStarted As Boolean = False
    Private PortListener As HttpListener
    Private disposedValue As Boolean
    Private ServerTask As Task
    Private Const PortPrefixTemplate As String = "http://127.0.0.1:{0}/"
    Private PortPrefix As String = String.Format(PortPrefixTemplate, 8080)

    Public Function ServerStart() As Boolean
        If ServerStarted Then
            Return False
        End If

        Dim TempPort As Integer = GetRandomUnusedPort()
        PortPrefix = String.Format(PortPrefixTemplate, TempPort)

        If ListenerStart(PortPrefix) = False Then
            Return False
        End If

        ServerWorkerRunningFlag = True
        ServerTask = Task.Run(AddressOf ServerWorker)
        Return True
    End Function

    Public Function ServerStop() As Boolean
        If Not ServerStarted Then
            Return False
        End If

        If ServerWorkerRunningFlag = True Then
            ServerWorkerRunningFlag = False
            ServerTask.Wait()
        End If

        If ListenerStop() = False Then
            Return False
        End If

        Return True
    End Function

    Private Function ListenerStart(prefix As String) As Boolean
        If Not HttpListener.IsSupported Then
            Throw New Exception("HttpListenerがサポートされていません")
        End If
        If prefix.Length = 0 Then
            Return False
        End If

        If PortListener IsNot Nothing Then
            Return False
        End If

        PortListener = New HttpListener
        PortListener.Prefixes.Add(prefix)

        PortListener.Start()

        Return True
    End Function

    Private Function ListenerStop() As Boolean
        Try
            PortListener.Stop()
        Catch e As Exception
            PortListener.Abort()
        End Try
        Return True
    End Function

    Private ServerWorkerRunningFlag As Boolean = False

    Private Sub ServerWorker()
        While ServerWorkerRunningFlag = True
            Dim context As HttpListenerContext = PortListener.GetContext
            Dim req As HttpListenerRequest = context.Request
            Dim res As HttpListenerResponse = context.Response

            Dim path As String = req.RawUrl
            If SrcFile.ContainsKey(ToPath(path)) Then
                With SrcFile.Item(ToPath(path))
                    Call SetResponseHeader(res)
                    res.ContentType = .ContentType
                    If .ContentEncoding <> "" Then
                        res.Headers.Add("Content-Encoding", .ContentEncoding)
                        'res.ContentEncoding = System.Text.Encoding.GetEncoding(.ContentEncoding)
                    End If
                    Dim buf As Byte() = .FileBody
                    res.ContentLength64 = buf.Length
                    res.OutputStream.Write(buf, 0, buf.Length)
                End With
            End If
            res.Close()
        End While
    End Sub

    Private Sub SetResponseHeader(res As HttpListenerResponse)
        With res.Headers
            .Add("Access-Control-Allow-Origin", "")
            .Add("Access-Control-Expose-Headers", "")
            .Add("Cross-Origin-Resource-Policy", "cross-origin")
        End With
    End Sub

End Class
