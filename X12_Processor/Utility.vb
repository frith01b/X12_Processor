Option Explicit On
Option Strict On


Imports System
    Imports System.Data
    Imports System.Net.Mail
    Imports System.Net

    Imports System.IO
    Imports System.Security.Principal
    Imports System.Collections.ObjectModel
    Imports System.Collections.Specialized
    Imports System.Reflection


Public Class Utility
    'Dim str_default() As String = {""}
    Shared ReadOnly ArgList As ReadOnlyCollection(Of String) = New ReadOnlyCollection(Of String)(Environment.GetCommandLineArgs())

    Public Sub New()

    End Sub

    '***************************************************************************************
    Shared Sub ParseArgs(ByRef Args As Dictionary(Of String, String))
        'Dim CommandLineArgs As System.Collections.ObjectModel.ReadOnlyCollection(Of String) = My.Application.CommandLineArgs
        Dim CommandLineArgs As System.Collections.ObjectModel.ReadOnlyCollection(Of String) = ArgList
        For i As Integer = 0 To CommandLineArgs.Count - 1
            If (CommandLineArgs(i).Substring(0, 1) = "-") Then
                If (i + 1 < CommandLineArgs.Count) Then
                    If (CommandLineArgs(i + 1).Substring(0, 1) <> "-") Then
                        ' strip the dash, save following string as the base value
                        Args.Add(CommandLineArgs(i).Substring(1, CommandLineArgs(i).Length - 1).ToUpper, CommandLineArgs(i + 1))
                        i = i + 1
                    Else
                        ' strip the dash,  value is just a null string 
                        Args.Add(CommandLineArgs(i).Substring(1, CommandLineArgs(i).Length - 1).ToUpper, "")
                    End If
                Else
                    ' strip the dash,  value is just a null string 
                    Args.Add(CommandLineArgs(i).Substring(1, CommandLineArgs(i).Length - 1).ToUpper, "")
                End If
            End If
        Next

    End Sub


    Shared Function Send_Email(EmailFrom As String, EmailTo As String, EmailSubject As String, EmailBody As String, Optional AttachFileNames() As String = Nothing, Optional s_CC As String = "", Optional s_BCC As String = "") As Boolean
        Dim retVal As Boolean = True
        Dim i_loop As Integer

        Try
            Dim SmtpServer As New SmtpClient("exch2k16", 25)
            Dim mail As New MailMessage()
            ' Dim MyIdentity As WindowsIdentity = WindowsIdentity.GetCurrent()
            ' Dim MyPrincipal As New WindowsPrincipal(MyIdentity)
            ' alternate method
            'AppDomain.CurrentDomain.SetPrincipalPolicy(CType(System.Threading.Thread.CurrentPrincipal,WindowsPrincipal));

            ' SmtpServer.Credentials = New NetworkCredential("spservice", "PASSWORDHERE")

            ' THIS IS A HACK , until we get certificates installed properly from server.
            System.Net.ServicePointManager.ServerCertificateValidationCallback =
              Function(se As Object,
                cert As System.Security.Cryptography.X509Certificates.X509Certificate,
                chain As System.Security.Cryptography.X509Certificates.X509Chain,
                sslerror As System.Net.Security.SslPolicyErrors) True

            '  SmtpServer.Credentials = New NetworkCredential("domain\\USER", "PASSWORD")
            SmtpServer.EnableSsl = False
            SmtpServer.UseDefaultCredentials = False

            mail = New MailMessage()
            mail.From = New MailAddress(EmailFrom)
            mail.ReplyToList.Add("admin@serversomewhere")
            mail.To.Add(EmailTo)

            mail.Subject = EmailSubject
            mail.Body = EmailBody
            If (AttachFileNames IsNot Nothing) Then
                For i_loop = 0 To AttachFileNames.Count - 1
                    If (AttachFileNames(i_loop).Length > 0 AndAlso File.Exists(AttachFileNames(i_loop))) Then
                        mail.Attachments.Add(New Net.Mail.Attachment(AttachFileNames(i_loop)))
                    End If
                Next
            End If

            If s_CC <> "" Then
                mail.CC.Add(s_CC)
            End If
            If s_BCC <> "" Then
                mail.Bcc.Add(s_BCC)
            End If

            SmtpServer.Send(mail)

            System.Net.ServicePointManager.ServerCertificateValidationCallback = Nothing
        Catch ex As Exception
            retVal = False
            Throw New SmtpException("Unable to send email" & vbCrLf & ex.Message)
        End Try

        Return retVal

    End Function

    Public Shared Sub WriteUTF8(filename As String)
        ' Write the string as utf-8.
        ' This also writes the 3-byte utf-8 preamble at the beginning of the file.
        Dim appendMode As Boolean = False ' This overwrites the entire file.
        Dim sw As New StreamWriter("out_utf8.txt", appendMode, System.Text.Encoding.UTF8)
        sw.Write("sql text here")
        sw.Close()
    End Sub
    Public Shared Function VerifyDirList(MyDirs As String()) As Integer
        ' create directory if doesnt exist, otherwise return array index of failure
        Dim result As Integer = 0
        Dim x As Integer
        For x = 0 To MyDirs.Count - 1
            If (Not System.IO.Directory.Exists(MyDirs(x))) Then
                Try
                    System.IO.Directory.CreateDirectory(MyDirs(x))
                Catch ex As Exception
                    result = x + 1
                    Exit For
                End Try
            End If
        Next
        Return result
    End Function




    Public Shared Function fetchInstance(ByVal fullyQualifiedClassName As String) As Object
        ' FullyqualifiedClassName expected similar to  System.Windows.Forms.Button
        Dim nspc As String = fullyQualifiedClassName.Substring(0, fullyQualifiedClassName.LastIndexOf("."c))
            Dim o As Object = Nothing
        Try
            For Each ay In Assembly.GetExecutingAssembly().GetReferencedAssemblies()
                If (ay.Name = nspc) Then
                    o = Assembly.Load(ay).CreateInstance(fullyQualifiedClassName)
                    Exit For
                End If
            Next
        Catch
            'ignore load error

        End Try
            Return o
        End Function
        Public Shared Function GetAscii(mychar As Char) As Integer
            Dim retval As Integer

            retval = Convert.ToByte(mychar)
            Return retval
        End Function
    End Class


