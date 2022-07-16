Option Strict On
Option Explicit On
'Ordered set of segments and nested loops of other segment sets
Public Class X12_850_Request_RecordSet
    Inherits RecordSet
    ' composed of fields or loop sets or individual segments

    Sub New()
        UpdateRecSetDefFile(ConfigInfo.RecordSetDir & "\RecordSet_850.def")
        If GetRecList() Is Nothing Then
            Load_RecordDef()
        End If
    End Sub

    Sub SetDefaults()

    End Sub
End Class


