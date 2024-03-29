﻿Option Strict On
Option Explicit On


Imports System.Data
Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization

Public Enum File_Format_List
    X12 = 1
    Delimited = 2
    FixedField = 3
    SQL = 4
    XML = 5
End Enum

<Serializable()>
Public Class ConfigInfo
    '''****************************************************************
    ''' all new public fields need corresponding private ones for serialization to work
    ''' (search for   "Non-Public properties" at bottom of code)
    '''****************************************************************
    Public Shared Configfile As String
    Public Shared ConfigDIR As String
    Public Shared FileLoadDir As String
    Public Shared InputFile As String
    Public Shared InputFileType As File_Format_List
    Public Shared OutputType As File_Format_List
    Public Shared OutputFormatFile As String
    Public Shared OutputDir As String
    Public Shared PartnerDir As String
    Public Shared PartnerID As String
    Public Shared ProcessedDir As String
    Public Shared RecordSetDir As String
    Public Shared SegmentDefDir As String

    '''**********************************************************
    Public Shared Function FromFile(MyConfigFile As String) As ConfigInfo
        Dim retry As Integer = 0
        Dim objStreamReader As StreamReader
        Dim serializer As Serialization.XmlSerializer
        Dim LocalConfig As ConfigInfo

LookTwice:
        If File.Exists(MyConfigFile) Then
            Try

                objStreamReader = New StreamReader(MyConfigFile)
                serializer = New Serialization.XmlSerializer(GetType(ConfigInfo))

                LocalConfig = DirectCast(serializer.Deserialize(objStreamReader), ConfigInfo)
            Catch ex As Exception
                retry = retry + 1
                If retry < 2 Then
#Disable Warning S907 ' "GoTo" statements should not be used
                    GoTo LookTwice
#Enable Warning S907 ' "GoTo" statements should not be used
                End If
                LocalConfig = Nothing
            End Try

            Return LocalConfig
        Else
            Return Nothing
        End If
    End Function
    '**********************************************************
    '**********************************************************
    '''Default values only for new installations
    Public Sub New()
        Configfile = "C:\temp\EDI\X12_Processor.conf"
        ConfigDIR = "C:\temp\EDI\"
        FileLoadDir = "C:\EDI\INBOUND"
        PartnerDir = "C:\temp\EDI\Partners"
        PartnerID = "FROM_AS2"  ' special qualifier to mean just read the AS2 file to get partner id--
        ProcessedDir = FileLoadDir & "\Processed"
        OutputDir = "C:\EDI\OUTBOUND"
        OutputFormatFile = "C:\temp\EDI\X12_OutputFormat.conf"
        OutputType = File_Format_List.X12
        SegmentDefDir = "C:\temp\EDI\SEGDEF"
        RecordSetDir = "C:\temp\EDI\RECDEF"

    End Sub
    '''**********************************************************
    Public Sub SaveConfigFile(MyconfigFile As String)

        Utility.VerifyDirList({Path.GetFullPath(MyconfigFile)})
        Dim writer As New System.Xml.Serialization.XmlSerializer(GetType(ConfigInfo))
        Dim file As New System.IO.StreamWriter(path:=MyconfigFile)
        writer.Serialize(file, Me)
        file.Close()

    End Sub
    '''**********************************************************
    Public Shared Sub MakeConfDirs()
        Utility.VerifyDirList({FileLoadDir, OutputDir, PartnerDir, OutputDir, SegmentDefDir,
                              RecordSetDir})
    End Sub

    '''*********************************
    '''Non-Public properties / copies of public members to make serialization work
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
    Property FileLoad_Dir As String
        Get
            Return FileLoadDir
        End Get
        Set(ByVal value As String)
            FileLoadDir = value
        End Set
    End Property
    Property Input_File As String
        Get
            Return InputFile
        End Get
        Set(ByVal value As String)
            InputFile = value
        End Set
    End Property
    Property Input_FileType As File_Format_List
        Get
            Return InputFileType
        End Get
        Set(ByVal value As File_Format_List)
            InputFileType = value
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
    Property Partner_ID As String
        Get
            Return PartnerID
        End Get
        Set(ByVal value As String)
            PartnerID = value
        End Set
    End Property
    Property Processed_Dir As String
        Get
            Return ProcessedDir
        End Get
        Set(ByVal value As String)
            ProcessedDir = value
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
    Property Output_FormatFile As String
        Get
            Return OutputFormatFile
        End Get
        Set(ByVal value As String)
            OutputFormatFile = value
        End Set
    End Property

    Property Output_Type As File_Format_List
        Get
            Return OutputType
        End Get
        Set(ByVal value As File_Format_List)
            OutputType = value
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
