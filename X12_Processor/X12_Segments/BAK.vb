Public Class BAK
    Inherits Segment
 Implements SegTranslate 
Public Sub New()  
 MyBase.New("") 
  End Sub 
  
  
  

    'F01_Purpose_code As String
    'Public F02_Ack_Type As String
    'Public F03_PO_Num As String
    'Public F04_Orig_PO_Date As String
    'Public F05_UNK As String
    'Public F06_UNK As String
    'Public F07_UNK As String
    'Public F08_UNK As String
    'Public F09_StatusDate As String
   Public Overloads Sub InitializeTranDef() Implements SegTranslate.InitializeTranDef 
  
  
  
        Dim SegmentDef As FieldDefSet = New FieldDefSet()
        SegmentDef.Name = "BAK"
        SegmentDef.FieldDefList.add(New FieldDef("F01_Purpose_code", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F02_Ack_Type", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F03_PO_Num", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F04_Orig_PO_Date", "ALPHA", 3, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F05_UNK", "ALPHA", 8, "NONE", "No", 0, "CCYYMMDD", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F06_UNK", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F07_UNK", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F08_UNK", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F09_StatusDate", "ALPHA", 3, "NONE", "No", 0, "", "NONE", " ", ""))
        
dim writer as  New System.Xml.Serialization.XmlSerializer(GetType(FieldDefSet))
        Dim file As New System.IO.Streamwriter(Path:=ConfigInfo.SegmentDefDir & "\" & SegmentDef.Name & ".def")

        MyBase.FieldDefs = SegmentDef
writer.Serialize(file, SegmentDef)
        file.Close()
    

end sub
 end class
