Imports Hinshlabs.CommandLineParser
Imports System.Collections.ObjectModel

Public Class CommandOut

    Private Sub New()

    End Sub

    Public Shared Sub Warning(ByVal format As String, ByVal ParamArray args() As String)
        Console.ForegroundColor = ConsoleColor.Yellow
        Console.WriteLine(format, args)
        Console.ResetColor()
    End Sub

    Public Shared Sub WarningUnrecognisedCommand(ByVal commandName As String)
        CommandOut.Warning("Unrecognized command: {0}.", commandName)
    End Sub

    Public Shared Sub Info(ByVal format As String, ByVal ParamArray args() As String)
        Console.WriteLine(format, args)
    End Sub

    Public Shared Sub [Error](ByVal format As String, ByVal ParamArray args() As String)
        Console.ForegroundColor = ConsoleColor.Red
        Console.WriteLine(format, args)
        Console.ResetColor()
    End Sub

    Friend Shared Sub WriteWithLeftMargin(ByVal value As String, ByVal margin As Integer)
        For Each s In SplitTextIntoChucks(value, Console.WindowWidth - (margin))
            Console.CursorLeft = margin
            Console.Write(s)
            Console.WriteLine()
        Next
    End Sub

    Private Shared Function SplitTextIntoChucks(ByVal value As String, ByVal maxLineLength As Integer) As List(Of String)
        Dim x As New List(Of String)
        While value.Length > 0
            Dim bit As String
            If value.Length < maxLineLength Then
                bit = value.Trim
                value = ""
            Else
                bit = RecurseGetNextBit(value, maxLineLength)
                value = value.Replace(bit, "").Trim
            End If
            x.Add(bit)
        End While
        Return x
    End Function

    Private Shared Function RecurseGetNextBit(ByVal value As String, ByVal maxLineLength As Integer) As String
        Dim firstBit As String = value.Substring(0, maxLineLength)
        Dim lastspace As Integer = firstBit.LastIndexOf(" ")
        Return value.Substring(0, lastspace).Trim
    End Function

End Class
