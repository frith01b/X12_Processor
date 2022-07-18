Public Class ITD
    '    F01_TERMS_TXT As String
    '        Public F02_Term_Basis As String
    '    Public F03_Terms_Discount As String
    '    Public F04_Terms_Due_Date As String
    '    Public F05_Terms_Days As String
    '    Public F06_Terms_Net_Due_Date As String
    '    Public F07_UNK As String
    '    Public F08_Terms_Disc_Amt As String
    '    Public F09_UNK As String
    '    Public F10_UNK As String
    '    Public F11_UNK As String
    '    Public F12_Terms_Desc As String
    Inherits Segment
 Implements SegTranslate 
Public Sub New()  
 MyBase.New("") 
  End Sub 
  
  
  

   Public Overloads Sub InitializeTranDef() Implements SegTranslate.InitializeTranDef 
  
  
  
        Dim SegmentDef As FieldDefSet = New FieldDefSet()
        SegmentDef.Name = "ITD"
        SegmentDef.FieldDefList.add(New FieldDef("F01_TERMS_TXT", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F02_Term_Basis", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F03_Terms_Discount", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F04_Terms_Due_Date", "ALPHA", 3, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F05_Terms_Days", "ALPHA", 8, "NONE", "No", 0, "CCYYMMDD", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F06_Terms_Net_Due_Date", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F07_UNK", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F08_Terms_Disc_Amt", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F09_UNK", "ALPHA", 3, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F10_UNK", "ALPHA", 8, "NONE", "No", 0, "CCYYMMDD", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F11_UNK", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F12_Terms_Desc", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", ""))
        
dim writer as  New System.Xml.Serialization.XmlSerializer(GetType(FieldDefSet))
        Dim file As New System.IO.Streamwriter(Path:=ConfigInfo.SegmentDefDir  & "\" &  SegmentDef.Name & ".def")
        MyBase.FieldDefs = SegmentDef
writer.Serialize(file, SegmentDef)
        file.Close()
    

end sub
 end class
