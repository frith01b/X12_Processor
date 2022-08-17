Option Strict On
Option Explicit On


<Serializable()>
Public Class PartnerInfo
    ''' AS2 Identifier = Partner ID
    Private _PartnerID As String
    ''' Interaction/Map / X12 Version
    Private _Version As String
    ''' From X12 ID Vendor ID to Host  (how they identify themselves to us
    Private _Sender_Qualifier As String
    Private _Sender_to_Host_ID As String
    ' How They identify us as  receiver
    Private _Host_Qualifier As String
    Private _Host_to_Sender_ID As String
    Private _Common_Name As String
    ''' Adjustments to transaction processing
    Private _Customizations As Dictionary(Of String, String)

    '''Tran_810_Batch_Num 
    '''    Tran_812_Batch_Num 
    '''    Tran_846_Batch_Num 
    '''    Tran_855_Batch_Num 
    '''    Tran_856_Batch_Num 
    '''    Tran_997_Batch_Num 
    '''    price_code 
    '''    M2K_PARTNER_ID 
    '''    partner_found 
    '''    N104 
    '''    Generate_812 
    '''    T810_Include_ITD 
    '''    T810_Include_FOB 
    '''    T810_Include_CTT 
    '''    T810_Use_N104_Code 
    '''    T810_N1_F1_EIC_Shipper_Code 
    '''    T810_N1_F4_RI_Code 
    '''    T810_Include_CAD 
    '''    T810_Cad_F2_Prefix 
    '''    T810_Alt_Cad_Layout 
    '''    T810_IT1_Item_Qual 
    '''    T810_IT1_Customer_Item_Qual 
    '''    T810_CTP_F8_Amt 
    '''    T810_ITD_Terms 
    '''    T810_CDD_F2_Item_Qual 
    '''    T810_N1_SET_TYPE 
    '''    T810_BIG_CreditMemo 
    '''    T856_HL_DTM_Output_PRE_POST 
    '''    T856_HL_DTM_Output_POST 
    '''    T856_CTT_Output 
    '''    X12_Always_Delimiter 
    '''    T850_Flip_Addr1_Addr2 
    '''    T855_Output_Ref_segment 
    '''    T856_Ship_Date_Ref 
    '''    T810_TRACKING_NTE 
    '''    T810_GS_08_VER 
    '''    T810_Include_PO4 
    '''    Our_Cust_Number

    Public Sub New(MyPartnerID As String)
        SetPartnerID(MyPartnerID)
    End Sub
    Public Sub New()
        'Default = get from file
        SetPartnerID("FROM_AS2")
    End Sub
    Property Common_Name As String
        Get
            Return _Common_Name
        End Get
        Set
            _Common_Name = Value
        End Set
    End Property

    Public Function GetHost_Qualifier() As String
        Return _Host_Qualifier
    End Function

    Public Sub SetHost_Qualifier(AutoPropertyValue As String)
        _Host_Qualifier = AutoPropertyValue
    End Sub

    Public Function GetSender_Qualifier() As String
        Return _Sender_Qualifier
    End Function

    Public Sub SetSender_Qualifier(AutoPropertyValue As String)
        _Sender_Qualifier = AutoPropertyValue
    End Sub
    ''' to X_12 ID Vendor ID to Host ( how they want to identify us when sending
    ''' AS2 Host ID 
    Property Host_ID As String

    Public Function GetHost_to_Sender_ID() As String
        Return _Host_to_Sender_ID
    End Function

    Public Sub SetHost_to_Sender_ID(AutoPropertyValue As String)
        _Host_to_Sender_ID = AutoPropertyValue
    End Sub

    Public Function GetSender_to_Host_ID() As String
        Return _Sender_to_Host_ID
    End Function

    Public Sub SetSender_to_Host_ID(AutoPropertyValue As String)
        _Sender_to_Host_ID = AutoPropertyValue
    End Sub

    Public Function GetVersion() As String
        Return _Version
    End Function

    Public Sub SetVersion(AutoPropertyValue As String)
        _Version = AutoPropertyValue
    End Sub

    Public Function GetCustomizations() As Dictionary(Of String, String)
        Return _Customizations
    End Function

    Public Sub SetCustomizations(AutoPropertyValue As Dictionary(Of String, String))
        _Customizations = AutoPropertyValue
    End Sub


    Public Function GetPartnerID() As String
        Return _PartnerID
    End Function

    Public Sub SetPartnerID(AutoPropertyValue As String)
        _PartnerID = AutoPropertyValue
    End Sub


    '''**********************************************************
    Public Sub SavePartnerInfo(MyPartnerFile As String)
        Dim writer As New System.Xml.Serialization.XmlSerializer(GetType(ConfigInfo))
        Dim file As New System.IO.StreamWriter(path:=MyPartnerFile)
        writer.Serialize(file, Me)
        file.Close()

    End Sub
End Class
