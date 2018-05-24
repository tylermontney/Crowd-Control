Public Class Settings
    Private Sub okBTN_Click(sender As Object, e As EventArgs) Handles okBTN.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub
End Class