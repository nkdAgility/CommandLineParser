' From http://www.codeproject.com/KB/recipes/commandlineparser.aspx?fid=14982&df=90&mpp=25&noise=3&sort=Position&view=Quick&fr=51#
' Author: Ray Hayes

Imports System.Reflection

Friend Class SwitchRecord

    ' Methods
    Public Sub New(ByVal name As String, ByVal description As String)
        Me.m_name = ""
        Me.m_description = ""
        Me.m_value = Nothing
        Me.m_switchType = GetType(Boolean)
        Me.m_Aliases = Nothing
        Me.m_Pattern = ""
        Me.m_SetMethod = Nothing
        Me.m_GetMethod = Nothing
        Me.m_PropertyOwner = Nothing
        Me.Initialize(name, description)
    End Sub

    Public Sub New(ByVal name As String, ByVal description As String, ByVal type As Type)
        Me.m_name = ""
        Me.m_description = ""
        Me.m_value = Nothing
        Me.m_switchType = GetType(Boolean)
        Me.m_Aliases = Nothing
        Me.m_Pattern = ""
        Me.m_SetMethod = Nothing
        Me.m_GetMethod = Nothing
        Me.m_PropertyOwner = Nothing
        If (((Not type Is GetType(Boolean)) AndAlso (Not type Is GetType(String))) AndAlso ((Not type Is GetType(Integer)) AndAlso Not type.IsEnum)) Then
            Throw New ArgumentException("Currently only Ints, Bool and Strings are supported")
        End If
        Me.m_switchType = type
        Me.Initialize(name, description)
    End Sub

    Public Sub AddAlias(ByVal [alias] As String)
        If (Me.m_Aliases Is Nothing) Then
            Me.m_Aliases = New ArrayList
        End If
        Me.m_Aliases.Add([alias])
        Me.BuildPattern()
    End Sub

    Private Sub BuildPattern()
        Dim str4 As String
        Dim name As String = Me.Name
        If ((Not Me.Aliases Is Nothing) AndAlso (Me.Aliases.Length > 0)) Then
            Dim str2 As String
            For Each str2 In Me.Aliases
                name = (name & "|" & str2)
            Next
        End If
        Dim str3 As String = "(\s|^)(?<match>(-{1,2}|/)("
        Dim str5 As String = "(?=(\s|$))"
        If (Me.Type Is GetType(Boolean)) Then
            str4 = ")(?<value>(\+|-){0,1}))"
        ElseIf (Me.Type Is GetType(String)) Then
            str4 = ")(?::|\s+))((?:"")(?<value>.+)(?:"")|(?<value>\S+))"
        ElseIf (Me.Type Is GetType(Integer)) Then
            str4 = ")(?::|\s+))((?<value>(-|\+)[0-9]+)|(?<value>[0-9]+))"
        Else
            If Not Me.Type.IsEnum Then
                Throw New ArgumentException
            End If
            Dim enumerations As String() = Me.Enumerations
            Dim str6 As String = enumerations(0)
            Dim i As Integer
            For i = 1 To enumerations.Length - 1
                str6 = (str6 & "|" & enumerations(i))
            Next i
            str4 = (")(?::|\s+))(?<value>" & str6 & ")")
        End If
        Me.m_Pattern = (str3 & name & str4 & str5)
    End Sub

    Private Sub Initialize(ByVal name As String, ByVal description As String)
        Me.m_name = name
        Me.m_description = description
        Me.BuildPattern()
    End Sub

    Public Sub Notify(ByVal value As Object)
        If ((Not Me.m_PropertyOwner Is Nothing) AndAlso (Not Me.m_SetMethod Is Nothing)) Then
            Dim parameters As Object() = New Object() {value}
            Me.m_SetMethod.Invoke(Me.m_PropertyOwner, parameters)
        End If
        Me.m_value = value
    End Sub


    ' Properties
    Public ReadOnly Property Aliases() As String()
        Get
            If (Me.m_Aliases Is Nothing) Then
                Return Nothing
            End If
            Return DirectCast(Me.m_Aliases.ToArray(GetType(String)), String())
        End Get
    End Property

    Public Property Description() As String
        Get
            Return Me.m_description
        End Get
        Set(ByVal value As String)
            Me.m_description = value
        End Set
    End Property

    Public ReadOnly Property Enumerations() As String()
        Get
            If Me.m_switchType.IsEnum Then
                Return [Enum].GetNames(Me.m_switchType)
            End If
            Return Nothing
        End Get
    End Property

    Public WriteOnly Property GetMethod() As MethodInfo
        Set(ByVal value As MethodInfo)
            Me.m_GetMethod = value
        End Set
    End Property

    Public ReadOnly Property InternalValue() As Object
        Get
            Return Me.m_value
        End Get
    End Property

    Public Property Name() As String
        Get
            Return Me.m_name
        End Get
        Set(ByVal value As String)
            Me.m_name = value
        End Set
    End Property

    Public ReadOnly Property Pattern() As String
        Get
            Return Me.m_Pattern
        End Get
    End Property

    Public WriteOnly Property PropertyOwner() As Object
        Set(ByVal value As Object)
            Me.m_PropertyOwner = value
        End Set
    End Property

    Public ReadOnly Property ReadValue() As Object
        Get
            Dim obj2 As Object = Nothing
            If ((Not Me.m_PropertyOwner Is Nothing) AndAlso (Not Me.m_GetMethod Is Nothing)) Then
                obj2 = Me.m_GetMethod.Invoke(Me.m_PropertyOwner, Nothing)
            End If
            Return obj2
        End Get
    End Property

    Public WriteOnly Property SetMethod() As MethodInfo
        Set(ByVal value As MethodInfo)
            Me.m_SetMethod = value
        End Set
    End Property

    Public ReadOnly Property Type() As Type
        Get
            Return Me.m_switchType
        End Get
    End Property

    Public ReadOnly Property Value() As Object
        Get
            If (Not Me.ReadValue Is Nothing) Then
                Return Me.ReadValue
            End If
            Return Me.m_value
        End Get
    End Property


    ' Fields
    Private m_Aliases As ArrayList
    Private m_description As String
    Private m_GetMethod As MethodInfo
    Private m_name As String
    Private m_Pattern As String
    Private m_PropertyOwner As Object
    Private m_SetMethod As MethodInfo
    Private m_switchType As Type
    Private m_value As Object
End Class