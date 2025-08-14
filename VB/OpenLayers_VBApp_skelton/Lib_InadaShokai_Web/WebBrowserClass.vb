Imports Microsoft.Web.WebView2

Public Class WebBrowserClass
    Private WithEvents ObjBrowser As WinForms.WebView2
    Private Structure StcModuleVar
        Public IsInitializationCompleted As Boolean
        Public IsNavigationCompleted As Boolean
        Public IsNavigating As Boolean
    End Structure
    Private mdl As StcModuleVar

    Public Sub New(obj As WinForms.WebView2)
        ObjBrowser = obj

        mdl.IsInitializationCompleted = False
        mdl.IsNavigationCompleted = False
        mdl.IsNavigating = False

        Call InitializeWebViewAsync()
    End Sub

    Public ReadOnly Property IsInitializationCompleted As Boolean
        Get
            Return mdl.IsInitializationCompleted
        End Get
    End Property

    Public ReadOnly Property IsNavigationCompleted As Boolean
        Get
            Return mdl.IsNavigationCompleted
        End Get
    End Property

    Public ReadOnly Property IsNavigating As Boolean
        Get
            Return mdl.IsNavigating
        End Get
    End Property

    ''' <summary>
    ''' ブラウザのサイト移動を行う
    ''' </summary>
    ''' <param name="url">移動先のURL</param>
    Public Sub Navigate(url As String)
        If mdl.IsInitializationCompleted = False Then
            Throw New Exception("初期化が完了していません")
        End If

        mdl.IsInitializationCompleted = False
        mdl.IsNavigating = True

        ObjBrowser.CoreWebView2.Navigate(url)
    End Sub

    Public Sub Reload()
        ObjBrowser.CoreWebView2.Reload()
    End Sub

    ''' <summary>
    ''' ブラウザ上で指定した文字列のjavascriptを実行する
    ''' </summary>
    ''' <param name="command">javascript文字列</param>
    ''' <returns></returns>
    Public Async Function ExecuteCommandAsync(command As String) As Task
        Await ObjBrowser.ExecuteScriptAsync(command)
    End Function

#Region "ナビゲーション処理"

    Public Event NavigationStarting(e As NavigationStartingEventArgs)
    Public Event NavigationCompleted(e As NavigationCompletedEventArgs)

    Public Class NavigationCompletedEventArgs
        Inherits EventArgs

        Public ReadOnly Property IsSuccess As Boolean
        Public ReadOnly Property HttpStatusCode As Integer

        Public Sub New(IsSuccess As Boolean, HttpStatusCode As Integer)
            Me.IsSuccess = IsSuccess
            Me.HttpStatusCode = HttpStatusCode
        End Sub

    End Class

    Public Class NavigationStartingEventArgs
        Inherits EventArgs

        Public ReadOnly Property URI As String

        Public Property Cancel As Boolean = False

        Public Sub New(URI As String)
            Me.URI = URI
        End Sub

    End Class

    Protected Friend Sub OnNavigationCompleted(e As NavigationCompletedEventArgs)
        RaiseEvent NavigationCompleted(e)
    End Sub

    Protected Friend Sub OnNavigationStarting(e As NavigationStartingEventArgs)
        RaiseEvent NavigationStarting(e)
    End Sub

    Private Sub ObjBrowser_NavigationCompleted(sender As Object, e As Core.CoreWebView2NavigationCompletedEventArgs) Handles ObjBrowser.NavigationCompleted
        mdl.IsNavigating = False
        mdl.IsNavigationCompleted = True
        Call OnNavigationCompleted(New NavigationCompletedEventArgs(IsSuccess:=e.IsSuccess, HttpStatusCode:=e.HttpStatusCode))
    End Sub

    Private Sub ObjBrowser_NavigationStarting(sender As Object, e As Core.CoreWebView2NavigationStartingEventArgs) Handles ObjBrowser.NavigationStarting
        mdl.IsNavigationCompleted = False
        mdl.IsNavigating = True
        Dim arge As New NavigationStartingEventArgs(URI:=e.Uri)
        Call OnNavigationStarting(arge)
        e.Cancel = arge.Cancel
    End Sub
#End Region

#Region "ブラウザオブジェクトの初期化処理"
    ''' <summary>
    ''' ブラウザオブジェクトの初期化開始
    ''' </summary>
    Private Async Sub InitializeWebViewAsync()
        Dim tempDir As String = IO.Path.Combine(IO.Path.GetTempPath(), My.Application.Info.AssemblyName)
        Dim env As Core.CoreWebView2Environment = Core.CoreWebView2Environment.CreateAsync(
            browserExecutableFolder:=Nothing,
            userDataFolder:=tempDir,
            options:=Nothing
        ).Result
        Await ObjBrowser.EnsureCoreWebView2Async(env)
    End Sub

    ''イベント定義
    Public Event InitializationCompleted(e As InitializationCompletedEventArgs)

    ''イベント引数の定義
    Public Class InitializationCompletedEventArgs
        Inherits EventArgs

        Public ReadOnly InitializationSuccess As Boolean

        Public Sub New(isSuccess As Boolean)
            InitializationSuccess = isSuccess
        End Sub
    End Class

    Protected Friend Sub OnInitializeCompleted(isSuccess As Boolean)
        RaiseEvent InitializationCompleted(New InitializationCompletedEventArgs(isSuccess:=isSuccess))
    End Sub

    Private Sub ObjBrowser_CoreWebView2InitializationCompleted(sender As Object, e As Core.CoreWebView2InitializationCompletedEventArgs) Handles ObjBrowser.CoreWebView2InitializationCompleted
        mdl.IsInitializationCompleted = e.IsSuccess
        Call OnInitializeCompleted(e.IsSuccess)
    End Sub
#End Region

#Region "ブラウザからの受信メッセージ処理"
    ''イベント定義
    Public Event BrowserMessageReceived(e As BrowserMessageReceivedEventArgs)

    ''イベント引数の定義
    Public Class BrowserMessageReceivedEventArgs
        Inherits EventArgs

        Public ReadOnly Message As String

        Public Sub New(msg As String)
            Message = msg
        End Sub

    End Class

    ''ブラウザメッセージ受信処理
    Protected Friend Sub OnBrowserMessageReceived(message As String)
        RaiseEvent BrowserMessageReceived(New BrowserMessageReceivedEventArgs(message))
    End Sub


    Private Sub ObjBrowser_WebMessageReceived(sender As Object, e As Core.CoreWebView2WebMessageReceivedEventArgs) Handles ObjBrowser.WebMessageReceived
        Dim text As String = e.TryGetWebMessageAsString()
        Call OnBrowserMessageReceived(text)
    End Sub
#End Region

End Class
