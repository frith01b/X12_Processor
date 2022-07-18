Public Class PO4
    'F01_PACK, F02_SIZE, F03_UOM, F04_UNKNOWN, F05_WEIGHT_QUAL, F06_GROSS_WEIGHT, F07_UNIT_MEASURE, F08_UNKNOWN, F09_UNKNOWN, F10_LENGTH, F11_WIDTH, F12_HEIGHT, F13_INNER_PACK 
    Inherits Segment
 Implements SegTranslate 
Public Sub New()
        MyBase.New("PO4")
    End Sub
    Public Overloads Sub InitializeTranDef() Implements SegTranslate.InitializeTranDef
        Dim SegmentDef As FieldDefSet = New FieldDefSet()
        SegmentDef.Name = "PO4"
        SegmentDef.FieldDefList.Add(New FieldDef("F01_PACK", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F02_SIZE", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F03_UOM", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F04_UNKNOWN", "ALPHA", 3, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F05_WEIGHT_QUAL", "ALPHA", 8, "NONE", "No", 0, "CCYYMMDD", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F06_GROSS_WEIGHT", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F07_UNIT_MEASURE", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F08_UNKNOWN", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F09_UNKNOWN", "ALPHA", 3, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F10_LENGTH", "ALPHA", 8, "NONE", "No", 0, "CCYYMMDD", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F11_WIDTH", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F12_HEIGHT", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F13_INNER_PACK", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F14_UNK", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", ""))

        Dim writer As New System.Xml.Serialization.XmlSerializer(GetType(FieldDefSet))
        Dim file As New System.IO.StreamWriter(path:=ConfigInfo.SegmentDefDir & "\" & SegmentDef.Name & ".def")
        MyBase.FieldDefs = SegmentDef
        writer.Serialize(file, SegmentDef)
        file.Close()


    End Sub
End class
