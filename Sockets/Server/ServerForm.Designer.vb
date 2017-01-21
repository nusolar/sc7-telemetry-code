<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ServerForm
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
        Me.components = New System.ComponentModel.Container()
        Me.StatusText = New System.Windows.Forms.TextBox()
        Me.btnStart = New System.Windows.Forms.Button()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cboIPAddresses = New System.Windows.Forms.ComboBox()
        Me.HostName = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'StatusText
        '
        Me.StatusText.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.StatusText.Location = New System.Drawing.Point(12, 33)
        Me.StatusText.Multiline = True
        Me.StatusText.Name = "StatusText"
        Me.StatusText.Size = New System.Drawing.Size(442, 271)
        Me.StatusText.TabIndex = 0
        '
        'btnStart
        '
        Me.btnStart.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnStart.Location = New System.Drawing.Point(379, 310)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.Size = New System.Drawing.Size(75, 23)
        Me.btnStart.TabIndex = 1
        Me.btnStart.Text = "Start"
        Me.btnStart.UseVisualStyleBackColor = True
        '
        'Timer1
        '
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(171, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(112, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "IP Address to listen on"
        '
        'cboIPAddresses
        '
        Me.cboIPAddresses.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboIPAddresses.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboIPAddresses.FormattingEnabled = True
        Me.cboIPAddresses.Location = New System.Drawing.Point(289, 6)
        Me.cboIPAddresses.Name = "cboIPAddresses"
        Me.cboIPAddresses.Size = New System.Drawing.Size(165, 21)
        Me.cboIPAddresses.TabIndex = 3
        '
        'HostName
        '
        Me.HostName.AutoSize = True
        Me.HostName.Location = New System.Drawing.Point(12, 9)
        Me.HostName.Name = "HostName"
        Me.HostName.Size = New System.Drawing.Size(39, 13)
        Me.HostName.TabIndex = 4
        Me.HostName.Text = "Label2"
        '
        'ServerForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(466, 342)
        Me.Controls.Add(Me.HostName)
        Me.Controls.Add(Me.cboIPAddresses)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnStart)
        Me.Controls.Add(Me.StatusText)
        Me.Name = "ServerForm"
        Me.Text = "Server"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents StatusText As TextBox
    Friend WithEvents btnStart As Button
    Friend WithEvents Timer1 As Timer
    Friend WithEvents Label1 As Label
    Friend WithEvents cboIPAddresses As ComboBox
    Friend WithEvents HostName As Label
End Class
