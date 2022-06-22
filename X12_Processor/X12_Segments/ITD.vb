Public Class ITD
    '    F01_TERMS_TXT As String
    '        Public F02_Term_Basis As String
    '    Public F03_Terms_Discount As String
    '    Public F04_Terms_Due_Date As String
    '    Public F05_Terms_Days As String
    '    Public F06_Terms_Net_Due_Date As String
    '    Public F07_UNK As String
    '    Public F08_Terms_Disc_Amt As String
    '    Public F09_UNK As String
    '    Public F10_UNK As String
    '    Public F11_UNK As String
    '    Public F12_Terms_Desc As String
    Inherits Segment
    Public Shared Sub InitializeTranDef()
        Dim SegmentDef As FieldDefSet = New FieldDefSet(12)
        SegmentDef.Name = "ITD"
        SegmentDef.FieldDefList(0) = New FieldDef("F01_TERMS_TXT", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(1) = New FieldDef("F02_Term_Basis", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", "")
        SegmentDef.FieldDefList(2) = New FieldDef("F03_Terms_Discount", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(3) = New FieldDef("F04_Terms_Due_Date", "ALPHA", 3, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(4) = New FieldDef("F05_Terms_Days", "ALPHA", 8, "NONE", "No", 0, "CCYYMMDD", "NONE", " ", "")
        SegmentDef.FieldDefList(5) = New FieldDef("F06_Terms_Net_Due_Date", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(6) = New FieldDef("F07_UNK", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", "")
        SegmentDef.FieldDefList(7) = New FieldDef("F08_Terms_Disc_Amt", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(8) = New FieldDef("F09_UNK", "ALPHA", 3, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(9) = New FieldDef("F10_UNK", "ALPHA", 8, "NONE", "No", 0, "CCYYMMDD", "NONE", " ", "")
        SegmentDef.FieldDefList(10) = New FieldDef("F11_UNK", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(11) = New FieldDef("F12_Terms_Desc", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", "")
        Dim writer As New System.Xml.Serialization.XmlSerializer(GetType(FieldDefSet))
        Dim file As New System.IO.StreamWriter(path:=ConfigInfo.SegmentDefDir & SegmentDef.Name & ".def")
        writer.Serialize(file, SegmentDef)
        file.Close()
    End Sub

End Class
