Public Class FOB
    'F01_SHIP_PAY, F02_SHIP_LOC_QUAL1, F03_SHIP_LOC1, F04_SHIP_LOC_QUAL2, F05_SHIP_LOC2
    Inherits Segment
 Implements SegTranslate 
Public Sub New()
        MyBase.New("FOB")
    End Sub 
  
  
  

   Public Overloads Sub InitializeTranDef() Implements SegTranslate.InitializeTranDef 
  
  
  
        Dim SegmentDef As FieldDefSet = New FieldDefSet()
        SegmentDef.Name = "ISA"
        SegmentDef.FieldDefList.add(New FieldDef("F01_SHIP_PAY", "ALPHA", 5, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F02_SHIP_LOC_QUAL1", "NUMERIC", 15, "NONE", "No", 0, "#,###.00", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F03_SHIP_LOC1", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F04_SHIP_LOC_QUAL2", "ALPHA", 3, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.add(New FieldDef("F05_SHIP_LOC2", "ALPHA", 8, "NONE", "No", 0, "CCYYMMDD", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F06_UNK", "ALPHA", 8, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F07_UNK", "ALPHA", 8, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F08_UNK", "ALPHA", 8, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F09_UNK", "ALPHA", 8, "NONE", "No", 0, "", "NONE", " ", ""))
        Dim writer As New System.Xml.Serialization.XmlSerializer(GetType(FieldDefSet))
        Dim file As New System.IO.Streamwriter(Path:=ConfigInfo.SegmentDefDir  & "\" &  SegmentDef.Name & ".def")
        MyBase.FieldDefs = SegmentDef
writer.Serialize(file, SegmentDef)
        file.Close()
    

end sub
 end class
