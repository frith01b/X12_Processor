Public Class SAC
    'F01_REC_IND, F02_CHARGE_CODE, F03_AGENCY_QUAL, F04_AGENCY_CODE, F05_AMT, F06_ALLOW, F07_PERCENT, F08_RATE, F09_UNITS, F10_QTY1, F11_QTY2, F12_HANDLING, F13_SAC_REF_ID, F14_SAC_OPTION, F15_SAC_DESC 
    Inherits Segment
    Public Shared Sub InitializeTranDef()
        Dim SegmentDef As FieldDefSet = New FieldDefSet(15)
        SegmentDef.Name = "SAC"
        SegmentDef.FieldDefList(0) = New FieldDef("F01_REC_IND", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(1) = New FieldDef("F02_CHARGE_CODE", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", "")
        SegmentDef.FieldDefList(2) = New FieldDef("F03_AGENCY_QUAL", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(3) = New FieldDef("F04_AGENCY_CODE", "ALPHA", 3, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(4) = New FieldDef("F05_AMT", "ALPHA", 8, "NONE", "No", 0, "CCYYMMDD", "NONE", " ", "")
        SegmentDef.FieldDefList(5) = New FieldDef("F06_ALLOW", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(6) = New FieldDef("F07_PERCENT", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", "")
        SegmentDef.FieldDefList(7) = New FieldDef("F08_RATE", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(8) = New FieldDef("F09_UNITS", "ALPHA", 3, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(9) = New FieldDef("F10_QTY1", "ALPHA", 8, "NONE", "No", 0, "CCYYMMDD", "NONE", " ", "")
        SegmentDef.FieldDefList(10) = New FieldDef("F11_QTY2", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(11) = New FieldDef("F12_HANDLING", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", "")
        SegmentDef.FieldDefList(12) = New FieldDef("F13_SAC_REF_ID", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(13) = New FieldDef("F14_SAC_OPTION", "ALPHA", 3, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(14) = New FieldDef("F15_SAC_DESC", "ALPHA", 8, "NONE", "No", 0, "CCYYMMDD", "NONE", " ", "")
        Dim writer As New System.Xml.Serialization.XmlSerializer(GetType(FieldDefSet))
        Dim file As New System.IO.StreamWriter(path:=ConfigInfo.SegmentDefDir & SegmentDef.Name & ".def")
        writer.Serialize(file, SegmentDef)
        file.Close()
    End Sub


End Class
