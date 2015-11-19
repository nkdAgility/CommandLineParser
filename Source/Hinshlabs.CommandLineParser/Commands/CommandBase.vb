Imports Hinshlabs.CommandLineParser
Imports System.Collections.ObjectModel

Public MustInherit Class CommandBase(Of TCommandLine As {New, CommandLineBase})
    Implements ICommandDefinition, ICommandExecution

    Private m_ParsedCommandLine As TCommandLine
    Private m_Parent As CommandCollection

    Protected ReadOnly Property CommandLine() As TCommandLine
        Get
            If m_ParsedCommandLine Is Nothing Then
                Try
                    m_ParsedCommandLine = Hinshlabs.CommandLineParser.CommandLineBase.CreateCommandLine(Of TCommandLine)()
                Catch ex As Exception
                    CommandOut.Error("Unable to create Command Line object: {0}", ex.ToString)
                End Try
            End If
            Return m_ParsedCommandLine
        End Get
    End Property

    Public ReadOnly Property Parent() As CommandCollection Implements ICommandDefinition.Parent
        Get
            Return m_Parent
        End Get
    End Property

    Public MustOverride ReadOnly Property Description() As String Implements ICommandDefinition.Description
    Public MustOverride ReadOnly Property Title() As String Implements ICommandDefinition.Title
    Public MustOverride ReadOnly Property Switches() As ReadOnlyCollection(Of SwitchInfo) Implements ICommandDefinition.Switches
    Public MustOverride ReadOnly Property Synopsis() As String Implements ICommandDefinition.Synopsis
    Public MustOverride ReadOnly Property Qualifications() As String Implements ICommandDefinition.Qualifications
    Public MustOverride ReadOnly Property Name() As String Implements ICommandDefinition.Name

    Public Sub New(ByVal parent As CommandCollection)
        m_Parent = parent
    End Sub


    Protected MustOverride Function RunCommand() As Integer
    Protected MustOverride Function ValidateCommand() As Boolean

    Public Function IsValid() As Boolean
        Return ValidateCommand()
    End Function

    Public Function Run(ByVal CommandLine As String) As Integer Implements ICommandExecution.Run
        Try
            m_ParsedCommandLine = Hinshlabs.CommandLineParser.CommandLineBase.CreateCommandLine(Of TCommandLine)(CommandLine)
        Catch ex As Exception
            CommandOut.Error("Unable to create Command Line object: {0}", ex.ToString)
        End Try
        If IsValid() Then
            RunCommand()
        End If
        m_ParsedCommandLine = Nothing
    End Function





End Class

