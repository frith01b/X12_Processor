Public Class FieldDef
    Public FName As String
    Public FType As String
    Public FLength As Integer
    ' validation list for lookup values / processing
    Public FCodeTableName As String
    Public Mandatory As Boolean
    Public SubFieldCount As Integer
    Public NumberFormat As String
    Public Alignment As String
    Public Padding As String
    'list of A->B mappings to substitute in data
    Public SubstituteGroupName As String
    Public StartPosition As Integer

    Public Sub New(Fname1 As String, Ftype1 As String, Flength1 As Integer, FcodeTableName1 As String, Mandatory1 As String, SubFieldCount1 As Integer, Numberformat1 As String, Alignment1 As String, Padding1 As String, SubGroupName1 As String, Optional StartPosition1 As Integer = 0)
        FName = Fname1
        FType = Ftype1
        FLength = Flength1
        FCodeTableName = FcodeTableName1
        If Mandatory1.ToUpper = "YES" Or Mandatory1.ToUpper = "TRUE" Then
            Mandatory = True
        Else
            Mandatory = False
        End If

        SubFieldCount = SubFieldCount1
        NumberFormat = Numberformat1
        Alignment = Alignment1
        Padding = Padding1
        SubstituteGroupName = SubGroupName1
        StartPosition = StartPosition1

    End Sub
    Public Sub New()
        FName = "F01_DefaultField"
        FType = "ALPHA"
        FLength = 30
        FCodeTableName = ""
        Mandatory = False

        SubFieldCount = 0
        NumberFormat = ""
        Alignment = "NONE"
        Padding = " "
        SubstituteGroupName = ""
        StartPosition = -1

    End Sub
End Class
