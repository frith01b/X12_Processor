Option Strict On
Option Explicit On

Public Class ACK
    Inherits Segment
    Implements SegTranslate
    Public Sub New()
        MyBase.New("ACK")
    End Sub
    'F01_LINE_ITEM_STATUS, F02_QTY, F03_UOM, F04_DATE_QUAL, F05_STATUS_DATE 


    '   DEFAULTS    1A              #       EA         079            CURRENT + 14

    ' status codes :
    'DR Item Accepted - Date Reschedule
    'IA Item Accepted
    'IB Item Backordered
    'IC Item Accepted - Changes Made
    'IP Item Accepted - Price Changed
    'IQ Item Accepted - Quantity Changed
    'IR Item Rejected

    '

    Public Overloads Sub InitializeTranDef() Implements SegTranslate.InitializeTranDef



        Dim SegmentDef As FieldDefSet = New FieldDefSet()
        SegmentDef.Name = "ACK"
        SegmentDef.FieldDefList.Add(New FieldDef("F01_LINE_ITEM_STATUS", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F02_QTY", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F03_UOM", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F04_DATE_QUAL", "ALPHA", 3, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F05_STATUS_DATE", "DATE", 8, "NONE", "No", 0, "CCYYMMDD", "NONE", " ", ""))
        
dim writer as  New System.Xml.Serialization.XmlSerializer(GetType(FieldDefSet))
        Dim file As New System.IO.Streamwriter(Path:=ConfigInfo.SegmentDefDir  & "\" &  SegmentDef.Name & ".def")
        MyBase.FieldDefs = SegmentDef
writer.Serialize(file, SegmentDef)
        file.Close()

    End Sub
End Class
