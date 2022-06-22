Option Strict On
Option Explicit On
Imports System.Data
Imports System.IO
Imports System.Xml

<Serializable()>
Public Class ConfigInfo
    Public Shared Configfile As String
    Public Shared ConfigDIR As String
    Public Shared PartnerDir As String
    Public Shared FileLoadDir As String
    Public Shared OutputDir As String
    Public Shared SegmentDefDir As String
    Public Shared RecordSetDir As String
    Public Sub New(MyConfigFile As String)
        Dim retry As Integer = 0

LookTwice:
        If File.Exists(MyConfigFile) Then

            Dim xmlFile As XmlReader
            xmlFile = XmlReader.Create(MyConfigFile, New XmlReaderSettings())
            Dim ds As New DataSet
            ds.ReadXml(xmlFile)
            Dim i As Integer
            For i = 0 To ds.Tables(0).Rows.Count - 1
                Select Case ds.Tables(0).Rows(i).Item(0).ToString
                    Case "Configfile"
                        Configfile = ds.Tables(0).Rows(i).Item(1).ToString
                    Case "ConfigDIR"
                        ConfigDIR = ds.Tables(0).Rows(i).Item(1).ToString
                    Case "PartnerDir"
                        PartnerDir = ds.Tables(0).Rows(i).Item(1).ToString
                    Case "FileLoadDir"
                        ds.Tables(0).Rows(i).Item(1).ToString()
                    Case "OutputDir"
                        OutputDir = ds.Tables(0).Rows(i).Item(1).ToString
                    Case "TranDefDir"
                        SegmentDefDir = ds.Tables(0).Rows(i).Item(1).ToString
                    Case "RecordSetDir"
                        RecordSetDir = ds.Tables(0).Rows(i).Item(1).ToString
                    Case Else
                End Select
            Next
            Call MakeConfDirs()
        Else
            If retry = 0 Then
                retry = retry + 1
                'run default constructor which creates default conf file
                Dim Myvar As ConfigInfo = New ConfigInfo

                GoTo LookTwice
            End If
        End If
    End Sub
    Public Sub New()
        Configfile = "C:\temp\EDI\X12_Processor.conf"
        ConfigDIR = "C:\temp\EDI\"
        PartnerDir = "C:\temp\EDI\Partners"
        FileLoadDir = "C:\EDI\INBOUND"
        OutputDir = "C:\EDI\OUTBOUND"
        SegmentDefDir = "C:\temp\EDI\SEGDEF"
        RecordSetDir = "C:\temp\EDI\RECDEF"
        Call MakeConfDirs()
        SaveConfigFile(Configfile)

    End Sub
    Public Sub SaveConfigFile(MyconfigFile As String)
        Dim writer As New System.Xml.Serialization.XmlSerializer(GetType(ConfigInfo))
        Dim file As New System.IO.StreamWriter(path:=MyconfigFile)
        writer.Serialize(file, Me)
        file.Close()

    End Sub
    Private Sub MakeConfDirs()
        If (Not System.IO.Directory.Exists(ConfigDIR)) Then
            System.IO.Directory.CreateDirectory(ConfigDIR)
        End If
        If (Not System.IO.Directory.Exists(PartnerDir)) Then
            System.IO.Directory.CreateDirectory(PartnerDir)
        End If
        If (Not System.IO.Directory.Exists(OutputDir)) Then
            System.IO.Directory.CreateDirectory(OutputDir)
        End If
        If (Not System.IO.Directory.Exists(SegmentDefDir)) Then
            System.IO.Directory.CreateDirectory(SegmentDefDir)
        End If
        If (Not System.IO.Directory.Exists(RecordSetDir)) Then
            System.IO.Directory.CreateDirectory(RecordSetDir)
        End If
    End Sub

    '*********************************
    'Non-Public properties / copies of public members to make serialization work
    Property Config_file As String
        Get
            Return Configfile
        End Get
        Set(ByVal value As String)
            Configfile = value
        End Set
    End Property

    Property Config_DIR As String
        Get
            Return ConfigDIR
        End Get
        Set(ByVal value As String)
            ConfigDIR = value
        End Set
    End Property
    Property Partner_Dir As String
        Get
            Return PartnerDir
        End Get
        Set(ByVal value As String)
            PartnerDir = value
        End Set
    End Property
    Property FileLoad_Dir As String
        Get
            Return FileLoadDir
        End Get
        Set(ByVal value As String)
            FileLoadDir = value
        End Set
    End Property
    Property Output_Dir As String
        Get
            Return OutputDir
        End Get
        Set(ByVal value As String)
            OutputDir = value
        End Set
    End Property
    Property SegmentDef_Dir As String
        Get
            Return SegmentDefDir
        End Get
        Set(ByVal value As String)
            SegmentDefDir = value
        End Set
    End Property
    Property RecordSet_Dir As String
        Get
            Return RecordSetDir
        End Get
        Set(ByVal value As String)
            RecordSetDir = value
        End Set
    End Property
    '*********************************
    ' END Non-Public properties / copies
End Class
