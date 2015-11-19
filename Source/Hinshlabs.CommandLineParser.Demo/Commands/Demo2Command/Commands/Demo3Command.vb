Imports Hinshlabs.CommandLineParser
Imports System.IO
Imports System.Collections.ObjectModel
Imports System.Net

Public Class Demo3Command
    Inherits CommandBase(Of Demo3CommandLine)

    Private m_PortalLocation As Uri

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "[Demo3Command.Description]"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Demo3"
        End Get
    End Property

    Public Sub New(ByVal parent As CommandCollection)
        MyBase.New(parent)
    End Sub

    Protected Overrides Function RunCommand() As Integer
        Try
            CommandOut.Warning("running Demo3")
            Return -1
        Catch ex As Exception
            CommandOut.Error("Failed: {0}", ex.ToString)
            Return -1
        End Try
    End Function

    Protected Overrides Function ValidateCommand() As Boolean
        Return True
    End Function

    Public Overrides ReadOnly Property Title() As String
        Get
            Return "[Demo3Command.Title]"
        End Get
    End Property

    Public Overrides ReadOnly Property Synopsis() As String
        Get
            Return "[Demo3Command.Synopsis]"
        End Get
    End Property

    Public Overrides ReadOnly Property Switches() As ReadOnlyCollection(Of SwitchInfo)
        Get
            Return CommandLine.Switches
        End Get
    End Property

    Public Overrides ReadOnly Property Qualifications() As String
        Get
            Return "[Demo3Command.Qualifications]"
        End Get
    End Property

End Class
