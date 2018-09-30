Imports GTA.Math

Public Class FollowPlayer : Inherits TickEvent

    Public Sub New()
        MyBase.New(New TimeSpan(0, 0, 1))
    End Sub

    Protected Overrides Sub DoEvent(script As Waifus)
        For Each waifu As Ped In script.waifuGuards
            If waifu.IsInCombat Then
                Continue For
            End If

            If Not waifu.IsDead Then
                Dim offset As Vector3 = script.offsetAroundMe()
                Dim distance# = Game.Player.Character.Position.DistanceTo(waifu.Position)

                ' If your waifu is too far away with player, then your waifus will running to you
                ' else walking
                If distance >= 15 Then
                    Call waifu.Task.RunTo(Game.Player.Character.Position, False)
                Else
                    Call waifu.Task.GoTo(Game.Player.Character, offset)
                End If
            End If
        Next
    End Sub
End Class
