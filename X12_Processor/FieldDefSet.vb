Public Class FieldDefSet
    Public Name As String
    Public FieldDefList As List(Of FieldDef)


    ReadOnly Property FieldCount As Integer
        Get
            Return FieldDefList.Count
        End Get
    End Property

    Public Sub New(FieldCount As Integer)
        FieldDefList = New List(Of FieldDef)
    End Sub
    Public Sub New()
        FieldDefList = New List(Of FieldDef)
    End Sub

End Class
