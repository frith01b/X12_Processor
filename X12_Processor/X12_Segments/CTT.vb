﻿Public Class CTT
    Inherits Segment
    'F01_PO_LINE_COUNT, F02_PO_LINE_QTY
    Public Shared Sub InitializeTranDef()
        Dim SegmentDef As FieldDefSet = New FieldDefSet(2)
        SegmentDef.Name = "CTT"
        SegmentDef.FieldDefList(0) = New FieldDef("F01_PO_LINE_COUNT", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(1) = New FieldDef("F02_PO_LINE_QTY", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", "")
        Dim writer As New System.Xml.Serialization.XmlSerializer(GetType(FieldDefSet))
        Dim file As New System.IO.StreamWriter(path:=ConfigInfo.SegmentDefDir & SegmentDef.Name & ".def")
        writer.Serialize(file, SegmentDef)
        file.Close()
    End Sub

End Class