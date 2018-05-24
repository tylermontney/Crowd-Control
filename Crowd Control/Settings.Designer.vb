<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Settings
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.okBTN = New System.Windows.Forms.Button()
        Me.placeHolderLBL = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'okBTN
        '
        Me.okBTN.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.okBTN.Location = New System.Drawing.Point(94, 73)
        Me.okBTN.Name = "okBTN"
        Me.okBTN.Size = New System.Drawing.Size(98, 33)
        Me.okBTN.TabIndex = 0
        Me.okBTN.Text = "OK"
        Me.okBTN.UseVisualStyleBackColor = True
        '
        'placeHolderLBL
        '
        Me.placeHolderLBL.AutoSize = True
        Me.placeHolderLBL.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.placeHolderLBL.Location = New System.Drawing.Point(41, 9)
        Me.placeHolderLBL.Name = "placeHolderLBL"
        Me.placeHolderLBL.Size = New System.Drawing.Size(207, 37)
        Me.placeHolderLBL.TabIndex = 1
        Me.placeHolderLBL.Text = "Coming soon"
        '
        'Settings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(310, 118)
        Me.Controls.Add(Me.placeHolderLBL)
        Me.Controls.Add(Me.okBTN)
        Me.Name = "Settings"
        Me.Text = "Settings"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents okBTN As Button
    Friend WithEvents placeHolderLBL As Label
End Class
