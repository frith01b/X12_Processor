Public Class AK5
    Inherits Segment
 Implements SegTranslate 
Public Sub New()
        MyBase.New("AK5")
    End Sub 
  
  
  

    'F01_TRANS_SET_ACK, F02_TRAN_ERR_CODE 
   Public Overloads Sub InitializeTranDef() Implements SegTranslate.InitializeTranDef 
  
  
  
        Dim SegmentDef As FieldDefSet = New FieldDefSet()
        SegmentDef.Name = "AK5"
        SegmentDef.FieldDefList.add(New FieldDef("F01_TRANS_SET_ACK", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F02_TRAN_ERR_CODE", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", ""))
        
dim writer as  New System.Xml.Serialization.XmlSerializer(GetType(FieldDefSet))
        Dim file As New System.IO.Streamwriter(Path:=ConfigInfo.SegmentDefDir  & "\" &  SegmentDef.Name & ".def")
        MyBase.FieldDefs = SegmentDef
writer.Serialize(file, SegmentDef)
        file.Close()
    

end sub
 end class
