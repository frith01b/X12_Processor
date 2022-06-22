Option Strict On
Option Explicit On

Public Class AK2
    Inherits Segment
    'F01_TRAN_SET_ID, F02_SET_CONTROL_NUM, F03_IMP_REF

    Public Shared Sub InitializeTranDef()
        Dim SegmentDef As FieldDefSet = New FieldDefSet(15)
        SegmentDef.Name = "ISA"
        SegmentDef.FieldDefList(0) = New FieldDef("F01_TRAN_SET_ID", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(1) = New FieldDef("F02_SET_CONTROL_NUM", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", "")
        SegmentDef.FieldDefList(2) = New FieldDef("F03_IMP_REF", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", "")
        Dim writer As New System.Xml.Serialization.XmlSerializer(GetType(FieldDefSet))
        Dim file As New System.IO.StreamWriter(path:=ConfigInfo.SegmentDefDir & SegmentDef.Name & ".def")
        writer.Serialize(file, SegmentDef)
        file.Close()
    End Sub

End Class
