Option Strict On
Option Explicit On
'''Ordered set of segments and nested loops of other segment sets

Public Class X12_850_Request_RecordSet
    Inherits RecordSet
    Implements RecordSetMethods_Intf

    ''' composed of fields or loop sets of individual segments
    Shared Rec_Def_850 As List(Of RecDefItem)
    Sub New()
        UpdateRecSetDefFile(ConfigInfo.RecordSetDir & "\RecordSet_850.def", "850")

        If Rec_Def_850 Is Nothing Then
            'load definition from file into Recordset.RecDefList
            Load_RecordDef()
            'save to shared member
            Rec_Def_850 = RecDefList
        Else
            ' load from shared member
            RecDefList = Rec_Def_850
            MyBase.ResetCurrrentPtr()
        End If
    End Sub



    Sub SetDefaults()
        Debug.Print("not yet implemented")
    End Sub
    ' Public Overloads Sub Import(Seg As Segment, SourceRecNum As Long) Implements RecordFunctions.Import

    ' End Sub
End Class