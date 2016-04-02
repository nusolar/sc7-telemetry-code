Imports System
Imports System.Runtime.InteropServices
Imports System.Data.SqlClient

<StructLayout(LayoutKind.Explicit)> _
Public Class cCANData
    <FieldOffset(0)> Private _AsUInt64 As System.UInt64
    <FieldOffset(0)> Private _AsInt64 As System.Int64
    <FieldOffset(0)> Private _AsUInt32_0 As System.UInt32
    <FieldOffset(4)> Private _AsUInt32_1 As System.UInt32
    <FieldOffset(0)> Private _AsInt32_0 As System.Int32
    <FieldOffset(4)> Private _AsInt32_1 As System.Int32
    <FieldOffset(0)> Private _AsUInt16_0 As System.UInt16
    <FieldOffset(2)> Private _AsUInt16_1 As System.UInt16
    <FieldOffset(4)> Private _AsUInt16_2 As System.UInt16
    <FieldOffset(6)> Private _AsUInt16_3 As System.UInt16
    <FieldOffset(0)> Private _AsInt16_0 As System.Int16
    <FieldOffset(2)> Private _AsInt16_1 As System.Int16
    <FieldOffset(4)> Private _AsInt16_2 As System.Int16
    <FieldOffset(6)> Private _AsInt16_3 As System.Int16
    <FieldOffset(0)> Private _AsSingle_0 As System.Single
    <FieldOffset(4)> Private _AsSingle_1 As System.Single
    <FieldOffset(0)> Private _AsDouble As System.Double
    <FieldOffset(0)> Private _AsByte_0 As System.Byte
    <FieldOffset(1)> Private _AsByte_1 As System.Byte
    <FieldOffset(2)> Private _AsByte_2 As System.Byte
    <FieldOffset(3)> Private _AsByte_3 As System.Byte
    <FieldOffset(4)> Private _AsByte_4 As System.Byte
    <FieldOffset(5)> Private _AsByte_5 As System.Byte
    <FieldOffset(6)> Private _AsByte_6 As System.Byte
    <FieldOffset(7)> Private _AsByte_7 As System.Byte
#Region "Public Properties"
    Public ReadOnly Property AsUInt64 As System.UInt64
        Get
            Return _AsUInt64
        End Get
    End Property
    Public ReadOnly Property AsInt64 As System.Int64
        Get
            Return _AsInt64
        End Get
    End Property
    Public ReadOnly Property AsUInt32 As System.UInt32()
        Get
            Return ({_AsUInt32_0, _AsUInt32_1})
        End Get
    End Property
    Public ReadOnly Property AsInt32 As System.Int32()
        Get
            Return ({_AsInt32_0, _AsInt32_1})
        End Get
    End Property
    Public ReadOnly Property AsUInt16 As System.UInt16()
        Get
            Return ({_AsUInt16_0, _AsUInt16_1, _AsUInt16_2, _AsUInt16_3})
        End Get
    End Property
    Public ReadOnly Property AsInt16 As System.Int16()
        Get
            Return ({_AsInt16_0, _AsInt16_1, _AsInt16_2, _AsInt16_3})
        End Get
    End Property
    Public ReadOnly Property AsSingle As System.Single()
        Get
            Return ({_AsSingle_0, _AsSingle_1})
        End Get
    End Property
    Public ReadOnly Property AsDouble As System.Double
        Get
            Return _AsDouble
        End Get
    End Property
    Public ReadOnly Property AsByte As System.Byte()
        Get
            Return ({_AsByte_0, _AsByte_1, _AsByte_2, _AsByte_3, _AsByte_4, _AsByte_5, _AsByte_6, _AsByte_7})
        End Get
    End Property

#End Region
#Region "Constructors"
    Sub New()
        MyBase.New()
    End Sub
    Sub New(ByVal AsUint64 As System.UInt64)
        MyBase.New()
        _AsUInt64 = AsUint64
    End Sub
    Sub New(ByVal AsHEX As String)
        MyBase.New()
        _AsUInt64 = Convert.ToUInt64(AsHEX, 16)
    End Sub
#End Region
End Class

Public Class cDataField

    Public Enum SummaryTypes As Integer
        None
        Average
        Minimum
        Maximum
    End Enum

    Public Enum DataTypes As Integer
        System_Int16 = 1
        System_Int32
        System_Double
        System_Decimal
        System_Date_Time
        System_Float
        System_UInt16
        System_UInt32
        System_UInt64
        System_Byte8
    End Enum

    Private _ID As Integer = 0
    Private _FieldName As String = ""
    Private _Tag As String = ""
    Private _CANTag As String = ""
    Private _CANByteOffset As Integer = 0
    Private _CANDataType As DataTypes = DataTypes.System_Byte8
    Private _IsCANValue As Boolean = False
    Private _SummaryType As SummaryTypes = SummaryTypes.None
    Private _Description As String = ""
    Private _DisplayFormat As String = ""
    Private _DataType As DataTypes = DataTypes.System_Int16
    Private _NoCharting As Boolean = False
    '
    Private _DataItemCount As Long = 0
    Private _DataItemValue As Nullable(Of Decimal)

#Region "Constructors"
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal dr As SqlDataReader)
        MyBase.New()
        DataReaderToClass(dr)
    End Sub
#End Region
#Region "Public Properties"
    Public ReadOnly Property FieldName As String
        Get
            FieldName = _FieldName
        End Get
    End Property
    Public ReadOnly Property Tag As String
        Get
            Tag = _Tag
        End Get
    End Property
    Public ReadOnly Property CANTag As String
        Get
            CANTag = _CANTag
        End Get
    End Property
    Public ReadOnly Property CANByteOffset As Integer
        Get
            CANByteOffset = _CANByteOffset
        End Get
    End Property
    Public ReadOnly Property CANDataType As Integer
        Get
            CANDataType = _CANDataType
        End Get
    End Property
    Public ReadOnly Property IsCANValue As String
        Get
            IsCANValue = _IsCANValue
        End Get
    End Property
    Public ReadOnly Property SummaryType As SummaryTypes
        Get
            SummaryType = _SummaryType
        End Get
    End Property
    Public ReadOnly Property Description As String
        Get
            Description = _Description
        End Get
    End Property
    Public ReadOnly Property DisplayFormat As String
        Get
            DisplayFormat = _DisplayFormat
        End Get
    End Property
    Friend ReadOnly Property DataType As DataTypes
        Get
            DataType = _DataType
        End Get
    End Property
    Public ReadOnly Property NoCharting As Boolean
        Get
            NoCharting = _NoCharting
        End Get
    End Property
    Public WriteOnly Property NewDataValue As cCANData
        Set(value As cCANData)
            If _DataItemValue Is Nothing Then
                _DataItemValue = 0
            End If
            Dim localvalue As Decimal
            Select Case CANDataType
                Case DataTypes.System_Float
                    localvalue = value.AsSingle(CANByteOffset / 4)
                Case DataTypes.System_Double
                    localvalue = value.AsDouble
                Case DataTypes.System_UInt16
                    localvalue = value.AsUInt16(CANByteOffset / 2)
                Case DataTypes.System_UInt32
                    localvalue = value.AsUInt32(CANByteOffset / 4)
                Case DataTypes.System_UInt64
                    localvalue = value.AsUInt64
                Case DataTypes.System_Int16
                    localvalue = value.AsInt16(CANByteOffset / 2)
                Case DataTypes.System_Int32
                    localvalue = value.AsInt32(CANByteOffset / 4)
                Case DataTypes.System_Byte8
                    localvalue = value.AsByte(CANByteOffset)
            End Select
            Select Case SummaryType
                Case SummaryTypes.Average
                    _DataItemCount += 1
                    _DataItemValue += localvalue
                Case SummaryTypes.Minimum
                    If localvalue < _DataItemValue Then
                        _DataItemValue = localvalue
                    End If
                Case SummaryTypes.Maximum
                    If localvalue > _DataItemValue Then
                        _DataItemValue = localvalue
                    End If
                Case Else
                    _DataItemValue = localvalue
            End Select
        End Set
    End Property
    Public ReadOnly Property DataValue As Nullable(Of Decimal)
        Get
            Select Case SummaryType
                Case SummaryTypes.Average
                    If (Not _DataItemValue Is Nothing) AndAlso _DataItemCount <> 0 Then
                        DataValue = _DataItemValue / _DataItemCount
                    Else
                        DataValue = _DataItemValue
                    End If
                Case Else
                    DataValue = _DataItemValue
            End Select
        End Get
    End Property
    Public ReadOnly Property DataValueAsString As String
        Get
            Dim value As Nullable(Of Decimal) = DataValue
            If value Is Nothing Then
                DataValueAsString = "NULL"
            Else
                DataValueAsString = value.ToString
            End If
        End Get
    End Property
#End Region
#Region "Public Methods"
    Public Sub DataReaderToClass(ByVal dr As SqlDataReader)
        _ID = dr("ID")
        _FieldName = dr("FieldName")
        _Tag = dr("Tag")
        _CANTag = dr("CANTag")
        _CANByteOffset = dr("CANByteOffset")
        _CANDataType = dr("CANDataType")
        _IsCANValue = dr("IsCANValue")
        _SummaryType = dr("SummaryType")
        _Description = dr("Description")
        _DisplayFormat = dr("DisplayFormat")
        _DataType = dr("DataType")
        _NoCharting = dr("NoCharting")
    End Sub
    Public Sub Reset()
        _DataItemCount = 0
        _DataItemValue = Nothing
    End Sub
#End Region
End Class
