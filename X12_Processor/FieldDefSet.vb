Public Class FieldDefSet
    Public Name As String
    Public FieldDefList() As FieldDef

    Public Sub New(FieldCount As Integer)
        ReDim FieldDefList(FieldCount)
    End Sub
    Public Sub New()
        ReDim FieldDefList(15)
    End Sub

End Class
