Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.Reflection

Module Program

    Public Function Main() As Integer
        Return GetType(Program).RunCLI(App.CommandLine)
    End Function

    <ExportAPI("/Run")>
    <Usage("/Run [/port <listen port, default=22335>]")>
    Public Function Run(args As CommandLine) As Integer
        Return New GTA5Multiplex(port:=args("/port") Or 22335).Run
    End Function
End Module
