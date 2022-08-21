Option Strict On
Option Explicit On


Imports System.Dynamic

Public Class RecDefItem
    Private _Seq As String
    Private _recname As String
    Private _mytype As String
    Private _myrepeat As String
    Private _EndTrigger As String
    Private _myloop As List(Of RecDefItem) = New List(Of RecDefItem)
    Private _LastRecord As String
    Private _Mandatory As String
    Private _Parent As RecDefItem
    Private _ParentSiblingList As List(Of RecDefItem)
    Public Shared Def_Record_Count As Long

    Property ParentSiblingList As List(Of RecDefItem)
        Get
            Return _ParentSiblingList
        End Get
        Set
            _ParentSiblingList = Value
        End Set
    End Property

    Property Parent As RecDefItem
        Get
            Return _Parent
        End Get
        Set
            _Parent = Value
        End Set
    End Property
    Property Seq As String
        Get
            Return _Seq
        End Get
        Set
            _Seq = Value
        End Set
    End Property

    Property recname As String
        Get
            Return _recname
        End Get
        Set
            _recname = Value
        End Set
    End Property

    Property mytype As String
        Get
            If _mytype IsNot Nothing Then
                Return _mytype
            Else
                Return ""
            End If
        End Get
        Set
            _mytype = Value
        End Set
    End Property

    Property myrepeat As String
        Get
            If _myrepeat IsNot Nothing Then
                Return _myrepeat
            Else
                Return ""
            End If

        End Get
        Set
            _myrepeat = Value
        End Set
    End Property
    Property LastRecord As String
        Get
            If _LastRecord IsNot Nothing Then
                Return _LastRecord
            Else
                Return ""
            End If

        End Get
        Set
            _LastRecord = Value
        End Set
    End Property

    Property Mandatory As String
        Get
            If _Mandatory IsNot Nothing Then
                Return _Mandatory
            Else
                Return ""
            End If
        End Get
        Set
            _Mandatory = Value
        End Set
    End Property

    Property EndTrigger As String
        Get
            If _EndTrigger IsNot Nothing Then
                Return _EndTrigger
            Else
                Return ""
            End If

        End Get
        Set
            _EndTrigger = Value
        End Set
    End Property

    Property myloop As List(Of RecDefItem)
        Get
            If _myloop Is Nothing Then
                Return New List(Of RecDefItem)
            Else
                Return _myloop
            End If
        End Get
        Set
            _myloop = Value
        End Set
    End Property

    Public Sub RecurseRecDef(myDefBlob As ExpandoObject, ByRef NewRecDefList As List(Of RecDefItem))
        ' @TODO Need to add a final def record which identifies end of search, or other way of quitting infinite loop

        UpdateRecDef(myDefBlob, NewRecDefList, Me)
        UpdateChildren(myDefBlob, NewRecDefList, Me)

        NewRecDefList.Sort(Function(X As RecDefItem, Y As RecDefItem)
                               Select Case CInt(X.Seq)
                                   Case Is < CInt(Y.Seq) : Return -1
                                   Case Is > CInt(Y.Seq) : Return 1
                                   Case Else : Return 0
                               End Select
                           End Function)

    End Sub


    Sub UpdateRecDef(RecAttributes As ExpandoObject, MyParentSiblings As List(Of RecDefItem), MyParent As RecDefItem)
        Dim y As Integer
        For y = 0 To RecAttributes.Count - 1
            Select Case RecAttributes(y).Key.ToUpper
                Case "#COMMENT"
                    'ignore comments
                Case "@ENDTRIGGER"
                    _EndTrigger = DirectCast(RecAttributes(y).Value, String)
                Case "@LASTRECORD"
                    _LastRecord = DirectCast(RecAttributes(y).Value, String)
                Case "@MANDATORY"
                    _Mandatory = DirectCast(RecAttributes(y).Value, String)
                Case "@NAME"
                    _recname = DirectCast(RecAttributes(y).Value, String)
                Case "@REPEAT"
                    _myrepeat = DirectCast(RecAttributes(y).Value, String)
                Case "@SEQNUM"
                    _Seq = DirectCast(RecAttributes(y).Value, String)
                Case "@TYPE"
                    _mytype = DirectCast(RecAttributes(y).Value, String)
                Case "@LOOP", "LOOP", "REC_LIST", "@REC_LIST"
                    ' Ignore, will be built by UpdateChildren
                Case Else
                    Interchange.AddError("Unknown record definition field" & RecAttributes(y).Key.ToUpper, Interchange.Error_Type_List.StdError)
            End Select
        Next y
        _ParentSiblingList = MyParentSiblings
        _Parent = MyParent
    End Sub
    '********************************************************************

    Sub UpdateChildren(ByRef myDefBlob As ExpandoObject, ParentSiblingList As List(Of RecDefItem), MyParent As RecDefItem)

        ' Dim subRecDefList As List(Of RecDefItem)
        Dim subRecDefItem As RecDefItem
        Dim expandoItem As ExpandoObject
        Dim expandoList As List(Of Object)

        Me.UpdateRecDef(myDefBlob, ParentSiblingList, MyParent)

        For loopX = 0 To myDefBlob.Count - 1
            Select Case myDefBlob(loopX).Key.ToUpper
                'attributes start with @

                Case "LOOP"
                    If TypeOf myDefBlob(loopX).Value Is ExpandoObject Then
                        expandoList = DirectCast(New List(Of Object)({myDefBlob(loopX).Value}), List(Of Object))
                    Else
                        ' multiple loops together
                        expandoList = DirectCast(myDefBlob(loopX).Value, List(Of Object))
                    End If

                    For Each expandoItem In expandoList
                        Def_Record_Count = Def_Record_Count + 1
                        subRecDefItem = New RecDefItem
                        subRecDefItem.UpdateRecDef(expandoItem, MyParent.myloop, subRecDefItem)
                        subRecDefItem.UpdateChildren(expandoItem, MyParent.myloop, subRecDefItem)
                        'For Each subrec In subRecDefList
                        '    myRecDefItem.myloop.Add(subrec)
                        'Next subrec
                        _myloop.Add(subRecDefItem)
                        If Interchange.debugflag Then
                            If ParentSiblingList.Count > 0 Then

                                Debug.Print("LoopM " & ParentSiblingList.Count & "-" & ParentSiblingList(ParentSiblingList.Count - 1).recname & " T" & ParentSiblingList(ParentSiblingList.Count - 1).mytype & " E" & ParentSiblingList(ParentSiblingList.Count - 1).EndTrigger & " SEQ#" & ParentSiblingList(ParentSiblingList.Count - 1).Seq & " L" & ParentSiblingList(ParentSiblingList.Count - 1).LastRecord & " M" & ParentSiblingList(ParentSiblingList.Count - 1).Mandatory)
                            End If
                            Debug.Print("Rec " & myloop.Count & "-" & myloop(myloop.Count - 1).recname & " T" & myloop(myloop.Count - 1).mytype & " E" & myloop(myloop.Count - 1).EndTrigger & " #" & myloop(myloop.Count - 1).Seq & " L" & myloop(myloop.Count - 1).LastRecord & " M" & myloop(myloop.Count - 1).Mandatory)
                        End If
                    Next expandoItem
                Case "REC_LIST"
                    If _myloop Is Nothing Then
                        _myloop = New List(Of RecDefItem)
                    End If
                    If TypeOf myDefBlob(loopX).Value Is ExpandoObject Then
                        expandoList = DirectCast(New List(Of Object)({myDefBlob(loopX).Value}), List(Of Object))
                    Else
                        ' multiple loops together
                        expandoList = DirectCast(myDefBlob(loopX).Value, List(Of Object))
                    End If

                    For Each expandoItem In expandoList
                        Def_Record_Count = Def_Record_Count + 1
                        subRecDefItem = New RecDefItem
                        subRecDefItem.UpdateRecDef(expandoItem, ParentSiblingList, Me)
                        _myloop.Add(subRecDefItem)
                        If Interchange.debugflag Then
                            If ParentSiblingList.Count > 0 Then
                                Debug.Print("Rec " & myloop.Count & "-" & myloop(myloop.Count - 1).recname & " T" & ParentSiblingList(ParentSiblingList.Count - 1).mytype & " E" & ParentSiblingList(ParentSiblingList.Count - 1).EndTrigger & " #" & ParentSiblingList(ParentSiblingList.Count - 1).Seq & " L" & ParentSiblingList(ParentSiblingList.Count - 1).LastRecord & " M" & ParentSiblingList(ParentSiblingList.Count - 1).Mandatory)
                            End If
                            Debug.Print("Rec " & myloop.Count & "-" & myloop(myloop.Count - 1).recname & " T" & myloop(myloop.Count - 1).mytype & " E" & myloop(myloop.Count - 1).EndTrigger & " #" & myloop(myloop.Count - 1).Seq & " L" & myloop(myloop.Count - 1).LastRecord & " M" & myloop(myloop.Count - 1).Mandatory)
                        End If
                    Next expandoItem

            End Select
        Next loopX
        _myloop.Sort(Function(sortX As RecDefItem, sortY As RecDefItem)
                         Select Case CInt(sortX.Seq)
                             Case Is < CInt(sortY.Seq) : Return -1
                             Case Is > CInt(sortY.Seq) : Return 1
                             Case Else : Return 0
                         End Select
                     End Function)


        'If myRecDefItem.recname <> "" Then
        '    myRecDefItem.ParentSiblingList = ParentSiblingList
        '    _myloop.Add(myRecDefItem)
        'Else
        '    Debug.Print("Empty Item")
        'End If


    End Sub
    '********************************************************************

End Class
