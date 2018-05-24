Public Class Controller
    Private EventWatcher_Logon As Eventing.Reader.EventLogWatcher

    Public Function Clear(Optional ByVal ExcludedUsers As List(Of String) = Nothing) As Dictionary(Of String, Boolean)
        Dim Users As List(Of String) = InactiveUsers()
        'Could make merging users with excluded more concise.
        If ExcludedUsers IsNot Nothing Then
            For Each User As String In ExcludedUsers
                Users.Remove(User)
            Next
        End If
        Dim LogoutResults As Dictionary(Of String, Boolean) = EndUserSessions(Users)

        Return LogoutResults
    End Function

    Public Sub New()
        AddLogonWatcher()
    End Sub

    Private Sub LogonHandler(sender As Object, e As Eventing.Reader.EventRecordWrittenEventArgs)
        'Detection should be better to determine who is the current user. Currently done by checking
        'target user of event record. Interactive logons are shared by accounts like DWM. Can resolve SID
        'but only for local accounts. Would need to do LDAP lookup for domain accounts, which feels like overkill.
        Dim XmlDoc As New Xml.XmlDocument
        XmlDoc.LoadXml(e.EventRecord.ToXml())
        Dim CurrentUser As String = String.Empty
        Dim CurrentDomain As String = String.Empty
        Dim LogonType As String = String.Empty

        For Each Tag In XmlDoc.GetElementsByTagName("Data")
            'Preferrably break as early as possible for (nominal) performance reasons
            If Tag.OuterXML.Contains("TargetUserName") Then CurrentUser = Tag.InnerText
            If Tag.OuterXML.Contains("LogonType") Then LogonType = Tag.InnerText
            If Tag.OuterXML.Contains("TargetDomainName") Then CurrentDomain = Tag.InnerText
        Next

        Dim IsInteractive As Boolean = (LogonType = "2" Or LogonType = "11")
        Dim IsNotServiceAccount As Boolean = (CurrentUser.Contains("UMFD") = False And CurrentUser.Contains("DWM") = False)

        If IsInteractive And IsNotServiceAccount Then Clear(New List(Of String) From {CurrentUser})
    End Sub

    Private Sub AddLogonWatcher()
        Dim EventQuery As New Eventing.Reader.EventLogQuery("Security", Eventing.Reader.PathType.LogName, "*[System/EventID=4624]")
        EventWatcher_Logon = New Eventing.Reader.EventLogWatcher(EventQuery)
        AddHandler EventWatcher_Logon.EventRecordWritten, AddressOf LogonHandler
        EventWatcher_Logon.Enabled = True
    End Sub

    Private Function InactiveUsers() As List(Of String)
        'Determines logged in users by seeing who owns each instance of a running explorer.exe process.
        'Locked users will also have LogonUI.exe open, but is usually owned by SYSTEM.
        Dim Users As New List(Of String)
        Dim CurrentUser As String = System.Security.Principal.WindowsIdentity.GetCurrent().Name

        Dim LogonUIs As Process() = Process.GetProcessesByName("explorer")
        For Each LogonUI As Process In LogonUIs
            Dim User As String = GetProcessOwner(LogonUI.Id)
            If User <> CurrentUser Then Users.Add(User.Split("\"c)(1))
        Next

        Return Users
    End Function

    Private Function EndUserSessions(ByVal Usernames As List(Of String)) As Dictionary(Of String, Boolean)
        'Implements an unmanaged WinAPI function to end the session. Pass username only (aka, not ABC\john.smith).
        Dim ReturnData As New Dictionary(Of String, Boolean)

        For Each Username As String In Usernames
            Dim Result As Boolean = WinAPI.UserLogout(Username)
            ReturnData.Add(Username, Result)
        Next

        Return ReturnData
    End Function

    Private Function GetProcessOwner(ByVal ProcID As Integer) As String
        Dim objSearcher As New Management.ManagementObjectSearcher("Select * From Win32_Process Where ProcessID = " & ProcID.ToString())
        Dim objResults As Management.ManagementObjectCollection = objSearcher.Get()

        For Each Result As Management.ManagementObject In objResults
            Dim ReturnData As String() = New String() {"", ""}
            Dim InvokeResult As Integer = Convert.ToInt32(Result.InvokeMethod("GetOwner", ReturnData))
            If InvokeResult = 0 Then Return ReturnData(1) & "\" & ReturnData(0)
        Next

        Return String.Empty
    End Function
End Class
