Imports System.Runtime.InteropServices

Class WinAPI
    <DllImport("wtsapi32.dll", SetLastError:=True, CharSet:=CharSet.Ansi)>
    Private Shared Function WTSLogoffSession(ByVal hServer As IntPtr, ByVal SessionId As Integer, ByVal bWait As Boolean) As Boolean
    End Function
    <DllImport("wtsapi32.dll", SetLastError:=True, CharSet:=CharSet.Ansi)>
    Private Shared Function WTSQuerySessionInformation(ByVal hServer As System.IntPtr, ByVal sessionId As Integer, ByVal wtsInfoClass As WTS_INFO_CLASS, <Out> ByRef ppBuffer As System.IntPtr, <Out> ByRef pBytesReturned As UInteger) As Boolean
    End Function
    <DllImport("wtsapi32.dll", SetLastError:=True, CharSet:=CharSet.Ansi)>
    Private Shared Sub WTSFreeMemory(ByVal pMemory As IntPtr)
    End Sub
    <DllImport("wtsapi32.dll", SetLastError:=True, CharSet:=CharSet.Ansi)>
    Private Shared Function WTSEnumerateSessions(ByVal hServer As IntPtr, <MarshalAs(UnmanagedType.U4)> ByVal Reserved As Int32, <MarshalAs(UnmanagedType.U4)> ByVal Version As Int32, ByRef ppSessionInfo As IntPtr, <MarshalAs(UnmanagedType.U4)> ByRef pCount As Int32) As Int32
    End Function
    <DllImport("wtsapi32.dll", SetLastError:=True, CharSet:=CharSet.Ansi)>
    Private Shared Function WTSOpenServer(<MarshalAs(UnmanagedType.LPStr)> ByVal pServerName As String) As IntPtr
    End Function


    Private Enum WTS_INFO_CLASS
        WTSInitialProgram
        WTSApplicationName
        WTSWorkingDirectory
        WTSOEMId
        WTSSessionId
        WTSUserName
        WTSWinStationName
        WTSDomainName
        WTSConnectState
        WTSClientBuildNumber
        WTSClientName
        WTSClientDirectory
        WTSClientProductId
        WTSClientHardwareId
        WTSClientAddress
        WTSClientDisplay
        WTSClientProtocolType
        WTSIdleTime
        WTSLogonTime
        WTSIncomingBytes
        WTSOutgoingBytes
        WTSIncomingFrames
        WTSOutgoingFrames
        WTSClientInfo
        WTSSessionInfo
    End Enum

    <StructLayout(LayoutKind.Sequential)>
    Private Structure WTS_SESSION_INFO
        Public SessionID As Int32
        <MarshalAs(UnmanagedType.LPStr)>
        Public pWinStationName As String
        Public State As WTS_CONNECTSTATE_CLASS
    End Structure

    Private Enum WTS_CONNECTSTATE_CLASS
        WTSActive
        WTSConnected
        WTSConnectQuery
        WTSShadow
        WTSDisconnected
        WTSIdle
        WTSListen
        WTSReset
        WTSDown
        WTSInit
    End Enum

    Private Shared Function GetUserSessionDictionary(ByVal server As IntPtr, ByVal sessions As List(Of Integer)) As Dictionary(Of String, Integer)
        Dim userSession As Dictionary(Of String, Integer) = New Dictionary(Of String, Integer)()

        For Each sessionId In sessions
            Dim uName As String = GetUserName(sessionId, server)
            If Not String.IsNullOrWhiteSpace(uName) Then userSession.Add(uName, sessionId)
        Next

        Return userSession
    End Function

    Private Shared Function GetSessionIDs(ByVal server As IntPtr) As List(Of Integer)
        Dim sessionIds As List(Of Integer) = New List(Of Integer)()
        Dim buffer As IntPtr = IntPtr.Zero
        Dim count As Integer = 0
        Dim retval As Integer = WTSEnumerateSessions(server, 0, 1, buffer, count)
        Dim dataSize As Integer = Marshal.SizeOf(GetType(WTS_SESSION_INFO))
        Dim current As Int64 = CInt(buffer)

        If retval <> 0 Then

            For i As Integer = 0 To count - 1
                Dim si As WTS_SESSION_INFO = CType(Marshal.PtrToStructure(CType(current, IntPtr), GetType(WTS_SESSION_INFO)), WTS_SESSION_INFO)
                current += dataSize
                sessionIds.Add(si.SessionID)
            Next

            WTSFreeMemory(buffer)
        End If

        Return sessionIds
    End Function

    Private Shared Function GetUserName(ByVal sessionId As Integer, ByVal server As IntPtr) As String
        Dim buffer As IntPtr = IntPtr.Zero
        Dim count As UInteger = 0
        Dim userName As String = String.Empty

        Try
            WTSQuerySessionInformation(server, sessionId, WTS_INFO_CLASS.WTSUserName, buffer, count)
            userName = Marshal.PtrToStringAnsi(buffer).ToUpper().Trim()
        Finally
            WTSFreeMemory(buffer)
        End Try

        Return userName
    End Function

    Public Shared Function UserLogout(ByVal Username As String) As Boolean
        Username = Username.Trim().ToUpper()
        Dim Server As IntPtr = WTSOpenServer(Environment.MachineName)
        Dim sessions As List(Of Integer) = GetSessionIDs(Server)
        Dim userSessionDictionary As Dictionary(Of String, Integer) = GetUserSessionDictionary(Server, sessions)

        If userSessionDictionary.ContainsKey(Username) Then
            Return WTSLogoffSession(0, userSessionDictionary(Username), True)
        Else
            Return False
        End If
    End Function
End Class