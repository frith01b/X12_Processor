Public Class Rec_Data_Type
    Dim SegData As List(Of SegTranslate)
    Dim SegLoops As List(Of Rec_Data_Type)
    Dim ItemName As String
    Dim ItemIndex As Long
    ' points to above hierarchy in child elements.
    Dim Parent As Object


    Public Sub New()
        SegData = New List(Of SegTranslate)
        SegLoops = New List(Of Rec_Data_Type)
        Parent = Nothing
    End Sub
    Public Sub add(mySegment As Rec_Data_Type)
        SegData.Add(mySegment)
    End Sub
End Class
