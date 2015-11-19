Imports System.Runtime.CompilerServices
Imports System.IO

Module CommandExtensions

    <Extension()> _
    Public Function FindCommand(ByVal commands As IEnumerable(Of ICommandDefinition), ByVal commandName As String) As ICommandDefinition
        Return (From c In commands Where c.Name.ToLowerInvariant = commandName.ToLowerInvariant).SingleOrDefault
    End Function

    <Extension()> _
    Public Sub WriteCommandListToConsole(ByVal commandColleciton As CommandCollection)
        Try
            Console.ForegroundColor = ConsoleColor.Cyan
            CommandOut.WriteWithLeftMargin(String.Format("{0} - {1}", My.Application.Info.AssemblyName.ToUpper, My.Application.Info.ProductName), 0)
            Console.ResetColor()
            Console.WriteLine()
            CommandOut.WriteWithLeftMargin(String.Format("Type {0} help <command name> for full description", My.Application.Info.AssemblyName), 0)
            Console.WriteLine()
            Console.WriteLine()
            Console.WriteLine("Commands:")
            Dim longestName As String = commandColleciton.commands.GetLongestName
            For Each cm In commandColleciton.Commands
                Console.Write(String.Format("{0} ", My.Application.Info.AssemblyName))
                Console.Write(commandColleciton.CommandNameInContext(cm.Name).ToLower)
                CommandOut.WriteWithLeftMargin(cm.Description, longestName + 10)
            Next
            Console.WriteLine()
        Catch exhelp As Exception
            CommandOut.[Error](exhelp.ToString)
        End Try

    End Sub

    <Extension()> _
    Public Function GetLongestName(ByVal commands As IEnumerable(Of ICommandDefinition)) As Integer
        Dim longest As Integer = 0
        For Each cm In commands
            If cm.Name.Length > longest Then
                longest = cm.Name.Length
            End If
        Next
        Return longest
    End Function

    <Extension()> _
    Public Function GetLongestName(ByVal switches As IEnumerable(Of SwitchInfo)) As Integer
        Dim longest As Integer = 0
        For Each s In switches
            If s.Name.Length > longest Then
                longest = s.Name.Length
            End If
        Next
        Return longest
    End Function



    <Extension()> _
   Public Sub WriteHelpToConsole(ByVal Command As ICommandDefinition, ByVal commandColleciton As CommandCollection)
        Try
            Console.ForegroundColor = ConsoleColor.Cyan
            CommandOut.WriteWithLeftMargin(String.Format("{0} - {1}", My.Application.Info.AssemblyName.ToUpper, My.Application.Info.ProductName), 0)
            Console.WriteLine()
            CommandOut.WriteWithLeftMargin(String.Format(" abipt {0} - {1}", commandColleciton.CommandNameInContext(Command.Name).tolower, Command.Title), 0)
            Console.ResetColor()
            Console.WriteLine()
            CommandOut.WriteWithLeftMargin(Command.Synopsis, 0)
            Console.WriteLine()
            Console.Write("Usage: ")
            CommandOut.WriteWithLeftMargin(Command.ToUsage, 8)
            Console.WriteLine()
            Dim longestSwitch As Integer = Command.Switches.GetLongestName
            For Each sinfo As SwitchInfo In Command.Switches
                If Not sinfo.Name.ToLowerInvariant = "showhelp".ToLowerInvariant Then
                    Console.CursorLeft = 0
                    Console.Write(" ")
                    Console.Write(String.Format("/{0}", sinfo.Name.ToLower))
                    For count As Integer = 0 To longestSwitch - sinfo.Name.Length
                        Console.Write(" ")
                    Next
                    CommandOut.WriteWithLeftMargin(sinfo.Description, longestSwitch + 7)
                End If
            Next
            Console.WriteLine()
            CommandOut.WriteWithLeftMargin(Command.Qualifications, 0)
            Console.WriteLine()
        Catch exhelp As Exception
            CommandOut.[Error](exhelp.ToString)
        End Try

    End Sub

    <Extension()> _
   Public Function ToUsage(ByVal Command As ICommandDefinition) As String
        Dim usage As New System.Text.StringBuilder
        usage.Append(String.Format("{0} {1} ", My.Application.Info.AssemblyName, Command.Parent.CommandNameInContext(Command.Name).ToLower))
        For Each sinfo As SwitchInfo In Command.Switches
            If Not sinfo.Name.ToLowerInvariant = "showhelp".ToLowerInvariant Then
                usage.Append(String.Format("/{0}", sinfo.Name.ToLower))
                If sinfo.IsEnum Then
                    usage.Append(":[")
                    Dim isFirst As Boolean = True
                    For Each i In [Enum].GetValues(sinfo.Type)
                        usage.Append(String.Format(IIf(isFirst, "{0}", "|{0}"), i.ToString.ToLower))
                        isFirst = False
                    Next
                    usage.Append("]")
                    usage.Append(" ")
                ElseIf sinfo.Type Is GetType(String) Then
                    usage.AppendFormat(":[{0}...] ", sinfo.Name.ToLower)
                ElseIf sinfo.Type Is GetType(Integer) Then
                    usage.AppendFormat(":[{0}num] ", sinfo.Name.ToLower)
                ElseIf sinfo.Type Is GetType(Integer) Then
                    usage.AppendFormat(":[{0}url] ", sinfo.Name.ToLower)
                ElseIf sinfo.Type Is GetType(Boolean) Then
                    ' Do nothing as boolean values equate to true is existing
                    usage.Append(" ")
                Else
                    usage.Append(":[?]")
                End If
            End If
        Next
        usage.AppendLine()
        Return usage.ToString
    End Function

End Module
