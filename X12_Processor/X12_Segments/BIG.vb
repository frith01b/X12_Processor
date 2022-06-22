Public Class BIG
    Inherits Segment
    'F01_Inv_Date, F02_Inv_Num, F03_PO_Date, F04_PO_Num, F05_Release_Num, F06_UNK, F07_Tran_Type
    Public Shared Sub InitializeTranDef()
        Dim SegmentDef As FieldDefSet = New FieldDefSet(7)
        SegmentDef.Name = "BIG"
        SegmentDef.FieldDefList(0) = New FieldDef("F01_Inv_Date", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(1) = New FieldDef("F02_Inv_Num", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", "")
        SegmentDef.FieldDefList(2) = New FieldDef("F03_PO_Date", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(3) = New FieldDef("F04_PO_Num", "ALPHA", 3, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(4) = New FieldDef("F05_Release_Num", "ALPHA", 8, "NONE", "No", 0, "CCYYMMDD", "NONE", " ", "")
        SegmentDef.FieldDefList(5) = New FieldDef("F06_UNK", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(6) = New FieldDef("F07_Tran_Type", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", "")
        Dim writer As New System.Xml.Serialization.XmlSerializer(GetType(FieldDefSet))
        Dim file As New System.IO.StreamWriter(path:=ConfigInfo.SegmentDefDir & SegmentDef.Name & ".def")
        writer.Serialize(file, SegmentDef)
        file.Close()
    End Sub

End Class
