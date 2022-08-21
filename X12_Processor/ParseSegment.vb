Option Strict On
Option Explicit On
Imports System.Collections.ObjectModel

Public Class ParseSegment
    Private _elements As String()
    Private _segID As String


    Public Property Elements As ReadOnlyCollection(Of String)
        Get
            Return New ReadOnlyCollection(Of String)(_elements)
        End Get
        Set(value As ReadOnlyCollection(Of String))

        End Set
    End Property

    Public Property SegID As String
        Get
            Return _segID
        End Get
        Set(value As String)
            _segID = value
        End Set
    End Property

    Public Sub New(NewSeg As String, NewElements As String())
        _segID = NewSeg
        _elements = NewElements
    End Sub


    Private Function Ranks(ByVal RankDefinition As XElement, ByVal Segments As IEnumerable(Of ParseSegment)) As IEnumerable(Of XStreamingElement)
        If RankDefinition.Name.LocalName = "Rank" Then
            Dim BeginningSegment As String = RankDefinition.Elements("Segment").First().Attribute("ID").Value
            Dim EndingSegment As String = RankDefinition.Elements("Segment").Last().Attribute("ID").Value
            Dim SegmentGroups As List(Of IEnumerable(Of ParseSegment)) = New List(Of IEnumerable(Of ParseSegment))()
            Dim CurrentGroup As List(Of ParseSegment) = Nothing
            For Each seg As ParseSegment In Segments
                If seg.SegID = BeginningSegment Then CurrentGroup = New List(Of ParseSegment)()
                If CurrentGroup IsNot Nothing Then CurrentGroup.Add(seg)
                If seg.SegID = EndingSegment Then
                    SegmentGroups.Add(CurrentGroup)
                    CurrentGroup = Nothing
                End If
            Next

            Return From g In SegmentGroups Select New XStreamingElement(RankDefinition.Attribute("Name").Value.Replace(" "c, "_"c), From e In RankDefinition.Elements() Select Ranks(e, g))
        End If

        If RankDefinition.Name.LocalName = "Segment" Then
            Dim Matching = From s In Segments Where s.SegID = RankDefinition.Attribute("ID").Value Select s
            Return {New XStreamingElement(RankDefinition.Attribute("Name").Value.Replace(" "c, "_"c), From s In Matching From e In RankDefinition.Elements("Element") Where s.Elements.Count >= Integer.Parse(e.Attribute("Position").Value) Select New XElement(e.Attribute("Name").Value.Replace(" "c, "_"c), s.Elements(Integer.Parse(e.Attribute("Position").Value) - 1)))}
        End If

        Return Nothing
    End Function



End Class
