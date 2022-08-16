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
    Private _ParentList As List(Of RecDefItem)

    Property ParentList As List(Of RecDefItem)
        Get
            Return _ParentList
        End Get
        Set
            _ParentList = Value
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
            If Not _mytype Is Nothing Then
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
            If Not _myrepeat Is Nothing Then
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
            If Not _LastRecord Is Nothing Then
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
            If Not _Mandatory Is Nothing Then
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
            If Not _EndTrigger Is Nothing Then
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
End Class
