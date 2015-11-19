' From http://www.codeproject.com/KB/recipes/commandlineparser.aspx?fid=14982&df=90&mpp=25&noise=3&sort=Position&view=Quick&fr=51#
' Author: Ray Hayes

Imports System.Reflection
Imports System.Text.RegularExpressions

<DefaultMember("Item")> _
Public Class Parser

    ' Fields
    Private m_applicationName As String
    Private m_commandLine As String
    Private m_splitParameters As String()
    Private m_switches As ArrayList
    Private m_workingString As String

    ' Properties
    Public ReadOnly Property ApplicationName() As String
        Get
            Return Me.m_applicationName
        End Get
    End Property

    Default Public ReadOnly Property Item(ByVal name As String) As Object
        Get
            If (Not Me.m_switches Is Nothing) Then
                Dim i As Integer
                For i = 0 To Me.m_switches.Count - 1
                    If (String.Compare(TryCast(Me.m_switches.Item(i), SwitchRecord).Name, name, True) = 0) Then
                        Return TryCast(Me.m_switches.Item(i), SwitchRecord).Value
                    End If
                Next i
            End If
            Return Nothing
        End Get
    End Property

    Public ReadOnly Property Parameters() As String()
        Get
            Return Me.m_splitParameters
        End Get
    End Property

    Public ReadOnly Property Switches() As SwitchInfo()
        Get
            If (Me.m_switches Is Nothing) Then
                Return Nothing
            End If
            Dim infoArray As SwitchInfo() = New SwitchInfo(Me.m_switches.Count - 1) {}
            Dim i As Integer
            For i = 0 To Me.m_switches.Count - 1
                infoArray(i) = New SwitchInfo(Me.m_switches.Item(i))
            Next i
            Return infoArray
        End Get
    End Property

    Public ReadOnly Property UnhandledSwitches() As String()
        Get
            Dim pattern As String = "(\s|^)(?<match>(-{1,2}|/)(.+?))(?=(\s|$))"
            Dim matchs As MatchCollection = New Regex(pattern, (RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase)).Matches(Me.m_workingString)
            If (matchs Is Nothing) Then
                Return Nothing
            End If
            Dim strArray As String() = New String(matchs.Count - 1) {}
            Dim i As Integer
            For i = 0 To matchs.Count - 1
                strArray(i) = matchs.Item(i).Groups.Item("match").Value
            Next i
            Return strArray
        End Get
    End Property

    ' Methods
    Public Sub New(ByVal commandLine As String)
        Me.m_commandLine = ""
        Me.m_workingString = ""
        Me.m_applicationName = ""
        Me.m_splitParameters = Nothing
        Me.m_switches = Nothing
        Me.m_commandLine = commandLine
    End Sub

    Public Sub New(ByVal commandLine As String, ByVal classForAutoAttributes As Object)
        Me.m_commandLine = ""
        Me.m_workingString = ""
        Me.m_applicationName = ""
        Me.m_splitParameters = Nothing
        Me.m_switches = Nothing
        Me.m_commandLine = commandLine
        Dim members As MemberInfo() = classForAutoAttributes.GetType.GetMembers
        Dim i As Integer
        For i = 0 To members.Length - 1
            Dim customAttributes As Object() = members(i).GetCustomAttributes(True)
            If (customAttributes.Length > 0) Then
                Dim record As SwitchRecord = Nothing
                Dim attribute As Attribute
                For Each attribute In customAttributes
                    If TypeOf attribute Is CommandLineSwitchAttribute Then
                        Dim attribute2 As CommandLineSwitchAttribute = DirectCast(attribute, CommandLineSwitchAttribute)
                        If TypeOf members(i) Is PropertyInfo Then
                            Dim info As PropertyInfo = DirectCast(members(i), PropertyInfo)
                            record = New SwitchRecord(attribute2.Name, attribute2.Description, info.PropertyType)
                            record.SetMethod = info.GetSetMethod
                            record.GetMethod = info.GetGetMethod
                            record.PropertyOwner = classForAutoAttributes
                            Exit For
                        End If
                    End If
                Next
                If (Not record Is Nothing) Then
                    Dim attribute3 As Attribute
                    For Each attribute3 In customAttributes
                        If TypeOf attribute3 Is CommandLineAliasAttribute Then
                            Dim attribute4 As CommandLineAliasAttribute = DirectCast(attribute3, CommandLineAliasAttribute)
                            record.AddAlias(attribute4.Alias)
                        End If
                    Next
                End If
                If (Not record Is Nothing) Then
                    If (Me.m_switches Is Nothing) Then
                        Me.m_switches = New ArrayList
                    End If
                    Me.m_switches.Add(record)
                End If
            End If
        Next i
    End Sub

    Public Sub AddSwitch(ByVal names As String(), ByVal description As String)
        If (Me.m_switches Is Nothing) Then
            Me.m_switches = New ArrayList
        End If
        Dim record As New SwitchRecord(names(0), description)
        Dim i As Integer
        For i = 1 To names.Length - 1
            record.AddAlias(names(i))
        Next i
        Me.m_switches.Add(record)
    End Sub

    Public Sub AddSwitch(ByVal name As String, ByVal description As String)
        If (Me.m_switches Is Nothing) Then
            Me.m_switches = New ArrayList
        End If
        Dim record As New SwitchRecord(name, description)
        Me.m_switches.Add(record)
    End Sub

    Private Sub ExtractApplicationName()
        Dim match As Match = New Regex("^(?<commandLine>("".+""|(\S)+))(?<remainder>.+)", RegexOptions.ExplicitCapture).Match(Me.m_commandLine)
        If ((Not match Is Nothing) AndAlso (Not match.Groups.Item("commandLine") Is Nothing)) Then
            Me.m_applicationName = match.Groups.Item("commandLine").Value
            Me.m_workingString = match.Groups.Item("remainder").Value
        End If
    End Sub

    Private Sub HandleSwitches()
        If (Not Me.m_switches Is Nothing) Then
            Dim record As SwitchRecord
            For Each record In Me.m_switches
                Dim regex As New Regex(record.Pattern, (RegexOptions.ExplicitCapture Or RegexOptions.IgnoreCase))
                Dim matchs As MatchCollection = regex.Matches(Me.m_workingString)
                If (Not matchs Is Nothing) Then
                    Dim i As Integer
                    For i = 0 To matchs.Count - 1
                        Dim str As String = Nothing
                        If ((Not matchs.Item(i).Groups Is Nothing) AndAlso (Not matchs.Item(i).Groups.Item("value") Is Nothing)) Then
                            str = matchs.Item(i).Groups.Item("value").Value
                        End If
                        If (record.Type Is GetType(Boolean)) Then
                            Dim flag As Boolean = True
                            If ((Not matchs.Item(i).Groups Is Nothing) AndAlso (Not matchs.Item(i).Groups.Item("value") Is Nothing)) Then
                                Dim str2 As String = str
                                If (Not str2 Is Nothing) Then
                                    str2 = String.IsInterned(str2)
                                    If (str2 Is "+") Then
                                        flag = True
                                    ElseIf (str2 Is "-") Then
                                        flag = False
                                    ElseIf ((str2 Is "") AndAlso (Not record.ReadValue Is Nothing)) Then
                                        flag = Not CBool(record.ReadValue)
                                    End If
                                End If
                            End If
                            record.Notify(flag)
                            Exit For
                        End If
                        If (record.Type Is GetType(String)) Then
                            record.Notify(str)
                        ElseIf (record.Type Is GetType(Integer)) Then
                            record.Notify(Integer.Parse(str))
                        ElseIf record.Type.IsEnum Then
                            record.Notify([Enum].Parse(record.Type, str, True))
                        End If
                    Next i
                End If
                Me.m_workingString = regex.Replace(Me.m_workingString, " ")
            Next
        End If
    End Sub

    Public Function InternalValue(ByVal name As String) As Object
        If (Not Me.m_switches Is Nothing) Then
            Dim i As Integer
            For i = 0 To Me.m_switches.Count - 1
                If (String.Compare(TryCast(Me.m_switches.Item(i), SwitchRecord).Name, name, True) = 0) Then
                    Return TryCast(Me.m_switches.Item(i), SwitchRecord).InternalValue
                End If
            Next i
        End If
        Return Nothing
    End Function

    Public Function Parse() As Boolean
        Me.ExtractApplicationName()
        Me.HandleSwitches()
        Me.SplitParameters()
        Return True
    End Function

    Private Sub SplitParameters()
        Dim matchs As MatchCollection = New Regex("((\s*(""(?<param>.+?)""|(?<param>\S+))))", RegexOptions.ExplicitCapture).Matches(Me.m_workingString)
        If (Not matchs Is Nothing) Then
            Me.m_splitParameters = New String(matchs.Count - 1) {}
            Dim i As Integer
            For i = 0 To matchs.Count - 1
                Me.m_splitParameters(i) = matchs.Item(i).Groups.Item("param").Value
            Next i
        End If
    End Sub

End Class


