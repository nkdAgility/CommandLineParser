Imports Hinshlabs.CommandLineParser
Imports System.Collections.ObjectModel

Public Class Demo3CommandLine
    Inherits CommandLineBase

    Private m_value1 As String
    Private m_value2 As Value2Values = Value2Values.Value1

    <CommandLineSwitch("Value1", "[CommandLineSwitch.Description]"), CommandLineAlias("v1")> _
    Public Property Value1() As String
        Get
            Return Me.m_value1
        End Get
        Set(ByVal value As String)
            Me.m_value1 = value
        End Set
    End Property

    <CommandLineSwitch("Value2", "[CommandLineSwitch.Description]"), CommandLineAlias("v2")> _
    Public Property Value2() As Value2Values
        Get
            Return Me.m_value2
        End Get
        Set(ByVal value As Value2Values)
            Me.m_value2 = value
        End Set
    End Property

    Public Enum Value2Values
        Value1
        Value2
        Value3
    End Enum

End Class