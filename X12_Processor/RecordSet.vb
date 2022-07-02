Option Strict On
Option Explicit On
Imports System.Dynamic
Imports X12_Processor

Interface RecordFunctions
    Sub Import(Seg As Segment, SourceRecNum As Long)
    Function GetData() As RecordSet
    ' Filter removes extraneous notes/data
    Function Filter() As String()
    'Remap adjusts incoming data to correct item #'s, code values , etc
    Sub ReMap()
    Function Output_X12() As String
    Function Validate() As Boolean

    Property Fields As Dictionary(Of String, Object)

End Interface

Partial Public Class RecordSet
    Inherits DynamicObject
    Implements RecordFunctions

    Dim Rec_Count As Long
    Dim RecData() As String

    Public Property Fields As Dictionary(Of String, Object) Implements RecordFunctions.Fields
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Dictionary(Of String, Object))
            Throw New NotImplementedException()
        End Set
    End Property

    Public Sub Import(Seg As Segment, SourceRecNum As Long) Implements RecordFunctions.Import
        Throw New NotImplementedException()
    End Sub

    Public Sub ReMap() Implements RecordFunctions.ReMap
        Throw New NotImplementedException()
    End Sub

    Public Function GetData() As RecordSet Implements RecordFunctions.GetData
        Throw New NotImplementedException()
    End Function

    Public Function Filter() As String() Implements RecordFunctions.Filter
        Throw New NotImplementedException()
    End Function

    Public Function Output_X12() As String Implements RecordFunctions.Output_X12
        Throw New NotImplementedException()
    End Function

    Public Function Validate() As Boolean Implements RecordFunctions.Validate
        Throw New NotImplementedException()
    End Function
End Class
