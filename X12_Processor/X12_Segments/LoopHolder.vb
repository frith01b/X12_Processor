Public Class LoopHolder
    Inherits Segment
    Implements SegTranslate
        Public Sub New()
        MyBase.New("LoopHolder")
    End Sub
        Public Overloads Sub InitializeTranDef() Implements SegTranslate.InitializeTranDef
            Dim SegmentDef As FieldDefSet = New FieldDefSet()
        SegmentDef.Name = "LoopHolder"
        SegmentDef.FieldDefList.Add(New FieldDef("F01_LoopName", "ALPHA", 50, "NONE", "No", 0, "", "NONE", " ", ""))

        Dim writer As New System.Xml.Serialization.XmlSerializer(GetType(FieldDefSet))
            Dim file As New System.IO.StreamWriter(path:=ConfigInfo.SegmentDefDir & "\" & SegmentDef.Name & ".def")
            MyBase.FieldDefs = SegmentDef
            writer.Serialize(file, SegmentDef)
            file.Close()
        End Sub

    End Class
