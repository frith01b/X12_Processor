Public Class GS
    'F01_REQ_TYPE, F02_SEND_ID, F03_RECV_ID, F04_SEND_DATE, F05_SEND_TIME, F06_GCN, F07_AGENCY, F08_GROUP_VER
    Inherits Segment
 Implements SegTranslate
    Public Sub New()
        MyBase.New("GS")
    End Sub
    Public Overloads Sub InitializeTranDef() Implements SegTranslate.InitializeTranDef
        Dim SegmentDef As FieldDefSet = New FieldDefSet()
        SegmentDef.Name = "GS"
        SegmentDef.FieldDefList.Add(New FieldDef("F01_REQ_TYPE", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F02_SEND_ID", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F03_RECV_ID", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F04_SEND_DATE", "ALPHA", 3, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F05_SEND_TIME", "ALPHA", 8, "NONE", "No", 0, "CCYYMMDD", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F06_GCN", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F07_AGENCY", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F08_GROUP_VER", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", ""))

        Dim writer As New System.Xml.Serialization.XmlSerializer(GetType(FieldDefSet))
        Dim file As New System.IO.StreamWriter(path:=ConfigInfo.SegmentDefDir & "\" & SegmentDef.Name & ".def")
        MyBase.FieldDefs = SegmentDef
        writer.Serialize(file, SegmentDef)
        file.Close()


    End Sub
End class
