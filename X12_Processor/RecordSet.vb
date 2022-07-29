' removed strict for dynamic type allow
'Option Strict On
'Option Explicit On

Imports System.Dynamic
Imports System.Xml
Imports X12_Processor
Imports XmlToDynamic.Core
Imports System.Reflection

Interface RecordFunctions
    Sub Import_X12(SegID As String, Seg As ParseSegment, SourceRecNum As Long)
    Function GetData() As RecordSet
    ' Filter removes extraneous notes/data
    Function Filter() As String()
    'Remap adjusts incoming data to correct item #'s, code values , etc
    Sub ReMap()
    Function Output_X12() As String
    Function Validate() As Boolean

    Sub Load_RecordDef()

    Sub Save_RecordDef()

    Property PrevSegSeq As Long

End Interface

Public Class RecordSet
    Inherits DynamicObject
    Implements RecordFunctions
    ' _LastSeq: keeps track of prior record position in definition.
    Dim _PrevSegSeq As Long = 0
    Dim RecSetDefFile As String
    Public Shared DefRec_Count As Long
    Dim Rec_Count As Long = 0
    ' ReCData = Decoded actual record data.
    Dim Rec_Data() As Rec_Data_Type
    ' Rec_DataPtr :  Point to current array for new records in correct definition loop for Rec_Data
    Dim Rec_DataPtr As Object

    '  Dim myxmlDyn As XmlToDynamic.Core.Extensions

    ' RecDefObj = placeholder for incoming dynamic definition prior to de-coding
    Dim RecDefObj As ExpandoObject
    'RecordDefPtr = point to current definition area.
    Dim RecordDefPtr As Object

    Shared TypedRecDef() As RecDefItem
    Public RecDefList As List(Of RecDefItem)
    Public Sub New()

    End Sub
    Public Sub New(MyBlob As ExpandoObject)

    End Sub

    Public Property PrevSegSeq As Long Implements RecordFunctions.PrevSegSeq
        Get
            Return _PrevSegSeq
        End Get
        Set(value As Long)
            _PrevSegSeq = value
        End Set
    End Property

    Public Sub Import_X12(SegID As String, Seg As ParseSegment, SourceRecNum As Long) Implements RecordFunctions.Import_X12
        'Dim fullyQualifiedClassName As String = "System.Windows.Forms.Button"
        'Dim o = fetchInstance(fullyQualifiedClassName)
        '' sometime later where you can narrow down the type or interface...
        'Dim b = CType(o, Control)

        Dim NewRec As Segment
        Dim Recname As String
        Dim CastRec As SegTranslate
        Dim testtype As ISA = New ISA
        Dim type As Type = testtype.GetType()
        Dim typeName As String = type.FullName
        If Interchange.debugflag Then

            Debug.Print(typeName)
        End If
        ' split worked well,but leaves extra  bytes in segment identifier
        Seg.SegID = SegID.Replace(vbLf, "")
        Seg.SegID = SegID.Replace(vbCr, "")

        Recname = "X12_Processor." & SegID
        NewRec = fetchInstance(Recname)
        ' cast to interface type(segtranslate) so we can use methods & properties directly
        CastRec = CType(NewRec, SegTranslate)
        If CastRec.FieldCount = 0 Then
            CastRec.InitializeTranDef()
            ' CastRec.SaveTranDef()
            ' CastRec.LoadFieldDef()
        End If
        CastRec.Import(Seg, SourceRecNum)
        If Interchange.debugflag Then

            For Each myfield In CastRec.Fields
                Debug.Print(myfield.Key & "  " & myfield.Value)
            Next
        End If

        'InsertRecData:  place castrec in Rec_Data structure matching hierarchy
        InsertRecData(CastRec)
    End Sub

    Private Sub InsertRecData(castRec As SegTranslate)
        If Rec_Count = 0 Then
            ReDim Rec_Data(Rec_Data.Count)
            Rec_Data(0) = New Rec_Data_Type
            Rec_DataPtr = Rec_Data
        End If
        FindNextSegment(castRec, Rec_DataPtr)
        ' if only new records added


    End Sub

    Public Sub ReMap() Implements RecordFunctions.ReMap
        Throw New NotImplementedException()
    End Sub

    Public Sub Load_RecordDef() Implements RecordFunctions.Load_RecordDef
        Dim x As Integer = 0
        If RecDefList Is Nothing Then

            Dim Myfile As New System.IO.StreamReader(path:=RecSetDefFile)
            Dim myBlobRecDef As ExpandoObject


            ' toDynamic is extended method from  XMLtoDynamic library (NUGET)
            RecDefObj = Extensions.ToDynamic(XDocument.Parse(Myfile.ReadToEnd()))
            ' @TODO  add rec types to definition
            For x = 0 To RecDefObj.Count - 1
                If Not RecDefObj(x).Key Is Nothing Then
                    Select Case RecDefObj(x).Key.ToUpper
                        Case "#COMMENT"
                        'ignore
                    ' first iteration should always be recdef
                        Case "RECDEF"
                            DefRec_Count = DefRec_Count + 1
                            myBlobRecDef = RecDefObj(x).Value
                            RecDefList = RecurseRecDef(myBlobRecDef)
                        Case Else
                            Interchange.AddError("Unknown record definition field" & RecDefObj(x).Key.ToUpper, Interchange.Error_Type_List.StdError)
                    End Select
                End If
            Next
            If Interchange.debugflag Then

                Debug.Print(RecDefList.Count & "  " & DefRec_Count)
            End If

        End If
        RecordDefPtr = RecDefList(0)

    End Sub

    Public Sub Save_RecordDef() Implements RecordFunctions.Save_RecordDef
        ' remember  [gettype]()  for current object type
        Dim writer As New System.Xml.Serialization.XmlSerializer([GetType]())
        Dim file As New System.IO.StreamWriter(path:=RecSetDefFile)
        writer.Serialize(file, Me)
        file.Close()
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

    Public Sub UpdateRecSetDefFile(NewRecFile As String)
        RecSetDefFile = NewRecFile
    End Sub

    Public Function RecurseRecDef(myDefBlob As ExpandoObject) As List(Of RecDefItem)
        Dim myrecdefitem As RecDefItem = New RecDefItem
        Dim NewRecDefList As List(Of RecDefItem) = New List(Of RecDefItem)
        Dim myloopList As List(Of Object)
        Dim rec_list_found As Boolean = False
        Dim loop_found As Boolean = False
        Dim x As Integer

        For x = 0 To myDefBlob.Count - 1
            ' find all keys before executing any action
            If Not myDefBlob(x).Key Is Nothing Then
                Select Case myDefBlob(x).Key.ToUpper
                    Case "#COMMENT"
                        'ignore
                    Case "@NAME"
                        myrecdefitem.recname = myDefBlob(x).Value
                    Case "@REPEAT"
                        myrecdefitem.myrepeat = myDefBlob(x).Value
                    Case "@SEQNUM"
                        myrecdefitem.Seq = myDefBlob(x).Value
                    Case "@TYPE"
                        myrecdefitem.mytype = myDefBlob(x).Value
                    Case "@ENDTRIGGER"
                        myrecdefitem.EndTrigger = myDefBlob(x).Value
                    Case "@LASTRECORD"
                        myrecdefitem.LastRecord = myDefBlob(x).Value
                    Case "@MANDATORY"
                        myrecdefitem.Mandatory = myDefBlob(x).Value
                    Case "LOOP"
                        If TypeOf myDefBlob(x).Value Is ExpandoObject Then
                            DefRec_Count = DefRec_Count + 1
                            myrecdefitem = New RecDefItem
                            MakeLoop(myDefBlob(x).Value, myrecdefitem)
                            NewRecDefList.Add(myrecdefitem)
                            If Interchange.debugflag Then

                                Debug.Print("Loop1 " & NewRecDefList.Count & "-" & NewRecDefList(NewRecDefList.Count - 1).recname & " T" & NewRecDefList(NewRecDefList.Count - 1).mytype & " E" & NewRecDefList(NewRecDefList.Count - 1).EndTrigger & " #" & NewRecDefList(NewRecDefList.Count - 1).Seq & " L" & NewRecDefList(NewRecDefList.Count - 1).LastRecord & " M" & NewRecDefList(NewRecDefList.Count - 1).Mandatory)
                            End If

                        Else
                                ' multiple loops together
                                myloopList = myDefBlob(x).Value
                            For Each myloop In myloopList
                                DefRec_Count = DefRec_Count + 1
                                myrecdefitem = New RecDefItem
                                MakeLoop(myloop, myrecdefitem)
                                NewRecDefList.Add(myrecdefitem)
                                If Interchange.debugflag Then

                                    Debug.Print("LoopM " & NewRecDefList.Count & "-" & NewRecDefList(NewRecDefList.Count - 1).recname & " T" & NewRecDefList(NewRecDefList.Count - 1).mytype & " E" & NewRecDefList(NewRecDefList.Count - 1).EndTrigger & " #" & NewRecDefList(NewRecDefList.Count - 1).Seq & " L" & NewRecDefList(NewRecDefList.Count - 1).LastRecord & " M" & NewRecDefList(NewRecDefList.Count - 1).Mandatory)
                                End If

                            Next myloop
                        End If

                    Case "REC_LIST"
                        If TypeOf myDefBlob(x).Value Is ExpandoObject Then
                            DefRec_Count = DefRec_Count + 1
                            NewRecDefList.Add(MakeRecDef(myDefBlob(x).Value))
                            If Interchange.debugflag Then

                                Debug.Print("Rec1 " & NewRecDefList.Count & "-" & NewRecDefList(NewRecDefList.Count - 1).recname & " T" & NewRecDefList(NewRecDefList.Count - 1).mytype & " E" & NewRecDefList(NewRecDefList.Count - 1).EndTrigger & " #" & NewRecDefList(NewRecDefList.Count - 1).Seq & " L" & NewRecDefList(NewRecDefList.Count - 1).LastRecord & " M" & NewRecDefList(NewRecDefList.Count - 1).Mandatory)
                            End If

                        Else
                                ' multiple records together
                                myloopList = myDefBlob(x).Value
                            For Each myloop In myloopList
                                DefRec_Count = DefRec_Count + 1
                                NewRecDefList.Add(MakeRecDef(myloop))
                                If Interchange.debugflag Then

                                    Debug.Print("RecM " & NewRecDefList.Count & "-" & NewRecDefList(NewRecDefList.Count - 1).recname & " T" & NewRecDefList(NewRecDefList.Count - 1).mytype & " E" & NewRecDefList(NewRecDefList.Count - 1).EndTrigger & " #" & NewRecDefList(NewRecDefList.Count - 1).Seq & " L" & NewRecDefList(NewRecDefList.Count - 1).LastRecord & " M" & NewRecDefList(NewRecDefList.Count - 1).Mandatory)
                                End If

                            Next myloop
                        End If

                    Case Else
                        Interchange.AddError("Unknown record definition field" & myDefBlob(x).Key.ToUpper, Interchange.Error_Type_List.StdError)
                End Select
            End If
        Next

        'Dim fullyQualifiedClassName As String = "System.Windows.Forms.Button"
        'Dim o = fetchInstance(fullyQualifiedClassName)
        '' sometime later where you can narrow down the type or interface...
        'Dim b = CType(o, Control)
        'b.Text = "test"
        'b.Top = 10
        'b.Left = 10
        'Controls.Add(b)

        Return NewRecDefList
    End Function

    Function MakeRecDef(RecAttributes As ExpandoObject) As RecDefItem
        Dim myRecDefItem As RecDefItem = New RecDefItem
        Dim y As Integer
        For y = 0 To RecAttributes.Count - 1

            Select Case RecAttributes(y).Key.ToUpper
                Case "#COMMENT"
                        'ignore
                Case "@SEQNUM"
                    myRecDefItem.Seq = RecAttributes(y).Value
                Case "@NAME"
                    myRecDefItem.recname = RecAttributes(y).Value
                Case "@TYPE"
                    myRecDefItem.mytype = RecAttributes(y).Value
                Case "@REPEAT"
                    myRecDefItem.myrepeat = RecAttributes(y).Value
                Case "@ENDTRIGGER"
                    myRecDefItem.EndTrigger = RecAttributes(y).Value
                Case "@MANDATORY"
                    myRecDefItem.Mandatory = RecAttributes(y).Value
                Case Else
                    Interchange.AddError("Unknown record definition field" & RecAttributes(y).Key.ToUpper, Interchange.Error_Type_List.StdError)
            End Select

        Next y
        Return myRecDefItem
    End Function
    Sub MakeLoop(myDefBlob As ExpandoObject, ByRef MyLoopDef As RecDefItem)
        Dim NewDefList As List(Of RecDefItem) = New List(Of RecDefItem)
        Dim myRecDefItem As RecDefItem = New RecDefItem
        Dim myloop As ExpandoObject
        Dim myloopList As List(Of Object)

        For x = 0 To myDefBlob.Count - 1
            Select Case myDefBlob(x).Key.ToUpper
                    'attributes start with @
                Case "#COMMENT"
                        'ignore
                Case "@NAME"
                    MyLoopDef.recname = myDefBlob(x).Value
                Case "@ENDTRIGGER"
                    MyLoopDef.EndTrigger = myDefBlob(x).Value
                Case "@LASTRECORD"
                    MyLoopDef.LastRecord = myDefBlob(x).Value
                Case "@MANDATORY"
                    MyLoopDef.Mandatory = myDefBlob(x).Value
                Case "@SEQNUM"
                    myRecDefItem.Seq = myDefBlob(x).Value
                Case "LOOP"
                    If TypeOf myDefBlob(x).Value Is ExpandoObject Then
                        DefRec_Count = DefRec_Count + 1
                        myRecDefItem = New RecDefItem
                        MakeLoop(myDefBlob(x).Value, myRecDefItem)
                        NewDefList.Add(myRecDefItem)
                        If Interchange.debugflag Then
                            Debug.Print("Loop1 " & NewDefList.Count & "-" & NewDefList(NewDefList.Count - 1).recname & " T" & NewDefList(NewDefList.Count - 1).mytype & " E" & NewDefList(NewDefList.Count - 1).EndTrigger & " #" & NewDefList(NewDefList.Count - 1).Seq & " L" & NewDefList(NewDefList.Count - 1).LastRecord & " M" & NewDefList(NewDefList.Count - 1).Mandatory)
                        End If

                    Else
                        ' multiple loops together
                        myloopList = myDefBlob(x).Value
                        For Each myloop In myloopList
                            DefRec_Count = DefRec_Count + 1
                            myRecDefItem = New RecDefItem
                            MakeLoop(myloop, myRecDefItem)
                            NewDefList.Add(myRecDefItem)
                            If Interchange.debugflag Then

                                Debug.Print("LoopM " & NewDefList.Count & "-" & NewDefList(NewDefList.Count - 1).recname & " T" & NewDefList(NewDefList.Count - 1).mytype & " E" & NewDefList(NewDefList.Count - 1).EndTrigger & " #" & NewDefList(NewDefList.Count - 1).Seq & " L" & NewDefList(NewDefList.Count - 1).LastRecord & " M" & NewDefList(NewDefList.Count - 1).Mandatory)
                            End If

                        Next myloop

                    End If
                Case "REC_LIST"
                    If TypeOf myDefBlob(x).Value Is ExpandoObject Then
                        DefRec_Count = DefRec_Count + 1
                        NewDefList.Add(MakeRecDef(myDefBlob(x).Value))
                        If Interchange.debugflag Then

                            Debug.Print("Rec1 " & NewDefList.Count & "-" & NewDefList(NewDefList.Count - 1).recname & " T" & NewDefList(NewDefList.Count - 1).mytype & " E" & NewDefList(NewDefList.Count - 1).EndTrigger & " #" & NewDefList(NewDefList.Count - 1).Seq & " L" & NewDefList(NewDefList.Count - 1).LastRecord & " M" & NewDefList(NewDefList.Count - 1).Mandatory)
                        End If

                    Else
                            ' multiple loops together
                            myloopList = myDefBlob(x).Value
                        For Each myloop In myloopList
                            DefRec_Count = DefRec_Count + 1
                            NewDefList.Add(MakeRecDef(myloop))
                            If Interchange.debugflag Then

                                Debug.Print("RecM " & NewDefList.Count & "-" & NewDefList(NewDefList.Count - 1).recname & " T" & NewDefList(NewDefList.Count - 1).mytype & " E" & NewDefList(NewDefList.Count - 1).EndTrigger & " #" & NewDefList(NewDefList.Count - 1).Seq & " L" & NewDefList(NewDefList.Count - 1).LastRecord & " M" & NewDefList(NewDefList.Count - 1).Mandatory)
                            End If

                        Next myloop
                    End If
                Case Else
                    Interchange.AddError("Unknown record definition field" & myDefBlob(x).Key.ToUpper, Interchange.Error_Type_List.StdError)
            End Select
        Next x

        MyLoopDef.myloop = NewDefList
    End Sub

    Function GetRecList(RecType As String) As List(Of RecDefItem)
        Return RecDefList
    End Function

    ' helper method used for Import function to create dynamic segment types
    Private Function fetchInstance(ByVal fullyQualifiedClassName As String) As Object
        Dim nspc As String = fullyQualifiedClassName.Substring(0, fullyQualifiedClassName.LastIndexOf("."c))
        Dim o As Object = Nothing
        Try
            o = Assembly.GetExecutingAssembly().CreateInstance(fullyQualifiedClassName)
        Catch
            Interchange.AddError("Error Creating object" & fullyQualifiedClassName, Interchange.Error_Type_List.StdError)
        End Try
        Return o
    End Function

    Function FindNextSegment(MySegment As SegTranslate, MyObj As Object) As Boolean
        Dim retVal As Boolean = False

        If (_PrevSegSeq > 0) Then
        Else
            ' first record
            If MySegment.RecordName = RecDefList(0).recname Then
                DirectCast(RecordDefPtr, Rec_Data_Type).add(MySegment)
                retVal = True
            Else
                Interchange.AddError("First Record does not match definition :Found " & MySegment.RecordName & " looking for: " & RecDefList(0).recname, Interchange.Error_Type_List.StdError)
            End If
        End If
        Return retVal
    End Function
End Class
