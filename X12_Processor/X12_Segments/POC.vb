Public Class POC
    'F01_PO_SEQ_ID, F02_RESPONSE_CODE, F03_PO_QTY, F04_UNUSED, F05_Unused, F06_PRICE, F07_UNUSED, F08_PROD_QUAL, F09_PROD_ID, F10_VEND_QUAL, F11_VEND_ID, F12_BUY_QUAL, F13_BUY_ID, F14_GLOBAL_QUAL, F15_GLOBAL_ID
    Inherits Segment
 Implements SegTranslate 
Public Sub New()
        MyBase.New("POC")
    End Sub 
  
  
  

   Public Overloads Sub InitializeTranDef() Implements SegTranslate.InitializeTranDef 
  
  
  
        Dim SegmentDef As FieldDefSet = New FieldDefSet()
        SegmentDef.Name = "POC"
        SegmentDef.FieldDefList.add(New FieldDef("F01_PO_SEQ_ID", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F02_RESPONSE_CODE", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F03_PO_QTY", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F04_UNUSED", "ALPHA", 3, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F05_Unused", "ALPHA", 8, "NONE", "No", 0, "CCYYMMDD", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F05_Unused", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F07_UNUSED", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F08_PROD_QUAL", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F09_PROD_ID", "ALPHA", 3, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F10_VEND_QUAL", "ALPHA", 8, "NONE", "No", 0, "CCYYMMDD", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F11_VEND_ID", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F12_BUY_QUAL", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F13_BUY_ID", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F14_GLOBAL_QUAL", "ALPHA", 3, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F15_GLOBAL_ID", "ALPHA", 8, "NONE", "No", 0, "CCYYMMDD", "NONE", " ", ""))
        
dim writer as  New System.Xml.Serialization.XmlSerializer(GetType(FieldDefSet))
        Dim file As New System.IO.Streamwriter(Path:=ConfigInfo.SegmentDefDir  & "\" &  SegmentDef.Name & ".def")
        MyBase.FieldDefs = SegmentDef
writer.Serialize(file, SegmentDef)
        file.Close()
    


end sub
 end class
