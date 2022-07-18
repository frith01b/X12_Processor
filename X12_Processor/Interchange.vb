Option Strict On
Option Explicit On

Imports System.Data
Imports System.IO
Imports System.Reflection
Imports System.Xml
Imports X12_Processor



Public Class Interchange
    Public Shared debugflag As Boolean = False
    Public Shared hasErrors As Boolean = False
    Public Shared ErrorMsg() As String
    Public Shared ErrorCount As Integer
    Public Shared ErrorType As Error_Type_List = Error_Type_List.Normal

    Public Shared RecordCount As Long
    Public Shared RecordDelimiter As String = "~"
    Public Shared FieldDelimiter As Char = CChar("*")
    Public Shared SubFieldDelimiter As Char = CChar(">")

    'Delimiter_Replace_Char:  character used to replace field data containing 
    'delimiters with, so records can be parsed properly later
    Public Shared Record_Delimiter_Replace_Char As String = "!"
    Public Shared FieldDelimiter_Replace_Char As String = "["
    Public Shared SubFieldDelimiter_Replace_Char As String = "]"

    Public Shared CurrentConfigInfo As ConfigInfo
    Public Shared CurrentPartnerInfo As PartnerInfo
    Public Shared MoreFiles As Boolean = False
    Public Enum Error_Type_List
        Normal = 1
        Warning = 2
        StdError = 3
        Critical = 4
    End Enum

    Public ExcelFile() As String
    Public ExcelFileCount As Long
    Public valid_x12 As Boolean = True
    Public rec_type As String = "UNK"
    Public CurrentRecordset As RecordSet

    Dim InputStream As IO.Stream
    Dim ProcessFileList As List(Of FileInfo) = New List(Of FileInfo)
    Dim Myfilename As String
    Dim MyOutDir As String
    Private CurrentStore As String
    Private reader As StreamReader
    Private msg_header(500) As Char

    Private message As String

    Private rec_850_seq As Long
    Private rec_855_seq As Long
    Private rec_856_seq As Long
    Private rec_810_seq As Long

    Private replace_char As String = "-"

    Dim Fileptr As System.IO.StreamWriter
    ' un-processed record and field separated initial data from file
    Dim segments As IEnumerable(Of ParseSegment)


    Dim di As IO.DirectoryInfo
    Dim singlefile As IO.FileInfo
    Dim local_errorWriter As TextWriter
    '*********************END OF CLASS VARIABLES ***************************

    'input_file.Name, s_ErrorFile, s_ConfigFile, s_PartnerID)
    Public Sub New(dict_Arguments As Dictionary(Of String, String))

        Dim SaveDefault As Boolean = False
        ' Processing directory
        Dim s_baseDir As String = ""
        ' destination dir
        Dim s_OutDir As String = ""
        ' exceptions file name
        Dim s_ErrorFile As String = ""
        ' interchange ID for remote system
        Dim s_PartnerID As String = ""
        '  processing options file
        Dim s_ConfigFile As String = ""
        ' optional input file name
        Dim s_InFile As String = ""
        dict_Arguments.TryGetValue("CONFIGFILE", s_ConfigFile)
        If s_ConfigFile IsNot Nothing Then
            ' read config file
            CurrentConfigInfo = ConfigInfo.FromFile(s_ConfigFile)

        End If
        If CurrentConfigInfo Is Nothing Then
            CurrentConfigInfo = New ConfigInfo
            SaveDefault = True
        End If

        ' arguments can over-ride config file vales
        dict_Arguments.TryGetValue("OUTDIR", s_OutDir)
        If s_OutDir IsNot Nothing Then
            CurrentConfigInfo.Output_Dir = s_OutDir
        End If

        dict_Arguments.TryGetValue("PARTNERID", s_PartnerID)
        If s_PartnerID IsNot Nothing Then
            CurrentConfigInfo.Partner_ID = s_PartnerID
        End If

        dict_Arguments.TryGetValue("INDIR", s_baseDir)
        If s_OutDir IsNot Nothing Then
            CurrentConfigInfo.Output_Dir = s_OutDir
        End If

        dict_Arguments.TryGetValue("INDIR", s_baseDir)
        If s_OutDir IsNot Nothing Then
            CurrentConfigInfo.Output_Dir = s_OutDir
        End If

        dict_Arguments.TryGetValue("INFILE", s_InFile)
        If s_InFile IsNot Nothing Then
            ConfigInfo.InputFile = s_InFile
        End If


        If s_ConfigFile Is Nothing And s_InFile Is Nothing And s_baseDir Is Nothing Then
            AddError("Usage: " & vbCrLf & "   Process_X12_REC -indir c:\DIRNAME -outdir c:\outputdir -email admin@site.com -server SqlSrv-R2  -database  EDI " _
                , Error_Type_List.Critical)
        Else
            'After parameter over-rides, make sure all directories exist (create if not there)
            ConfigInfo.MakeConfDirs()
            If SaveDefault Then
                CurrentConfigInfo.SaveConfigFile(ConfigInfo.Configfile)
            End If
            'Load Partner info
            CurrentPartnerInfo = New PartnerInfo(CurrentConfigInfo.Partner_ID)

        End If

    End Sub

    Public Shared Sub AddError(NewErrorMsg As String, level As Error_Type_List)
        ErrorCount = ErrorCount + 1
        If ErrorCount <= 10000 Then
            ReDim Preserve ErrorMsg(ErrorCount)
            ErrorMsg(ErrorCount - 1) = NewErrorMsg
        End If
        If level > ErrorType Then
            ErrorType = level
        End If
        Select Case ErrorType
            Case Error_Type_List.Critical
                hasErrors = True
            Case Error_Type_List.StdError
                hasErrors = True
        End Select
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
    Public Sub FindFiles()

        Dim dirlist As IO.FileInfo()
        Dim di As IO.DirectoryInfo
        Dim input_file As IO.FileInfo

        If ConfigInfo.FileLoadDir IsNot Nothing Then
            di = New IO.DirectoryInfo(ConfigInfo.FileLoadDir)
            ' @TODO  add file import filter by type
            dirlist = di.GetFiles("*.*")

            'list the names of all files in the specified directory
            For Each input_file In dirlist
                ProcessFileList.Add(input_file)
                MoreFiles = True
            Next
        Else
            ' make 1 entry array
            singlefile = New IO.FileInfo(ConfigInfo.InputFile)
            ReDim Preserve dirlist(0)
            dirlist(0) = singlefile
            MoreFiles = True
        End If



    End Sub
    Public Sub ImportNextFile()
        Dim NextFilename As String
        Dim pos As Integer
        If ConfigInfo.InputFileType = 0 Then
            ConfigInfo.InputFileType = File_Format_List.X12
        End If
        If ProcessFileList.Count > 0 Then

            NextFilename = ProcessFileList(0).FullName
            '@TODO  check for input file type, branch on other types

            If ProcessFileList(0).Length > 0 Then
                reader = New StreamReader(NextFilename)
                '
                reader.ReadBlock(msg_header, 0, 10)
                message = msg_header
                If message.Length > 3 Then
                    If message.Substring(0, 3) = "ISA" Or message.Substring(1, 3) = "ISA" Then
                        reader.DiscardBufferedData()
                        reader.BaseStream.Seek(0, System.IO.SeekOrigin.Begin)
                        message = reader.ReadToEnd
                        '@TODO  verify it matches expected separators
                        RecordDelimiter = message(105)
                        FieldDelimiter = message(103)
                        SubFieldDelimiter = message(104)
                        valid_x12 = True
                        Try
                            segments = (From seg In message.Split(CChar(RecordDelimiter)) Where Not String.IsNullOrEmpty(seg)
                                        Select New ParseSegment(
                                                 seg.Substring(0, seg.IndexOf(FieldDelimiter)),
                                                 seg.Split(FieldDelimiter).Skip(1).ToArray()))

                        Catch ex As Exception
                            AddError("Inbound Error X12", Error_Type_List.Critical)

                        End Try
                        pos = message.IndexOf(RecordDelimiter + "ST" + FieldDelimiter)
                        If pos > 0 Then
                            rec_type = message.Substring(pos + 4, 3)
                        Else
                            RecordDelimiter = vbCrLf
                            pos = message.IndexOf(RecordDelimiter + "ST" + FieldDelimiter)
                            If pos > 0 Then
                                rec_type = message.Substring(pos + 5, 3)
                                Try
                                    segments = (From seg In message.Split(CChar(RecordDelimiter)) Where Not String.IsNullOrEmpty(seg)
                                                Select New ParseSegment(
                                                 seg.Substring(0, seg.IndexOf(FieldDelimiter)),
                                                 seg.Split(FieldDelimiter).Skip(1).ToArray()))

                                Catch ex As Exception
                                    AddError("Inbound Error X12", Error_Type_List.Critical)

                                End Try
                            Else
                                AddError("Start Record not found (ST) in file " & NextFilename, Error_Type_List.Critical)
                            End If
                        End If
                    Else
                        ' not X12
                        If message.Substring(0, 3) = """H""" Or message.Substring(1, 3) = """H""" Then
                            RecordDelimiter = vbCrLf
                            FieldDelimiter = CChar("~")
                            Try
                                segments = (From seg In message.Split(CChar(RecordDelimiter)) Where Not String.IsNullOrEmpty(seg)
                                            Select New ParseSegment(
                                                seg.Substring(0, seg.IndexOf(FieldDelimiter)),
                                                seg.Split(FieldDelimiter).Skip(1).ToArray()))

                            Catch ex As Exception
                                AddError("Parsing error M2K", Error_Type_List.Critical)
                            End Try
                            ' found an M2k X12 message
                            rec_type = segments(0).Elements(1).ToString
                            rec_type = rec_type.Replace("""", "")
                        Else
                            AddError("Header Record not found", Error_Type_List.Critical)
                        End If
                    End If
                Else
                    '@TODO  add message #'s
                    AddError("Message too short", Error_Type_List.Critical)
                End If
            End If
            ' Remove processed file from list
            ' @TODO  consider moving if flag set
            ProcessFileList.RemoveAt(0)

        Else
            MoreFiles = False
        End If

    End Sub
    Public Sub PostProcess()

    End Sub
    Public Sub Export()

    End Sub

    Public Sub Validate()
        Select Case ConfigInfo.InputFileType
            Case File_Format_List.X12
                Validate_X12()
            Case File_Format_List.Delimited
                Validate_Delimited()
            Case File_Format_List.FixedField
                Validate_FixedField()
            Case File_Format_List.SQL




        End Select
    End Sub
    Public Sub Validate_X12()
        Select Case rec_type
            Case "810"
            Case "832"
            Case "850"
                ' Load Record Set Definition
                CurrentRecordset = New X12_850_Request_RecordSet
                Merge_Data()
            Case "855"
            Case "856"
            Case Else

        End Select
    End Sub
    Public Sub Validate_Delimited()
        Select Case rec_type
            Case "850"

        End Select
    End Sub
    Public Sub Validate_FixedField()
        Select Case rec_type
            Case "850"

        End Select
    End Sub

    Public Sub Validate_SQL()
        Select Case rec_type
            Case "850"

        End Select
    End Sub
    Public Shared Sub Dump_X12_Errors()
        Dim X As Integer
        Console.WriteLine("X12 Error Status:" & Interchange.ErrorType)
        Console.WriteLine("Number of errors:" & Interchange.ErrorCount)

        For X = 0 To Interchange.ErrorCount - 1
            Console.WriteLine(Interchange.ErrorMsg(X))
            Debug.Print(Interchange.ErrorMsg(X))
        Next
    End Sub
    Public Sub Merge_Data()
        Dim myrecid As String
        Dim Prev_Rec As String = ""

        'RecSetDefFile
        For Each rec In segments
            RecordCount = RecordCount + 1
            myrecid = rec.SegID.Replace("""", "")
            myrecid = rec.SegID.Replace(vbCr, "")
            myrecid = rec.SegID.Replace(vbLf, "")
            CurrentRecordset.Import_X12(myrecid, rec, RecordCount)



            ' need to generate CTT & SE & GE & IEA
            If DebugFlag Then
                Debug.Print("Next" + myrecid)
            End If
            Prev_Rec = myrecid
        Next ' rec in segments


    End Sub

End Class
