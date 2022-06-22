Public Class PID
    'F01_DESC_TYPE, F02_DESC_CODE, F03_ORG_Code, F04_Code_Desc, F05_DESC
    Inherits Segment
    Public Shared Sub InitializeTranDef()
        Dim SegmentDef As FieldDefSet = New FieldDefSet(5)
        SegmentDef.Name = "PID"
        SegmentDef.FieldDefList(0) = New FieldDef("F01_DESC_TYPE", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(1) = New FieldDef("F02_DESC_CODE", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", "")
        SegmentDef.FieldDefList(2) = New FieldDef("F03_ORG_Code", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(3) = New FieldDef("F04_Code_Desc", "ALPHA", 3, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(4) = New FieldDef("F05_DESC", "ALPHA", 8, "NONE", "No", 0, "CCYYMMDD", "NONE", " ", "")
        Dim writer As New System.Xml.Serialization.XmlSerializer(GetType(FieldDefSet))
        Dim file As New System.IO.StreamWriter(path:=ConfigInfo.SegmentDefDir & SegmentDef.Name & ".def")
        writer.Serialize(file, SegmentDef)
        file.Close()
    End Sub

End Class
