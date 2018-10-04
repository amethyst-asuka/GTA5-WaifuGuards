﻿Imports System.Runtime.CompilerServices
Imports GTA.Math

Public Class FollowPlayer : Inherits TickEvent(Of WaifuScript)

    Public Sub New()
        MyBase.New(New TimeSpan(0, 0, 1))
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Protected Overrides Sub DoEvent(script As WaifuScript)
        Call PlayerUnion(script, skipAssert:=Function(waifu) waifu.IsDead OrElse waifu.IsInCombat)
    End Sub

    Public Shared Sub PlayerUnion(script As WaifuScript, skipAssert As Func(Of Waifu, Boolean))
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

            If waifu.DistanceToPlayer >= 15 Then
                Call waifu.TakeAction(
                    Sub(action As Tasks)
                        Call action.RunTo(Game.Player.Character.Position - offset, False)
                    End Sub)
            Else
                Call waifu.TakeAction(
                    Sub(action As Tasks)
                        Call action.GoTo(Game.Player.Character, offset)
                    End Sub)
            End If
        Next
    End Sub
End Class
