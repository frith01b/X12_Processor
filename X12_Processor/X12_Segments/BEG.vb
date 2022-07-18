Public Class BEG
    'F01_PURPOSE, F02_ORDER_TYPE, F03_ORD_NUM, F04_ORD_DATE
    Inherits Segment
 Implements SegTranslate 
Public Sub New()
        MyBase.New("BEG")
    End Sub
    Public Overloads Sub InitializeTranDef() Implements SegTranslate.InitializeTranDef
        Dim SegmentDef As FieldDefSet = New FieldDefSet()
        SegmentDef.Name = "BEG"
        SegmentDef.FieldDefList.Add(New FieldDef("F01_PURPOSE", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F02_ORDER_TYPE", "NUMERIC", 2, "NONE", "No", 0, "#,###.00", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F03_ORD_NUM", "ALPHA", 22, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F04_RELEASENUM", "ALPHA", 30, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F05_ORD_DATE", "ALPHA", 8, "NONE", "No", 0, "CCYYMMDD", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F06_CONTRACTNUM", "ALPHA", 30, "NONE", "No", 0, "", "NONE", " ", ""))

        Dim writer As New System.Xml.Serialization.XmlSerializer(GetType(FieldDefSet))
        Dim file As New System.IO.StreamWriter(path:=ConfigInfo.SegmentDefDir & "\" & SegmentDef.Name & ".def")
        MyBase.FieldDefs = SegmentDef
        writer.Serialize(file, SegmentDef)
        file.Close()


    End Sub
End class
