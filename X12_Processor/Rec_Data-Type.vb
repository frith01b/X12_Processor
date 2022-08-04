Public Class Rec_Data_Type
    Dim SegData As List(Of SegTranslate)
    Dim SegLoops As List(Of Rec_Data_Type)
    Dim ItemName As String
    Property ItemIndex As Long
        Get
            Return _ItemIndex
        End Get
        Set
            _ItemIndex = Value
        End Set
    End Property
    ''' points to above hierarchy in child elements.
    Dim Parent As Object
    Private _ItemIndex As Long

    Public Sub New()
        SegData = New List(Of SegTranslate)
        SegLoops = New List(Of Rec_Data_Type)
        ItemIndex = 0
        Parent = Nothing
    End Sub
    Public Sub addSegment(mySegment As SegTranslate)
        SegData.Add(mySegment)
    End Sub
    Public Sub addLoop(myLoop As Rec_Data_Type)
        SegLoops.Add(myLoop)
    End Sub
End Class
