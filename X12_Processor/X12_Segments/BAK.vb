﻿Public Class BAK
    Inherits Segment
    'F01_Purpose_code As String
    'Public F02_Ack_Type As String
    'Public F03_PO_Num As String
    'Public F04_Orig_PO_Date As String
    'Public F05_UNK As String
    'Public F06_UNK As String
    'Public F07_UNK As String
    'Public F08_UNK As String
    'Public F09_StatusDate As String
    Public Shared Sub InitializeTranDef()
        Dim SegmentDef As FieldDefSet = New FieldDefSet(9)
        SegmentDef.Name = "BAK"
        SegmentDef.FieldDefList(0) = New FieldDef("F01_Purpose_code", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(1) = New FieldDef("F02_Ack_Type", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", "")
        SegmentDef.FieldDefList(2) = New FieldDef("F03_PO_Num", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(3) = New FieldDef("F04_Orig_PO_Date", "ALPHA", 3, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(4) = New FieldDef("F05_UNK", "ALPHA", 8, "NONE", "No", 0, "CCYYMMDD", "NONE", " ", "")
        SegmentDef.FieldDefList(5) = New FieldDef("F06_UNK", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(6) = New FieldDef("F07_UNK", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", "")
        SegmentDef.FieldDefList(7) = New FieldDef("F08_UNK", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(8) = New FieldDef("F09_StatusDate", "ALPHA", 3, "NONE", "No", 0, "", "NONE", " ", "")
        Dim writer As New System.Xml.Serialization.XmlSerializer(GetType(FieldDefSet))
        Dim file As New System.IO.StreamWriter(path:=ConfigInfo.SegmentDefDir & "\" & SegmentDef.Name & ".def")

        writer.Serialize(file, SegmentDef)
        file.Close()
    End Sub

End Class