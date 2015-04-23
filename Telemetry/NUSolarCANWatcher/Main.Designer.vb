<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Main
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
        Me.CANCheckTimer = New System.Windows.Forms.Timer(Me.components)
        Me.btnClose = New System.Windows.Forms.Button()
        Me.DataGrid = New System.Windows.Forms.DataGridView()
        Me.FieldName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CanTag = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ByteOffset = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Value = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.chkPause = New System.Windows.Forms.CheckBox()
        Me.CANRead_BW = New System.ComponentModel.BackgroundWorker()
        Me.CANParse_BW = New System.ComponentModel.BackgroundWorker()
        Me.SaveDataTimer = New System.Windows.Forms.Timer(Me.components)
        CType(Me.DataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'CANCheckTimer
        '
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.Location = New System.Drawing.Point(439, 342)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 0
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'DataGrid
        '
        Me.DataGrid.AllowUserToAddRows = False
        Me.DataGrid.AllowUserToDeleteRows = False
        Me.DataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.FieldName, Me.CanTag, Me.ByteOffset, Me.Value})
        Me.DataGrid.Location = New System.Drawing.Point(12, 12)
        Me.DataGrid.Name = "DataGrid"
        Me.DataGrid.ReadOnly = True
        Me.DataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGrid.Size = New System.Drawing.Size(502, 313)
        Me.DataGrid.TabIndex = 1
        '
        'FieldName
        '
        Me.FieldName.HeaderText = "Field Name"
        Me.FieldName.Name = "FieldName"
        Me.FieldName.ReadOnly = True
        '
        'CanTag
        '
        Me.CanTag.HeaderText = "CAN Tag"
        Me.CanTag.Name = "CanTag"
        Me.CanTag.ReadOnly = True
        '
        'ByteOffset
        '
        Me.ByteOffset.HeaderText = "Byte Offset"
        Me.ByteOffset.Name = "ByteOffset"
        Me.ByteOffset.ReadOnly = True
        '
        'Value
        '
        Me.Value.HeaderText = "Data Value"
        Me.Value.Name = "Value"
        Me.Value.ReadOnly = True
        '
        'chkPause
        '
        Me.chkPause.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkPause.AutoSize = True
        Me.chkPause.Location = New System.Drawing.Point(12, 342)
        Me.chkPause.Name = "chkPause"
        Me.chkPause.Size = New System.Drawing.Size(90, 17)
        Me.chkPause.TabIndex = 2
        Me.chkPause.Text = "Pause Polling"
        Me.chkPause.UseVisualStyleBackColor = True
        '
        'CANRead_BW
        '
        Me.CANRead_BW.WorkerSupportsCancellation = True
        '
        'Main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(526, 377)
        Me.Controls.Add(Me.chkPause)
        Me.Controls.Add(Me.DataGrid)
        Me.Controls.Add(Me.btnClose)
        Me.Name = "Main"
        Me.Text = "NUSolarCANWatcher"
        CType(Me.DataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents CANCheckTimer As System.Windows.Forms.Timer
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents DataGrid As System.Windows.Forms.DataGridView
    Friend WithEvents chkPause As System.Windows.Forms.CheckBox
    Friend WithEvents FieldName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CanTag As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ByteOffset As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Value As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CANRead_BW As System.ComponentModel.BackgroundWorker
    Friend WithEvents CANParse_BW As System.ComponentModel.BackgroundWorker
    Friend WithEvents SaveDataTimer As System.Windows.Forms.Timer

End Class
