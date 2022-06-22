Public Class AK3
    Inherits Segment
    'F01_SEG_ID, F02_SEG_POS, F03_LOOP_ID, F04_SEG_ERR_CODE As String
    Public Shared Sub InitializeTranDef()
        Dim SegmentDef As FieldDefSet = New FieldDefSet(4)
        SegmentDef.Name = "AK3"
        SegmentDef.FieldDefList(0) = New FieldDef("F01_SEG_ID", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(1) = New FieldDef("F02_SEG_POS", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", "")
        SegmentDef.FieldDefList(2) = New FieldDef("F03_LOOP_ID", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(3) = New FieldDef("F04_SEG_ERR_CODE", "ALPHA", 3, "NONE", "No", 0, "", "NONE", " ", "")
        Dim writer As New System.Xml.Serialization.XmlSerializer(GetType(FieldDefSet))
        Dim file As New System.IO.StreamWriter(path:=ConfigInfo.SegmentDefDir & SegmentDef.Name & ".def")
        writer.Serialize(file, SegmentDef)
        file.Close()
    End Sub

End Class
