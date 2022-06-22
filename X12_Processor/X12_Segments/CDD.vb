Public Class CDD
    Inherits Segment

    'F01_Adj_Reason, F02_DRCR_Flag, F03_UNK, F04_Amt, F05_UNK, F06_UNK, F07_Quantity, F08_UM, F09_UNK, F10_Price_Code, F11_Price 
    Public Shared Sub InitializeTranDef()
        Dim SegmentDef As FieldDefSet = New FieldDefSet(11)
        SegmentDef.Name = "CDD"
        SegmentDef.FieldDefList(0) = New FieldDef("F01_Adj_Reason", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(1) = New FieldDef("F02_DRCR_Flag", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", "")
        SegmentDef.FieldDefList(2) = New FieldDef("F03_UNK", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(3) = New FieldDef("F04_Amt", "ALPHA", 3, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(4) = New FieldDef("F05_UNK", "ALPHA", 8, "NONE", "No", 0, "CCYYMMDD", "NONE", " ", "")
        SegmentDef.FieldDefList(5) = New FieldDef("F06_UNK", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(6) = New FieldDef("F07_Quantity", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", "")
        SegmentDef.FieldDefList(7) = New FieldDef("F08_UM", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(8) = New FieldDef("F09_UNK", "ALPHA", 3, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(9) = New FieldDef("F10_Price_Code", "ALPHA", 8, "NONE", "No", 0, "CCYYMMDD", "NONE", " ", "")
        SegmentDef.FieldDefList(10) = New FieldDef("F11_Price", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", "")
        Dim writer As New System.Xml.Serialization.XmlSerializer(GetType(FieldDefSet))
        Dim file As New System.IO.StreamWriter(path:=ConfigInfo.SegmentDefDir & SegmentDef.Name & ".def")
        writer.Serialize(file, SegmentDef)
        file.Close()
    End Sub

End Class
