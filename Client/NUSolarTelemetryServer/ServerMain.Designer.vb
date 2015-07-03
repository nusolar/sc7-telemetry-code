<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ServerMain
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
        Me.statusbar = New System.Windows.Forms.StatusStrip()
        Me.tsRowNumber = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ssConnectedStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripContainer1 = New System.Windows.Forms.ToolStripContainer()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.chkLogProtocol = New System.Windows.Forms.CheckBox()
        Me.chkLogMessages = New System.Windows.Forms.CheckBox()
        Me.chkPause = New System.Windows.Forms.CheckBox()
        Me.chkWriteNewRows = New System.Windows.Forms.CheckBox()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.HelloTimer = New System.Windows.Forms.Timer(Me.components)
        Me.txtTrace = New System.Windows.Forms.TextBox()
        Me.statusbar.SuspendLayout()
        Me.ToolStripContainer1.BottomToolStripPanel.SuspendLayout()
        Me.ToolStripContainer1.ContentPanel.SuspendLayout()
        Me.ToolStripContainer1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'statusbar
        '
        Me.statusbar.Dock = System.Windows.Forms.DockStyle.None
        Me.statusbar.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsRowNumber, Me.ssConnectedStatus})
        Me.statusbar.Location = New System.Drawing.Point(0, 0)
        Me.statusbar.Name = "statusbar"
        Me.statusbar.Size = New System.Drawing.Size(486, 22)
        Me.statusbar.TabIndex = 4
        Me.statusbar.Text = "StatusStrip1"
        '
        'tsRowNumber
        '
        Me.tsRowNumber.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsRowNumber.Name = "tsRowNumber"
        Me.tsRowNumber.Size = New System.Drawing.Size(325, 17)
        Me.tsRowNumber.Spring = True
        Me.tsRowNumber.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ssConnectedStatus
        '
        Me.ssConnectedStatus.Name = "ssConnectedStatus"
        Me.ssConnectedStatus.Size = New System.Drawing.Size(100, 17)
        Me.ssConnectedStatus.Text = "ssConnectedStatus"
        Me.ssConnectedStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ToolStripContainer1
        '
        '
        'ToolStripContainer1.BottomToolStripPanel
        '
        Me.ToolStripContainer1.BottomToolStripPanel.Controls.Add(Me.statusbar)
        '
        'ToolStripContainer1.ContentPanel
        '
        Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.txtTrace)
        Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.GroupBox1)
        Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.btnClose)
        Me.ToolStripContainer1.ContentPanel.Size = New System.Drawing.Size(486, 269)
        Me.ToolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ToolStripContainer1.LeftToolStripPanelVisible = False
        Me.ToolStripContainer1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStripContainer1.Name = "ToolStripContainer1"
        Me.ToolStripContainer1.RightToolStripPanelVisible = False
        Me.ToolStripContainer1.Size = New System.Drawing.Size(486, 291)
        Me.ToolStripContainer1.TabIndex = 5
        Me.ToolStripContainer1.Text = "ToolStripContainer1"
        Me.ToolStripContainer1.TopToolStripPanelVisible = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.chkLogProtocol)
        Me.GroupBox1.Controls.Add(Me.chkLogMessages)
        Me.GroupBox1.Controls.Add(Me.chkPause)
        Me.GroupBox1.Controls.Add(Me.chkWriteNewRows)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 8)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(462, 121)
        Me.GroupBox1.TabIndex = 3
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Options"
        '
        'chkLogProtocol
        '
        Me.chkLogProtocol.AutoSize = True
        Me.chkLogProtocol.Location = New System.Drawing.Point(30, 65)
        Me.chkLogProtocol.Name = "chkLogProtocol"
        Me.chkLogProtocol.Size = New System.Drawing.Size(85, 17)
        Me.chkLogProtocol.TabIndex = 3
        Me.chkLogProtocol.Text = "Log protocol"
        Me.chkLogProtocol.UseVisualStyleBackColor = True
        '
        'chkLogMessages
        '
        Me.chkLogMessages.AutoSize = True
        Me.chkLogMessages.Location = New System.Drawing.Point(30, 42)
        Me.chkLogMessages.Name = "chkLogMessages"
        Me.chkLogMessages.Size = New System.Drawing.Size(139, 17)
        Me.chkLogMessages.TabIndex = 2
        Me.chkLogMessages.Text = "Log incoming messages"
        Me.chkLogMessages.UseVisualStyleBackColor = True
        '
        'chkPause
        '
        Me.chkPause.AutoSize = True
        Me.chkPause.Location = New System.Drawing.Point(30, 88)
        Me.chkPause.Name = "chkPause"
        Me.chkPause.Size = New System.Drawing.Size(127, 17)
        Me.chkPause.TabIndex = 1
        Me.chkPause.Text = "Pause receipt of data"
        Me.chkPause.UseVisualStyleBackColor = True
        '
        'chkWriteNewRows
        '
        Me.chkWriteNewRows.AutoSize = True
        Me.chkWriteNewRows.Checked = True
        Me.chkWriteNewRows.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkWriteNewRows.Location = New System.Drawing.Point(30, 19)
        Me.chkWriteNewRows.Name = "chkWriteNewRows"
        Me.chkWriteNewRows.Size = New System.Drawing.Size(170, 17)
        Me.chkWriteNewRows.TabIndex = 0
        Me.chkWriteNewRows.Text = "Write new rows to history table"
        Me.chkWriteNewRows.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.Location = New System.Drawing.Point(399, 233)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 2
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Text = "NotifyIcon1"
        Me.NotifyIcon1.Visible = True
        '
        'Timer1
        '
        '
        'HelloTimer
        '
        Me.HelloTimer.Interval = 2500
        '
        'txtTrace
        '
        Me.txtTrace.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTrace.Location = New System.Drawing.Point(12, 138)
        Me.txtTrace.Multiline = True
        Me.txtTrace.Name = "txtTrace"
        Me.txtTrace.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtTrace.Size = New System.Drawing.Size(462, 89)
        Me.txtTrace.TabIndex = 4
        '
        'ServerMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(486, 291)
        Me.Controls.Add(Me.ToolStripContainer1)
        Me.MaximizeBox = False
        Me.Name = "ServerMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "NUSolar Telemetery Server"
        Me.WindowState = System.Windows.Forms.FormWindowState.Minimized
        Me.statusbar.ResumeLayout(False)
        Me.statusbar.PerformLayout()
        Me.ToolStripContainer1.BottomToolStripPanel.ResumeLayout(False)
        Me.ToolStripContainer1.BottomToolStripPanel.PerformLayout()
        Me.ToolStripContainer1.ContentPanel.ResumeLayout(False)
        Me.ToolStripContainer1.ContentPanel.PerformLayout()
        Me.ToolStripContainer1.ResumeLayout(False)
        Me.ToolStripContainer1.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents statusbar As System.Windows.Forms.StatusStrip
    Friend WithEvents tsRowNumber As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ssConnectedStatus As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripContainer1 As System.Windows.Forms.ToolStripContainer
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents chkWriteNewRows As System.Windows.Forms.CheckBox
    Friend WithEvents chkPause As System.Windows.Forms.CheckBox
    Friend WithEvents chkLogMessages As System.Windows.Forms.CheckBox
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents HelloTimer As System.Windows.Forms.Timer
    Friend WithEvents chkLogProtocol As System.Windows.Forms.CheckBox
    Friend WithEvents txtTrace As System.Windows.Forms.TextBox

End Class
