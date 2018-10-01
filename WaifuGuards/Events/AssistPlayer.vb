Public Class AssistPlayer : Inherits TickEvent

    Public Sub New()
        MyBase.New(New TimeSpan(0, 0, 1))
    End Sub

    Protected Overrides Sub DoEvent(script As WaifuScript)
        If Game.Player.Character.IsInCombat AndAlso Game.Player.IsTargettingAnything Then
            Dim target As Entity = Game.Player.GetTargetedEntity

            ' Only shoot at ped
            If target Is Nothing OrElse Not TypeOf target Is Ped Then
                Return
            End If

            For Each waifu As Waifu In script.waifuGuards.Where(Function(w) w.IsAvailable)
                Call waifu.TakeAction(
                    Sub(actions As Tasks)
                        Call actions.ClearAllImmediately()
                        Call actions.ShootAt(target)
                    End Sub)
            Next
        End If
    End Sub
End Class
