Public Class BGN
    Inherits Segment
 Implements SegTranslate 
Public Sub New()  
 MyBase.New("") 
  End Sub 
  
  
  

    'F01_Tran_Set_Purpose As String
    'Public F02_Ref_ID As String
    'Public F03_Date As String
    'Public F04_Time As String
    'Public F05_Currency As String
    'Public F06_Tran_Set_Ref As String
   Public Overloads Sub InitializeTranDef() Implements SegTranslate.InitializeTranDef 
  
  
  
        Dim SegmentDef As FieldDefSet = New FieldDefSet()
        SegmentDef.Name = "BGN"
        SegmentDef.FieldDefList.add(New FieldDef("F01_Tran_Set_Purpose", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F02_Ref_ID", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F03_Date", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F04_Time", "ALPHA", 3, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F05_Currency", "ALPHA", 8, "NONE", "No", 0, "CCYYMMDD", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F06_Tran_Set_Ref", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", ""))
        
dim writer as  New System.Xml.Serialization.XmlSerializer(GetType(FieldDefSet))
        Dim file As New System.IO.Streamwriter(Path:=ConfigInfo.SegmentDefDir  & "\" &  SegmentDef.Name & ".def")
        MyBase.FieldDefs = SegmentDef
writer.Serialize(file, SegmentDef)
        file.Close()
    

end sub
 end class
