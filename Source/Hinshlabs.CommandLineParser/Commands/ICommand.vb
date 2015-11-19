Imports Hinshlabs.CommandLineParser
Imports System.IO
Imports System.Collections.ObjectModel

Public Interface ICommandExecution

    Function Run(ByVal ComamndLine As String) As Integer

End Interface

Public Interface ICommandDefinition

    ReadOnly Property Name() As String
    ReadOnly Property Title() As String
    ReadOnly Property Parent() As CommandCollection
    ReadOnly Property Description() As String
    ReadOnly Property Synopsis() As String
    ReadOnly Property Switches() As ReadOnlyCollection(Of SwitchInfo)
    ReadOnly Property Qualifications() As String

End Interface
