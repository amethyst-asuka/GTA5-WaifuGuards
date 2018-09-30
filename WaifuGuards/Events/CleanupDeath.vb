Imports WaifuGuards

Public Class CleanupDeath : Inherits TickEvent

    Public Sub New()
        MyBase.New(New TimeSpan(0, 0, 30))
    End Sub

    Protected Overrides Sub DoEvent(script As Waifus)
        For Each waifu In script.waifuGuards.ToArray
            If waifu.IsDead Then
                Call waifu.Delete()
                Call script.waifuGuards.Remove(waifu)
            End If
        Next
    End Sub
End Class
