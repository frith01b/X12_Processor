Option Explicit On
Option Strict On

Imports System.Xml
Imports System.Globalization
Imports System.Text
Imports System.IO
Imports X12_Processor

'+ init
'+ log
'o validate
'+ Load
'+ Identify
'+ config
'o pre-process
'+ process
'o post-process
'+ output
'o reports
'o notify

Module Module1
    Dim dict_NamedArgs As New Dictionary(Of String, String)

    Dim db_Sql As X12_Processor.DB_Util = Nothing
    Dim b_success As Boolean = True
    Dim OutputPath As String

    Sub Main()
        Call X12_Processor.Utility.ParseArgs(dict_NamedArgs)

        If (dict_NamedArgs.Count > 0) Then

            Dim edi_processor As New X12_Processor.Interchange(dict_NamedArgs)
            OutputPath = Interchange.CurrentConfigInfo.Output_Dir
            If OutputPath.Trim.Length = 0 Then
                OutputPath = "c:\temp"
                If Not Directory.Exists(OutputPath) Then
                    Directory.CreateDirectory(OutputPath)
                End If
            End If
            '@TODO need to add function that accepts pre/post processing function reference for easy integration.
            '@TODO develop flat reference in recordset object to all transactions to allow easy direct update.
            If Not Interchange.hasErrors Then
                edi_processor.FindFiles()
                While Interchange.MoreFiles
                    'After Import, all segments will be available in CurrentRecordSet 
                    edi_processor.ImportNextFile()
                    ' Validate converts raw data to X12
                    edi_processor.Validate()
                    ' PostProcess includes record cleanup, substitutions, filtering, and re-format prep
                    edi_processor.PostProcess()

                        edi_processor.Export(OutputPath & "\" & Path.GetFileNameWithoutExtension(edi_processor.CurrentImportFile) & ".850")

                    If Interchange.hasErrors Then
                        b_success = False
                        Interchange.Dump_X12_Errors()
                        'Exit While
                    End If
                End While


                If Not Interchange.hasErrors Then
                    b_success = False
                    Interchange.Dump_X12_Errors()
                End If
            Else
                b_success = False
                Interchange.Dump_X12_Errors()
            End If
        End If
    End Sub


End Module
