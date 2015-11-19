' From http://www.codeproject.com/KB/recipes/commandlineparser.aspx?fid=14982&df=90&mpp=25&noise=3&sort=Position&view=Quick&fr=51#
' Author: Ray Hayes

<AttributeUsage(AttributeTargets.Property)> _
Public Class CommandLineSwitchAttribute
    Inherits Attribute
    ' Methods
    Public Sub New(ByVal name As String, ByVal description As String)
        Me.m_name = name
        Me.m_description = description
    End Sub


    ' Properties
    Public ReadOnly Property Description() As String
        Get
            Return Me.m_description
        End Get
    End Property

    Public ReadOnly Property Name() As String
        Get
            Return Me.m_name
        End Get
    End Property


    ' Fields
    Private m_description As String = ""
    Private m_name As String = ""
End Class

