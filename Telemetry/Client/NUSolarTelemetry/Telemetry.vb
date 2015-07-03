Public Class Telemetry
    Const k_Delimeter As String = ","
    Private Class DataValue
        Private _value As Object
        Private _MyType As String
        Public Name As String = ""
        Public Description As String = ""
        Public DisplayFormat As String = ""
        Public Tag As String = ""
        Public NoCharting As Boolean = False
        Public NoSave As Boolean = False
        Public ChartMe As Boolean = False
        Public Offset As Double = 0
        Public Scale As Double = 1
        Public Property Value As Object
            Get
                Select Case _MyType
                    Case "System.Double"
                        'If Scale <> 1 And CType(_value, System.Double) <> 0 Then Stop
                        Value = CType((CType(_value, System.Double) + Offset) * Scale, System.Double)
                    Case "System.Int16"
                        'If Scale <> 1 And CType(_value, System.Int16) <> 0 Then Stop
                        Value = CType((CType(_value, System.Int16) + Offset) * Scale, System.Int16)
                    Case "System.Int32"
                        'If Scale <> 1 And CType(_value, System.Int32) <> 0 Then Stop
                        Value = CType((CType(_value, System.Int32) + Offset) * Scale, System.Int32)
                    Case Else
                        Value = _value
                End Select
            End Get
            Set(ByVal value As Object)
                If IsDBNull(value) Then
                    Select Case _MyType
                        Case "System.DateTime"
                            _value = Date.MinValue
                        Case "System.Double"
                            _value = CType(-1 * Offset, System.Double)
                        Case "System.TimeSpan"
                            _value = TimeSpan.MinValue
                        Case "System.Int16"
                            _value = CType(-1 * Offset, System.Int16)
                        Case "System.Int32"
                            _value = CType(-1 * Offset, System.Int32)
                        Case "System.String"
                            _value = ""
                        Case Else
                            _value = Nothing
                    End Select
                Else
                    Select Case _MyType
                        Case "System.DateTime"
                            _value = CType(value.ToString, System.DateTime)
                        Case "System.Double"
                            _value = CType(value, System.Double)
                        Case "System.TimeSpan"
                            _value = System.TimeSpan.Parse(value.ToString)
                        Case "System.Int16"
                            _value = CType(value, System.Int16)
                        Case "System.Int32"
                            _value = CType(value, System.Int32)
                        Case "System.String"
                            _value = CType(value, System.String)
                        Case Else
                            _value = Nothing
                    End Select
                End If
            End Set
        End Property
        Public ReadOnly Property ValueAsString As String
            Get
                Select Case _MyType
                    Case "System.DateTime"
                        ValueAsString = CType(_value, System.DateTime).ToString("yyyy-MM-dd HH:mm:ss")
                    Case "System.Double"
                        ValueAsString = CType(_value, System.Double).ToString
                    Case "System.TimeSpan"
                        ValueAsString = CType(_value, System.TimeSpan).ToString
                    Case "System.Int16"
                        ValueAsString = CType(_value, System.Int16).ToString
                    Case "System.Int32"
                        ValueAsString = CType(_value, System.Int32).ToString
                    Case "System.String"
                        ValueAsString = CType(_value, System.String).ToString
                    Case Else
                        ValueAsString = _value.ToString
                End Select
            End Get
        End Property
        Sub New(ByVal MyName As String, ByVal MyType As String)
            Name = MyName
            _MyType = MyType
        End Sub
    End Class
    Private _DataValues As New Collection
    Public Event ChartSelectionsChanged(ByVal Parameter As String, ByVal ChartParameter As Boolean)
    Public NewData As Boolean = False
    Public Sub New()
        _DataValues = New Collection
    End Sub
    Public Sub New(ByVal Definition As String)
        Dim _datavalue As DataValue = Nothing
        Dim _doc As New Xml.XmlDocument
        _doc.LoadXml(Definition)
        _DataValues = New Collection
        For Each group As Xml.XmlNode In _doc.GetElementsByTagName("dataitems")
            If (group.Attributes IsNot Nothing) Then 'needed for comments in the XML
                For Each child As Xml.XmlNode In group.ChildNodes
                    If (child.Attributes IsNot Nothing) Then 'needed for comments in the XML
                        If (child.Attributes("name") IsNot Nothing) Then
                            _datavalue = New DataValue(child.Attributes("name").InnerXml, child.Attributes("type").InnerXml)
                            _datavalue.DisplayFormat = child.Attributes("format").InnerXml
                            _datavalue.Description = child.Attributes("description").InnerXml
                            _datavalue.Tag = child.Attributes("tag").InnerXml
                            _datavalue.NoCharting = CType(child.Attributes("nocharting").InnerXml, Boolean)
                            _datavalue.NoSave = CType(child.Attributes("nosave").InnerXml, Boolean)
                            If (child.Attributes("scale") IsNot Nothing) Then
                                _datavalue.Scale = CType(child.Attributes("scale").InnerXml, Double)
                            End If
                            If (child.Attributes("offset") IsNot Nothing) Then
                                _datavalue.Offset = CType(child.Attributes("offset").InnerXml, Double)
                            End If
                            _DataValues.Add(_datavalue, _datavalue.Name)
                        End If
                    End If
                Next
            End If
        Next
    End Sub
    Public ReadOnly Property PropertyAttribute(ByVal PropertyNumber As Long, ByVal AttributeName As String) As Object
        Get
            PropertyAttribute = Nothing
            Select Case AttributeName
                Case "Parameter"
                    PropertyAttribute = CType(_DataValues(PropertyNumber + 1), DataValue).Name
                Case "Value"
                    If CType(_DataValues(PropertyNumber + 1), DataValue).DisplayFormat <> "" Then
                        PropertyAttribute = Format(CType(_DataValues(PropertyNumber + 1), DataValue).Value, CType(_DataValues(PropertyNumber + 1), DataValue).DisplayFormat)
                    Else
                        PropertyAttribute = CType(_DataValues(PropertyNumber + 1), DataValue).Value
                    End If
                Case "Chart"
                    PropertyAttribute = CType(_DataValues(PropertyNumber + 1), DataValue).ChartMe
            End Select
        End Get
    End Property
    Public Property PropertyValue(ByVal PropertyName As String) As Object
        Get
            PropertyValue = CType(_DataValues(PropertyName), DataValue).Value
        End Get
        Set(ByVal value As Object)
            CType(_DataValues(PropertyName), DataValue).Value = value
        End Set
    End Property
    Public Property DisplayFormat(ByVal PropertyName As String) As String
        Get
            DisplayFormat = CType(_DataValues(PropertyName), DataValue).DisplayFormat
        End Get
        Set(ByVal value As String)
            CType(_DataValues(PropertyName), DataValue).DisplayFormat = value
        End Set
    End Property
    Public Property ChartMe(ByVal PropertyName As String) As Boolean
        Get
            ChartMe = CType(_DataValues(PropertyName), DataValue).ChartMe
        End Get
        Set(ByVal value As Boolean)
            CType(_DataValues(PropertyName), DataValue).ChartMe = value
            RaiseEvent ChartSelectionsChanged(PropertyName, value)
        End Set
    End Property
    Public ReadOnly Property FieldCount As Integer
        Get
            FieldCount = _DataValues.Count
        End Get
    End Property
    Public WriteOnly Property DataRowToClass() As DataRow
        Set(value As DataRow)
            For i As Integer = value.Table.Columns("ts1").Ordinal To value.Table.Columns.Count - 1
                PropertyValue(value.Table.Columns(i).Caption) = value.Item(i)
            Next
        End Set
    End Property
    Public Property Serialized() As String
        Get
            Serialized = k_Delimeter
            For Each o As DataValue In _DataValues
                Serialized &= o.Tag & "=" & o.ValueAsString & k_Delimeter
            Next
        End Get
        Set(value As String)
            Dim i As Integer
            Dim s As String
            For Each o As DataValue In _DataValues
                i = value.IndexOf(o.Tag & "=")
                If i > -1 Then
                    i += o.Tag.Length + 1
                    Try
                        s = value.Substring(i, value.IndexOf(k_Delimeter, i) - i)
                        o.Value = IIf(s = "NULL", DBNull.Value, s)
                    Catch
                    End Try
                End If
            Next
        End Set
    End Property
    Public Sub InsertRow(ByVal Connection As System.Data.Odbc.OdbcConnection)
        Dim sql As String = "INSERT INTO History ("
        For Each o As DataValue In _DataValues
            If Not o.NoSave Then
                sql &= o.Name & ","
            End If
        Next
        sql = sql.Remove(sql.Length - 1)
        sql &= ") VALUES ("
        For Each o As DataValue In _DataValues
            If Not o.NoSave Then
                sql &= "'" & o.ValueAsString & "',"
            End If
        Next
        sql = sql.Remove(sql.Length - 1)
        sql &= ")"
        Dim cmd As New Odbc.OdbcCommand(sql)
        With cmd
            .Connection = Connection
            .CommandType = CommandType.Text
            .ExecuteNonQuery()
        End With

    End Sub
End Class
