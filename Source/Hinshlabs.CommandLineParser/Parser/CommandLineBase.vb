Imports System.IO
Imports System.Collections.ObjectModel

Public Class CommandLineBase

    Private m_Parser As Parser
    Private m_showHelp As Boolean = False

    <CommandLineSwitch("ShowHelp", "Shows the help for the application"), CommandLineAlias("Help")> _
    Public Property ShowHelp() As Boolean
        Get
            Return Me.m_showHelp
        End Get
        Set(ByVal value As Boolean)
            Me.m_showHelp = value
        End Set
    End Property

    Protected Property Parser() As Parser
        Get
            Return m_Parser
        End Get
        Private Set(ByVal value As Parser)
            m_Parser = value
        End Set
    End Property

    Public ReadOnly Property Switches() As ReadOnlyCollection(Of SwitchInfo)
        Get
            Return New ReadOnlyCollection(Of SwitchInfo)(Me.Parser.Switches.ToList)
        End Get
    End Property


    ''' <summary>
    ''' You must use the shared methods to get an instance of this class
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub New()

    End Sub

    ''' <summary>
    ''' Created a command line object using the Environment.CommandLine information
    ''' </summary>
    ''' <typeparam name="TCommandLine">The concrete type of object to create</typeparam>
    ''' <returns>An instance of the object</returns>
    ''' <remarks></remarks>
    Public Shared Function CreateCommandLine(Of TCommandLine As {New, CommandLineBase})() As TCommandLine
        Return CreateCommandLine(Of TCommandLine)(Environment.CommandLine)
    End Function

    ''' <summary>
    ''' Created a command line object using the Environment.CommandLine information
    ''' </summary>
    ''' <typeparam name="TCommandLine">The concrete type of object to create</typeparam>
    ''' <param name="CommandLine">The command line arguments to parse</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CreateCommandLine(Of TCommandLine As {New, CommandLineBase})(ByVal CommandLine As String) As TCommandLine
        Dim instance As New TCommandLine
        Dim parser As New Parser(CommandLine, instance)
        parser.Parse()
        instance.Parser = parser
        Return instance
    End Function

    Public Sub WriteHelp(ByVal output As TextWriter)
        output.WriteLine("Program Name      : {0}", Parser.ApplicationName)
        output.WriteLine("Non-switch Params : {0}", Parser.Parameters.Length)
        Dim i As Integer
        For i = 0 To Parser.Parameters.Length - 1
            output.WriteLine("                {0} : {1}", i, Parser.Parameters(i))
        Next i
        output.WriteLine("----")
        output.WriteLine("Value of ShowHelp    : {0}", Me.ShowHelp)
        output.WriteLine("----")
        Dim switches As SwitchInfo() = Parser.Switches
        If (Not switches Is Nothing) Then
            output.WriteLine("There are {0} registered switches:", switches.Length)
            Dim info As SwitchInfo
            For Each info In switches
                output.WriteLine("Command : {0} - [{1}]", info.Name, info.Description)
                output.Write("Type    : {0} ", info.Type)
                If info.IsEnum Then
                    output.Write("- Enums allowed (")
                    Dim str As String
                    For Each str In info.Enumerations
                        output.Write("{0} ", str)
                    Next
                    output.Write(")")
                End If
                output.WriteLine()
                If (Not info.Aliases Is Nothing) Then
                    output.Write("Aliases : [{0}] - ", info.Aliases.Length)
                    Dim str2 As String
                    For Each str2 In info.Aliases
                        output.Write(" {0}", str2)
                    Next
                    output.WriteLine()
                End If
                output.WriteLine("------> Value is : {0} (Without any callbacks {1})" & ChrW(10), IIf((Not info.Value Is Nothing), info.Value, "(Unknown)"), IIf((Not info.InternalValue Is Nothing), info.InternalValue, "(Unknown)"))
            Next
        Else
            output.WriteLine("There are no registered switches.")
        End If
        output.WriteLine("----")
        If (Not Parser.Item("help") Is Nothing) Then
            output.WriteLine("Request for help = {0}", Parser.Item("help"))
        Else
            output.WriteLine("Request for help has no associated value.")
        End If
        output.WriteLine("User Name is {0}", Parser.Item("name"))
    End Sub


End Class
