' From http://www.codeproject.com/KB/recipes/commandlineparser.aspx?fid=14982&df=90&mpp=25&noise=3&sort=Position&view=Quick&fr=51#
' Author: Ray Hayes

Public Class SwitchInfo
    ' Methods
    Public Sub New(ByVal rec As Object)
        If Not TypeOf rec Is SwitchRecord Then
            Throw New ArgumentException
        End If
        Me.m_Switch = rec
    End Sub


    ' Properties
    Public ReadOnly Property Aliases() As String()
        Get
            Return TryCast(Me.m_Switch, SwitchRecord).Aliases
        End Get
    End Property

    Public ReadOnly Property Description() As String
        Get
            Return TryCast(Me.m_Switch, SwitchRecord).Description
        End Get
    End Property

    Public ReadOnly Property Enumerations() As String()
        Get
            Return TryCast(Me.m_Switch, SwitchRecord).Enumerations
        End Get
    End Property

    Public ReadOnly Property InternalValue() As Object
        Get
            Return TryCast(Me.m_Switch, SwitchRecord).InternalValue
        End Get
    End Property

    Public ReadOnly Property IsEnum() As Boolean
        Get
            Return TryCast(Me.m_Switch, SwitchRecord).Type.IsEnum
        End Get
    End Property

    Public ReadOnly Property Name() As String
        Get
            Return TryCast(Me.m_Switch, SwitchRecord).Name
        End Get
    End Property

    Public ReadOnly Property Type() As Type
        Get
            Return TryCast(Me.m_Switch, SwitchRecord).Type
        End Get
    End Property

    Public ReadOnly Property Value() As Object
        Get
            Return TryCast(Me.m_Switch, SwitchRecord).Value
        End Get
    End Property


    ' Fields
    Private m_Switch As Object = Nothing
End Class