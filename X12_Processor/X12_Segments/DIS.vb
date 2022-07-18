Public Class DIS
    Inherits Segment
 Implements SegTranslate 
Public Sub New()  
 MyBase.New("") 
  End Sub 
  
  
  

    'F01_DIs_id
   Public Overloads Sub InitializeTranDef() Implements SegTranslate.InitializeTranDef 
  
  
  
        Dim SegmentDef As FieldDefSet = New FieldDefSet()
        SegmentDef.Name = "DIS"
        SegmentDef.FieldDefList.add(New FieldDef("F01_DIs_id", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", ""))
        
dim writer as  New System.Xml.Serialization.XmlSerializer(GetType(FieldDefSet))
        Dim file As New System.IO.Streamwriter(Path:=ConfigInfo.SegmentDefDir  & "\" &  SegmentDef.Name & ".def")
        MyBase.FieldDefs = SegmentDef
writer.Serialize(file, SegmentDef)
        file.Close()
    

end sub
 end class
