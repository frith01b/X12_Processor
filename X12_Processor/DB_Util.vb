Option Strict On
Option Explicit On

Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Data.Odbc
''' Version 2.0
'''Imports System.ServiceProcess
''' example connection strings.
'''    Const s_conn As String = "Server=ServerName.prod.corp.com;database=OneTouchBE;UID=Domain\serviceAcct;PWD=********;Trusted Connection=false"
'''Const s_conn_iseries As String = "DSN=PROD_UC;DBQ=QGPL;UID=svcAcct;PWD=********;CommandTimeout=120;executionTimeout=240"
'''
''' Example creation statement 
'''        Dim db_Sql As SQL = New SQL(s_conn, tw_errorWriter)
'''
''' if failure,  app should check ErrorEx   for SQLException Info.
'''*******************************************************************************

Public Class DB_Util
    Implements IDisposable

    Enum DB_TYPE
        MSSQL = 1
        ODBC = 2
    End Enum

    Private s_conn As String
    ''' 1 or higher = print debug messages, 0 = no additional mesgs
    Private DEBUG_FLAG As Integer = 0

    WithEvents sql_con As SqlConnection
    WithEvents db2_con As OdbcConnection

    Dim sql_errorWriter As TextWriter
    Public e_CurrDbType As DB_TYPE

    Dim ds_ActiveDataset As New DataSet()

    Private _RowProcessor As [Delegate]
    Public Delegate Sub RowMethod(ByRef NewData As Data.DataTable)
    Public ErrorEx As SqlException

    Private paramCmd As SqlCommand

    '''***************************************************************************************

    Public Sub New(ByVal s_db_conn As String, ByVal NewDbType As DB_TYPE, ByRef my_errorWriter As TextWriter)
        s_conn = s_db_conn
        e_CurrDbType = NewDbType

        sql_errorWriter = my_errorWriter
        OpenDatabase()

    End Sub
    '''***************************************************************************************
    ''' substituted function must match DoRowMethod Method Signature exactly.

    Public Sub DoRowMethod(NewData As DataTable)
        'check to see if there is anything to Invoke
        If _RowProcessor <> Nothing Then
            Try
                _RowProcessor.DynamicInvoke(NewData)
            Catch ex As SqlException
                sql_errorWriter.WriteLine(ex.Message)
                ErrorEx = ex
            End Try

        End If
    End Sub
    '''***************************************************************************************
    Public Sub AddRowProcessor(ByVal MyMethod As RowMethod)
        _RowProcessor = MulticastDelegate.Combine(_RowProcessor, MyMethod)
    End Sub
    '''***************************************************************************************
    Public Sub RemoveProcessor(ByVal MyMethod As RowMethod)
        _RowProcessor = MulticastDelegate.Remove(_RowProcessor, MyMethod)
    End Sub

    '''***************************************************************************************
    '''Generic Version
    Function OpenDatabase() As Boolean
        If e_CurrDbType = DB_TYPE.MSSQL Then
            Return OpenDatabase(sql_con)
        End If
        If e_CurrDbType = DB_TYPE.ODBC Then
            Return OpenDatabase(db2_con)
        End If
        Return False
    End Function
    '''***************************************************************************************
    ''' MS SQL Version
    Function OpenDatabase(ByRef db_con As SqlConnection) As Boolean
        Dim ErrorVal As Boolean
        ErrorVal = True
        Try
            db_con = New SqlConnection(s_conn)
            db_con.Open()
        Catch ex As SqlException
            sql_errorWriter.WriteLine("Unable to Connect to Database " & s_conn & vbCrLf & ex.Message)
            ErrorVal = False
            ErrorEx = ex
        End Try

        Return ErrorVal

    End Function
    '''***************************************************************************************
    ''' ODBC Version 
    Function OpenDatabase(ByRef db_con As OdbcConnection) As Boolean
        Dim ErrorVal As Boolean
        ErrorVal = True
        Try
            db_con = New OdbcConnection(s_conn)
            db_con.ConnectionTimeout = 360
            db_con.Open()
        Catch ex As SqlException
            sql_errorWriter.WriteLine("Unable to Connect to ODBC Database " & s_conn & vbCrLf & ex.Message)
            ErrorVal = False
            ErrorEx = ex
        End Try

        Return ErrorVal

    End Function
    '''***************************************************************************************
    Function GatherData(sql As String, ByRef ds As DataSet) As Boolean
        Dim RetVal As Boolean = True

        If e_CurrDbType = DB_TYPE.MSSQL Then
            RetVal = GatherData(sql, ds, sql_con)
        End If
        If e_CurrDbType = DB_TYPE.ODBC Then
            RetVal = GatherData(sql, ds, db2_con)
        End If
        Return RetVal
    End Function

    '''***************************************************************************************
    Function GatherData(sql As String, ByRef ds As DataSet, ByRef db_con As SqlConnection) As Boolean
        Dim ErrorVal As Boolean = True
        Dim cmd As New SqlCommand(sql, db_con)
        Dim da As New SqlDataAdapter(cmd)
        da.SelectCommand.CommandTimeout = 360

        If (DEBUG_FLAG > 0) Then
            sql_errorWriter.WriteLine("Gather data sql " & vbCrLf & sql)
        End If
        Try
            da.Fill(ds)
        Catch ex As SqlException
            sql_errorWriter.WriteLine("Query failed " & sql & vbCrLf & ex.Message)
            ErrorVal = False
            ErrorEx = ex
        End Try

        If (DEBUG_FLAG > 0) Then
            sql_errorWriter.WriteLine("end Gather data sql " & ds.Tables(0).Rows.Count)
        End If

        Return ErrorVal

    End Function
    '''***************************************************************************************
    Function GatherData(sql As String, ByRef ds As DataSet, ByRef db_con As OdbcConnection) As Boolean
        Dim ErrorVal As Boolean = True

        If (DEBUG_FLAG > 0) Then
            sql_errorWriter.WriteLine("Gather data odbc " & vbCrLf & sql)
        End If

        Dim dtAdapter As System.Data.Odbc.OdbcDataAdapter
        dtAdapter = New System.Data.Odbc.OdbcDataAdapter(sql, db_con)
        dtAdapter.SelectCommand.CommandTimeout = 3600
        Try
            dtAdapter.Fill(ds)
        Catch ex As SqlException
            sql_errorWriter.WriteLine("ODBC Query failed " & sql & vbCrLf & ex.Message)
            ErrorVal = False
            ErrorEx = ex
        End Try

        If (DEBUG_FLAG > 0) Then
            sql_errorWriter.WriteLine("end Gather data sql " & ds.Tables(0).Rows.Count)
        End If

        Return ErrorVal

    End Function
    ' execute prepped & loaded param data gather
    Function GatherData_Execute(ByRef ds As DataSet) As Boolean
        Dim ErrorVal As Boolean = True

        Dim da As New SqlDataAdapter(paramCmd)

        da.SelectCommand.CommandTimeout = 360

        If (DEBUG_FLAG > 0) Then
            sql_errorWriter.WriteLine("Gather data sql " & vbCrLf & paramCmd.CommandText)
        End If
        Try
            da.Fill(ds)
        Catch ex As SqlException
            sql_errorWriter.WriteLine("Query failed " & paramCmd.CommandText & vbCrLf & ex.Message)
            ErrorVal = False
            ErrorEx = ex
        End Try

        If (DEBUG_FLAG > 0) Then
            sql_errorWriter.WriteLine("end Gather data sql " & ds.Tables(0).Rows.Count)
        End If

        Return ErrorVal

    End Function

    '''***************************************************************************************

    Function UpdateData(sql As String) As Integer
        Dim RetVal As Integer = 0

        If e_CurrDbType = DB_TYPE.MSSQL Then
            RetVal = UpdateData(sql, sql_con)
        End If
        If e_CurrDbType = DB_TYPE.ODBC Then
            RetVal = UpdateData(sql, db2_con)
        End If

        Return RetVal

    End Function


    '''***************************************************************************************

    Function UpdateData(sql As String, db_con As SqlConnection) As Integer
        Dim retVal As Integer = -1
        Dim UpdateCmd As New SqlCommand(sql, db_con)
        UpdateCmd.CommandTimeout = 3600

        Try
            retVal = UpdateCmd.ExecuteNonQuery()
        Catch ex As SqlException
            sql_errorWriter.WriteLine("Error: " & ex.ToString())
            ErrorEx = ex
        End Try


        Return retVal

    End Function
    '''***************************************************************************************

    Function UpdateData(sql As String, db_con As OdbcConnection) As Integer
        Dim retVal As Integer = -1
        Dim UpdateCmd As New OdbcCommand(sql, db_con)
        UpdateCmd.CommandTimeout = 3600

        Try
            retVal = UpdateCmd.ExecuteNonQuery()
        Catch ex As SqlException
            sql_errorWriter.WriteLine("Error: " & ex.ToString())
            ErrorEx = ex
        End Try

        Return retVal

    End Function
    '''***************************************************************************************
    Function ProcessSqlByRow(sql As String) As Boolean
        Dim RetVal As Boolean = True

        If e_CurrDbType = DB_TYPE.MSSQL Then
            RetVal = ProcessSqlByRow(sql, sql_con)
        End If
        If e_CurrDbType = DB_TYPE.ODBC Then
            RetVal = ProcessSqlByRow(sql, db2_con)
        End If

        Return RetVal
    End Function

    '''***************************************************************************************
    Function ProcessSqlByRow(sql As String, ByRef db_con As SqlConnection) As Boolean
        Dim RetVal As Boolean = True
        Dim RecordTable As New DataTable()

        Using Com As New SqlCommand(sql, db_con)
            Using RDR = Com.ExecuteReader()
                If RDR.HasRows Then
                    Do While Not RDR.IsClosed
                        RDR.Read()
                        RecordTable.Load(RDR)
                        DoRowMethod(RecordTable)
                        RecordTable.Clear()
                    Loop
                End If
            End Using
        End Using
        Return RetVal

    End Function

    '''***************************************************************************************
    Function ProcessSqlByRow(sql As String, ByRef db_con As OdbcConnection) As Boolean
        Dim RetVal As Boolean = True

        Dim RecordTable As New DataTable()

        Using Com As New OdbcCommand(sql, db_con)
            Using RDR = Com.ExecuteReader()
                If RDR.HasRows Then
                    Do While RDR.Read
                        RecordTable.Load(RDR)
                        DoRowMethod(RecordTable)
                        RecordTable.Clear()
                    Loop
                End If
            End Using
        End Using
        Return RetVal
    End Function

    '''***************************************************************************************


    Public Sub CloseDatabase()
        If e_CurrDbType = DB_TYPE.MSSQL Then
            sql_con.Close()
        End If
        If e_CurrDbType = DB_TYPE.ODBC Then
            db2_con.Close()
        End If

    End Sub
    '***************************************************************************************

#Region "IDisposable Support"
    ''' To detect redundant calls
    Private disposedValue As Boolean

    ''' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue AndAlso disposing Then

            ' TODO: dispose managed state (managed objects).

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ''' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
    '''***************************************************************************************

    Function RunStoredProc(sql As String) As Integer
        Dim RetVal As Integer = 0

        If e_CurrDbType = DB_TYPE.MSSQL Then
            RetVal = RunStoredProc(sql, sql_con)
        End If
        If e_CurrDbType = DB_TYPE.ODBC Then
            RetVal = RunStoredProc(sql, db2_con)
        End If

        Return RetVal

    End Function


    '''***************************************************************************************

    Function RunStoredProc(sql As String, db_con As SqlConnection) As Integer
        Dim retVal As Integer = -1
        Dim UpdateCmd As New SqlCommand(sql, db_con)
        UpdateCmd.CommandType = CommandType.StoredProcedure

        UpdateCmd.CommandTimeout = 3600

        Try
            retVal = UpdateCmd.ExecuteNonQuery()
        Catch ex As SqlException
            sql_errorWriter.WriteLine("Error: " & ex.ToString())
            ErrorEx = ex
        End Try


        Return retVal

    End Function
    '''***************************************************************************************

    Function RunStoredProc(sql As String, db_con As OdbcConnection) As Integer
        Dim retVal As Integer = -1
        Dim UpdateCmd As New OdbcCommand(sql, db_con)
        UpdateCmd.CommandType = CommandType.StoredProcedure

        UpdateCmd.CommandTimeout = 3600

        Try
            retVal = UpdateCmd.ExecuteNonQuery()
        Catch ex As SqlException
            sql_errorWriter.WriteLine("Error: " & ex.ToString())
            ErrorEx = ex
        End Try

        Return retVal

    End Function
    '''***************************************************************************************
    '''needs adaptation for odbc

    Sub PrepParamCmd(sqlstr As String)
        paramCmd = New SqlCommand(sqlstr, sql_con)
    End Sub

    Sub AddParam(pname As String, ptype As SqlDbType, pvalue As String)
        paramCmd.Parameters.Add(pname, ptype, pvalue.Length).Value = pvalue
    End Sub
    Sub AddParam(pname As String, ptype As SqlDbType, pvalue As Date)
        paramCmd.Parameters.Add(pname, ptype, 20).Value = pvalue
    End Sub
    Sub AddParam(pname As String, ptype As SqlDbType, pvalue As Decimal)
        paramCmd.Parameters.Add(pname, ptype, 16).Value = pvalue
    End Sub

    Function ParamCmd_Execute() As String
        Dim retmsg As String = ""
        Try
            paramCmd.ExecuteScalar()
        Catch ex As Exception
            Debug.Print("Error - " & ex.Message)
            retmsg = ex.Message
        End Try
        Return retmsg
    End Function
    Sub ParamCmd_Clear()
        paramCmd.Parameters.Clear()
    End Sub
#End Region

End Class

