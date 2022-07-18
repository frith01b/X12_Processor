Public Class CTT
    Inherits Segment
 Implements SegTranslate 
Public Sub New()
        MyBase.New("CTT")
    End Sub 
  
  
  

    'F01_PO_LINE_COUNT, F02_PO_LINE_QTY
   Public Overloads Sub InitializeTranDef() Implements SegTranslate.InitializeTranDef 
  
  
  
        Dim SegmentDef As FieldDefSet = New FieldDefSet()
        SegmentDef.Name = "CTT"
        SegmentDef.FieldDefList.add(New FieldDef("F01_PO_LINE_COUNT", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F02_PO_LINE_QTY", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", ""))
        
dim writer as  New System.Xml.Serialization.XmlSerializer(GetType(FieldDefSet))
        Dim file As New System.IO.Streamwriter(Path:=ConfigInfo.SegmentDefDir  & "\" &  SegmentDef.Name & ".def")
        MyBase.FieldDefs = SegmentDef
writer.Serialize(file, SegmentDef)
        file.Close()
    

end sub
 end class
