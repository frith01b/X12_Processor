' removed strict for dynamic type allow
'Option Strict On
'Option Explicit On

Imports System.Dynamic
Imports System.Xml
Imports X12_Processor
Imports XmlToDynamic.Core
Imports System.Reflection

Interface RecordSetMethods_Intf
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
    Implements RecordSetMethods_Intf

    Public Const MAX_Seq_Const = 9999999

    Dim Rec_Count As Long = 0
    ' RecData = Decoded actual record data with correct segment types.
    Dim Rec_Data As List(Of SegTranslate) = New List(Of SegTranslate)
    ' Rec_DataPtr :  Point to current array for new records in correct definition loop for Rec_Data
    Dim Current_Rec_ListPtr As List(Of SegTranslate)
    ' _PrevSeq: keeps track of prior record position in definition.
    Dim Current_Rec_data As SegTranslate
    Dim _PrevSegSeq As Long = 0

    '  Dim myxmlDyn As XmlToDynamic.Core.Extensions

    'RecordDefPtr = point to current definition area.
    Dim RecordDefPtr As List(Of RecDefItem)

    ' ***********************************
    '          Record Definition variables

    Dim RecSetDefFile As String
    Public Shared DefRec_Count As Long
    ' RecDefObj = placeholder for incoming dynamic definition prior to de-coding
    Dim RecDefObj As ExpandoObject
    Shared TypedRecDef() As RecDefItem
    Public RecDefList As List(Of RecDefItem)

    Public Sub New()
        Current_Rec_ListPtr = Rec_Data
    End Sub
    Public Sub New(MyBlob As ExpandoObject)

    End Sub

    Public Property PrevSegSeq As Long Implements RecordSetMethods_Intf.PrevSegSeq
        Get
            Return _PrevSegSeq
        End Get
        Set(value As Long)
            _PrevSegSeq = value
        End Set
    End Property

    Public Sub Import_X12(SegID As String, Seg As ParseSegment, SourceRecNum As Long) Implements RecordSetMethods_Intf.Import_X12
        'Dim fullyQualifiedClassName As String = "System.Windows.Forms.Button"
        'Dim o = fetchInstance(fullyQualifiedClassName)
        '' sometime later where you can narrow down the type or interface...
        'Dim b = CType(o, Control)

        Dim NewRec As Segment
        Dim Recname As String
        Dim CastRec As SegTranslate
        ' split worked well,but leaves extra  bytes in segment identifier
        Seg.SegID = SegID.Replace(vbLf, "")
        Seg.SegID = SegID.Replace(vbCr, "")

        Recname = "X12_Processor." & SegID
        'initialize the empty record for correct sub-type
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
        FindNextSegment(castRec, Current_Rec_ListPtr)
        ' if only new records added


    End Sub

    Public Sub ReMap() Implements RecordSetMethods_Intf.ReMap
        Throw New NotImplementedException()
    End Sub

    Public Sub Load_RecordDef() Implements RecordSetMethods_Intf.Load_RecordDef
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
        RecordDefPtr = RecDefList

    End Sub

    Public Sub Save_RecordDef() Implements RecordSetMethods_Intf.Save_RecordDef
        ' remember  [gettype]()  for current object type
        Dim writer As New System.Xml.Serialization.XmlSerializer([GetType]())
        Dim file As New System.IO.StreamWriter(path:=RecSetDefFile)
        writer.Serialize(file, Me)
        file.Close()
    End Sub

    Public Function GetData() As RecordSet Implements RecordSetMethods_Intf.GetData
        Throw New NotImplementedException()
    End Function

    Public Function Filter() As String() Implements RecordSetMethods_Intf.Filter
        Throw New NotImplementedException()
    End Function

    Public Function Output_X12() As String Implements RecordSetMethods_Intf.Output_X12
        Throw New NotImplementedException()
    End Function

    Public Function Validate() As Boolean Implements RecordSetMethods_Intf.Validate
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
        Dim loopx As Integer

        For loopx = 0 To myDefBlob.Count - 1
            ' find all keys before executing any action
            If Not myDefBlob(loopx).Key Is Nothing Then
                Select Case myDefBlob(loopx).Key.ToUpper
                    Case "#COMMENT"
                        'ignore
                    Case "@NAME"
                        myrecdefitem.recname = myDefBlob(loopx).Value
                    Case "@REPEAT"
                        myrecdefitem.myrepeat = myDefBlob(loopx).Value
                    Case "@SEQNUM"
                        myrecdefitem.Seq = myDefBlob(loopx).Value
                    Case "@TYPE"
                        myrecdefitem.mytype = myDefBlob(loopx).Value
                    Case "@ENDTRIGGER"
                        myrecdefitem.EndTrigger = myDefBlob(loopx).Value
                    Case "@LASTRECORD"
                        myrecdefitem.LastRecord = myDefBlob(loopx).Value
                    Case "@MANDATORY"
                        myrecdefitem.Mandatory = myDefBlob(loopx).Value
                    Case "LOOP"
                        If TypeOf myDefBlob(loopx).Value Is ExpandoObject Then
                            DefRec_Count = DefRec_Count + 1


                            NewRecDefList.Add(MakeLoop(myDefBlob(loopx).Value))
                            If Interchange.debugflag Then

                                Debug.Print("Loop1 " & NewRecDefList.Count & "-" & NewRecDefList(NewRecDefList.Count - 1).recname & " T" & NewRecDefList(NewRecDefList.Count - 1).mytype & " E" & NewRecDefList(NewRecDefList.Count - 1).EndTrigger & " #" & NewRecDefList(NewRecDefList.Count - 1).Seq & " L" & NewRecDefList(NewRecDefList.Count - 1).LastRecord & " M" & NewRecDefList(NewRecDefList.Count - 1).Mandatory)
                            End If

                        Else
                            ' multiple loops together
                            myloopList = myDefBlob(loopx).Value
                            For Each myloop In myloopList
                                DefRec_Count = DefRec_Count + 1


                                NewRecDefList.Add(MakeLoop(myloop))
                                If Interchange.debugflag Then

                                    Debug.Print("LoopM " & NewRecDefList.Count & "-" & NewRecDefList(NewRecDefList.Count - 1).recname & " T" & NewRecDefList(NewRecDefList.Count - 1).mytype & " E" & NewRecDefList(NewRecDefList.Count - 1).EndTrigger & " #" & NewRecDefList(NewRecDefList.Count - 1).Seq & " L" & NewRecDefList(NewRecDefList.Count - 1).LastRecord & " M" & NewRecDefList(NewRecDefList.Count - 1).Mandatory)
                                End If

                            Next myloop
                        End If

                    Case "REC_LIST"
                        If TypeOf myDefBlob(loopx).Value Is ExpandoObject Then
                            DefRec_Count = DefRec_Count + 1
                            NewRecDefList.Add(MakeRecDef(myDefBlob(loopx).Value))
                            If Interchange.debugflag Then

                                Debug.Print("Rec1 " & NewRecDefList.Count & "-" & NewRecDefList(NewRecDefList.Count - 1).recname & " T" & NewRecDefList(NewRecDefList.Count - 1).mytype & " E" & NewRecDefList(NewRecDefList.Count - 1).EndTrigger & " #" & NewRecDefList(NewRecDefList.Count - 1).Seq & " L" & NewRecDefList(NewRecDefList.Count - 1).LastRecord & " M" & NewRecDefList(NewRecDefList.Count - 1).Mandatory)
                            End If

                        Else
                            ' multiple records together
                            myloopList = myDefBlob(loopx).Value
                            For Each myloop In myloopList
                                DefRec_Count = DefRec_Count + 1
                                NewRecDefList.Add(MakeRecDef(myloop))
                                If Interchange.debugflag Then

                                    Debug.Print("RecM " & NewRecDefList.Count & "-" & NewRecDefList(NewRecDefList.Count - 1).recname & " T" & NewRecDefList(NewRecDefList.Count - 1).mytype & " E" & NewRecDefList(NewRecDefList.Count - 1).EndTrigger & " #" & NewRecDefList(NewRecDefList.Count - 1).Seq & " L" & NewRecDefList(NewRecDefList.Count - 1).LastRecord & " M" & NewRecDefList(NewRecDefList.Count - 1).Mandatory)
                                End If

                            Next myloop
                        End If

                    Case Else
                        Interchange.AddError("Unknown record definition field" & myDefBlob(loopx).Key.ToUpper, Interchange.Error_Type_List.StdError)
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
        ' Ensure List is in Sequence order by sorting it by seq field numerically
        NewRecDefList.Sort(Function(X As RecDefItem, Y As RecDefItem)
                               Select Case CInt(X.Seq)
                                   Case Is < CInt(Y.Seq) : Return -1
                                   Case Is > CInt(Y.Seq) : Return 1
                                   Case Else : Return 0
                               End Select
                           End Function)
        Return NewRecDefList
    End Function

    Function MakeRecDef(RecAttributes As ExpandoObject) As RecDefItem
        Dim myRecDefItem As RecDefItem = New RecDefItem
        Dim y As Integer
        For y = 0 To RecAttributes.Count - 1

            Select Case RecAttributes(y).Key.ToUpper
                Case "#COMMENT"
                    'ignore comments
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
    Function MakeLoop(ByRef myDefBlob As ExpandoObject) As RecDefItem
        Dim NewDefList As List(Of RecDefItem) = New List(Of RecDefItem)
        Dim myRecDefItem As RecDefItem = New RecDefItem
        Dim myloop As ExpandoObject
        Dim myloopList As List(Of Object)
        Dim delayedAdd As Boolean = False

        For x = 0 To myDefBlob.Count - 1
            Select Case myDefBlob(x).Key.ToUpper
                'attributes start with @
                Case "#COMMENT"
                    'ignore
                Case "@NAME"
                    myRecDefItem.recname = myDefBlob(x).Value
                Case "@ENDTRIGGER"
                    myRecDefItem.EndTrigger = myDefBlob(x).Value
                Case "@LASTRECORD"
                    myRecDefItem.LastRecord = myDefBlob(x).Value
                Case "@MANDATORY"
                    myRecDefItem.Mandatory = myDefBlob(x).Value
                Case "@SEQNUM"
                    myRecDefItem.Seq = myDefBlob(x).Value
                Case "LOOP"
                    If TypeOf myDefBlob(x).Value Is ExpandoObject Then
                        DefRec_Count = DefRec_Count + 1
                        'myRecDefItem = New RecDefItem
                        NewDefList.Add(MakeLoop(myDefBlob(x).Value))

                        If Interchange.debugflag Then
                            Debug.Print("Loop1 " & NewDefList.Count & "-" & NewDefList(NewDefList.Count - 1).recname & " T" & NewDefList(NewDefList.Count - 1).mytype & " E" & NewDefList(NewDefList.Count - 1).EndTrigger & " #" & NewDefList(NewDefList.Count - 1).Seq & " L" & NewDefList(NewDefList.Count - 1).LastRecord & " M" & NewDefList(NewDefList.Count - 1).Mandatory)
                        End If

                    Else
                        ' multiple loops together
                        myloopList = myDefBlob(x).Value
                        For Each myloop In myloopList
                            DefRec_Count = DefRec_Count + 1
                            NewDefList.Add(MakeLoop(myloop))
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

                            Debug.Print("Rec " & NewDefList.Count & "-" & NewDefList(NewDefList.Count - 1).recname & " T" & NewDefList(NewDefList.Count - 1).mytype & " E" & NewDefList(NewDefList.Count - 1).EndTrigger & " #" & NewDefList(NewDefList.Count - 1).Seq & " L" & NewDefList(NewDefList.Count - 1).LastRecord & " M" & NewDefList(NewDefList.Count - 1).Mandatory)

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
        ' Add Loop definition once all attributes set.
        If delayedAdd = True Then
            NewDefList.Add(myRecDefItem)
        End If


        myRecDefItem.myloop = NewDefList
        Return myRecDefItem
    End Function

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

    Function FindNextSegment(MySegment As SegTranslate, ByRef MyParent As List(Of SegTranslate)) As Boolean
        Dim retVal As Boolean = False
        Dim infoFound As FinderInfo

        If (_PrevSegSeq > 0) Then
            infoFound = WhereIsNextSegment(MySegment.RecordName)
            Select Case infoFound.NextRecPosition
                Case FinderInfo.Rec_Def_Relation.Sibling
                    Current_Rec_ListPtr.Add(MySegment)
                    Current_Rec_data = MySegment

                Case FinderInfo.Rec_Def_Relation.Child
                    Dim myLoop As LoopHolder = New LoopHolder
                    myLoop.RecIndex = infoFound.Seq
                    myLoop.RecordName = infoFound.RecordName
                    myLoop.Parent = Current_Rec_data
                    myLoop.AddLoopItem(MySegment)

                    Current_Rec_ListPtr.Add(myLoop)
                    ' update pointers
                    Current_Rec_ListPtr = myLoop.LoopData
                    Current_Rec_data = MySegment
                Case FinderInfo.Rec_Def_Relation.Parent
                    Current_Rec_ListPtr.Add(MySegment)
                Case Else
                    Current_Rec_ListPtr.Add(MySegment)

            End Select
        Else
            ' first record
            If MySegment.RecordName = RecDefList(0).mytype Then

                MyParent.Add(MySegment)
                _PrevSegSeq = RecDefList(0).Seq
                RecordDefPtr = RecDefList
                Current_Rec_ListPtr = MyParent
                Current_Rec_data = MyParent(0)
                retVal = True
            Else
                Interchange.AddError("First Record does not match definition :Found " & MySegment.GetType.ToString & " looking for: " & RecDefList(0).recname, Interchange.Error_Type_List.StdError)
            End If
        End If
        Return retVal
    End Function

    ''' <summary>
    ''' WhereIsNextSegment
    ''' Sets the Current RecordDefPtr to parent of current record segement
    ''' and sets the RecordDataPtr to the parent where the segment should belong.
    ''' </summary>
    ''' <param name="recordName"></param>
    ''' <returns></returns>
    Private Function WhereIsNextSegment(recordName As String) As FinderInfo
        Dim RetVal As FinderInfo = New FinderInfo
        Dim NextMandatorySeq As Long = MAX_Seq_Const
        Dim SaveMandRecName As String = ""
        Dim curseq As Long = 0

        ' check siblings first
        If Not RecordDefPtr Is Nothing Then
            For Each rec In RecordDefPtr
                If rec.Seq >= _PrevSegSeq Then
                    If recordName = rec.mytype Then
                        RetVal.NextRecPosition = FinderInfo.Rec_Def_Relation.Sibling
                        _PrevSegSeq = rec.Seq
                        curseq = rec.Seq
                        Exit For

                    Else
                        If rec.Mandatory.ToUpper = "Y" And rec.Seq > _PrevSegSeq And NextMandatorySeq = MAX_Seq_Const And rec.myloop.Count = 0 Then
                            NextMandatorySeq = rec.Seq
                            SaveMandRecName = rec.mytype
                            Exit For
                        End If
                    End If
                End If
            Next
        End If
        If RetVal.NextRecPosition = FinderInfo.Rec_Def_Relation.NotFound Then

            ' check children 2nd
            For Each rec In RecordDefPtr
                For Each myloop In rec.myloop
                    If myloop.Seq >= _PrevSegSeq Then
                        If recordName = myloop.mytype Then
                            RetVal.NextRecPosition = FinderInfo.Rec_Def_Relation.Child
                            _PrevSegSeq = myloop.Seq
                            curseq = myloop.Seq
                            Exit For

                        Else
                            If myloop.Mandatory.ToUpper = "Y" And rec.myloop.Count = 0 And NextMandatorySeq = MAX_Seq_Const Then
                                NextMandatorySeq = myloop.Seq
                                SaveMandRecName = myloop.mytype
                                Exit For
                            End If
                        End If
                    End If
                Next
            Next
        End If
        If RetVal.NextRecPosition = FinderInfo.Rec_Def_Relation.NotFound Then
            ' check parent siblings 3rd.
        End If
        If NextMandatorySeq < curseq Then
            Interchange.AddError("Mandatory Record Not Found: " & NextMandatorySeq & " " & SaveMandRecName & " looking for: " & recordName, Interchange.Error_Type_List.StdError)
            RetVal.NextRecPosition = FinderInfo.Rec_Def_Relation.Mandatory_Missing
        End If


        Return RetVal
    End Function
End Class
