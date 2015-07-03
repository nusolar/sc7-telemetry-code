Imports System.Data
Imports System.Data.Common
Imports System.Data.Odbc
Imports System.Windows.Forms.DataVisualization
Imports System.IO
Imports System.Threading

Public Class Main

    Private Const k_VelocityField = "VelocityVehicle"
    Private Const k_MotorAmpField = "CurrentBus"

    Private _data As DataTable
    Private _row As Integer
    Private WithEvents _CurrentTelemetry As Telemetry = Nothing
    Private _TelemetryConfiguration As String = ""
    Private _SyncLockObject As New Object
    Private _LastUpdate As Date
    Private _ChartInterval As Integer = 1   ' Seconds
    Private _BPCTripVoltage As Double = My.Settings.BPCTripVoltage
    Private _BPCTripVoltageWarning As Double = _BPCTripVoltage * My.Settings.BPCTripVoltageWarning
    Private _Client As Client = Nothing
    Private _RecentHistory As New Collection
    Private Delegate Sub ProcessUpdateDelegate(ByVal buffer As String)
    Private Delegate Sub ConnectionLostDelegate(ByVal Connected As Boolean)
    Public Sub ConnectionStatusChange(ByVal Connected As Boolean)
        '
        '   If called on the wrong thread, invoke self to get onto the correct thread
        '
        If Me.InvokeRequired Then
            Me.Invoke(New ConnectionLostDelegate(AddressOf ConnectionStatusChange), Connected)
            Exit Sub
        End If
        '
        If Connected Then
            ssConnectedStatus.Image = My.Resources.Connected_Small.ToBitmap
            ssConnectedStatus.Text = "Connected to telemetry server"
        Else
            ssConnectedStatus.Image = My.Resources.Disconnected_Small.ToBitmap
            ssConnectedStatus.Text = "Not connected to telemetry server"
        End If
    End Sub
    Private Sub AddToCache(ByVal TelemetryObject As Telemetry)
        Dim o As New Telemetry(_TelemetryConfiguration)
        o.Serialized = TelemetryObject.Serialized
        _RecentHistory.Add(o)
        If _RecentHistory.Count > My.Settings.MaximumSeriesPoints Then
            _RecentHistory.Remove(1)
        End If
    End Sub
    Public Sub ProcessUpdate(ByVal Buffer As String)
        Try
            '
            '   If called on the wrong thread, invoke self to get onto the correct thread
            '
            If Me.InvokeRequired Then
                Me.Invoke(New ProcessUpdateDelegate(AddressOf ProcessUpdate), Buffer)
                Exit Sub
            End If
            '
            '   Ignore updates if in historic mode
            '
            If My.Settings.DisplayMode = "Historic" Then
                Exit Sub
            End If
            '
            '   If simply a ping from the server, ignore
            '
            If Buffer = "PING" Then
                Exit Sub
            End If
            '
            '   Process the data
            '
            SyncLock _SyncLockObject
                _CurrentTelemetry.Serialized = Buffer
                'If CType(_CurrentTelemetry.PropertyValue("ts1"), DateTime) <> _LastUpdate Then
                _LastUpdate = CType(_CurrentTelemetry.PropertyValue("ts1"), DateTime)
                gridTelemetry.Refresh()
                '
                '   Add to memory-based recent history
                '
                AddToCache(_CurrentTelemetry)
                '
                '   Update the key data items in the summary panel
                '
                UpdateKeyData(_CurrentTelemetry)
                '
                '   Update Charts
                '
                UpdateCharts(_CurrentTelemetry)
                '
                '   Update the temperature displays on the temperature tab
                '
                UpdateTempteratureDisplays(tabTemperatures, _CurrentTelemetry)

                'End If
            End SyncLock
        Catch ex As Exception
            MessageBox.Show("Unexpected error: " & ex.Message & " while processing update from telemetry server.", "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub LoadData()
        Dim cn As OdbcConnection
        Dim cmd As New OdbcCommand
        Dim dr As OdbcDataReader
        Dim fromDate As DateTime
        Dim toDate As DateTime
        Try
            Me.Cursor = Cursors.WaitCursor
            cn = New OdbcConnection(My.Settings.ODBCConnectionString)
            cn.Open()
            '
            toDate = lastTimeStamp.Value
            fromDate = toDate.AddSeconds(-1.1 * (My.Settings.MaximumSeriesPoints / _ChartInterval))
            _LastUpdate = fromDate.AddMinutes(-1)
            With cmd
                .CommandText = "Call p_uiGetHistoryData('" & fromDate.ToString("yyyy-MM-dd HH:mm:ss") & "','" & toDate.ToString("yyyy-MM-dd HH:mm:ss") & "', NULL)"
                .CommandType = CommandType.Text
                .Connection = cn
                dr = .ExecuteReader
            End With

            _data = New DataTable
            _data.Load(dr)
            _row = -1
            ResetCharts()
            If _data.Rows.Count > 0 Then
                '
                '   Skip bad data
                '
                _row = 0
                Do While IsDBNull(_data.Rows(_row).Item("CurrentArray"))
                    _row += 1
                Loop
                '
                '   Populate the charts with data.
                '
                For i As Integer = _row To _data.Rows.Count - 1
                    _CurrentTelemetry.DataRowToClass = _data.Rows(i)
                    AddToCache(_CurrentTelemetry)
                    UpdateCharts(_CurrentTelemetry)
                Next
            End If
            '
            '   Make sure the chart scale is ok
            '
            With chartMain
                For Each area As DataVisualization.Charting.ChartArea In .ChartAreas
                    If .Series(0).Points.Count > 0 Then
                        area.AxisX.Minimum = .Series(0).Points(0).XValue
                        area.AxisX.Maximum = .Series(0).Points(.Series(0).Points.Count - 1).XValue
                        If area.Name <> "Battery" Then
                            area.AxisY.Minimum = [Double].NaN
                            area.AxisY.Maximum = [Double].NaN
                            area.RecalculateAxesScale()
                        End If
                    End If
                Next
                .Invalidate()
            End With
            '
            '   If missing data, fill in with last values
            '
            If _row < 0 Then
                _CurrentTelemetry.PropertyValue("ts1") = fromDate
            End If
            For i As Integer = _data.Rows.Count To My.Settings.MaximumSeriesPoints
                _CurrentTelemetry.PropertyValue("ts1") = CType(_CurrentTelemetry.PropertyValue("ts1"), System.DateTime).AddSeconds(_ChartInterval)
                AddToCache(_CurrentTelemetry)
                UpdateCharts(_CurrentTelemetry)
            Next
            _LastUpdate = CType(_CurrentTelemetry.PropertyValue("ts1"), DateTime)
            gridTelemetry.Refresh()
            UpdateKeyData(_CurrentTelemetry)
            UpdateTempteratureDisplays(tabTemperatures, _CurrentTelemetry)
            cn.Close()
        Catch ex As Exception
            MessageBox.Show("Unexpected error: " & ex.Message & " while loading historic data.", "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            _data = Nothing
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Private Sub ConfigureControls()
        Try
            With tb
                .Items("ID_Exit").Image = My.Resources.Exit_Small.ToBitmap
                .Items("ID_RetrieveData").Image = My.Resources.Resume_Small.ToBitmap
            End With
            With chartMain
                .Series.Clear()
                .ChartAreas.Clear()
                With .ChartAreas.Add("Current")
                    .AxisX.LabelStyle.Format = "T"
                    .AxisX.LabelAutoFitMaxFontSize = 8
                    .AxisY.LabelAutoFitMaxFontSize = 8
                    .IsSameFontSizeForAllAxes = True
                    .AxisY.Title = "AMPS"
                    .AxisY.LabelStyle.Format = k_CurrentFormat
                End With
                With .ChartAreas.Add("Speed")
                    .AxisX.LabelStyle.Format = "T"
                    .AxisX.LabelAutoFitMaxFontSize = 8
                    .AxisY.LabelAutoFitMaxFontSize = 8
                    .IsSameFontSizeForAllAxes = True
                    .AxisY.Title = "MPH"
                    .AxisY.LabelStyle.Format = k_SpeedFormat
                End With
                With .ChartAreas.Add("Battery")
                    .AxisX.LabelStyle.Format = "T"
                    .AxisX.LabelAutoFitMaxFontSize = 8
                    .AxisY.LabelAutoFitMaxFontSize = 8
                    .AxisY.Maximum = 4.2
                    .AxisY.Minimum = 3.0
                    .IsSameFontSizeForAllAxes = True
                    .AxisY.Title = "Volts"
                    .AxisY.LabelStyle.Format = k_VoltageFormat
                End With

                With .Series.Add("Array")
                    .ChartType = DataVisualization.Charting.SeriesChartType.Line
                    .YValueType = DataVisualization.Charting.ChartValueType.Double
                    .XValueType = Charting.ChartValueType.DateTime
                    .BorderWidth = 1
                    .ChartArea = "Current"
                End With
                With .Series.Add("Motor")
                    .ChartType = DataVisualization.Charting.SeriesChartType.Line
                    .YValueType = DataVisualization.Charting.ChartValueType.Double
                    .XValueType = Charting.ChartValueType.DateTime
                    .BorderWidth = 1
                    .ChartArea = "Current"
                End With
                With .Series.Add("Battery")
                    .ChartType = DataVisualization.Charting.SeriesChartType.Line
                    .YValueType = DataVisualization.Charting.ChartValueType.Double
                    .XValueType = Charting.ChartValueType.DateTime
                    .BorderWidth = 1
                    .ChartArea = "Current"
                End With
                With .Series.Add("Speed")
                    .ChartType = DataVisualization.Charting.SeriesChartType.Line
                    .YValueType = DataVisualization.Charting.ChartValueType.Double
                    .XValueType = Charting.ChartValueType.DateTime
                    .BorderWidth = 1
                    .ChartArea = "Speed"
                End With
                With .Series.Add("High Pack")
                    .ChartType = DataVisualization.Charting.SeriesChartType.Line
                    .YValueType = DataVisualization.Charting.ChartValueType.Double
                    .XValueType = Charting.ChartValueType.DateTime
                    .BorderWidth = 1
                    .Color = Color.FromArgb(119, 147, 60)
                    .ChartArea = "Battery"
                End With
                With .Series.Add("Average Pack")
                    .ChartType = DataVisualization.Charting.SeriesChartType.Line
                    .YValueType = DataVisualization.Charting.ChartValueType.Double
                    .XValueType = Charting.ChartValueType.DateTime
                    .BorderWidth = 1
                    .Color = Color.FromArgb(252, 242, 106)
                    .ChartArea = "Battery"
                End With
                With .Series.Add("Low Pack")
                    .ChartType = DataVisualization.Charting.SeriesChartType.Line
                    .YValueType = DataVisualization.Charting.ChartValueType.Double
                    .XValueType = Charting.ChartValueType.DateTime
                    .BorderWidth = 1
                    .Color = Color.FromArgb(192, 80, 77)
                    .ChartArea = "Battery"
                End With
            End With
            '
            With chartBatteryStatus
                .Series.Clear()
                .ChartAreas.Clear()
                With .ChartAreas.Add("Voltage")
                    .AxisX.Interval = 1
                    .AxisX.MajorGrid.Enabled = False
                    .AxisY.Title = "Voltage"
                    .AxisX.LabelAutoFitMaxFontSize = 8
                    .AxisY.LabelAutoFitMaxFontSize = 8
                    .IsSameFontSizeForAllAxes = True
                    .AxisY.Maximum = 4.2
                    .AxisY.Minimum = 3.0
                    .AxisY.LabelStyle.Format = k_VoltageFormat
                End With
                With .Series.Add("BPC Trip Voltage")
                    .ChartType = DataVisualization.Charting.SeriesChartType.StackedBar
                    .YValueType = DataVisualization.Charting.ChartValueType.Double
                    .XValueType = Charting.ChartValueType.String
                    .ChartArea = "Voltage"
                    .IsValueShownAsLabel = False
                    .LabelFormat = k_VoltageFormat
                    .Font = New Font("Arial", 8)
                    .Color = Color.FromArgb(192, 80, 77)
                End With
                With .Series.Add("BPC Trip Warning")
                    .ChartType = DataVisualization.Charting.SeriesChartType.StackedBar
                    .YValueType = DataVisualization.Charting.ChartValueType.Double
                    .XValueType = Charting.ChartValueType.String
                    .ChartArea = "Voltage"
                    .IsValueShownAsLabel = False
                    .LabelFormat = k_VoltageFormat
                    .Font = New Font("Arial", 8)
                    .Color = Color.FromArgb(252, 242, 106)
                End With
                With .Series.Add("Voltage")
                    .ChartType = DataVisualization.Charting.SeriesChartType.StackedBar
                    .YValueType = DataVisualization.Charting.ChartValueType.Double
                    .XValueType = Charting.ChartValueType.String
                    .ChartArea = "Voltage"
                    .IsValueShownAsLabel = False
                    .LabelFormat = k_VoltageFormat
                    .Font = New Font("Arial", 8)
                    .Color = Color.FromArgb(119, 147, 60)
                    .LabelForeColor = Color.Black
                End With

                '
                '   Define the packs
                '
                For i As Integer = 1 To k_BatteryPacks
                    .Series("BPC Trip Voltage").Points(.Series("BPC Trip Voltage").Points.AddXY(i - 1, _BPCTripVoltage)).AxisLabel = "Brick " & i.ToString
                    .Series("BPC Trip Warning").Points(.Series("BPC Trip Warning").Points.AddXY(i - 1, _BPCTripVoltageWarning)).AxisLabel = "Brick " & i.ToString
                    .Series("Voltage").Points(.Series("Voltage").Points.AddXY(i - 1, 0)).AxisLabel = "Brick " & i.ToString
                Next

            End With
            '
            With gridTelemetry
                .Columns.Clear()
                .Columns.Add("Parameter", "Parameter")
                .Columns.Add("Value", "Value")
                Dim column As New DataGridViewCheckBoxColumn(False)
                With column
                    .Name = "Chart"
                    .HeaderText = " Chart "
                    .HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                End With
                .Columns.Add(column)
                '
                With .Columns("Value")
                    .HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
                .RowHeadersVisible = False
                .VirtualMode = True
                .RowCount = _CurrentTelemetry.FieldCount
            End With
            '
            With chartUserSpecified
                .ChartAreas.Clear()
                .Series.Clear()
            End With
            '
            lastTimeStamp.Value = Date.Now
            cboDisplayMode.SelectedItem = My.Settings.DisplayMode
            cboDataInterval.SelectedItem = My.Settings.DataInterval

            '
        Catch ex As Exception
            MessageBox.Show("Unexpected error: " & ex.Message & " while configuring controls.", "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub AddUserSelectedChart(ByVal Parameter As String)
        Try
            With chartUserSpecified
                If .ChartAreas.Count = 0 Then
                    With .ChartAreas.Add("1")
                        .AxisX.LabelStyle.Format = "T"
                        .AxisX.LabelAutoFitMaxFontSize = 8
                        .AxisY.LabelAutoFitMaxFontSize = 8
                        .IsSameFontSizeForAllAxes = True
                        '.AxisY.LabelStyle.Format = _CurrentTelemetry.DisplayFormat(Parameter)
                        .AxisX.Minimum = chartMain.ChartAreas("Current").AxisX.Minimum
                        .AxisX.Maximum = chartMain.ChartAreas("Current").AxisX.Maximum
                    End With
                End If
                With .Series.Add(Parameter)
                    .ChartType = DataVisualization.Charting.SeriesChartType.Line
                    .YValueType = DataVisualization.Charting.ChartValueType.Double
                    .XValueType = Charting.ChartValueType.DateTime
                    .BorderWidth = 1
                End With
                For Each s As DataVisualization.Charting.Series In .Series
                    s.Points.Clear()
                    If _RecentHistory.Count = 0 Then
                        For i = 0 To chartMain.Series(0).Points.Count - 1
                            s.Points.AddXY(chartMain.Series(0).Points(i).XValue, 0)
                        Next
                    End If
                Next
            End With
            For Each o As Telemetry In _RecentHistory
                UpdateUserSpecifiedChart(o)
            Next
        Catch ex As Exception
            MessageBox.Show("Unexpected error: " & ex.Message & " while adding user-selected charts.", "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub RemoveUserSelectedChart(ByVal Parameter As String)
        Try
            With chartUserSpecified
                .Series.Remove(.Series(Parameter))
                If .Series.Count = 0 Then
                    .ChartAreas.Clear()
                End If
            End With
        Catch ex As Exception
            MessageBox.Show("Unexpected error: " & ex.Message & " while removing user-selected charts.", "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub ConfigureBuffer()
        _TelemetryConfiguration = File.ReadAllText("Telemetry.xml", System.Text.Encoding.GetEncoding("iso-8859-1"))
        _CurrentTelemetry = New Telemetry(_TelemetryConfiguration)
    End Sub
    Private Sub StartListening()
        Try
            ConnectionStatusChange(False)
            _Client = New Client(Me, My.Settings.ServerAddress, My.Settings.ServerPort)
            _Client.Connect()
        Catch ex As Exception
            MessageBox.Show("Unexpected error: " & ex.Message & " while initiating communications with telemetry server.", "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub Main_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ConfigureBuffer()
        ConfigureControls()
        SetChartVisibilities()
        StartListening()
    End Sub
    Private Sub UpdateKeyData(ByVal TelemetryObject As Telemetry)
        lastTimeStamp.Value = CType(TelemetryObject.PropertyValue("ts1"), DateTime)
        FormatValue(TelemetryObject.PropertyValue("CurrentArray"), k_CurrentFormat, lblArrayCurrent)
        FormatValue(TelemetryObject.PropertyValue("CurrentBattery"), k_CurrentFormat, lblBatteryCurrent)
        FormatValue(TelemetryObject.PropertyValue(k_MotorAmpField), k_CurrentFormat, lblMotorCurrent)
        FormatValue(TelemetryObject.PropertyValue("VoltageMin"), k_VoltageFormat, lblLowestBateryVoltage)
        FormatValue(TelemetryObject.PropertyValue("TempMax"), k_TemperatureFormat, lblHighestBaterryTemperature)
        FormatValue(TelemetryObject.PropertyValue(k_VelocityField), k_SpeedFormat, lblVehicleSpeed)
        FormatValue(TelemetryObject.PropertyValue("Odometer"), k_SpeedFormat, lblOdometer)
        '
        If CType(TelemetryObject.PropertyValue("EnableRegen"), System.Int16) <> 0 Then
            lblRegenStatus.Text = "Enabled"
            lblRegenStatus.ForeColor = Color.Green
        Else
            lblRegenStatus.Text = "off"
            lblRegenStatus.ForeColor = Color.Black
        End If
        If CType(TelemetryObject.PropertyValue("EnableCruise"), System.Int16) <> 0 Then
            FormatValue(_CurrentTelemetry.PropertyValue("VelocityCruise"), k_SpeedFormat, lblCruiseSpeed)
            lblCruiseSpeed.ForeColor = Color.Green
        Else
            lblCruiseSpeed.Text = "off"
            lblCruiseSpeed.ForeColor = Color.Black
        End If
    End Sub
    Private Sub ResetCharts()
        With chartMain
            For Each s As DataVisualization.Charting.Series In .Series
                s.Points.Clear()
            Next
        End With
    End Sub
    Public Property Zoomable As Boolean
        Get
            Zoomable = chartMain.ChartAreas(0).AxisX.ScaleView.Zoomable
        End Get
        Set(ByVal value As Boolean)
            For Each area As DataVisualization.Charting.ChartArea In chartMain.ChartAreas
                area.CursorX.IsUserEnabled = value
                area.CursorX.IsUserSelectionEnabled = value
                area.AxisX.ScaleView.Zoomable = value
                area.AxisX.ScrollBar.IsPositionedInside = value
                area.CursorY.IsUserEnabled = value
                area.CursorY.IsUserSelectionEnabled = value
                area.AxisY.ScaleView.Zoomable = value
                area.AxisY.ScrollBar.IsPositionedInside = value
                If value Then
                    area.CursorX.LineColor = Color.Red
                    area.CursorY.LineColor = Color.Red
                Else
                    area.CursorX.LineColor = Color.Transparent
                    area.CursorY.LineColor = Color.Transparent
                End If
            Next
        End Set
    End Property
    Private ReadOnly Property SummaryChartVisible(ByVal ChartName As String) As Boolean
        Get
            SummaryChartVisible = CType(btnInclude.DropDownItems(ChartName), ToolStripMenuItem).CheckState = CheckState.Checked
        End Get
    End Property
    Private Sub SetChartVisibilities()
        If chartMain.ChartAreas.Count = 3 Then
            chartMain.ChartAreas("Current").Visible = SummaryChartVisible("ID_Current")
            chartMain.ChartAreas("Speed").Visible = SummaryChartVisible("ID_Speed")
            chartMain.ChartAreas("Battery").Visible = SummaryChartVisible("ID_Battery")
        End If
    End Sub

    Private Sub UpdateCharts(ByVal TelemetryObject As Telemetry)
        '
        '   Update Charts
        '
        Dim pointsremoved As Boolean = False
        Dim charttime As Double = CType(TelemetryObject.PropertyValue("ts1"), DateTime).ToOADate()
        With chartMain
            If .Series("Array").Points.Count = 0 Then
                .ChartAreas("Current").AxisX.Minimum = charttime
                .ChartAreas("Current").AxisX.Maximum = _LastUpdate.AddSeconds(My.Settings.MaximumSeriesPoints / _ChartInterval).ToOADate()
                .ChartAreas("Speed").AxisX.Minimum = .ChartAreas("Current").AxisX.Minimum
                .ChartAreas("Speed").AxisX.Maximum = .ChartAreas("Current").AxisX.Maximum
                .ChartAreas("Battery").AxisX.Minimum = .ChartAreas("Current").AxisX.Minimum
                .ChartAreas("Battery").AxisX.Maximum = .ChartAreas("Current").AxisX.Maximum
            End If
            .Series("Array").Points.AddXY(charttime, TelemetryObject.PropertyValue("CurrentArray"))
            .Series("Motor").Points.AddXY(charttime, TelemetryObject.PropertyValue(k_MotorAmpField))
            .Series("Battery").Points.AddXY(charttime, TelemetryObject.PropertyValue("CurrentBattery"))
            .Series("Speed").Points.AddXY(charttime, TelemetryObject.PropertyValue(k_VelocityField))
            .Series("High Pack").Points.AddXY(charttime, TelemetryObject.PropertyValue("VoltageMax"))
            .Series("Average Pack").Points.AddXY(charttime, TelemetryObject.PropertyValue("VoltageAvg"))
            .Series("Low Pack").Points.AddXY(charttime, TelemetryObject.PropertyValue("VoltageMin"))
            '
            '   Keep constant number of points on chart
            '
            For Each s As DataVisualization.Charting.Series In .Series
                If s.Points.Count > My.Settings.MaximumSeriesPoints Then
                    While s.Points.Count > My.Settings.MaximumSeriesPoints
                        s.Points.RemoveAt(0)
                    End While
                    pointsremoved = True
                End If
            Next
            '
            '   Now adjust the axis
            '
            If pointsremoved Then
                For Each area As DataVisualization.Charting.ChartArea In .ChartAreas
                    area.AxisX.Minimum = .Series(0).Points(0).XValue
                    area.AxisX.Maximum = .Series(0).Points(.Series(0).Points.Count - 1).XValue
                    If area.Name <> "Battery" Then
                        area.AxisY.Minimum = [Double].NaN
                        area.AxisY.Maximum = [Double].NaN
                        area.RecalculateAxesScale()
                    End If
                Next
                .Invalidate()
            End If
        End With
        '
        Dim a As DataVisualization.Charting.TextAnnotation = Nothing
        With chartBatteryStatus
            .Series("Voltage").Points.SuspendUpdates()
            .Series("Voltage").Points.Clear()
            .Annotations.Clear()
            For i As Integer = 1 To k_BatteryPacks
                '
                '   Based on the voltage, determine what parts of the three items exist
                '
                If CType(TelemetryObject.PropertyValue("VoltageBrick" & i.ToString), Double) - _BPCTripVoltage - _BPCTripVoltageWarning > 0 Then
                    .Series("Voltage").Points(.Series("Voltage").Points.AddXY(i - 1, CType(TelemetryObject.PropertyValue("VoltageBrick" & i.ToString), Double) - _BPCTripVoltage - _BPCTripVoltageWarning)).AxisLabel = "Brick " & i.ToString
                Else
                    .Series("Voltage").Points(.Series("Voltage").Points.AddXY(i - 1, 0)).AxisLabel = "Brick " & i.ToString
                End If
                a = New DataVisualization.Charting.TextAnnotation
                With a
                    .Text = Format(CType(TelemetryObject.PropertyValue("VoltageBrick" & i.ToString), Double), k_VoltageFormat)
                    .AnchorDataPoint = chartBatteryStatus.Series("Voltage").Points(i - 1)
                    .AnchorAlignment = ContentAlignment.MiddleLeft
                    .SmartLabelStyle.Enabled = False
                End With
                chartBatteryStatus.Annotations.Add(a)
            Next
            .Series("Voltage").Points.ResumeUpdates()
        End With
        '
        UpdateUserSpecifiedChart(TelemetryObject)

    End Sub
    Private Sub UpdateUserSpecifiedChart(ByVal TelemetryObject As Telemetry)
        Dim pointsremoved As Boolean = False
        Dim charttime As Double = CType(TelemetryObject.PropertyValue("ts1"), DateTime).ToOADate()
        With chartUserSpecified
            If .Series.Count > 0 Then
                pointsremoved = False
                For Each s As DataVisualization.Charting.Series In .Series
                    s.Points.AddXY(charttime, TelemetryObject.PropertyValue(s.Name))
                    If s.Points.Count > My.Settings.MaximumSeriesPoints Then
                        While s.Points.Count > My.Settings.MaximumSeriesPoints
                            s.Points.RemoveAt(0)
                        End While
                        pointsremoved = True
                    End If
                Next
                If pointsremoved Then
                    For Each area As DataVisualization.Charting.ChartArea In .ChartAreas
                        area.AxisX.Minimum = .Series(0).Points(0).XValue
                        area.AxisX.Maximum = .Series(0).Points(.Series(0).Points.Count - 1).XValue
                        area.AxisY.Minimum = [Double].NaN
                        area.AxisY.Maximum = [Double].NaN
                        area.RecalculateAxesScale()
                    Next
                    .Invalidate()
                End If
            End If
        End With
    End Sub
    Private Sub UpdateTempteratureDisplays(ByVal root As Control, ByVal TelemetryObject As Telemetry)
        For Each c As Control In root.Controls
            If c.Controls.Count > 0 Then
                UpdateTempteratureDisplays(c, TelemetryObject)
            Else
                If TypeOf (c) Is Label Then
                    If Not c.Tag Is Nothing Then
                        If CType(c.Tag, System.String) = "temperature" Then
                            FormatValue(TelemetryObject.PropertyValue(c.Name.Replace("lbl", "")), TelemetryObject.DisplayFormat(c.Name.Replace("lbl", "")), c)
                        End If
                    End If
                End If
            End If
        Next
    End Sub
    Private Sub ID_Exit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ID_Exit.Click
        Close()
    End Sub
    Private Sub gridTelemetry_CellValueNeeded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellValueEventArgs) Handles gridTelemetry.CellValueNeeded
        SyncLock _SyncLockObject
            e.Value = _CurrentTelemetry.PropertyAttribute(e.RowIndex, gridTelemetry.Columns(e.ColumnIndex).Name)
        End SyncLock
    End Sub
    Private Sub gridTelemetry_CellValuePushed(sender As Object, e As System.Windows.Forms.DataGridViewCellValueEventArgs) Handles gridTelemetry.CellValuePushed
        If gridTelemetry.Columns(e.ColumnIndex).Name = "Chart" Then
            SyncLock _SyncLockObject
                _CurrentTelemetry.ChartMe(_CurrentTelemetry.PropertyAttribute(e.RowIndex, "Parameter")) = e.Value
            End SyncLock
        End If
    End Sub
    Private Sub gridTelemetry_CurrentCellDirtyStateChanged(sender As Object, e As System.EventArgs) Handles gridTelemetry.CurrentCellDirtyStateChanged
        If TypeOf (gridTelemetry.CurrentCell) Is DataGridViewCheckBoxCell Then
            gridTelemetry.CommitEdit(DataGridViewDataErrorContexts.Commit)
        End If
    End Sub
    Private Sub _CurrentTelemetry_ChartSelectionsChanged(ByVal Parameter As String, ByVal ChartParameter As Boolean) Handles _CurrentTelemetry.ChartSelectionsChanged
        If ChartParameter Then
            AddUserSelectedChart(Parameter)
        Else
            RemoveUserSelectedChart(Parameter)
        End If
    End Sub
    Private Sub chartBatteryStatus_FormatNumber(ByVal sender As Object, ByVal e As System.Windows.Forms.DataVisualization.Charting.FormatNumberEventArgs) Handles chartBatteryStatus.FormatNumber
        '
        '   Add back the BPC Trip voltage to the data point value so the actual voltage is displayed
        '
        If e.ElementType = Charting.ChartElementType.DataPoint Then
            e.LocalizedValue = Format(e.Value + _BPCTripVoltage + _BPCTripVoltageWarning, e.Format)
        End If
    End Sub
    Private Sub cboDataInterval_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cboDataInterval.SelectedIndexChanged
        My.Settings.DataInterval = cboDataInterval.SelectedItem
        My.Settings.Save()
    End Sub
    Private Sub cboDisplayMode_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cboDisplayMode.SelectedIndexChanged
        My.Settings.DisplayMode = cboDisplayMode.SelectedItem
        My.Settings.Save()
        Select Case My.Settings.DisplayMode
            Case "Real time"
                lastTimeStamp.Enabled = False
                ID_RetrieveData.Enabled = False
                lastTimeStamp.Value = Date.Now
                LoadData()
            Case "Historic"
                lastTimeStamp.Enabled = True
                ID_RetrieveData.Enabled = True
        End Select
    End Sub

    Private Sub ID_RetrieveData_Click(sender As Object, e As System.EventArgs) Handles ID_RetrieveData.Click
        LoadData()
    End Sub

    Private Sub tbZoomable_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbZoomable.CheckedChanged
        Zoomable = (tbZoomable.Checked)
    End Sub

    Private Sub ID_Current_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ID_Current.CheckStateChanged, ID_Battery.CheckStateChanged, ID_Speed.CheckStateChanged
        SetChartVisibilities()
    End Sub
End Class