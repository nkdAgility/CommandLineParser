Imports Hinshlabs.CommandLineParser
Imports System.Collections.ObjectModel

Public Class CommandCollection
    Implements ICommandDefinition, ICommandExecution

    Private _commands As IList(Of ICommandDefinition)
    Private _name As String = "root"
    Private _parent As CommandCollection
    Private _defaultExitAction As DefaultCommandAction

    Friend ReadOnly Property Commands() As IList(Of ICommandDefinition)
        Get
            If _commands Is Nothing Then
                _commands = New List(Of ICommandDefinition)
                CreateCommands(_commands)
            End If
            Return _commands
        End Get
    End Property

    Public ReadOnly Property Parent() As CommandCollection Implements ICommandDefinition.Parent
        Get
            Return _parent
        End Get
    End Property

    Public Overridable ReadOnly Property Description() As String Implements ICommandDefinition.Description
        Get
            Return "A collection of commands"
        End Get
    End Property

    Public Overridable ReadOnly Property Title() As String Implements ICommandDefinition.Title
        Get
            Return "Command Collection"
        End Get
    End Property

    Public ReadOnly Property Switches() As ReadOnlyCollection(Of SwitchInfo) Implements ICommandDefinition.Switches
        Get
            Return Nothing
        End Get
    End Property

    Public Overridable ReadOnly Property Synopsis() As String Implements ICommandDefinition.Synopsis
        Get
            Return String.Empty
        End Get
    End Property

    Public Overridable ReadOnly Property Qualifications() As String Implements ICommandDefinition.Qualifications
        Get
            Return String.Empty
        End Get
    End Property

    Public ReadOnly Property Name() As String Implements ICommandDefinition.Name
        Get
            Return _name
        End Get
    End Property

    Public Sub New(ByVal name As String, ByVal parent As CommandCollection, Optional defaultExitAction As DefaultCommandAction = DefaultCommandAction.Exit)
        _name = name
        _parent = parent
        _defaultExitAction = defaultExitAction
    End Sub

    Public Sub AddCommand(ByVal command As ICommandDefinition)
        Commands.Add(command)
    End Sub

    Protected Overridable Sub CreateCommands(ByVal commands As IEnumerable(Of ICommandDefinition))

    End Sub

    Public Function CommandNameInContext(ByVal Name As String) As String
        CommandNameInContext = Name
        Dim current As CommandCollection = Me
        Do Until current._parent Is Nothing
            CommandNameInContext = current.Name & " " & CommandNameInContext
            current = current._parent
        Loop
        Return CommandNameInContext
    End Function

    Private Function GetCountToRoot() As Integer
        GetCountToRoot = 0
        Dim current As CommandCollection = Me
        Do Until current._parent Is Nothing
            GetCountToRoot = GetCountToRoot + 1
            current = current._parent
        Loop
        Return GetCountToRoot
    End Function

    Public Function IsValid() As Boolean
        Return True
    End Function

    Public Function Run(ByVal CommandLine As String) As Integer Implements ICommandExecution.Run
        ' need to find next command
        Dim args() As String = CommandLine.Split(" ")
        Dim rootCount As Integer = GetCountToRoot()
        Dim showHelp As Boolean = False
        Dim CommandName As String = ""
        Dim SelectedCommand As ICommandExecution = Nothing
        Try

            If args Is Nothing Or args.Length = 0 OrElse args(0).Length = 0 OrElse GetCountToRoot() = args.Length Then
                Me.WriteCommandListToConsole()
                Return 1
            Else
                CommandName = args(rootCount)
                If CommandName.ToLower = "help" Then
                    showHelp = True
                    If args.Length - 1 > rootCount Then
                        CommandName = args(rootCount + 1)
                    Else
                        Me.WriteCommandListToConsole()
                        Return 1
                    End If
                End If
            End If

            SelectedCommand = _commands.FindCommand(CommandName)

            If SelectedCommand Is Nothing Then
                CommandOut.Warning("Unrecognized command: {0}.", CommandName)
                Return 2
            Else
                If showHelp Then
                    CType(SelectedCommand, ICommandDefinition).WriteHelpToConsole(Me)
                Else
                    SelectedCommand.Run(CommandLine)
                End If

            End If

        Catch ex As Exception
            CommandOut.Error(ex.ToString)
        End Try
    End Function

    Public Function Run() As Integer
        Dim commandline As String = Environment.CommandLine
        commandline = commandline.Substring(commandline.IndexOf(".exe") + 5)
        Do
            Run(commandline)
            If String.IsNullOrEmpty(commandline.Trim) And Not _defaultExitAction = DefaultCommandAction.Exit Then
                Console.ForegroundColor = ConsoleColor.Cyan
                Console.Write(">>")
                Console.ResetColor()
                commandline = Console.ReadLine()
            Else
                commandline = ""
            End If
        Loop Until commandline = "exit" Or _defaultExitAction = DefaultCommandAction.Exit

    End Function



End Class
