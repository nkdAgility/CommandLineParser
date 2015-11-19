Imports $rootnamespace$
Imports System.IO
Imports System.Collections.ObjectModel
Imports System.Net

Public Class Demo1Command
    Inherits CommandBase(Of Demo1CommandLine)


    Private m_PortalLocation As Uri

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "demo 1 command demonstrates a sinle nested command"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Demo1"
        End Get
    End Property

    Protected Overrides Function ValidateCommand() As Boolean
        Return True
    End Function

    Public Overrides ReadOnly Property Title() As String
        Get
            Return "demo 1"
        End Get
    End Property

    Public Overrides ReadOnly Property Synopsis() As String
        Get
            Return "demo 1 command"
        End Get
    End Property

    Public Overrides ReadOnly Property Switches() As ReadOnlyCollection(Of SwitchInfo)
        Get
            Return CommandLine.Switches
        End Get
    End Property

    Public Overrides ReadOnly Property Qualifications() As String
        Get
            Return String.Empty
        End Get
    End Property

    Public Sub New(ByVal parent As CommandCollection)
        MyBase.New(parent)
    End Sub

    Protected Overrides Function RunCommand() As Integer
        Try
            CommandOut.Warning("running Demo1")
            Return -1
        Catch ex As Exception
            CommandOut.Error("Failed: {0}", ex.ToString)
            Return -1
        End Try
    End Function

End Class
