Option Strict On
Option Explicit On
'Ordered set of segments and nested loops of other segment sets
Public Class X12_850_Request_RecordSet
    Inherits RecordSet
    Implements RecordFunctions

    ' composed of fields or loop sets of individual segments
    Shared Rec_Def_850 As List(Of RecDefItem)
    Sub New()
        UpdateRecSetDefFile(ConfigInfo.RecordSetDir & "\RecordSet_850.def")

        If Rec_Def_850 Is Nothing Then
            Load_RecordDef()
            Rec_Def_850 = RecDefList
        End If
    End Sub

    Sub SetDefaults()

    End Sub
    ' Public Overloads Sub Import(Seg As Segment, SourceRecNum As Long) Implements RecordFunctions.Import

    ' End Sub
End Class