Public Class Core
    Private ni As NotifyIcon
    Private SettingsDlg As Settings
    Private usrCont As Controller
    Private Sub Core_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Possibly replace a hidden window with nothing, but still keep the NotifyIcon component?
        WindowState = FormWindowState.Minimized
        Me.Hide()
        Me.ShowIcon = False

        Dim SettingsMI = New MenuItem("Settings")
        AddHandler SettingsMI.Click, AddressOf Settings_Click
        Dim TestMI = New MenuItem("Test")
        AddHandler TestMI.Click, AddressOf Test_Click
        Dim ExitMI = New MenuItem("Exit")
        AddHandler ExitMI.Click, AddressOf Exit_Click

        Dim cm As New ContextMenu
        With cm.MenuItems
            .Add(SettingsMI)
            .Add(TestMI)
            'Text divider is temporary.
            .Add(New MenuItem("_____________"))
            .Add(ExitMI)
        End With
        ni = New NotifyIcon With {
            .ContextMenu = cm,
            .Icon = New Icon(Environment.CurrentDirectory & "\" & "logos\tray.ico"),
            .Visible = True
        }

        SettingsDlg = New Settings
        usrCont = New Controller
    End Sub

    Private Sub Settings_Click(sender As Object, e As EventArgs)
        SettingsDlg.ShowDialog()
    End Sub

    Private Sub Test_Click(sender As Object, e As EventArgs)
        Dim Results = usrCont.Clear()
        Dim sb As New Text.StringBuilder
        For Each item In Results
            sb.AppendLine(item.Key & ": " & item.Value)
        Next

        MsgBox(sb.ToString())
    End Sub

    Private Sub Exit_Click(sender As Object, e As EventArgs)
        Environment.Exit(0)
    End Sub
End Class