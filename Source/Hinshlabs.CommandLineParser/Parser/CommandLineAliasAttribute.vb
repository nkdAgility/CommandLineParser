' From http://www.codeproject.com/KB/recipes/commandlineparser.aspx?fid=14982&df=90&mpp=25&noise=3&sort=Position&view=Quick&fr=51#
' Author: Ray Hayes

<AttributeUsage(AttributeTargets.Property)> _
Public Class CommandLineAliasAttribute
    Inherits Attribute
    ' Methods
    Public Sub New(ByVal [alias] As String)
        Me.m_Alias = [alias]
    End Sub


    ' Properties
    Public ReadOnly Property [Alias]() As String
        Get
            Return Me.m_Alias
        End Get
    End Property

    ' Fields
    Protected m_Alias As String = ""

End Class