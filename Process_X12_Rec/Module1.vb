Option Explicit On

Imports System.Xml
Imports System.Globalization
Imports System.Text
Imports System.IO
Imports X12_Processor

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

    Dim db_Sql As X12_Processor.DB_Util = Nothing
    Dim b_success As Boolean = True

    Sub Main()
        Call X12_Processor.Utility.ParseArgs(dict_NamedArgs)

        If (dict_NamedArgs.Count > 0) Then

            Dim edi_processor As New X12_Processor.Interchange(dict_NamedArgs)
            If Not Interchange.hasErrors Then
                edi_processor.FindFiles()
                While Interchange.MoreFiles And Not Interchange.hasErrors
                    'After Import, all segments will be available in CurrentRecordSet 
                    edi_processor.ImportNextFile()
                    ' Validate converts raw data to X12
                    edi_processor.Validate()
                    If Not Interchange.hasErrors Then
                        ' PostProcess includes record cleanup, substitutions, filtering, and re-format prep
                        edi_processor.PostProcess()
                        If Not Interchange.hasErrors Then
                            ' wont over-write existing, always adds timestamp to processed files and unique Xmit #.
                            edi_processor.Export()
                        Else
                            b_success = False
                            Interchange.Dump_X12_Errors()
                            Exit While
                        End If
                    Else
                        b_success = False
                        Interchange.Dump_X12_Errors()
                        Exit While
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
