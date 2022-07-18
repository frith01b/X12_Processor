Public Class REF
    'F01_REF_ID, F02_REF_TXT 
    Inherits Segment
 Implements SegTranslate 
Public Sub New()
        MyBase.New("REF")
    End Sub 
  
  
  

   Public Overloads Sub InitializeTranDef() Implements SegTranslate.InitializeTranDef 
  
  
  
        Dim SegmentDef As FieldDefSet = New FieldDefSet()
        SegmentDef.Name = "REF"
        SegmentDef.FieldDefList.add(New FieldDef("F01_REF_ID", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F02_REF_TXT", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", ""))
        
dim writer as  New System.Xml.Serialization.XmlSerializer(GetType(FieldDefSet))
        Dim file As New System.IO.Streamwriter(Path:=ConfigInfo.SegmentDefDir  & "\" &  SegmentDef.Name & ".def")
        MyBase.FieldDefs = SegmentDef
writer.Serialize(file, SegmentDef)
        file.Close()
    

end sub
 end class
