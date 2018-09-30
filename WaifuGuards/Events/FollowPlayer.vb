Imports GTA.Math

Public Class FollowPlayer : Inherits TickEvent

    Public Sub New()
        MyBase.New(New TimeSpan(0, 0, 1))
    End Sub

    Protected Overrides Sub DoEvent(script As Waifus)
        For Each waifu In script.waifuGuards
            If Not waifu.IsDead Then
                Dim offset As Vector3 = script.offsetAroundMe()

                ' If the player is running, then your waifus will running to you
                ' else walking
                If Game.Player.Character.IsRunning Then
                    Call waifu.Task.RunTo(Game.Player.Character.Position, False)
                Else
                    Call waifu.Task.GoTo(Game.Player.Character, offset)
                End If
            End If
        Next
    End Sub
End Class
