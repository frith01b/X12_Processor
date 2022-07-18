Option Strict On
Option Explicit On

Public Class ISA
    Inherits Segment
    Implements SegTranslate
    Public Sub New()
        MyBase.New("ISA")
    End Sub

    Public Overloads Sub InitializeTranDef() Implements SegTranslate.InitializeTranDef
        Dim SegmentDef As FieldDefSet = New FieldDefSet()
        SegmentDef.Name = "ISA"
        SegmentDef.FieldDefList.Add(New FieldDef("F01_AUTH", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F02_AUTH_INF", "NUMERIC", 10, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F03_SECURITY", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F04_SEC_INF", "ALPHA", 10, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F05_SOURCE_QUAL", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F06_SOURCE_ID", "ALPHA", 15, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F07_RECEIVER_QUAL", "NUMERIC", 2, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F08_RECEIVER_ID", "ALPHA", 15, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F09_RECV_ALPHA", "ALPHA", 6, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F10_RECV_TIME", "ALPHA", 4, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F11_INTL", "ALPHA", 1, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F12_ISA_VERSION", "NUMERIC", 5, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F13_ICN", "ALPHA", 9, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F14_ACK", "ALPHA", 1, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F15_TEST", "ALPHA", 1, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F16_SUBELEMENTDELIM", "ALPHA", 1, "NONE", "No", 0, "", "NONE", " ", ""))
        MyBase.FieldDefs = SegmentDef

        Dim writer As New System.Xml.Serialization.XmlSerializer(GetType(FieldDefSet))
        Dim file As New System.IO.StreamWriter(path:=ConfigInfo.SegmentDefDir & "\" & SegmentDef.Name & ".def")
        MyBase.FieldDefs = SegmentDef
        writer.Serialize(file, SegmentDef)
        file.Close()

    End Sub
End Class
