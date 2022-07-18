Option Strict On
Option Explicit On

Public Class AK2
    Inherits Segment
 Implements SegTranslate 
Public Sub New()
        MyBase.New("AK2")
    End Sub 
  
  
  

    'F01_TRAN_SET_ID, F02_SET_CONTROL_NUM, F03_IMP_REF

   Public Overloads Sub InitializeTranDef() Implements SegTranslate.InitializeTranDef 
  
  
  
        Dim SegmentDef As FieldDefSet = New FieldDefSet()
        SegmentDef.Name = "ISA"
        SegmentDef.FieldDefList.add(New FieldDef("F01_TRAN_SET_ID", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F02_SET_CONTROL_NUM", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F03_IMP_REF", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", ""))
        
dim writer as  New System.Xml.Serialization.XmlSerializer(GetType(FieldDefSet))
        Dim file As New System.IO.Streamwriter(Path:=ConfigInfo.SegmentDefDir  & "\" &  SegmentDef.Name & ".def")
        MyBase.FieldDefs = SegmentDef
writer.Serialize(file, SegmentDef)
        file.Close()
    

end sub
 end class
