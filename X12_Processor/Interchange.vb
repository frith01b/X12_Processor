Option Strict On
Option Explicit On

Imports System.Data
Imports System.IO
Imports System.Reflection
Imports System.Xml

Public Class Interchange
    Shared ErrorState As Boolean = False
    Shared ErrorMsg() As String
    Shared ErrorCount As Integer
    Shared ErrorType As Error_Type_List

    Public Shared RecordCount As Long
    Public Shared RecordDelimiter As String
    Public Shared ElementDelimiter As String
    Public Shared SegmentDelimiter As String
    'Delimiter_Replace_Char:  character used to replace field data containing 
    'delimiters with, so records can be parsed properly later
    Public Shared Record_Delimiter_Replace_Char As String = "!"
    Public Shared ElementDelimiter_Replace_Char As String = "["
    Public Shared SegmentDelimiter_Replace_Char As String = "]"

    Public Shared CurrentConfigInfo As ConfigInfo
    Public Shared CurrentPartnerInfo As PartnerInfo
    Enum Error_Type_List
        Normal = 1
        Warning = 2
        StdError = 3
        Critical = 4
    End Enum
    Dim InputStream As IO.Stream

    Dim Myfilename As String
    Dim MyOutDir As String
    Public ExcelFile() As String
    Public ExcelFileCount As Long

    Private CurrentStore As String

    Private reader As StreamReader
    Private msg_header(500) As Char

    Private message As String

    Private X12SegDelimiter As String = "~"

    Private X12ElemDelimiter As Char = CChar("*")
    Private X12SubElemDelimiter As Char = CChar(">")
    Private rec_850_seq As Long
    Private rec_855_seq As Long
    Private rec_856_seq As Long
    Private rec_810_seq As Long

    Private replace_char As String = "-"

    Dim Fileptr As System.IO.StreamWriter
    Dim segments As IEnumerable(Of ParseSegment)

    Public valid_x12 As Boolean = True
    Public rec_type As String = "UNK"

    'input_file.Name, s_ErrorFile, s_ConfigFile, s_PartnerID)
    Public Sub New(filename As String, NewErrorfile As String, ConfigFileName As String, PartnerID As String)
        Dim pos As Integer
        Dim local_errorWriter As TextWriter

        local_errorWriter = Console.Error
        Dim InputSegDelimiter As String
        Dim InputElementDelimiter As String
        ' read config file
        CurrentConfigInfo = New ConfigInfo(ConfigFileName)
        'Load Partner info


        BAK.InitializeTranDef()

        reader = New StreamReader(filename)
        '
        reader.ReadBlock(msg_header, 0, 10)
        message = msg_header
        If message.Length > 3 Then
            If message.Substring(0, 3) = "ISA" Or message.Substring(1, 3) = "ISA" Then

                message = message & reader.ReadToEnd()
                X12SegDelimiter = message(105)
                X12ElemDelimiter = message(103)
                valid_x12 = True
                Try
                    segments = (From seg In message.Split(CChar(X12SegDelimiter)) Where Not String.IsNullOrEmpty(seg)
                                Select New ParseSegment(
                     seg.Substring(0, seg.IndexOf(X12ElemDelimiter)),
                    seg.Split(X12ElemDelimiter).Skip(1).ToArray()))

                Catch ex As Exception
                    Debug.Print("Inbound Error X12")
                    local_errorWriter.Write("ERROR in Split :" & ex.Message)
                    valid_x12 = False
                End Try
                pos = message.IndexOf(X12SegDelimiter + "ST" + X12ElemDelimiter)
                If pos > 0 Then
                    rec_type = message.Substring(pos + 4, 3)
                Else
                    valid_x12 = False
                End If
            Else
                ' not X12
                If message.Substring(0, 3) = """H""" Or message.Substring(1, 3) = """H""" Then
                    InputSegDelimiter = vbCrLf
                    InputElementDelimiter = "~"
                    Try
                        segments = (From seg In message.Split(CChar(InputSegDelimiter)) Where Not String.IsNullOrEmpty(seg)
                                    Select New ParseSegment(
                     seg.Substring(0, seg.IndexOf(InputElementDelimiter)),
                    seg.Split(X12ElemDelimiter).Skip(1).ToArray()))

                    Catch ex As Exception
                        Debug.Print("outbound error X12")
                        local_errorWriter.Write("ERROR in Split :" & ex.Message)
                    End Try
                    ' found an M2k X12 message
                    rec_type = segments(0).Elements(1).ToString
                    rec_type = rec_type.Replace("""", "")
                Else
                    valid_x12 = False
                End If
            End If
        Else
            valid_x12 = False
        End If


    End Sub

    Public Shared Sub AddError(NewErrorMsg As String, level As Error_Type_List)
        ErrorCount = ErrorCount + 1
        ReDim Preserve ErrorMsg(ErrorCount)
        ErrorMsg(ErrorCount - 1) = NewErrorMsg
        ErrorType = level
    End Sub
    Private Sub LoadConfig(ConfigFileName As String)
        Dim xmlFile As XmlReader
        xmlFile = XmlReader.Create(ConfigFileName, New XmlReaderSettings())
        Dim ds As New DataSet
        ds.ReadXml(xmlFile)
        Dim i As Integer
        For i = 0 To ds.Tables(0).Rows.Count - 1

        Next
    End Sub
    Private Sub LoadPartner(PartnerFileName As String)
        Dim xmlFile As XmlReader
        xmlFile = XmlReader.Create(PartnerFileName, New XmlReaderSettings())
        Dim ds As New DataSet
        ds.ReadXml(xmlFile)
        Dim i As Integer
        For i = 0 To ds.Tables(0).Rows.Count - 1

        Next
    End Sub

End Class
