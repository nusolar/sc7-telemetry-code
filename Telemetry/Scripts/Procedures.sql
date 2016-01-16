SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sys_LoadReferenceTables]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sys_LoadReferenceTables]
GO

CREATE PROCEDURE sys_LoadReferenceTables
AS

    SET NOCOUNT ON
    
    /* Loads reference table data */
    
    /* Summary Types */
    DELETE FROM tblValidSummaryType
    
    INSERT INTO tblValidSummaryType (ID, SummaryType) VALUES (0, 'None')
    INSERT INTO tblValidSummaryType (ID, SummaryType) VALUES (1, 'Average')
    INSERT INTO tblValidSummaryType (ID, SummaryType) VALUES (2, 'Minimum')
    INSERT INTO tblValidSummaryType (ID, SummaryType) VALUES (3, 'Maximum')
   
    /* Data Types */
    
    DELETE FROM tblValidDataType
    
    INSERT INTO tblValidDataType (ID, DataType, SQLDataType) VALUES (1, 'System.Int16', 'SmallInt')
    INSERT INTO tblValidDataType (ID, DataType, SQLDataType) VALUES (2, 'System.Int32', 'Integer')
    INSERT INTO tblValidDataType (ID, DataType, SQLDataType) VALUES (3, 'System.Double', 'Decimal(28,14)')
    INSERT INTO tblValidDataType (ID, DataType, SQLDataType) VALUES (4, 'System.Decimal', 'Decimal(28,14)')
    INSERT INTO tblValidDataType (ID, DataType, SQLDataType) VALUES (5, 'System.DateTime', 'DateTime')
    INSERT INTO tblValidDataType (ID, DataType, SQLDataType) VALUES (6, 'System.Float', 'Float')
    INSERT INTO tblValidDataType (ID, DataType, SQLDataType) VALUES (7, 'System.UInt16', 'Numeric(20)')
    INSERT INTO tblValidDataType (ID, DataType, SQLDataType) VALUES (8, 'System.UInt32', 'Numeric(10)')
    INSERT INTO tblValidDataType (ID, DataType, SQLDataType) VALUES (9, 'System.UInt64', 'Numeric(5)')
    INSERT INTO tblValidDataType (ID, DataType, SQLDataType) VALUES (10, 'System.Byte(8)', 'Binary(8)')


GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sys_LoadDataItems]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sys_LoadDataItems]
GO

CREATE PROCEDURE sys_LoadDataItems
AS

    /* Defines the data items used by the telemetry system */
    
DECLARE @type_none int,
        @type_average int,
        @type_minimum int,
        @type_maximum int,
        @datatype_Int16 int,
        @datatype_Int32 int,
        @datatype_Double int,
        @datatype_Decimal int,
        @datatype_DateTime int,
        @datatype_Float int,
        @datatype_UInt16 int,
        @datatype_UInt32 int,
        @datatype_UInt64 int,
        @datatype_Byte8 int,
        @chart_yes bit,
        @chart_no bit
        
    SET NOCOUNT ON
    
    DELETE FROM tblDataItems
    EXEC sys_LoadReferenceTables
    
    SELECT @chart_yes = 0, @chart_no = 1
    
    /* Lookup reference values */
    
    SELECT @type_none = ID FROM tblValidSummaryType WHERE SummaryType = 'None'
    SELECT @type_average = ID FROM tblValidSummaryType WHERE SummaryType = 'Average'
    SELECT @type_minimum = ID FROM tblValidSummaryType WHERE SummaryType = 'Minimum'
    SELECT @type_maximum = ID FROM tblValidSummaryType WHERE SummaryType = 'Maximum'
    
    SELECT @datatype_Int16 = ID FROM tblValidDataType WHERE DataType = 'System.Int16'
    SELECT @datatype_Int32 = ID FROM tblValidDataType WHERE DataType = 'System.Int32'
    SELECT @datatype_Double = ID FROM tblValidDataType WHERE DataType = 'System.Double'
    SELECT @datatype_Decimal = ID FROM tblValidDataType WHERE DataType = 'System.Decimal'
    SELECT @datatype_DateTime = ID FROM tblValidDataType WHERE DataType = 'System.DateTime'
    SELECT @datatype_Float = ID FROM tblValidDataType WHERE DataType = 'System.Float'
    SELECT @datatype_UInt16 = ID FROM tblValidDataType WHERE DataType = 'System.UInt16'
    SELECT @datatype_UInt32 = ID FROM tblValidDataType WHERE DataType = 'System.UInt32'
    SELECT @datatype_UInt64 = ID FROM tblValidDataType WHERE DataType = 'System.UInt64'
    SELECT @datatype_Byte8 = ID FROM tblValidDataType WHERE DataType = 'System.Byte(8)'
 
    /* Define data items */
	-- Metadata columns (CAN ID = null)  
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('RowNum', 'ROWN', NULL, @type_none, 'Row Number', '#,##0', @datatype_Int32, @chart_no, 0, @datatype_Byte8)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('TimeStamp', 'TS', NULL, @type_none, 'Time Stamp', 'T', @datatype_DateTime, @chart_no, 0, @datatype_Byte8)

	-- Motor controller columns (base addr = 0x400)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('MC_SerialNumber', 'MCNO', '400', @type_none, 'MC Serial Number', '#,##0.00;#,##0.00', @datatype_UInt32, @chart_yes, 4, @datatype_UInt32)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('MC_TrituimID', 'TID', '400', @type_none, 'MC Trituim ID', '#,##0.00;#,##0.00', @datatype_UInt32, @chart_yes, 0, @datatype_UInt32)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('MC_ActiveMotor', 'ACTM', '401', @type_none, 'Active Motor Index', '#,##0.00;#,##0.00', @datatype_UInt16, @chart_yes, 4, @datatype_UInt16)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('MC_ErrorFlags', 'MEFL', '401', @type_none, 'MC Error Flags', '#,##0.00;#,##0.00', @datatype_UInt16, @chart_yes, 2, @datatype_UInt16)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('MC_LimitFlags', 'MLFL', '401', @type_none, 'MC Limit Flags', '#,##0.00;#,##0.00', @datatype_UInt16, @chart_yes, 0, @datatype_UInt16)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('MC_BusCurrent', 'IBUS', '402', @type_average, 'Bus Current', '#,##0.00;#,##0.00', @datatype_Float, @chart_yes, 4, @datatype_Float)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('MC_BusVoltage', 'VBUS', '402', @type_average, 'Bus Voltage', '#,##0.00;#,##0.00', @datatype_Float, @chart_yes, 0, @datatype_Float)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('MC_VehicleVelocity', 'VELV', '403', @type_average, 'Vehicle Velocity', '#,##0.00;#,##0.00', @datatype_Float, @chart_yes, 4, @datatype_Float)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('MC_MotorVelocity', 'VELM', '403', @type_average, 'Motor Velocity', '#,##0.00;#,##0.00', @datatype_Float, @chart_yes, 0, @datatype_Float)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('MC_CurrentPhaseA', 'IPHA', '404', @type_average, 'Phase A Current', '#,##0.00;#,##0.00', @datatype_Float, @chart_yes, 4, @datatype_Float)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('MC_CurrentPhaseB', 'IPHB', '404', @type_average, 'Phase B Current', '#,##0.00;#,##0.00', @datatype_Float, @chart_yes, 0, @datatype_Float)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('MC_MotorVoltageReal', 'VMRL', '405', @type_average, 'Real Motor Voltage', '#,##0.00;#,##0.00', @datatype_Float, @chart_yes, 4, @datatype_Float)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('MC_MotorVoltageImag', 'VMIM', '405', @type_average, 'Imaginary Motor Voltage', '#,##0.00;#,##0.00', @datatype_Float, @chart_yes, 0, @datatype_Float)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('MC_MotorCurrentReal', 'IMRL', '406', @type_average, 'Real Motor Current', '#,##0.00;#,##0.00', @datatype_Float, @chart_yes, 4, @datatype_Float)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('MC_MotorCurrentImag', 'IMIM', '406', @type_average, 'Imaginary Motor Current', '#,##0.00;#,##0.00', @datatype_Float, @chart_yes, 0, @datatype_Float)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('MC_BackEMFReal', 'EMFR', '407', @type_average, 'Real Motor Back EMF', '#,##0.00;#,##0.00', @datatype_Float, @chart_yes, 4, @datatype_Float)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('MC_BackEMFImag', 'EMFI', '407', @type_average, 'Imaginary Motor Back EMF', '#,##0.00;#,##0.00', @datatype_Float, @chart_yes, 0, @datatype_Float)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('MC_15VSupply', '15VS', '408', @type_average, '15 Volt Supply Voltage', '#,##0.00;#,##0.00', @datatype_Float, @chart_yes, 4, @datatype_Float)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('MC_1.65VSupply', '165R', '408', @type_average, '1.65 Voltage Reference', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 0, @datatype_Float)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('MC_2.5VSupply', '25VS', '409', @type_average, '2.5 Volt Supply Voltage', '#,##0.00;#,##0.00', @datatype_Float, @chart_yes, 4, @datatype_Float)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('MC_1.2VSupply', '12VS', '409', @type_average, '1.2 Volt Supply Voltage', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 0, @datatype_Float)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('MC_FanSpeed', 'FRPM', '40A', @type_average, 'Fan Speed (RPM)', '#,##0.00;#,##0.00', @datatype_Float, @chart_yes, 4, @datatype_Float)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('MC_FanPercent', 'FPER', '40A', @type_average, 'Fan Drive Percent', '#,##0.00;#,##0.00', @datatype_Float, @chart_yes, 0, @datatype_Float)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('MC_HeatsinkTemp', 'THTS', '40B', @type_average, 'Heat Sink Temperature', '#,##0.00;#,##0.00', @datatype_Float, @chart_yes, 4, @datatype_Float)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('MC_MotorTemp', 'TMOT', '40B', @type_average, 'Motor Temperature', '#,##0.00;#,##0.00', @datatype_Float, @chart_yes, 0, @datatype_Float)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('MC_AirInletTemp', 'TINL', '40C', @type_average, 'Air Inlet Temperature', '#,##0.00;#,##0.00', @datatype_Float, @chart_yes, 4, @datatype_Float)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('MC_ProcessorTemp', 'TPRO', '40C', @type_average, 'Processor Temperature', '#,##0.00;#,##0.00', @datatype_Float, @chart_yes, 0, @datatype_Float)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('MC_AirOutletTemp', 'TOUT', '40D', @type_average, 'Air Outlet Temperature', '#,##0.00;#,##0.00', @datatype_Float, @chart_yes, 4, @datatype_Float)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('MC_CapacitorTemp', 'TCAP', '40D', @type_average, 'Capacitor Temperature', '#,##0.00;#,##0.00', @datatype_Float, @chart_yes, 0, @datatype_Float)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('MC_AmpHoursDCBus', 'AMPH', '40E', @type_average, 'DC Bus Amp Hours', '#,##0.00;#,##0.00', @datatype_Float, @chart_yes, 4, @datatype_Float)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('MC_Odometer', 'ODO', '40E', @type_average, 'Odometer (Meters)', '#,##0.00;#,##0.00', @datatype_Float, @chart_yes, 0, @datatype_Float)
    
	-- Driver controls columns (base addr = 0x500)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('DC_SerialNumber', 'DCNO', '500', @type_none, 'DC Serial Number', '#,##0.00;#,##0.00', @datatype_UInt32, @chart_yes, 4, @datatype_UInt32)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('DC_ID', 'DCID', '500', @type_none, 'DC ID', '#,##0.00;#,##0.00', @datatype_UInt32, @chart_yes, 0, @datatype_UInt32)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('DC_MotorCurrent', 'DCI', '501', @type_average, 'Requested Motor Current', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 4, @datatype_Float)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('DC_MotorVelocity', 'DCV', '501', @type_average, 'Requested Motor Velocity', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 0, @datatype_Float)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
	                  VALUES ('DC_StatusFlags', 'GEAR', '505', @type_none, 'Car Gear State', '#,##0', @datatype_Byte8, @chart_yes, 7, @datatype_Byte8)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
	                  VALUES ('DC_ErrorFlags', 'DCEF', '505', @type_none, 'Driver Controls Error Flags', '#,##0', @datatype_Byte8, @chart_yes, 6, @datatype_Byte8)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('DC_CANErrorFlags', 'DCCE', '505', @type_none, 'Driver Controls CAN Error Flags', '#,##0', @datatype_UInt16, @chart_yes, 4, @datatype_UInt16)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('DC_RegenPercent', 'RGPT', '505', @type_average, 'Regen Pedal Position', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 3, @datatype_Byte8)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('DC_AccelPercent', 'ACPT', '505', @type_average, 'Accelerator Position', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 2, @datatype_Byte8)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('DC_IgnitionSwitch', 'DCST', '505', @type_none, 'Driver Controls Status Flags', '#,##0', @datatype_UInt16, @chart_yes, 0, @datatype_UInt16)    

	--- BMS columns (base addr = 0x600)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_SerialNumber', 'BNO', '600', @type_none, 'BMS Serial Number', '#,##0.00;#,##0.00', @datatype_UInt32, @chart_yes, 4, @datatype_UInt32)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_ID', 'BNO', '600', @type_none, 'BMS ID', '#,##0.00;#,##0.00', @datatype_UInt32, @chart_yes, 0, @datatype_UInt32)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_CellTemp01', 'TC01', '601', @type_average, 'Cell 1 Temperature', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 6, @datatype_Int16)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_PCBTemp01', 'PB01', '601', @type_average, 'PCB 1 Temperature', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 4, @datatype_Int16)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_CMU01', 'CN01', '601', @type_none, 'CMU 1 Serial Number', '#,##0.00;#,##0.00', @datatype_UInt32, @chart_yes, 0, @datatype_UInt32)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_VoltageCell01', 'VC01', '602', @type_average, 'Cell 1 Voltage', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 0, @datatype_Int16)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_VoltageCell02', 'VC02', '602', @type_average, 'Cell 2 Voltage', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 2, @datatype_Int16)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_VoltageCell03', 'VC03', '602', @type_average, 'Cell 3 Voltage', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 4, @datatype_Int16)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_VoltageCell04', 'VC04', '602', @type_average, 'Cell 4 Voltage', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 6, @datatype_Int16)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_VoltageCell05', 'VC05', '603', @type_average, 'Cell 5 Voltage', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 0, @datatype_Int16)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_VoltageCell06', 'VC06', '603', @type_average, 'Cell 6 Voltage', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 2, @datatype_Int16)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_VoltageCell07', 'VC07', '603', @type_average, 'Cell 7 Voltage', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 4, @datatype_Int16)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_VoltageCell08', 'VC08', '603', @type_average, 'Cell 8 Voltage', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 6, @datatype_Int16)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_CellTemp02', 'TC02', '604', @type_average, 'Cell 2 Temperature', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 6, @datatype_Int16)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_PCBTemp02', 'PB01', '604', @type_average, 'PCB 2 Temperature', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 4, @datatype_Int16)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_CMU02', 'CN02', '604', @type_none, 'CMU 2 Serial Number', '#,##0.00;#,##0.00', @datatype_UInt32, @chart_yes, 0, @datatype_UInt32)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_VoltageCell09', 'VC09', '605', @type_average, 'Cell 9 Voltage', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 0, @datatype_Int16)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_VoltageCell10', 'VC10', '605', @type_average, 'Cell 10 Voltage', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 2, @datatype_Int16)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_VoltageCell11', 'VC11', '605', @type_average, 'Cell 11 Voltage', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 4, @datatype_Int16)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_VoltageCell12', 'VC12', '605', @type_average, 'Cell 12 Voltage', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 6, @datatype_Int16)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_VoltageCell13', 'VC13', '606', @type_average, 'Cell 13 Voltage', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 0, @datatype_Int16)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_VoltageCell14', 'VC14', '606', @type_average, 'Cell 14 Voltage', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 2, @datatype_Int16)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_VoltageCell15', 'VC15', '606', @type_average, 'Cell 15 Voltage', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 4, @datatype_Int16)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_VoltageCell16', 'VC16', '606', @type_average, 'Cell 16 Voltage', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 6, @datatype_Int16)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_CellTemp03', 'TC03', '607', @type_average, 'Cell 3 Temperature', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 6, @datatype_Int16)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_PCBTemp03', 'PB03', '607', @type_average, 'PCB 3 Temperature', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 4, @datatype_Int16)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_CMU03', 'CN03', '607', @type_none, 'CMU 3 Serial Number', '#,##0.00;#,##0.00', @datatype_UInt32, @chart_yes, 0, @datatype_UInt32)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_VoltageCell17', 'VC17', '608', @type_average, 'Cell 17 Voltage', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 0, @datatype_Int16)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_VoltageCell18', 'VC18', '608', @type_average, 'Cell 18 Voltage', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 2, @datatype_Int16)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_VoltageCell19', 'VC19', '608', @type_average, 'Cell 19 Voltage', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 4, @datatype_Int16)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_VoltageCell20', 'VC20', '608', @type_average, 'Cell 20 Voltage', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 6, @datatype_Int16)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_VoltageCell21', 'VC21', '609', @type_average, 'Cell 21 Voltage', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 0, @datatype_Int16)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_VoltageCell22', 'VC22', '609', @type_average, 'Cell 22 Voltage', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 2, @datatype_Int16)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_VoltageCell23', 'VC23', '609', @type_average, 'Cell 23 Voltage', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 4, @datatype_Int16)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_VoltageCell24', 'VC24', '609', @type_average, 'Cell 24 Voltage', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 6, @datatype_Int16)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_CellTemp04', 'TC04', '60A', @type_average, 'Cell 4 Temperature', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 6, @datatype_Int16)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_PCBTemp04', 'PB04', '60A', @type_average, 'PCB 4 Temperature', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 4, @datatype_Int16)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_CMU04', 'CN04', '60A', @type_none, 'CMU 4 Serial Number', '#,##0.00;#,##0.00', @datatype_UInt32, @chart_yes, 0, @datatype_UInt32)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_VoltageCell25', 'VC25', '60B', @type_average, 'Cell 25 Voltage', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 0, @datatype_Int16)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_VoltageCell26', 'VC26', '60B', @type_average, 'Cell 26 Voltage', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 2, @datatype_Int16)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_VoltageCell27', 'VC27', '60B', @type_average, 'Cell 27 Voltage', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 4, @datatype_Int16)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_VoltageCell28', 'VC28', '60B', @type_average, 'Cell 28 Voltage', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 6, @datatype_Int16)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_VoltageCell29', 'VC29', '60C', @type_average, 'Cell 29 Voltage', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 0, @datatype_Int16)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_VoltageCell30', 'VC30', '60C', @type_average, 'Cell 30 Voltage', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 2, @datatype_Int16)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_VoltageCell31', 'VC31', '60C', @type_average, 'Cell 31 Voltage', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 4, @datatype_Int16)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_VoltageCell32', 'VC32', '60C', @type_average, 'Cell 32 Voltage', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 6, @datatype_Int16)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_SOCPercent', 'SOCP', '6F4', @type_average, 'SOC (Percent)', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 4, @datatype_Float)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_SOCAh', 'SOCA', '6F4', @type_average, 'SOC (Ah)', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 0, @datatype_Float)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_BalanceSOCPercent', 'BSCP', '6F5', @type_average, 'Balance SOC (Percent)', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 4, @datatype_Float)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_BalanceSOCAh', 'BSCA', '6F5', @type_average, 'Balance SOC (Ah)', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 0, @datatype_Float)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_PackCapacity', 'PCAP', '6F6', @type_average, 'Pack Capacity (Ah)', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 6, @datatype_UInt16)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_DisCellVoltageErr', 'DVER', '6F6', @type_average, 'Discharging Cell Voltage Error (mV)', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 4, @datatype_Int16)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_CellTempMargin', 'CTMR', '6F6', @type_average, 'Cell Temperature Margin (1/10 deg C)', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 2, @datatype_Int16)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_ChargeCellVoltageErr', 'CVER', '6F6', @type_average, 'Charging Cell Voltage Error (mV)', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 0, @datatype_Int16)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_PrechargeTimer', 'PCT', '6F7', @type_average, 'Precharge Timer Count', '#,##0.00;#,##0.00', @datatype_Float, @chart_yes, 7, @datatype_Byte8)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_PrechargeTimerElapsed', 'PCTE', '6F7', @type_none, 'Precharge Timer Elapsed?', '#,##0.00;#,##0.00', @datatype_Byte8, @chart_yes, 6, @datatype_Byte8)	
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_ContactorVoltage', 'CVTG', '6F7', @type_average, '12V Contactor Supply Voltage', '#,##0.00;#,##0.00', @datatype_Float, @chart_yes, 2, @datatype_UInt16)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_PrechargeState', 'PCS', '6F7', @type_none, 'Precharge Status', '#,##0.00;#,##0.00', @datatype_Byte8, @chart_yes, 1, @datatype_Byte8)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_ContactorStatus', 'PCDS', '6F7', @type_none, 'Precharge Contactor Driver Status', '#,##0.00;#,##0.00', @datatype_Byte8, @chart_yes, 0, @datatype_Byte8)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_VoltageMax', 'VMAX', '6F8', @type_maximum, 'Maximum Cell Voltage (mV)', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 2, @datatype_UInt16)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_VoltageMin', 'VMIN', '6F8', @type_minimum, 'Minimum Cell Voltage (mV)', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 0, @datatype_UInt16)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_TempMax', 'TMAX', '6F9', @type_maximum, 'Maximum Cell Temperature', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 2, @datatype_UInt16)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_TempMin', 'TMIN', '6F9', @type_minimum, 'Minimum Cell Temperature', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 0, @datatype_UInt16)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_PackCurrent', 'CBAT', '6FA', @type_average, 'Battery Current', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 4, @datatype_Int32)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
					  VALUES ('BMS_PackVoltage', 'VBAT', '6FA', @type_average, 'Pack Voltage', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 0, @datatype_UInt32)
    INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_Firmware', 'BFRW', '6FB', @type_none, 'BMS Firmware Build Number', '#,##0.00;#,##0.00', @datatype_UInt16, @chart_yes, 6, @datatype_UInt16)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_CMUCount', 'BCCT', '6FB', @type_none, 'CMU Count', '#,##0.00;#,##0.00', @datatype_Byte8, @chart_yes, 5, @datatype_Byte8)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_PackStatusFlags', 'BPST', '6FB', @type_none, 'Pack Status Flags', '#,##0.00;#,##0.00', @datatype_Byte8, @chart_yes, 4, @datatype_Byte8)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_BalVoltageThreshFalling', 'BVTF', '6FB', @type_average, 'Balance Voltage Threshold - Falling', '#,##0.00;#,##0.00', @datatype_UInt16, @chart_yes, 2, @datatype_UInt16)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_BalVoltageThreshRising', 'BVTR', '6FB', @type_average, 'Balance Voltage Threshold - Rising', '#,##0.00;#,##0.00', @datatype_UInt16, @chart_yes, 0, @datatype_UInt16)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_CMUCurrentUse', 'BCMI', '6FC', @type_average, 'CMU 12V Current Consumption (mA)', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 6, @datatype_UInt16)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_FanCurrentUse', 'BCFI', '6FC', @type_average, 'Contactor/Fan 12V Current Consumption (mA)', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 4, @datatype_UInt16)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_FanSpeed1', 'BFS1', '6FC', @type_average, 'Fan Speed 1 (rpm)', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 2, @datatype_UInt16)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_FanSpeed0', 'BFS0', '6FC', @type_average, 'Fan Speed 0 (rpm)', '#,##0.00;#,##0.00', @datatype_Double, @chart_yes, 0, @datatype_UInt16)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_ModelID', 'BMID', '6FD', @type_none, 'BMU Model ID', '#,##0.00;#,##0.00', @datatype_Byte8, @chart_yes, 5, @datatype_Byte8)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_HardwareVersion', 'BHVR', '6FD', @type_none, 'BMU Hardware Version', '#,##0.00;#,##0.00', @datatype_Byte8, @chart_yes, 4, @datatype_Byte8)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('BMS_StatusFlags', 'BSFL', '6FD', @type_none, 'BMS Status Flags', '#,##0.00;#,##0.00', @datatype_UInt32, @chart_yes, 0, @datatype_UInt32)

	-- Steering wheel columns (base addr = 0x700)
	INSERT INTO tblDataItems (FieldName, Tag, CANTag, SummaryType, Description, DisplayFormat, DataType, NoCharting, CANByteOffset, CANDataType)
                      VALUES ('SW_Data', 'SWDF', '701', @type_none, 'Steering Wheel Data', '#,##0.00;#,##0.00', @datatype_Byte8, @chart_yes, 0, @datatype_Byte8)


 GO
 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sys_CreateHistoryTable]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sys_CreateHistoryTable]
GO

CREATE PROCEDURE sys_CreateHistoryTable
AS

    /* Create the telemetry history table based on the data definitions in tblDataItems */

DECLARE @FieldName          varchar(50),
        @DataType           varchar(50),
        @sql                varchar(max),
        @crlf               varchar(2)
        
    SET NOCOUNT ON
    
    /* Drop the table if it exists */
    
    IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblHistory]') AND type in (N'U'))
        DROP TABLE [dbo].[tblHistory]
        
    SELECT @crlf = CHAR(10) + CHAR(13)
    SELECT @sql = 'CREATE TABLE tblHistory (RowNum [int] IDENTITY(1,1) NOT NULL,' + @crlf +
                                           'TimeStamp DateTime NULL DEFAULT(GETDATE())'
    
    /* Loop through the defined fields, building up the field definition portion of the CREATE TABLE command */
    
    DECLARE fields CURSOR FAST_FORWARD FOR SELECT FieldName, SQLDataType FROM tblDataItems d JOIN tblValidDataType v ON D.DataType = v.ID
                                                  WHERE NOT (FieldName IN ('RowNum','TimeStamp'))
                                                  ORDER BY FieldName
    OPEN fields
    FETCH fields INTO @FieldName, @DataType
    WHILE (@@FETCH_STATUS = 0)
    BEGIN
        SELECT @sql = @sql + ','  + @crlf + '[' + @FieldName + '] ' + @DataType + ' NULL'
        FETCH fields INTO @FieldName, @DataType
    END
    CLOSE fields
    DEALLOCATE fields
    
    /* Add the primary key and index on the timestamp column */
    
    SELECT @sql = @sql + @crlf +'CONSTRAINT [PK_tblHistory] PRIMARY KEY CLUSTERED ([RowNum] ASC))' + @crlf +
                                'CREATE NONCLUSTERED INDEX [IDX_TimeStamp] ON [dbo].[tblHistory] ([TimeStamp] ASC)'  

    /* Create the table */
    
    EXEC(@sql)
GO
    
GO
 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[p_GetCANFields]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[p_GetCANFields]
GO

CREATE PROCEDURE p_GetCANFields
AS

    SET NOCOUNT ON
    
    /* Return all telemetry fields that are collectable from CAN */
    
    SELECT * FROM tblDataItems WHERE IsCanValue = 1
                               ORDER BY CANTag, CANByteOffset

GO