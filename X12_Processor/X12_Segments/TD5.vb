﻿Public Class TD5
    'CUSTOMER1
    'F01_IC_QUAL, F02_IC_CODE, F03_TRAN_TYPE, F04_ROUTE, F05_SVC
    'STD
    'F01_Packaging_Code, F02_Lading_Qty, F03_ID_CODE_QUAL, F04_ID_CODE, F05_SVC, F06_Weight_Qual, F07_Weight, F08_UM
    Inherits Segment
    Public Shared Sub InitializeTranDef()
        Dim SegmentDef As FieldDefSet = New FieldDefSet(5)
        SegmentDef.Name = "TD5"
        SegmentDef.FieldDefList(0) = New FieldDef("F01_Packaging_Code", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(1) = New FieldDef("F02_Lading_Qty", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", "")
        SegmentDef.FieldDefList(2) = New FieldDef("F03_ID_CODE_QUAL", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(3) = New FieldDef("F04_ID_CODE", "ALPHA", 3, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(4) = New FieldDef("F05_SVC", "ALPHA", 8, "NONE", "No", 0, "CCYYMMDD", "NONE", " ", "")
        SegmentDef.FieldDefList(5) = New FieldDef("F06_Weight_Qual", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", "")
        SegmentDef.FieldDefList(6) = New FieldDef("F07_Weight", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", "")
        SegmentDef.FieldDefList(7) = New FieldDef("F08_UM", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", "")
        Dim writer As New System.Xml.Serialization.XmlSerializer(GetType(FieldDefSet))
        Dim file As New System.IO.StreamWriter(path:=ConfigInfo.SegmentDefDir & SegmentDef.Name & ".def")
        writer.Serialize(file, SegmentDef)
        file.Close()
    End Sub

End Class