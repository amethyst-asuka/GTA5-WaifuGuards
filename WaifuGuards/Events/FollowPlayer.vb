Imports System.Runtime.CompilerServices
Imports GTA.Math

Public Class FollowPlayer : Inherits TickEvent(Of WaifuScript)

    Public ReadOnly Property StopFollow As Boolean = False

    Public Sub New()
        MyBase.New(New TimeSpan(0, 0, 1))
    End Sub

    Public Sub Start()
        _StopFollow = False
    End Sub

    Public Sub [Stop](waifus As IEnumerable(Of Waifu))
        _StopFollow = True

        For Each waifu In waifus
            Call waifu.TakeAction(
                Sub(actions As Tasks)
                    Call actions.ClearAllImmediately()
                End Sub)
        Next
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Protected Overrides Sub DoEvent(script As WaifuScript)
        If Not StopFollow Then
            Call PlayerUnion(script, skipAssert:=Function(waifu) waifu.IsDead OrElse waifu.IsInCombat)
        End If
    End Sub

    Public Shared Sub PlayerUnion(script As WaifuScript, skipAssert As Func(Of Waifu, Boolean))
        Dim playerOutOfVehicle As Boolean = Not Game.Player.Character.IsInVehicle

        For Each waifu As Waifu In script.waifuGuards
            If True = skipAssert(waifu) Then
                Continue For
            End If

            Dim offset As Vector3 = script.offsetAroundMe()

            ' If your waifu is too far away with player, then your waifus will running to you
            ' else walking
            If waifu.IsInCombat Then
                Call waifu.TakeAction(
                    Sub(action As Tasks)
                        Call action.ClearAllImmediately()
                    End Sub)
            End If

            If waifu.DistanceToPlayer >= 20 Then
                Call waifu.TakeAction(
                    Sub(action As Tasks)
                        Call action.RunTo(Game.Player.Character.Position - offset, False)
                    End Sub)
            ElseIf waifu.DistanceToPlayer >= 5 Then
                Call waifu.TakeAction(
                    Sub(action As Tasks)
                        Call action.GoTo(Game.Player.Character, offset)
                    End Sub)
            Else
                ' too close, do nothing
                Call waifu.TakeAction(
                    Sub(actions As Tasks)
                        Call actions.WanderAround(Game.Player.Character.Position, 10)
                    End Sub)
            End If

            If playerOutOfVehicle AndAlso waifu.IsInVehicle Then
                Call waifu.TakeAction(
                    Sub(actions As Tasks)
                        Call actions.LeaveVehicle()
                    End Sub)
            End If
        Next
    End Sub
End Class
