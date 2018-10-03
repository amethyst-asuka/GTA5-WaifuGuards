Imports System.Runtime.CompilerServices

Public Class PendingQueue(Of T As Script)

    Friend ReadOnly pendings As New List(Of PendingEvent(Of T))
    Friend ReadOnly script As T

    Sub New(script As T)
        Me.script = script
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Sub Add([event] As PendingEvent(Of T))
        Call pendings.Add([event])
    End Sub

    Public Sub Tick()
        Dim actives As PendingEvent(Of T)() = pendings _
            .Where(Function(task) task.IsReady) _
            .ToArray

        For Each task As PendingEvent(Of T) In actives
            Call task.Tick(script)
            Call pendings.Remove(task)
        Next
    End Sub
End Class
