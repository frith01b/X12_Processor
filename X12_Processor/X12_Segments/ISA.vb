Option Strict On
Option Explicit On

Public Class ISA

    'MyISA.F05_SOURCE_QUAL = PartnerInfo.Source_Qualifier
    '      MyISA.F06_SOURCE_ID = PartnerInfo.Source_ID
    '      MyISA.F07_RECEIVER_QUAL = PartnerInfo.Receiver_Qualifier
    '      MyISA.F08_RECEIVER_ID = PartnerInfo.Receiver_ID
    'F01_AUTH, F02_AUTH_INF, F03_SECURITY, F04_SEC_INF, F05_SOURCE_QUAL, F06_SOURCE_ID, F07_RECEIVER_QUAL, F08_RECEIVER_ID, F09_RECV_DATE, F10_RECV_TIME, F11_INTL, F12_ISA_VERSION, F13_ICN, F14_ACK, F15_TEST
    Inherits Segment
    Public Shared Sub InitializeTranDef()
        Dim SegmentDef As FieldDefSet = New FieldDefSet(15)
        SegmentDef.Name = "ISA"
        SegmentDef.FieldDefList(0) = New FieldDef("F01_AUTH", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(1) = New FieldDef("F02_AUTH_INF", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", "")
        SegmentDef.FieldDefList(2) = New FieldDef("F03_SECURITY", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(3) = New FieldDef("F04_SEC_INF", "ALPHA", 3, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(4) = New FieldDef("F05_SOURCE_QUAL", "ALPHA", 8, "NONE", "No", 0, "CCYYMMDD", "NONE", " ", "")
        SegmentDef.FieldDefList(5) = New FieldDef("F06_SOURCE_ID", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(6) = New FieldDef("F07_RECEIVER_QUAL", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", "")
        SegmentDef.FieldDefList(7) = New FieldDef("F08_RECEIVER_ID", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(8) = New FieldDef("F09_RECV_ALPHA", "ALPHA", 3, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(9) = New FieldDef("F10_RECV_TIME", "ALPHA", 8, "NONE", "No", 0, "CCYYMMDD", "NONE", " ", "")
        SegmentDef.FieldDefList(10) = New FieldDef("F11_INTL", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(11) = New FieldDef("F12_ISA_VERSION", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", "")
        SegmentDef.FieldDefList(12) = New FieldDef("F13_ICN", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(13) = New FieldDef("F14_ACK", "ALPHA", 3, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(14) = New FieldDef("F15_TEST", "ALPHA", 8, "NONE", "No", 0, "CCYYMMDD", "NONE", " ", "")
        Dim writer As New System.Xml.Serialization.XmlSerializer(GetType(FieldDefSet))
        Dim file As New System.IO.StreamWriter(path:=ConfigInfo.SegmentDefDir & SegmentDef.Name & ".def")
        writer.Serialize(file, SegmentDef)
        file.Close()
    End Sub

End Class
