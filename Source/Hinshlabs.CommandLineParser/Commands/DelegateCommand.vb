Imports Hinshlabs.CommandLineParser
Imports System.IO
Imports System.Collections.ObjectModel
Imports System.Net

Public Class DelegateCommand(Of TCommandLine As {New, CommandLineBase})
    Inherits CommandBase(Of TCommandLine)

    Private m_Description As String
    Private m_Title As String
    Private m_Synopsis As String
    Private m_Qualifications As String
    Private m_name As String
    Private m_RunCommand As Func(Of Integer)

    Public Overrides ReadOnly Property Description() As String
        Get
            Return m_Description
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return m_name
        End Get
    End Property

    Protected Overrides Function RunCommand() As Integer
        Try
            Return m_RunCommand.Invoke
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
            Return m_Title
        End Get
    End Property

    Public Overrides ReadOnly Property Synopsis() As String
        Get
            Return m_Synopsis
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

    Public Sub New(ByVal parent As CommandCollection, ByVal name As String, ByVal runCommand As Func(Of Integer), ByVal title As String, ByVal qualifications As String, ByVal synopsis As String, ByVal description As String)
        MyBase.New(parent)
        m_name = name
        m_RunCommand = runCommand
        m_Title = title
        m_Qualifications = qualifications
        m_Synopsis = synopsis
        m_Description = description
    End Sub

End Class
