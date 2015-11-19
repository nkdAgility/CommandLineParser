Module DemoModule

    Sub Main(ByVal args() As String)
        Dim commandAction As DefaultCommandAction = DefaultCommandAction.Exit
        If Debugger.IsAttached Then
            commandAction = DefaultCommandAction.Loop
            Console.WriteLine("..running in loop mode...")
        End If
        ' Create root command collection
        Dim root As New CommandCollection("root", Nothing, commandAction)
        ' Add "demo1" fixed command
        root.AddCommand(New Demo1Command(root))
        ' Create sub command colleciton
        Dim innerlist As New CommandCollection("subcmd", root)
        ' Add "demo2" delegate comamnd
        innerlist.AddCommand(New DelegateCommand(Of Demo3CommandLine)(root, "Demo2", AddressOf OnDemo2Run, "demo 2", "no additional information", "demo 2 command", "This command shows how to delegate the run method using the delegate command"))
        ' Add "demo3" fixed command
        innerlist.AddCommand(New Demo3Command(root))
        ' Add sub command collcection to root
        root.AddCommand(innerlist)
        ' Start run
        root.Run()

    End Sub

    Private Function OnDemo2Run() As Integer
        Console.WriteLine("Running demo 2")
        Return 0
    End Function

End Module
