<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ClientForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ServerHostName = New System.Windows.Forms.TextBox()
        Me.ServerData = New System.Windows.Forms.TextBox()
        Me.btnConnect = New System.Windows.Forms.Button()
        Me.ConnectionStatus = New System.Windows.Forms.Label()
        Me.btnPause = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(21, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(38, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Server"
        '
        'ServerHostName
        '
        Me.ServerHostName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ServerHostName.Location = New System.Drawing.Point(81, 12)
        Me.ServerHostName.Name = "ServerHostName"
        Me.ServerHostName.Size = New System.Drawing.Size(375, 20)
        Me.ServerHostName.TabIndex = 1
        Me.ServerHostName.Text = "localhost"
        '
        'ServerData
        '
        Me.ServerData.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ServerData.Location = New System.Drawing.Point(25, 38)
        Me.ServerData.Multiline = True
        Me.ServerData.Name = "ServerData"
        Me.ServerData.Size = New System.Drawing.Size(431, 297)
        Me.ServerData.TabIndex = 2
        '
        'btnConnect
        '
        Me.btnConnect.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnConnect.Location = New System.Drawing.Point(381, 345)
        Me.btnConnect.Name = "btnConnect"
        Me.btnConnect.Size = New System.Drawing.Size(75, 23)
        Me.btnConnect.TabIndex = 3
        Me.btnConnect.Text = "Connect"
        Me.btnConnect.UseVisualStyleBackColor = True
        '
        'ConnectionStatus
        '
        Me.ConnectionStatus.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ConnectionStatus.AutoSize = True
        Me.ConnectionStatus.Location = New System.Drawing.Point(12, 358)
        Me.ConnectionStatus.Name = "ConnectionStatus"
        Me.ConnectionStatus.Size = New System.Drawing.Size(78, 13)
        Me.ConnectionStatus.TabIndex = 4
        Me.ConnectionStatus.Text = "Not connected"
        '
        'btnPause
        '
        Me.btnPause.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPause.Location = New System.Drawing.Point(300, 345)
        Me.btnPause.Name = "btnPause"
        Me.btnPause.Size = New System.Drawing.Size(75, 23)
        Me.btnPause.TabIndex = 5
        Me.btnPause.Text = "Pause"
        Me.btnPause.UseVisualStyleBackColor = True
        '
        'ClientForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(469, 380)
        Me.Controls.Add(Me.btnPause)
        Me.Controls.Add(Me.ConnectionStatus)
        Me.Controls.Add(Me.btnConnect)
        Me.Controls.Add(Me.ServerData)
        Me.Controls.Add(Me.ServerHostName)
        Me.Controls.Add(Me.Label1)
        Me.Name = "ClientForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Client"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents ServerHostName As TextBox
    Friend WithEvents ServerData As TextBox
    Friend WithEvents btnConnect As Button
    Friend WithEvents ConnectionStatus As Label
    Friend WithEvents btnPause As Button
End Class
