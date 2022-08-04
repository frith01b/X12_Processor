Option Strict On
Option Explicit On


Imports System.Dynamic

Public Class RecDefItem
    Private _Seq As String
    Private _recname As String
    Private _mytype As String
    Private _myrepeat As String
    Private _EndTrigger As String
    Private _myloop As List(Of RecDefItem)
    Private _LastRecord As String
    Private _Mandatory As String
    Private _Parent As Object

    Property Parent As Object
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
            Return _mytype
        End Get
        Set
            _mytype = Value
        End Set
    End Property

    Property myrepeat As String
        Get
            Return _myrepeat
        End Get
        Set
            _myrepeat = Value
        End Set
    End Property
    Property LastRecord As String
        Get
            Return _LastRecord
        End Get
        Set
            _LastRecord = Value
        End Set
    End Property

    Property Mandatory As String
        Get
            Return _Mandatory
        End Get
        Set
            _Mandatory = Value
        End Set
    End Property

    Property EndTrigger As String
        Get
            Return _EndTrigger
        End Get
        Set
            _EndTrigger = Value
        End Set
    End Property

    Property myloop As List(Of RecDefItem)
        Get
            Return _myloop
        End Get
        Set
            _myloop = Value
        End Set
    End Property
End Class
