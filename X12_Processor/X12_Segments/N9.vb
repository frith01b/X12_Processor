﻿Public Class N9
    'F01_N9_REF_ID, F02_Text 
    Inherits Segment
    Public Shared Sub InitializeTranDef()
        Dim SegmentDef As FieldDefSet = New FieldDefSet(2)
        SegmentDef.Name = "N9"
        SegmentDef.FieldDefList(0) = New FieldDef("F01_N9_REF_ID", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(1) = New FieldDef("F02_Text", "ALPHA", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", "")
        Dim writer As New System.Xml.Serialization.XmlSerializer(GetType(FieldDefSet))
        Dim file As New System.IO.StreamWriter(path:=ConfigInfo.SegmentDefDir & SegmentDef.Name & ".def")
        writer.Serialize(file, SegmentDef)
        file.Close()
    End Sub

End Class