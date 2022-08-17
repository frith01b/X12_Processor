Option Strict On
Option Explicit On

Public Class ISA
    Inherits Segment
    Implements SegTranslate
    Public Sub New()
        MyBase.New("ISA")
    End Sub

    Public Overloads Sub InitializeTranDef() Implements SegTranslate.InitializeTranDef
        Dim SegmentDef As FieldDefSet = New FieldDefSet()
        SegmentDef.Name = "ISA"
        SegmentDef.FieldDefList.Add(New FieldDef("F01_AUTH", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F02_AUTH_INF", "NUMERIC", 10, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F03_SECURITY", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F04_SEC_INF", "ALPHA", 10, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F05_SOURCE_QUAL", "ALPHA", 2, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F06_SOURCE_ID", "ALPHA", 15, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F07_RECEIVER_QUAL", "NUMERIC", 2, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F08_RECEIVER_ID", "ALPHA", 15, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F09_RECV_DATE", "ALPHA", 6, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F10_RECV_TIME", "ALPHA", 4, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F11_INTL", "ALPHA", 1, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F12_ISA_VERSION", "NUMERIC", 5, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F13_ICN", "ALPHA", 9, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F14_ACK", "ALPHA", 1, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F15_TEST", "ALPHA", 1, "NONE", "No", 0, "", "NONE", " ", ""))
        SegmentDef.FieldDefList.Add(New FieldDef("F16_SUBELEMENTDELIM", "ALPHA", 1, "NONE", "No", 0, "", "NONE", " ", ""))
        MyBase.FieldDefs = SegmentDef

        Dim writer As New System.Xml.Serialization.XmlSerializer(GetType(FieldDefSet))
        Dim file As New System.IO.StreamWriter(path:=ConfigInfo.SegmentDefDir & "\" & SegmentDef.Name & ".def")
        MyBase.FieldDefs = SegmentDef
        writer.Serialize(file, SegmentDef)
        file.Close()

    End Sub

    Public Overloads Function Output_X12() As String Implements SegTranslate.Output_X12
        Dim retVal As String = ""
        ' @TODO  make copy to allow for updating without changing

        Fields("F05_SOURCE_QUAL") = Interchange.CurrentPartnerInfo.GetSender_Qualifier
        Fields("F06_SOURCE_ID") = Interchange.CurrentPartnerInfo.GetSender_to_Host_ID
        Fields("F07_RECEIVER_QUAL") = Interchange.CurrentPartnerInfo.GetHost_Qualifier()
        Fields("F08_RECEIVER_ID") = Interchange.CurrentPartnerInfo.GetHost_to_Sender_ID

        'need to substring & PAD & Strip quotes

        retVal &= Get_Field_LPAD(RecordName, 3, " ")

        retVal &= Get_Field_LPAD(Fields("F01_AUTH"), 2, " ")
        retVal &= Get_Field_LPAD(Fields("F02_AUTH_INF"), 10, " ")
        retVal &= Get_Field_LPAD(Fields("F03_SECURITY"), 2, " ")
        retVal &= Get_Field_LPAD(Fields("F04_SEC_INF"), 10, " ")
        retVal &= Get_Field_LPAD(Fields("F05_SOURCE_QUAL"), 2, " ")
        retVal &= Get_Field_LPAD(Fields("F06_SOURCE_ID"), 15, " ")
        retVal &= Get_Field_LPAD(Fields("F07_RECEIVER_QUAL"), 2, " ")
        retVal &= Get_Field_LPAD(Fields("F08_RECEIVER_ID"), 15, " ")
        retVal &= Get_Field_LPAD(Fields("F09_RECV_DATE"), 6, " ")
        retVal &= Get_Field_LPAD(Fields("F10_RECV_TIME"), 4, " ")
        retVal &= Get_Field_LPAD(Fields("F11_INTL"), 1, " ")
        retVal &= Get_Field_LPAD(Fields("F12_ISA_VERSION"), 5, "0")
        retVal &= Get_Field_LPAD(Fields("F13_ICN"), 9, "0")
        retVal &= Get_Field_LPAD(Fields("F14_ACK"), 1, " ")
        retVal &= Get_Field_LPAD(Fields("F15_TEST"), 1, " ")
        retVal &= Interchange.SubFieldDelimiter
        retVal &= Interchange.FieldDelimiter
        Return retVal

    End Function

End Class
