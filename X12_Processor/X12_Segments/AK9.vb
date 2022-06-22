Public Class AK9
    Inherits Segment
    'F01_FUNC_ACK, F02_TRAN_SET_TOTAL, F03_REC_TRAN_CNT, F04_ACCEPT_TRAN_CNT, F05_FUNC_ERR_CODE
    Public Shared Sub InitializeTranDef()
        Dim SegmentDef As FieldDefSet = New FieldDefSet(5)
        SegmentDef.Name = "AK9"
        SegmentDef.FieldDefList(0) = New FieldDef("F01_FUNC_ACK", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(1) = New FieldDef("F02_TRAN_SET_TOTAL", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", "")
        SegmentDef.FieldDefList(2) = New FieldDef("F03_REC_TRAN_CNT", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(3) = New FieldDef("F04_ACCEPT_TRAN_CNT", "ALPHA", 3, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(4) = New FieldDef("F05_FUNC_ERR_CODE", "ALPHA", 8, "NONE", "No", 0, "CCYYMMDD", "NONE", " ", "")
        Dim writer As New System.Xml.Serialization.XmlSerializer(GetType(FieldDefSet))
        Dim file As New System.IO.StreamWriter(path:=ConfigInfo.SegmentDefDir & SegmentDef.Name & ".def")
        writer.Serialize(file, SegmentDef)
        file.Close()
    End Sub

End Class
