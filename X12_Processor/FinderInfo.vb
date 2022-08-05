''' <summary>
''' Return value for function WhereIsNextSegment
''' </summary>
Public Class FinderInfo
    Private _RecordName As String
    Private _Seq As Long
    Private _NextRecPosition As Rec_Def_Relation = Rec_Def_Relation.NotFound
    Private _EndOfLoop As Boolean = False
    Private _StartOfLoop As Boolean = False
    Public Enum Rec_Def_Relation
        NotFound = 1
        Sibling = 2
        Child = 3
        Parent = 4
        Mandatory_Missing = 5
    End Enum

    Property EndOfLoop As Boolean
        Get
            Return _EndOfLoop
        End Get
        Set
            _EndOfLoop = Value
        End Set
    End Property
    Property StartOfLoop As Boolean
        Get
            Return _StartOfLoop
        End Get
        Set
            _StartOfLoop = Value
        End Set
    End Property


    Property NextRecPosition As Rec_Def_Relation
        Get
            Return _NextRecPosition
        End Get
        Set
            _NextRecPosition = Value
        End Set
    End Property

    Property RecordName As String
        Get
            Return _RecordName
        End Get
        Set
            _RecordName = Value
        End Set
    End Property
    Property Seq As Long
        Get
            Return _Seq
        End Get
        Set
            _Seq = Value
        End Set
    End Property

End Class
