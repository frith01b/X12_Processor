Public Class PO1
    'F01_PO_SEQ_ID, F02_PO_QTY, F03_UOM, F04_PRICE, F05_Unused, F06_PROD_QUAL, F07_PROD_ID, F08_VEND_QUAL, F09_VEND_ID, F10_BUY_QUAL, F11_BUY_ID, F12_GLOBAL_QUAL, F13_GLOBAL_ID
    Inherits Segment
 Implements SegTranslate 
Public Sub New()
        MyBase.New("PO1")
    End Sub 
  
  
  

   Public Overloads Sub InitializeTranDef() Implements SegTranslate.InitializeTranDef 
  
  
  
        Dim SegmentDef As FieldDefSet = New FieldDefSet()
        SegmentDef.Name = "PO1"
        SegmentDef.FieldDefList.add(New FieldDef("F01_PO_SEQ_ID", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F02_PO_QTY", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F03_UOM", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F04_PRICE", "ALPHA", 3, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F05_Unused", "ALPHA", 8, "NONE", "No", 0, "CCYYMMDD", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F06_PROD_QUAL", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F07_PROD_ID", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F08_VEND_QUAL", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F09_VEND_ID", "ALPHA", 3, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F10_BUY_QUAL", "ALPHA", 8, "NONE", "No", 0, "CCYYMMDD", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F11_BUY_ID", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F12_GLOBAL_QUAL", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F13_GLOBAL_ID", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", ""))
        
dim writer as  New System.Xml.Serialization.XmlSerializer(GetType(FieldDefSet))
        Dim file As New System.IO.Streamwriter(Path:=ConfigInfo.SegmentDefDir  & "\" &  SegmentDef.Name & ".def")
        MyBase.FieldDefs = SegmentDef
writer.Serialize(file, SegmentDef)
        file.Close()
    

end sub
 end class
