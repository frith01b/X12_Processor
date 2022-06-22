Option Explicit On

Imports System.Xml
Imports System.Globalization
Imports System.Text
Imports System.IO


'init
'log
'validate
'Load
'Identify
'config
'pre-process
'process
'post-process
'output
'reports
'notify

Module Module1
    Dim dict_NamedArgs As New Dictionary(Of String, String)
    Dim tw_errorWriter As TextWriter = Console.Error
    Dim db_Sql As X12_Processor.DB_Util = Nothing

    Sub Main()


        Dim s_baseDir As String = ""
        Dim s_OutDir As String = ""
        Dim s_ErrorFile As String = ""
        Dim s_PartnerID As String = ""
        Dim s_ConfigFile As String = ""
        Dim Fname As String
        Dim Lname As String
        Dim Mname As String
        Dim tempDate As String
        Dim tempValue As String
        Dim tempPayPeriod As String
        Dim tempPayFreq As String
        Dim b_success As Boolean = True
        Dim s_Server As String = ""
        Dim s_database As String = ""

        Call X12_Processor.Utility.ParseArgs(dict_NamedArgs)

        If (dict_NamedArgs.Count > 0 And dict_NamedArgs.TryGetValue("INDIR", s_baseDir) And dict_NamedArgs.TryGetValue("OUTDIR", s_OutDir)) Then
            dict_NamedArgs.TryGetValue("PARTNERID", s_PartnerID)
            dict_NamedArgs.TryGetValue("CONFIGFILE", s_ConfigFile)
            If (Not System.IO.Directory.Exists(s_OutDir)) Then
                System.IO.Directory.CreateDirectory(s_OutDir)
            End If

            If (Not System.IO.Directory.Exists(s_baseDir & "\Processed")) Then
                System.IO.Directory.CreateDirectory(s_baseDir & "\Processed")
            End If

            Dim culture = New CultureInfo("en")

            Dim di As New IO.DirectoryInfo(s_baseDir)
            Dim dirlist As IO.FileInfo() = di.GetFiles("*.edi")
            Dim input_file As IO.FileInfo

            'list the names of all files in the specified directory
            For Each input_file In dirlist
                Dim edi_processor As New X12_Processor.Interchange(s_baseDir + "\" + input_file.Name, s_ErrorFile, s_ConfigFile, s_PartnerID)
                Dim myRecId As String
                If edi_processor.valid_x12 Then
                    myRecId = edi_processor.rec_type
                    Select Case myRecId
                        Case "810"
                            'edi_processor.Process_810()
                        Case "850"
                            'edi_processor.Process_850()
                        Case "855"
                            'edi_processor.Process_855()
                        Case "856"
                            'edi_processor.Process_856()
                        Case Else
                            MsgBox("Invalid rec type")
                    End Select
                Else
                    tw_errorWriter.Write(s_baseDir + "\" + input_file.Name + "is not Valid X12 file")
                End If
            Next

        Else  ' if args do not exist

            tw_errorWriter.WriteLine("Usage: " & vbCrLf & "   Process_X12_REC -indir c:\DIRNAME -outdir c:\outputdir -email admin@site.com -server SqlSrv-R2  -database  EDI ")
        End If

    End Sub
End Module
