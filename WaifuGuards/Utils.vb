Imports System.Runtime.CompilerServices

Module Utils

    <Extension>
    Public Function Append(list As IEnumerable(Of String), add As IEnumerable(Of String)) As String()
        With New List(Of String)
            If Not list Is Nothing Then
                Call .AddRange(list)
            End If
            If Not add Is Nothing Then
                Call .AddRange(add)
            End If

            Return .ToArray
        End With
    End Function
End Module
