Public Class AK5
    Inherits Segment
    'F01_TRANS_SET_ACK, F02_TRAN_ERR_CODE 
    Public Shared Sub InitializeTranDef()
        Dim SegmentDef As FieldDefSet = New FieldDefSet(2)
        SegmentDef.Name = "AK5"
        SegmentDef.FieldDefList(0) = New FieldDef("F01_TRANS_SET_ACK", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(1) = New FieldDef("F02_TRAN_ERR_CODE", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", "")
        Dim writer As New System.Xml.Serialization.XmlSerializer(GetType(FieldDefSet))
        Dim file As New System.IO.StreamWriter(path:=ConfigInfo.SegmentDefDir & SegmentDef.Name & ".def")
        writer.Serialize(file, SegmentDef)
        file.Close()
    End Sub

End Class
