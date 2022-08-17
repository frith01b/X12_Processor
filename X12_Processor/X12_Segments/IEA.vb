Public Class IEA
    'F01_GROUP_QTY, F02_END_ICN
    Inherits Segment
 Implements SegTranslate 
Public Sub New()
        MyBase.New("IEA")
    End Sub

    Public Overloads Sub InitializeTranDef() Implements SegTranslate.InitializeTranDef

        Dim SegmentDef As FieldDefSet = New FieldDefSet()
        SegmentDef.Name = "IEA"
        SegmentDef.FieldDefList.Add(New FieldDef("F01_GROUP_QTY", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F02_END_ICN", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", ""))

        Dim writer As New System.Xml.Serialization.XmlSerializer(GetType(FieldDefSet))
        Dim file As New System.IO.Streamwriter(Path:=ConfigInfo.SegmentDefDir & "\" & SegmentDef.Name & ".def")
        MyBase.FieldDefs = SegmentDef
        writer.Serialize(file, SegmentDef)
        file.Close()


    End Sub

End class
