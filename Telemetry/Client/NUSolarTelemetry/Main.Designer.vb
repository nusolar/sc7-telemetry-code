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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim ChartArea1 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend1 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series1 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim ChartArea2 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend2 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series2 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Main))
        Dim ChartArea3 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend3 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series3 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.lastTimeStamp = New System.Windows.Forms.DateTimePicker()
        Me.lblOdometer = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.lblCruiseSpeed = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.lblRegenStatus = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.lblVehicleSpeed = New System.Windows.Forms.Label()
        Me.lblHighestBaterryTemperature = New System.Windows.Forms.Label()
        Me.lblLowestBateryVoltage = New System.Windows.Forms.Label()
        Me.lblMotorCurrent = New System.Windows.Forms.Label()
        Me.lblBatteryCurrent = New System.Windows.Forms.Label()
        Me.lblArrayCurrent = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblUpdateTimeStamp = New System.Windows.Forms.Label()
        Me.gridTelemetry = New System.Windows.Forms.DataGridView()
        Me.tabCharts = New System.Windows.Forms.TabControl()
        Me.tabSummary = New System.Windows.Forms.TabPage()
        Me.chartMain = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.tabBatteryVoltages = New System.Windows.Forms.TabPage()
        Me.chartBatteryStatus = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.tabTemperatures = New System.Windows.Forms.TabPage()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.lblTempHeatsink = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.lblTempCapacitor = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.lblTempProcessor = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.lblTempAirOutlet = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.lblTempAirInlet = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.lblTempMotor = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.lblTemperature32 = New System.Windows.Forms.Label()
        Me.lblTemperature31 = New System.Windows.Forms.Label()
        Me.lblTemperature30 = New System.Windows.Forms.Label()
        Me.lblTemperature29 = New System.Windows.Forms.Label()
        Me.lblTemperature28 = New System.Windows.Forms.Label()
        Me.lblTemperature27 = New System.Windows.Forms.Label()
        Me.lblTemperature26 = New System.Windows.Forms.Label()
        Me.lblTemperature25 = New System.Windows.Forms.Label()
        Me.lblTemperature24 = New System.Windows.Forms.Label()
        Me.lblTemperature23 = New System.Windows.Forms.Label()
        Me.lblTemperature22 = New System.Windows.Forms.Label()
        Me.lblTemperature21 = New System.Windows.Forms.Label()
        Me.lblTemperature20 = New System.Windows.Forms.Label()
        Me.lblTemperature19 = New System.Windows.Forms.Label()
        Me.lblTemperature18 = New System.Windows.Forms.Label()
        Me.lblTemperature17 = New System.Windows.Forms.Label()
        Me.lblTemperature16 = New System.Windows.Forms.Label()
        Me.lblTemperature15 = New System.Windows.Forms.Label()
        Me.lblTemperature14 = New System.Windows.Forms.Label()
        Me.lblTemperature13 = New System.Windows.Forms.Label()
        Me.lblTemperature12 = New System.Windows.Forms.Label()
        Me.lblTemperature11 = New System.Windows.Forms.Label()
        Me.lblTemperature10 = New System.Windows.Forms.Label()
        Me.lblTemperature9 = New System.Windows.Forms.Label()
        Me.lblTemperature8 = New System.Windows.Forms.Label()
        Me.lblTemperature7 = New System.Windows.Forms.Label()
        Me.lblTemperature6 = New System.Windows.Forms.Label()
        Me.lblTemperature5 = New System.Windows.Forms.Label()
        Me.lblTemperature4 = New System.Windows.Forms.Label()
        Me.lblTemperature3 = New System.Windows.Forms.Label()
        Me.lblTemperature2 = New System.Windows.Forms.Label()
        Me.lblTemperature1 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.tabUserSpecified = New System.Windows.Forms.TabPage()
        Me.chartUserSpecified = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.tb = New System.Windows.Forms.ToolStrip()
        Me.ID_Exit = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
        Me.cboDisplayMode = New System.Windows.Forms.ToolStripComboBox()
        Me.ToolStripLabel2 = New System.Windows.Forms.ToolStripLabel()
        Me.cboDataInterval = New System.Windows.Forms.ToolStripComboBox()
        Me.ID_RetrieveData = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnInclude = New System.Windows.Forms.ToolStripDropDownButton()
        Me.ID_Current = New System.Windows.Forms.ToolStripMenuItem()
        Me.ID_Speed = New System.Windows.Forms.ToolStripMenuItem()
        Me.ID_Battery = New System.Windows.Forms.ToolStripMenuItem()
        Me.tbZoomable = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripContainer1 = New System.Windows.Forms.ToolStripContainer()
        Me.statusbar = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ssConnectedStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripContainer2 = New System.Windows.Forms.ToolStripContainer()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        CType(Me.gridTelemetry, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabCharts.SuspendLayout()
        Me.tabSummary.SuspendLayout()
        CType(Me.chartMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabBatteryVoltages.SuspendLayout()
        CType(Me.chartBatteryStatus, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabTemperatures.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabUserSpecified.SuspendLayout()
        CType(Me.chartUserSpecified, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tb.SuspendLayout()
        Me.ToolStripContainer1.ContentPanel.SuspendLayout()
        Me.ToolStripContainer1.TopToolStripPanel.SuspendLayout()
        Me.ToolStripContainer1.SuspendLayout()
        Me.statusbar.SuspendLayout()
        Me.ToolStripContainer2.BottomToolStripPanel.SuspendLayout()
        Me.ToolStripContainer2.ContentPanel.SuspendLayout()
        Me.ToolStripContainer2.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.SplitContainer2)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.tabCharts)
        Me.SplitContainer1.Size = New System.Drawing.Size(954, 566)
        Me.SplitContainer1.SplitterDistance = 287
        Me.SplitContainer1.TabIndex = 0
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.SplitContainer2.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer2.Name = "SplitContainer2"
        Me.SplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.lastTimeStamp)
        Me.SplitContainer2.Panel1.Controls.Add(Me.lblOdometer)
        Me.SplitContainer2.Panel1.Controls.Add(Me.Label10)
        Me.SplitContainer2.Panel1.Controls.Add(Me.lblCruiseSpeed)
        Me.SplitContainer2.Panel1.Controls.Add(Me.Label9)
        Me.SplitContainer2.Panel1.Controls.Add(Me.lblRegenStatus)
        Me.SplitContainer2.Panel1.Controls.Add(Me.Label8)
        Me.SplitContainer2.Panel1.Controls.Add(Me.lblVehicleSpeed)
        Me.SplitContainer2.Panel1.Controls.Add(Me.lblHighestBaterryTemperature)
        Me.SplitContainer2.Panel1.Controls.Add(Me.lblLowestBateryVoltage)
        Me.SplitContainer2.Panel1.Controls.Add(Me.lblMotorCurrent)
        Me.SplitContainer2.Panel1.Controls.Add(Me.lblBatteryCurrent)
        Me.SplitContainer2.Panel1.Controls.Add(Me.lblArrayCurrent)
        Me.SplitContainer2.Panel1.Controls.Add(Me.Label6)
        Me.SplitContainer2.Panel1.Controls.Add(Me.Label5)
        Me.SplitContainer2.Panel1.Controls.Add(Me.Label4)
        Me.SplitContainer2.Panel1.Controls.Add(Me.Label3)
        Me.SplitContainer2.Panel1.Controls.Add(Me.Label2)
        Me.SplitContainer2.Panel1.Controls.Add(Me.Label1)
        Me.SplitContainer2.Panel1.Controls.Add(Me.lblUpdateTimeStamp)
        Me.SplitContainer2.Panel1.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.gridTelemetry)
        Me.SplitContainer2.Size = New System.Drawing.Size(287, 566)
        Me.SplitContainer2.SplitterDistance = 272
        Me.SplitContainer2.TabIndex = 0
        '
        'lastTimeStamp
        '
        Me.lastTimeStamp.AllowDrop = True
        Me.lastTimeStamp.CustomFormat = "MM/dd/yyyy HH:mm:ss"
        Me.lastTimeStamp.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.lastTimeStamp.Location = New System.Drawing.Point(131, 6)
        Me.lastTimeStamp.Name = "lastTimeStamp"
        Me.lastTimeStamp.Size = New System.Drawing.Size(146, 21)
        Me.lastTimeStamp.TabIndex = 19
        '
        'lblOdometer
        '
        Me.lblOdometer.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOdometer.Location = New System.Drawing.Point(180, 209)
        Me.lblOdometer.Name = "lblOdometer"
        Me.lblOdometer.Size = New System.Drawing.Size(97, 17)
        Me.lblOdometer.TabIndex = 18
        Me.lblOdometer.Text = "Odometer"
        Me.lblOdometer.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(10, 210)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(62, 15)
        Me.Label10.TabIndex = 17
        Me.Label10.Text = "Odometer"
        '
        'lblCruiseSpeed
        '
        Me.lblCruiseSpeed.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCruiseSpeed.Location = New System.Drawing.Point(180, 165)
        Me.lblCruiseSpeed.Name = "lblCruiseSpeed"
        Me.lblCruiseSpeed.Size = New System.Drawing.Size(97, 17)
        Me.lblCruiseSpeed.TabIndex = 16
        Me.lblCruiseSpeed.Text = "Cruise speed"
        Me.lblCruiseSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(10, 166)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(84, 15)
        Me.Label9.TabIndex = 15
        Me.Label9.Text = "Cruise control"
        '
        'lblRegenStatus
        '
        Me.lblRegenStatus.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRegenStatus.Location = New System.Drawing.Point(180, 143)
        Me.lblRegenStatus.Name = "lblRegenStatus"
        Me.lblRegenStatus.Size = New System.Drawing.Size(97, 17)
        Me.lblRegenStatus.TabIndex = 14
        Me.lblRegenStatus.Text = "Regen status"
        Me.lblRegenStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(10, 144)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(44, 15)
        Me.Label8.TabIndex = 13
        Me.Label8.Text = "Regen"
        '
        'lblVehicleSpeed
        '
        Me.lblVehicleSpeed.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVehicleSpeed.Location = New System.Drawing.Point(180, 187)
        Me.lblVehicleSpeed.Name = "lblVehicleSpeed"
        Me.lblVehicleSpeed.Size = New System.Drawing.Size(97, 17)
        Me.lblVehicleSpeed.TabIndex = 12
        Me.lblVehicleSpeed.Text = "Vehicle speed"
        Me.lblVehicleSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblHighestBaterryTemperature
        '
        Me.lblHighestBaterryTemperature.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHighestBaterryTemperature.Location = New System.Drawing.Point(186, 121)
        Me.lblHighestBaterryTemperature.Name = "lblHighestBaterryTemperature"
        Me.lblHighestBaterryTemperature.Size = New System.Drawing.Size(91, 17)
        Me.lblHighestBaterryTemperature.TabIndex = 11
        Me.lblHighestBaterryTemperature.Text = "Highest temp"
        Me.lblHighestBaterryTemperature.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblLowestBateryVoltage
        '
        Me.lblLowestBateryVoltage.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLowestBateryVoltage.Location = New System.Drawing.Point(175, 99)
        Me.lblLowestBateryVoltage.Name = "lblLowestBateryVoltage"
        Me.lblLowestBateryVoltage.Size = New System.Drawing.Size(102, 17)
        Me.lblLowestBateryVoltage.TabIndex = 10
        Me.lblLowestBateryVoltage.Text = "Lowest voltage"
        Me.lblLowestBateryVoltage.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblMotorCurrent
        '
        Me.lblMotorCurrent.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMotorCurrent.Location = New System.Drawing.Point(184, 77)
        Me.lblMotorCurrent.Name = "lblMotorCurrent"
        Me.lblMotorCurrent.Size = New System.Drawing.Size(93, 17)
        Me.lblMotorCurrent.TabIndex = 9
        Me.lblMotorCurrent.Text = "Motor current"
        Me.lblMotorCurrent.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblBatteryCurrent
        '
        Me.lblBatteryCurrent.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBatteryCurrent.Location = New System.Drawing.Point(175, 55)
        Me.lblBatteryCurrent.Name = "lblBatteryCurrent"
        Me.lblBatteryCurrent.Size = New System.Drawing.Size(102, 17)
        Me.lblBatteryCurrent.TabIndex = 8
        Me.lblBatteryCurrent.Text = "Battery current"
        Me.lblBatteryCurrent.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblArrayCurrent
        '
        Me.lblArrayCurrent.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblArrayCurrent.Location = New System.Drawing.Point(186, 33)
        Me.lblArrayCurrent.Name = "lblArrayCurrent"
        Me.lblArrayCurrent.Size = New System.Drawing.Size(91, 17)
        Me.lblArrayCurrent.TabIndex = 7
        Me.lblArrayCurrent.Text = "Array current"
        Me.lblArrayCurrent.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(10, 188)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(85, 15)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "Vehicle speed"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(10, 122)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(160, 15)
        Me.Label5.TabIndex = 5
        Me.Label5.Text = "Highest Battery temperature"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(10, 100)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(129, 15)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Lowest Battery voltage"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(10, 78)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(78, 15)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Motor current"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(10, 56)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(85, 15)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Battery current"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(10, 34)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(75, 15)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Array current"
        '
        'lblUpdateTimeStamp
        '
        Me.lblUpdateTimeStamp.AutoSize = True
        Me.lblUpdateTimeStamp.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUpdateTimeStamp.Location = New System.Drawing.Point(4, 9)
        Me.lblUpdateTimeStamp.Name = "lblUpdateTimeStamp"
        Me.lblUpdateTimeStamp.Size = New System.Drawing.Size(72, 15)
        Me.lblUpdateTimeStamp.TabIndex = 0
        Me.lblUpdateTimeStamp.Text = "Last update"
        '
        'gridTelemetry
        '
        Me.gridTelemetry.AllowUserToAddRows = False
        Me.gridTelemetry.AllowUserToDeleteRows = False
        Me.gridTelemetry.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.gridTelemetry.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None
        Me.gridTelemetry.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        Me.gridTelemetry.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridTelemetry.DefaultCellStyle = DataGridViewCellStyle1
        Me.gridTelemetry.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridTelemetry.Location = New System.Drawing.Point(0, 0)
        Me.gridTelemetry.Name = "gridTelemetry"
        Me.gridTelemetry.Size = New System.Drawing.Size(287, 290)
        Me.gridTelemetry.TabIndex = 0
        '
        'tabCharts
        '
        Me.tabCharts.Controls.Add(Me.tabSummary)
        Me.tabCharts.Controls.Add(Me.tabBatteryVoltages)
        Me.tabCharts.Controls.Add(Me.tabTemperatures)
        Me.tabCharts.Controls.Add(Me.tabUserSpecified)
        Me.tabCharts.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabCharts.Location = New System.Drawing.Point(0, 0)
        Me.tabCharts.Name = "tabCharts"
        Me.tabCharts.SelectedIndex = 0
        Me.tabCharts.Size = New System.Drawing.Size(663, 566)
        Me.tabCharts.TabIndex = 3
        '
        'tabSummary
        '
        Me.tabSummary.Controls.Add(Me.chartMain)
        Me.tabSummary.Location = New System.Drawing.Point(4, 22)
        Me.tabSummary.Name = "tabSummary"
        Me.tabSummary.Padding = New System.Windows.Forms.Padding(3)
        Me.tabSummary.Size = New System.Drawing.Size(655, 540)
        Me.tabSummary.TabIndex = 3
        Me.tabSummary.Text = "Summary"
        Me.tabSummary.UseVisualStyleBackColor = True
        '
        'chartMain
        '
        ChartArea1.Name = "ChartArea1"
        Me.chartMain.ChartAreas.Add(ChartArea1)
        Me.chartMain.Dock = System.Windows.Forms.DockStyle.Fill
        Legend1.Alignment = System.Drawing.StringAlignment.Center
        Legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom
        Legend1.LegendStyle = System.Windows.Forms.DataVisualization.Charting.LegendStyle.Row
        Legend1.Name = "Legend1"
        Me.chartMain.Legends.Add(Legend1)
        Me.chartMain.Location = New System.Drawing.Point(3, 3)
        Me.chartMain.Name = "chartMain"
        Series1.ChartArea = "ChartArea1"
        Series1.Legend = "Legend1"
        Series1.Name = "Series1"
        Me.chartMain.Series.Add(Series1)
        Me.chartMain.Size = New System.Drawing.Size(649, 534)
        Me.chartMain.TabIndex = 2
        Me.chartMain.Text = "chartMain"
        '
        'tabBatteryVoltages
        '
        Me.tabBatteryVoltages.Controls.Add(Me.chartBatteryStatus)
        Me.tabBatteryVoltages.Location = New System.Drawing.Point(4, 22)
        Me.tabBatteryVoltages.Name = "tabBatteryVoltages"
        Me.tabBatteryVoltages.Padding = New System.Windows.Forms.Padding(3)
        Me.tabBatteryVoltages.Size = New System.Drawing.Size(655, 540)
        Me.tabBatteryVoltages.TabIndex = 1
        Me.tabBatteryVoltages.Text = "Battery Voltages"
        Me.tabBatteryVoltages.UseVisualStyleBackColor = True
        '
        'chartBatteryStatus
        '
        ChartArea2.Name = "ChartArea1"
        Me.chartBatteryStatus.ChartAreas.Add(ChartArea2)
        Me.chartBatteryStatus.Dock = System.Windows.Forms.DockStyle.Fill
        Legend2.Alignment = System.Drawing.StringAlignment.Center
        Legend2.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom
        Legend2.Enabled = False
        Legend2.LegendStyle = System.Windows.Forms.DataVisualization.Charting.LegendStyle.Row
        Legend2.Name = "Legend1"
        Me.chartBatteryStatus.Legends.Add(Legend2)
        Me.chartBatteryStatus.Location = New System.Drawing.Point(3, 3)
        Me.chartBatteryStatus.Name = "chartBatteryStatus"
        Series2.ChartArea = "ChartArea1"
        Series2.Legend = "Legend1"
        Series2.Name = "Series1"
        Me.chartBatteryStatus.Series.Add(Series2)
        Me.chartBatteryStatus.Size = New System.Drawing.Size(649, 534)
        Me.chartBatteryStatus.TabIndex = 1
        Me.chartBatteryStatus.Text = "chartBatteryStatus"
        '
        'tabTemperatures
        '
        Me.tabTemperatures.Controls.Add(Me.GroupBox1)
        Me.tabTemperatures.Controls.Add(Me.Label11)
        Me.tabTemperatures.Controls.Add(Me.Label7)
        Me.tabTemperatures.Controls.Add(Me.lblTemperature32)
        Me.tabTemperatures.Controls.Add(Me.lblTemperature31)
        Me.tabTemperatures.Controls.Add(Me.lblTemperature30)
        Me.tabTemperatures.Controls.Add(Me.lblTemperature29)
        Me.tabTemperatures.Controls.Add(Me.lblTemperature28)
        Me.tabTemperatures.Controls.Add(Me.lblTemperature27)
        Me.tabTemperatures.Controls.Add(Me.lblTemperature26)
        Me.tabTemperatures.Controls.Add(Me.lblTemperature25)
        Me.tabTemperatures.Controls.Add(Me.lblTemperature24)
        Me.tabTemperatures.Controls.Add(Me.lblTemperature23)
        Me.tabTemperatures.Controls.Add(Me.lblTemperature22)
        Me.tabTemperatures.Controls.Add(Me.lblTemperature21)
        Me.tabTemperatures.Controls.Add(Me.lblTemperature20)
        Me.tabTemperatures.Controls.Add(Me.lblTemperature19)
        Me.tabTemperatures.Controls.Add(Me.lblTemperature18)
        Me.tabTemperatures.Controls.Add(Me.lblTemperature17)
        Me.tabTemperatures.Controls.Add(Me.lblTemperature16)
        Me.tabTemperatures.Controls.Add(Me.lblTemperature15)
        Me.tabTemperatures.Controls.Add(Me.lblTemperature14)
        Me.tabTemperatures.Controls.Add(Me.lblTemperature13)
        Me.tabTemperatures.Controls.Add(Me.lblTemperature12)
        Me.tabTemperatures.Controls.Add(Me.lblTemperature11)
        Me.tabTemperatures.Controls.Add(Me.lblTemperature10)
        Me.tabTemperatures.Controls.Add(Me.lblTemperature9)
        Me.tabTemperatures.Controls.Add(Me.lblTemperature8)
        Me.tabTemperatures.Controls.Add(Me.lblTemperature7)
        Me.tabTemperatures.Controls.Add(Me.lblTemperature6)
        Me.tabTemperatures.Controls.Add(Me.lblTemperature5)
        Me.tabTemperatures.Controls.Add(Me.lblTemperature4)
        Me.tabTemperatures.Controls.Add(Me.lblTemperature3)
        Me.tabTemperatures.Controls.Add(Me.lblTemperature2)
        Me.tabTemperatures.Controls.Add(Me.lblTemperature1)
        Me.tabTemperatures.Controls.Add(Me.PictureBox1)
        Me.tabTemperatures.Location = New System.Drawing.Point(4, 22)
        Me.tabTemperatures.Name = "tabTemperatures"
        Me.tabTemperatures.Padding = New System.Windows.Forms.Padding(3)
        Me.tabTemperatures.Size = New System.Drawing.Size(655, 540)
        Me.tabTemperatures.TabIndex = 4
        Me.tabTemperatures.Text = "Temperatures"
        Me.tabTemperatures.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.GroupBox2)
        Me.GroupBox1.Controls.Add(Me.lblTempMotor)
        Me.GroupBox1.Controls.Add(Me.Label12)
        Me.GroupBox1.Location = New System.Drawing.Point(5, 436)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(642, 101)
        Me.GroupBox1.TabIndex = 54
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Other components"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.lblTempHeatsink)
        Me.GroupBox2.Controls.Add(Me.Label17)
        Me.GroupBox2.Controls.Add(Me.lblTempCapacitor)
        Me.GroupBox2.Controls.Add(Me.Label16)
        Me.GroupBox2.Controls.Add(Me.lblTempProcessor)
        Me.GroupBox2.Controls.Add(Me.Label15)
        Me.GroupBox2.Controls.Add(Me.lblTempAirOutlet)
        Me.GroupBox2.Controls.Add(Me.Label14)
        Me.GroupBox2.Controls.Add(Me.lblTempAirInlet)
        Me.GroupBox2.Controls.Add(Me.Label13)
        Me.GroupBox2.Location = New System.Drawing.Point(164, 18)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(472, 77)
        Me.GroupBox2.TabIndex = 52
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Motor Controller"
        '
        'lblTempHeatsink
        '
        Me.lblTempHeatsink.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTempHeatsink.Location = New System.Drawing.Point(351, 18)
        Me.lblTempHeatsink.Name = "lblTempHeatsink"
        Me.lblTempHeatsink.Size = New System.Drawing.Size(40, 17)
        Me.lblTempHeatsink.TabIndex = 58
        Me.lblTempHeatsink.Tag = "temperature"
        Me.lblTempHeatsink.Text = "TempHeatsink"
        Me.lblTempHeatsink.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(286, 18)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(59, 15)
        Me.Label17.TabIndex = 57
        Me.Label17.Text = "Heat sink"
        '
        'lblTempCapacitor
        '
        Me.lblTempCapacitor.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTempCapacitor.Location = New System.Drawing.Point(222, 44)
        Me.lblTempCapacitor.Name = "lblTempCapacitor"
        Me.lblTempCapacitor.Size = New System.Drawing.Size(40, 17)
        Me.lblTempCapacitor.TabIndex = 56
        Me.lblTempCapacitor.Tag = "temperature"
        Me.lblTempCapacitor.Text = "TempCapacitor"
        Me.lblTempCapacitor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(152, 45)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(60, 15)
        Me.Label16.TabIndex = 55
        Me.Label16.Text = "Capacitor"
        '
        'lblTempProcessor
        '
        Me.lblTempProcessor.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTempProcessor.Location = New System.Drawing.Point(222, 18)
        Me.lblTempProcessor.Name = "lblTempProcessor"
        Me.lblTempProcessor.Size = New System.Drawing.Size(40, 17)
        Me.lblTempProcessor.TabIndex = 54
        Me.lblTempProcessor.Tag = "temperature"
        Me.lblTempProcessor.Text = "TempProcessor"
        Me.lblTempProcessor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(152, 19)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(64, 15)
        Me.Label15.TabIndex = 53
        Me.Label15.Text = "Processor"
        '
        'lblTempAirOutlet
        '
        Me.lblTempAirOutlet.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTempAirOutlet.Location = New System.Drawing.Point(78, 43)
        Me.lblTempAirOutlet.Name = "lblTempAirOutlet"
        Me.lblTempAirOutlet.Size = New System.Drawing.Size(40, 17)
        Me.lblTempAirOutlet.TabIndex = 52
        Me.lblTempAirOutlet.Tag = "temperature"
        Me.lblTempAirOutlet.Text = "TempAirOutlet"
        Me.lblTempAirOutlet.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(18, 44)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(54, 15)
        Me.Label14.TabIndex = 51
        Me.Label14.Text = "Air outlet"
        '
        'lblTempAirInlet
        '
        Me.lblTempAirInlet.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTempAirInlet.Location = New System.Drawing.Point(78, 17)
        Me.lblTempAirInlet.Name = "lblTempAirInlet"
        Me.lblTempAirInlet.Size = New System.Drawing.Size(40, 17)
        Me.lblTempAirInlet.TabIndex = 50
        Me.lblTempAirInlet.Tag = "temperature"
        Me.lblTempAirInlet.Text = "TempAirInlet"
        Me.lblTempAirInlet.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(18, 18)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(47, 15)
        Me.Label13.TabIndex = 0
        Me.Label13.Text = "Air inlet"
        '
        'lblTempMotor
        '
        Me.lblTempMotor.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTempMotor.Location = New System.Drawing.Point(65, 20)
        Me.lblTempMotor.Name = "lblTempMotor"
        Me.lblTempMotor.Size = New System.Drawing.Size(91, 17)
        Me.lblTempMotor.TabIndex = 51
        Me.lblTempMotor.Tag = "temperature"
        Me.lblTempMotor.Text = "TempMotor"
        Me.lblTempMotor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(22, 21)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(37, 15)
        Me.Label12.TabIndex = 0
        Me.Label12.Text = "Motor"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(297, 405)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(58, 13)
        Me.Label11.TabIndex = 53
        Me.Label11.Text = "front of car"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(6, 13)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(40, 13)
        Me.Label7.TabIndex = 52
        Me.Label7.Text = "Battery"
        '
        'lblTemperature32
        '
        Me.lblTemperature32.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblTemperature32.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTemperature32.Location = New System.Drawing.Point(585, 67)
        Me.lblTemperature32.Name = "lblTemperature32"
        Me.lblTemperature32.Size = New System.Drawing.Size(40, 17)
        Me.lblTemperature32.TabIndex = 43
        Me.lblTemperature32.Tag = "temperature"
        Me.lblTemperature32.Text = "T32"
        Me.lblTemperature32.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTemperature31
        '
        Me.lblTemperature31.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblTemperature31.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTemperature31.Location = New System.Drawing.Point(506, 67)
        Me.lblTemperature31.Name = "lblTemperature31"
        Me.lblTemperature31.Size = New System.Drawing.Size(40, 17)
        Me.lblTemperature31.TabIndex = 42
        Me.lblTemperature31.Tag = "temperature"
        Me.lblTemperature31.Text = "T31"
        Me.lblTemperature31.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTemperature30
        '
        Me.lblTemperature30.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblTemperature30.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTemperature30.Location = New System.Drawing.Point(426, 67)
        Me.lblTemperature30.Name = "lblTemperature30"
        Me.lblTemperature30.Size = New System.Drawing.Size(40, 17)
        Me.lblTemperature30.TabIndex = 41
        Me.lblTemperature30.Tag = "temperature"
        Me.lblTemperature30.Text = "T30"
        Me.lblTemperature30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTemperature29
        '
        Me.lblTemperature29.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblTemperature29.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTemperature29.Location = New System.Drawing.Point(345, 67)
        Me.lblTemperature29.Name = "lblTemperature29"
        Me.lblTemperature29.Size = New System.Drawing.Size(40, 17)
        Me.lblTemperature29.TabIndex = 40
        Me.lblTemperature29.Tag = "temperature"
        Me.lblTemperature29.Text = "T29"
        Me.lblTemperature29.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTemperature28
        '
        Me.lblTemperature28.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblTemperature28.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTemperature28.Location = New System.Drawing.Point(267, 67)
        Me.lblTemperature28.Name = "lblTemperature28"
        Me.lblTemperature28.Size = New System.Drawing.Size(40, 17)
        Me.lblTemperature28.TabIndex = 39
        Me.lblTemperature28.Tag = "temperature"
        Me.lblTemperature28.Text = "T28"
        Me.lblTemperature28.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTemperature27
        '
        Me.lblTemperature27.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblTemperature27.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTemperature27.Location = New System.Drawing.Point(187, 67)
        Me.lblTemperature27.Name = "lblTemperature27"
        Me.lblTemperature27.Size = New System.Drawing.Size(40, 17)
        Me.lblTemperature27.TabIndex = 38
        Me.lblTemperature27.Tag = "temperature"
        Me.lblTemperature27.Text = "T27"
        Me.lblTemperature27.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTemperature26
        '
        Me.lblTemperature26.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblTemperature26.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTemperature26.Location = New System.Drawing.Point(107, 67)
        Me.lblTemperature26.Name = "lblTemperature26"
        Me.lblTemperature26.Size = New System.Drawing.Size(40, 17)
        Me.lblTemperature26.TabIndex = 37
        Me.lblTemperature26.Tag = "temperature"
        Me.lblTemperature26.Text = "T26"
        Me.lblTemperature26.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTemperature25
        '
        Me.lblTemperature25.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblTemperature25.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTemperature25.Location = New System.Drawing.Point(27, 67)
        Me.lblTemperature25.Name = "lblTemperature25"
        Me.lblTemperature25.Size = New System.Drawing.Size(40, 17)
        Me.lblTemperature25.TabIndex = 36
        Me.lblTemperature25.Tag = "temperature"
        Me.lblTemperature25.Text = "T25"
        Me.lblTemperature25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTemperature24
        '
        Me.lblTemperature24.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblTemperature24.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTemperature24.Location = New System.Drawing.Point(585, 164)
        Me.lblTemperature24.Name = "lblTemperature24"
        Me.lblTemperature24.Size = New System.Drawing.Size(40, 17)
        Me.lblTemperature24.TabIndex = 35
        Me.lblTemperature24.Tag = "temperature"
        Me.lblTemperature24.Text = "T24"
        Me.lblTemperature24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTemperature23
        '
        Me.lblTemperature23.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblTemperature23.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTemperature23.Location = New System.Drawing.Point(506, 164)
        Me.lblTemperature23.Name = "lblTemperature23"
        Me.lblTemperature23.Size = New System.Drawing.Size(40, 17)
        Me.lblTemperature23.TabIndex = 34
        Me.lblTemperature23.Tag = "temperature"
        Me.lblTemperature23.Text = "T23"
        Me.lblTemperature23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTemperature22
        '
        Me.lblTemperature22.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblTemperature22.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTemperature22.Location = New System.Drawing.Point(426, 164)
        Me.lblTemperature22.Name = "lblTemperature22"
        Me.lblTemperature22.Size = New System.Drawing.Size(40, 17)
        Me.lblTemperature22.TabIndex = 33
        Me.lblTemperature22.Tag = "temperature"
        Me.lblTemperature22.Text = "T22"
        Me.lblTemperature22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTemperature21
        '
        Me.lblTemperature21.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblTemperature21.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTemperature21.Location = New System.Drawing.Point(345, 165)
        Me.lblTemperature21.Name = "lblTemperature21"
        Me.lblTemperature21.Size = New System.Drawing.Size(40, 17)
        Me.lblTemperature21.TabIndex = 32
        Me.lblTemperature21.Tag = "temperature"
        Me.lblTemperature21.Text = "T21"
        Me.lblTemperature21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTemperature20
        '
        Me.lblTemperature20.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblTemperature20.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTemperature20.Location = New System.Drawing.Point(267, 164)
        Me.lblTemperature20.Name = "lblTemperature20"
        Me.lblTemperature20.Size = New System.Drawing.Size(40, 17)
        Me.lblTemperature20.TabIndex = 31
        Me.lblTemperature20.Tag = "temperature"
        Me.lblTemperature20.Text = "T20"
        Me.lblTemperature20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTemperature19
        '
        Me.lblTemperature19.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblTemperature19.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTemperature19.Location = New System.Drawing.Point(187, 165)
        Me.lblTemperature19.Name = "lblTemperature19"
        Me.lblTemperature19.Size = New System.Drawing.Size(40, 17)
        Me.lblTemperature19.TabIndex = 30
        Me.lblTemperature19.Tag = "temperature"
        Me.lblTemperature19.Text = "T19"
        Me.lblTemperature19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTemperature18
        '
        Me.lblTemperature18.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblTemperature18.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTemperature18.Location = New System.Drawing.Point(107, 164)
        Me.lblTemperature18.Name = "lblTemperature18"
        Me.lblTemperature18.Size = New System.Drawing.Size(40, 17)
        Me.lblTemperature18.TabIndex = 29
        Me.lblTemperature18.Tag = "temperature"
        Me.lblTemperature18.Text = "T18"
        Me.lblTemperature18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTemperature17
        '
        Me.lblTemperature17.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblTemperature17.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTemperature17.Location = New System.Drawing.Point(27, 164)
        Me.lblTemperature17.Name = "lblTemperature17"
        Me.lblTemperature17.Size = New System.Drawing.Size(40, 17)
        Me.lblTemperature17.TabIndex = 28
        Me.lblTemperature17.Tag = "temperature"
        Me.lblTemperature17.Text = "T17"
        Me.lblTemperature17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTemperature16
        '
        Me.lblTemperature16.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblTemperature16.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTemperature16.Location = New System.Drawing.Point(585, 252)
        Me.lblTemperature16.Name = "lblTemperature16"
        Me.lblTemperature16.Size = New System.Drawing.Size(40, 17)
        Me.lblTemperature16.TabIndex = 27
        Me.lblTemperature16.Tag = "temperature"
        Me.lblTemperature16.Text = "T16"
        Me.lblTemperature16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTemperature15
        '
        Me.lblTemperature15.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblTemperature15.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTemperature15.Location = New System.Drawing.Point(506, 252)
        Me.lblTemperature15.Name = "lblTemperature15"
        Me.lblTemperature15.Size = New System.Drawing.Size(40, 17)
        Me.lblTemperature15.TabIndex = 26
        Me.lblTemperature15.Tag = "temperature"
        Me.lblTemperature15.Text = "T15"
        Me.lblTemperature15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTemperature14
        '
        Me.lblTemperature14.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblTemperature14.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTemperature14.Location = New System.Drawing.Point(426, 252)
        Me.lblTemperature14.Name = "lblTemperature14"
        Me.lblTemperature14.Size = New System.Drawing.Size(40, 17)
        Me.lblTemperature14.TabIndex = 25
        Me.lblTemperature14.Tag = "temperature"
        Me.lblTemperature14.Text = "T14"
        Me.lblTemperature14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTemperature13
        '
        Me.lblTemperature13.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblTemperature13.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTemperature13.Location = New System.Drawing.Point(345, 252)
        Me.lblTemperature13.Name = "lblTemperature13"
        Me.lblTemperature13.Size = New System.Drawing.Size(40, 17)
        Me.lblTemperature13.TabIndex = 24
        Me.lblTemperature13.Tag = "temperature"
        Me.lblTemperature13.Text = "T13"
        Me.lblTemperature13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTemperature12
        '
        Me.lblTemperature12.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblTemperature12.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTemperature12.Location = New System.Drawing.Point(267, 252)
        Me.lblTemperature12.Name = "lblTemperature12"
        Me.lblTemperature12.Size = New System.Drawing.Size(40, 17)
        Me.lblTemperature12.TabIndex = 23
        Me.lblTemperature12.Tag = "temperature"
        Me.lblTemperature12.Text = "T12"
        Me.lblTemperature12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTemperature11
        '
        Me.lblTemperature11.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblTemperature11.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTemperature11.Location = New System.Drawing.Point(187, 252)
        Me.lblTemperature11.Name = "lblTemperature11"
        Me.lblTemperature11.Size = New System.Drawing.Size(40, 17)
        Me.lblTemperature11.TabIndex = 22
        Me.lblTemperature11.Tag = "temperature"
        Me.lblTemperature11.Text = "T11"
        Me.lblTemperature11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTemperature10
        '
        Me.lblTemperature10.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblTemperature10.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTemperature10.Location = New System.Drawing.Point(107, 252)
        Me.lblTemperature10.Name = "lblTemperature10"
        Me.lblTemperature10.Size = New System.Drawing.Size(40, 17)
        Me.lblTemperature10.TabIndex = 21
        Me.lblTemperature10.Tag = "temperature"
        Me.lblTemperature10.Text = "T10"
        Me.lblTemperature10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTemperature9
        '
        Me.lblTemperature9.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblTemperature9.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTemperature9.Location = New System.Drawing.Point(27, 252)
        Me.lblTemperature9.Name = "lblTemperature9"
        Me.lblTemperature9.Size = New System.Drawing.Size(40, 17)
        Me.lblTemperature9.TabIndex = 20
        Me.lblTemperature9.Tag = "temperature"
        Me.lblTemperature9.Text = "T9"
        Me.lblTemperature9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTemperature8
        '
        Me.lblTemperature8.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblTemperature8.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTemperature8.Location = New System.Drawing.Point(585, 340)
        Me.lblTemperature8.Name = "lblTemperature8"
        Me.lblTemperature8.Size = New System.Drawing.Size(40, 17)
        Me.lblTemperature8.TabIndex = 19
        Me.lblTemperature8.Tag = "temperature"
        Me.lblTemperature8.Text = "T8"
        Me.lblTemperature8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTemperature7
        '
        Me.lblTemperature7.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblTemperature7.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTemperature7.Location = New System.Drawing.Point(506, 340)
        Me.lblTemperature7.Name = "lblTemperature7"
        Me.lblTemperature7.Size = New System.Drawing.Size(40, 17)
        Me.lblTemperature7.TabIndex = 18
        Me.lblTemperature7.Tag = "temperature"
        Me.lblTemperature7.Text = "T7"
        Me.lblTemperature7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTemperature6
        '
        Me.lblTemperature6.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblTemperature6.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTemperature6.Location = New System.Drawing.Point(426, 340)
        Me.lblTemperature6.Name = "lblTemperature6"
        Me.lblTemperature6.Size = New System.Drawing.Size(40, 17)
        Me.lblTemperature6.TabIndex = 17
        Me.lblTemperature6.Tag = "temperature"
        Me.lblTemperature6.Text = "T6"
        Me.lblTemperature6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTemperature5
        '
        Me.lblTemperature5.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblTemperature5.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTemperature5.Location = New System.Drawing.Point(345, 340)
        Me.lblTemperature5.Name = "lblTemperature5"
        Me.lblTemperature5.Size = New System.Drawing.Size(40, 17)
        Me.lblTemperature5.TabIndex = 16
        Me.lblTemperature5.Tag = "temperature"
        Me.lblTemperature5.Text = "T5"
        Me.lblTemperature5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTemperature4
        '
        Me.lblTemperature4.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblTemperature4.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTemperature4.Location = New System.Drawing.Point(267, 340)
        Me.lblTemperature4.Name = "lblTemperature4"
        Me.lblTemperature4.Size = New System.Drawing.Size(40, 17)
        Me.lblTemperature4.TabIndex = 15
        Me.lblTemperature4.Tag = "temperature"
        Me.lblTemperature4.Text = "T4"
        Me.lblTemperature4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTemperature3
        '
        Me.lblTemperature3.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblTemperature3.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTemperature3.Location = New System.Drawing.Point(187, 340)
        Me.lblTemperature3.Name = "lblTemperature3"
        Me.lblTemperature3.Size = New System.Drawing.Size(40, 17)
        Me.lblTemperature3.TabIndex = 14
        Me.lblTemperature3.Tag = "temperature"
        Me.lblTemperature3.Text = "T3"
        Me.lblTemperature3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTemperature2
        '
        Me.lblTemperature2.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblTemperature2.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTemperature2.Location = New System.Drawing.Point(107, 340)
        Me.lblTemperature2.Name = "lblTemperature2"
        Me.lblTemperature2.Size = New System.Drawing.Size(40, 17)
        Me.lblTemperature2.TabIndex = 13
        Me.lblTemperature2.Tag = "temperature"
        Me.lblTemperature2.Text = "T2"
        Me.lblTemperature2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTemperature1
        '
        Me.lblTemperature1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblTemperature1.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTemperature1.Location = New System.Drawing.Point(27, 340)
        Me.lblTemperature1.Name = "lblTemperature1"
        Me.lblTemperature1.Size = New System.Drawing.Size(40, 17)
        Me.lblTemperature1.TabIndex = 12
        Me.lblTemperature1.Tag = "temperature"
        Me.lblTemperature1.Text = "T1"
        Me.lblTemperature1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PictureBox1
        '
        Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(6, 30)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(641, 372)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 51
        Me.PictureBox1.TabStop = False
        '
        'tabUserSpecified
        '
        Me.tabUserSpecified.Controls.Add(Me.chartUserSpecified)
        Me.tabUserSpecified.Location = New System.Drawing.Point(4, 22)
        Me.tabUserSpecified.Name = "tabUserSpecified"
        Me.tabUserSpecified.Padding = New System.Windows.Forms.Padding(3)
        Me.tabUserSpecified.Size = New System.Drawing.Size(655, 540)
        Me.tabUserSpecified.TabIndex = 2
        Me.tabUserSpecified.Text = "User Specified"
        Me.tabUserSpecified.UseVisualStyleBackColor = True
        '
        'chartUserSpecified
        '
        ChartArea3.Name = "ChartArea1"
        Me.chartUserSpecified.ChartAreas.Add(ChartArea3)
        Me.chartUserSpecified.Dock = System.Windows.Forms.DockStyle.Fill
        Legend3.Alignment = System.Drawing.StringAlignment.Center
        Legend3.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom
        Legend3.LegendStyle = System.Windows.Forms.DataVisualization.Charting.LegendStyle.Row
        Legend3.Name = "Legend1"
        Me.chartUserSpecified.Legends.Add(Legend3)
        Me.chartUserSpecified.Location = New System.Drawing.Point(3, 3)
        Me.chartUserSpecified.Name = "chartUserSpecified"
        Series3.ChartArea = "ChartArea1"
        Series3.Legend = "Legend1"
        Series3.Name = "Series1"
        Me.chartUserSpecified.Series.Add(Series3)
        Me.chartUserSpecified.Size = New System.Drawing.Size(649, 534)
        Me.chartUserSpecified.TabIndex = 1
        Me.chartUserSpecified.Text = "chartUserSpecified"
        '
        'tb
        '
        Me.tb.Dock = System.Windows.Forms.DockStyle.None
        Me.tb.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.tb.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ID_Exit, Me.ToolStripSeparator1, Me.ToolStripLabel1, Me.cboDisplayMode, Me.ToolStripLabel2, Me.cboDataInterval, Me.ID_RetrieveData, Me.ToolStripSeparator2, Me.btnInclude, Me.tbZoomable})
        Me.tb.Location = New System.Drawing.Point(0, 0)
        Me.tb.Name = "tb"
        Me.tb.Size = New System.Drawing.Size(954, 25)
        Me.tb.Stretch = True
        Me.tb.TabIndex = 1
        Me.tb.Text = "ToolStrip1"
        '
        'ID_Exit
        '
        Me.ID_Exit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ID_Exit.Image = CType(resources.GetObject("ID_Exit.Image"), System.Drawing.Image)
        Me.ID_Exit.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ID_Exit.Name = "ID_Exit"
        Me.ID_Exit.Size = New System.Drawing.Size(23, 22)
        Me.ID_Exit.Text = "Exit"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(70, 22)
        Me.ToolStripLabel1.Text = "Display mode"
        '
        'cboDisplayMode
        '
        Me.cboDisplayMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboDisplayMode.Items.AddRange(New Object() {"Real time", "Historic"})
        Me.cboDisplayMode.Name = "cboDisplayMode"
        Me.cboDisplayMode.Size = New System.Drawing.Size(121, 25)
        '
        'ToolStripLabel2
        '
        Me.ToolStripLabel2.Name = "ToolStripLabel2"
        Me.ToolStripLabel2.Size = New System.Drawing.Size(69, 22)
        Me.ToolStripLabel2.Text = "Data interval"
        '
        'cboDataInterval
        '
        Me.cboDataInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboDataInterval.Enabled = False
        Me.cboDataInterval.Items.AddRange(New Object() {"1 Second", "5 Seconds", "15 Seconds", "30 Seconds", "1 Minute", "5 Minutes", "10 Minutes"})
        Me.cboDataInterval.Name = "cboDataInterval"
        Me.cboDataInterval.Size = New System.Drawing.Size(121, 25)
        '
        'ID_RetrieveData
        '
        Me.ID_RetrieveData.Image = CType(resources.GetObject("ID_RetrieveData.Image"), System.Drawing.Image)
        Me.ID_RetrieveData.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ID_RetrieveData.Name = "ID_RetrieveData"
        Me.ID_RetrieveData.Size = New System.Drawing.Size(93, 22)
        Me.ID_RetrieveData.Text = "Retrieve data"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'btnInclude
        '
        Me.btnInclude.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.btnInclude.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ID_Current, Me.ID_Speed, Me.ID_Battery})
        Me.btnInclude.Image = CType(resources.GetObject("btnInclude.Image"), System.Drawing.Image)
        Me.btnInclude.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnInclude.Name = "btnInclude"
        Me.btnInclude.Size = New System.Drawing.Size(116, 22)
        Me.btnInclude.Text = "Include on summary"
        '
        'ID_Current
        '
        Me.ID_Current.Checked = True
        Me.ID_Current.CheckOnClick = True
        Me.ID_Current.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ID_Current.Name = "ID_Current"
        Me.ID_Current.Size = New System.Drawing.Size(127, 22)
        Me.ID_Current.Text = "Currents"
        '
        'ID_Speed
        '
        Me.ID_Speed.Checked = True
        Me.ID_Speed.CheckOnClick = True
        Me.ID_Speed.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ID_Speed.Name = "ID_Speed"
        Me.ID_Speed.Size = New System.Drawing.Size(127, 22)
        Me.ID_Speed.Text = "Speed"
        '
        'ID_Battery
        '
        Me.ID_Battery.CheckOnClick = True
        Me.ID_Battery.Name = "ID_Battery"
        Me.ID_Battery.Size = New System.Drawing.Size(127, 22)
        Me.ID_Battery.Text = "Battery"
        '
        'tbZoomable
        '
        Me.tbZoomable.CheckOnClick = True
        Me.tbZoomable.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tbZoomable.Image = CType(resources.GetObject("tbZoomable.Image"), System.Drawing.Image)
        Me.tbZoomable.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tbZoomable.Name = "tbZoomable"
        Me.tbZoomable.Size = New System.Drawing.Size(57, 22)
        Me.tbZoomable.Text = "Zoomable"
        '
        'ToolStripContainer1
        '
        Me.ToolStripContainer1.BottomToolStripPanelVisible = False
        '
        'ToolStripContainer1.ContentPanel
        '
        Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.SplitContainer1)
        Me.ToolStripContainer1.ContentPanel.Size = New System.Drawing.Size(954, 566)
        Me.ToolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ToolStripContainer1.LeftToolStripPanelVisible = False
        Me.ToolStripContainer1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStripContainer1.Name = "ToolStripContainer1"
        Me.ToolStripContainer1.RightToolStripPanelVisible = False
        Me.ToolStripContainer1.Size = New System.Drawing.Size(954, 591)
        Me.ToolStripContainer1.TabIndex = 2
        Me.ToolStripContainer1.Text = "ToolStripContainer1"
        '
        'ToolStripContainer1.TopToolStripPanel
        '
        Me.ToolStripContainer1.TopToolStripPanel.Controls.Add(Me.tb)
        '
        'statusbar
        '
        Me.statusbar.Dock = System.Windows.Forms.DockStyle.None
        Me.statusbar.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1, Me.ssConnectedStatus})
        Me.statusbar.Location = New System.Drawing.Point(0, 0)
        Me.statusbar.Name = "statusbar"
        Me.statusbar.Size = New System.Drawing.Size(954, 22)
        Me.statusbar.TabIndex = 3
        Me.statusbar.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.None
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(839, 17)
        Me.ToolStripStatusLabel1.Spring = True
        '
        'ssConnectedStatus
        '
        Me.ssConnectedStatus.Name = "ssConnectedStatus"
        Me.ssConnectedStatus.Size = New System.Drawing.Size(100, 17)
        Me.ssConnectedStatus.Text = "ssConnectedStatus"
        Me.ssConnectedStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ToolStripContainer2
        '
        '
        'ToolStripContainer2.BottomToolStripPanel
        '
        Me.ToolStripContainer2.BottomToolStripPanel.Controls.Add(Me.statusbar)
        '
        'ToolStripContainer2.ContentPanel
        '
        Me.ToolStripContainer2.ContentPanel.AutoScroll = True
        Me.ToolStripContainer2.ContentPanel.Controls.Add(Me.ToolStripContainer1)
        Me.ToolStripContainer2.ContentPanel.Size = New System.Drawing.Size(954, 591)
        Me.ToolStripContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ToolStripContainer2.LeftToolStripPanelVisible = False
        Me.ToolStripContainer2.Location = New System.Drawing.Point(0, 0)
        Me.ToolStripContainer2.Name = "ToolStripContainer2"
        Me.ToolStripContainer2.RightToolStripPanelVisible = False
        Me.ToolStripContainer2.Size = New System.Drawing.Size(954, 613)
        Me.ToolStripContainer2.TabIndex = 4
        Me.ToolStripContainer2.Text = "ToolStripContainer2"
        Me.ToolStripContainer2.TopToolStripPanelVisible = False
        '
        'Main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(954, 613)
        Me.Controls.Add(Me.ToolStripContainer2)
        Me.Name = "Main"
        Me.Text = "NUSolar Telemetry"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel1.PerformLayout()
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer2.ResumeLayout(False)
        CType(Me.gridTelemetry, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabCharts.ResumeLayout(False)
        Me.tabSummary.ResumeLayout(False)
        CType(Me.chartMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabBatteryVoltages.ResumeLayout(False)
        CType(Me.chartBatteryStatus, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabTemperatures.ResumeLayout(False)
        Me.tabTemperatures.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabUserSpecified.ResumeLayout(False)
        CType(Me.chartUserSpecified, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tb.ResumeLayout(False)
        Me.tb.PerformLayout()
        Me.ToolStripContainer1.ContentPanel.ResumeLayout(False)
        Me.ToolStripContainer1.TopToolStripPanel.ResumeLayout(False)
        Me.ToolStripContainer1.TopToolStripPanel.PerformLayout()
        Me.ToolStripContainer1.ResumeLayout(False)
        Me.ToolStripContainer1.PerformLayout()
        Me.statusbar.ResumeLayout(False)
        Me.statusbar.PerformLayout()
        Me.ToolStripContainer2.BottomToolStripPanel.ResumeLayout(False)
        Me.ToolStripContainer2.BottomToolStripPanel.PerformLayout()
        Me.ToolStripContainer2.ContentPanel.ResumeLayout(False)
        Me.ToolStripContainer2.ResumeLayout(False)
        Me.ToolStripContainer2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblUpdateTimeStamp As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblVehicleSpeed As System.Windows.Forms.Label
    Friend WithEvents lblHighestBaterryTemperature As System.Windows.Forms.Label
    Friend WithEvents lblLowestBateryVoltage As System.Windows.Forms.Label
    Friend WithEvents lblMotorCurrent As System.Windows.Forms.Label
    Friend WithEvents lblBatteryCurrent As System.Windows.Forms.Label
    Friend WithEvents lblArrayCurrent As System.Windows.Forms.Label
    Friend WithEvents tb As System.Windows.Forms.ToolStrip
    Friend WithEvents ID_Exit As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripContainer1 As System.Windows.Forms.ToolStripContainer
    Friend WithEvents gridTelemetry As System.Windows.Forms.DataGridView
    Friend WithEvents tabCharts As System.Windows.Forms.TabControl
    Friend WithEvents tabSummary As System.Windows.Forms.TabPage
    Friend WithEvents chartMain As System.Windows.Forms.DataVisualization.Charting.Chart
    Friend WithEvents tabBatteryVoltages As System.Windows.Forms.TabPage
    Friend WithEvents chartBatteryStatus As System.Windows.Forms.DataVisualization.Charting.Chart
    Friend WithEvents tabUserSpecified As System.Windows.Forms.TabPage
    Friend WithEvents chartUserSpecified As System.Windows.Forms.DataVisualization.Charting.Chart
    Friend WithEvents lblRegenStatus As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents lblCruiseSpeed As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents lblOdometer As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents tabTemperatures As System.Windows.Forms.TabPage
    Friend WithEvents lblTemperature8 As System.Windows.Forms.Label
    Friend WithEvents lblTemperature7 As System.Windows.Forms.Label
    Friend WithEvents lblTemperature6 As System.Windows.Forms.Label
    Friend WithEvents lblTemperature5 As System.Windows.Forms.Label
    Friend WithEvents lblTemperature4 As System.Windows.Forms.Label
    Friend WithEvents lblTemperature3 As System.Windows.Forms.Label
    Friend WithEvents lblTemperature2 As System.Windows.Forms.Label
    Friend WithEvents lblTemperature1 As System.Windows.Forms.Label
    Friend WithEvents lblTemperature32 As System.Windows.Forms.Label
    Friend WithEvents lblTemperature31 As System.Windows.Forms.Label
    Friend WithEvents lblTemperature30 As System.Windows.Forms.Label
    Friend WithEvents lblTemperature29 As System.Windows.Forms.Label
    Friend WithEvents lblTemperature28 As System.Windows.Forms.Label
    Friend WithEvents lblTemperature27 As System.Windows.Forms.Label
    Friend WithEvents lblTemperature26 As System.Windows.Forms.Label
    Friend WithEvents lblTemperature25 As System.Windows.Forms.Label
    Friend WithEvents lblTemperature24 As System.Windows.Forms.Label
    Friend WithEvents lblTemperature23 As System.Windows.Forms.Label
    Friend WithEvents lblTemperature22 As System.Windows.Forms.Label
    Friend WithEvents lblTemperature21 As System.Windows.Forms.Label
    Friend WithEvents lblTemperature20 As System.Windows.Forms.Label
    Friend WithEvents lblTemperature19 As System.Windows.Forms.Label
    Friend WithEvents lblTemperature18 As System.Windows.Forms.Label
    Friend WithEvents lblTemperature17 As System.Windows.Forms.Label
    Friend WithEvents lblTemperature16 As System.Windows.Forms.Label
    Friend WithEvents lblTemperature15 As System.Windows.Forms.Label
    Friend WithEvents lblTemperature14 As System.Windows.Forms.Label
    Friend WithEvents lblTemperature13 As System.Windows.Forms.Label
    Friend WithEvents lblTemperature12 As System.Windows.Forms.Label
    Friend WithEvents lblTemperature11 As System.Windows.Forms.Label
    Friend WithEvents lblTemperature10 As System.Windows.Forms.Label
    Friend WithEvents lblTemperature9 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents lblTempMotor As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents lblTempAirOutlet As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents lblTempAirInlet As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents lblTempHeatsink As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents lblTempCapacitor As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents lblTempProcessor As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents ssConnectedStatus As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripContainer2 As System.Windows.Forms.ToolStripContainer
    Friend WithEvents statusbar As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripLabel1 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents cboDisplayMode As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents ToolStripLabel2 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents cboDataInterval As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents lastTimeStamp As System.Windows.Forms.DateTimePicker
    Friend WithEvents ID_RetrieveData As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tbZoomable As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnInclude As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents ID_Current As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ID_Speed As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ID_Battery As System.Windows.Forms.ToolStripMenuItem
End Class
