Public Class GE
    'F01_SET_QTY As Long
    'Public F02_GROUP_END_NUM
    Inherits Segment
 Implements SegTranslate 
Public Sub New()
        MyBase.New("GE")
    End Sub 
  
  
  

   Public Overloads Sub InitializeTranDef() Implements SegTranslate.InitializeTranDef 
  
  
  
        Dim SegmentDef As FieldDefSet = New FieldDefSet()
        SegmentDef.Name = "GE"
        SegmentDef.FieldDefList.add(New FieldDef("F01_SET_QTY", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F02_GROUP_END_NUM", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", ""))
        
dim writer as  New System.Xml.Serialization.XmlSerializer(GetType(FieldDefSet))
        Dim file As New System.IO.Streamwriter(Path:=ConfigInfo.SegmentDefDir  & "\" &  SegmentDef.Name & ".def")
        MyBase.FieldDefs = SegmentDef
writer.Serialize(file, SegmentDef)
        file.Close()
    

end sub
 end class
