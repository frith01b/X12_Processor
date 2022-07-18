Public Class AK9
    Inherits Segment
 Implements SegTranslate 
Public Sub New()
        MyBase.New("AK9")
    End Sub 
  
  
  

    'F01_FUNC_ACK, F02_TRAN_SET_TOTAL, F03_REC_TRAN_CNT, F04_ACCEPT_TRAN_CNT, F05_FUNC_ERR_CODE
   Public Overloads Sub InitializeTranDef() Implements SegTranslate.InitializeTranDef 
  
  
  
        Dim SegmentDef As FieldDefSet = New FieldDefSet()
        SegmentDef.Name = "AK9"
        SegmentDef.FieldDefList.add(New FieldDef("F01_FUNC_ACK", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F02_TRAN_SET_TOTAL", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F03_REC_TRAN_CNT", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F04_ACCEPT_TRAN_CNT", "ALPHA", 3, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F05_FUNC_ERR_CODE", "ALPHA", 8, "NONE", "No", 0, "CCYYMMDD", "NONE", " ", ""))
        
dim writer as  New System.Xml.Serialization.XmlSerializer(GetType(FieldDefSet))
        Dim file As New System.IO.Streamwriter(Path:=ConfigInfo.SegmentDefDir  & "\" &  SegmentDef.Name & ".def")
        MyBase.FieldDefs = SegmentDef
writer.Serialize(file, SegmentDef)
        file.Close()
    

end sub
 end class
