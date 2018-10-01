Imports System.Runtime.CompilerServices
Imports GTA.Math

Public Class AttackEvent : Inherits TickEvent(Of PedScript)

    Dim rand As New Random

    Public Sub New()
        MyBase.New(New TimeSpan(0, 0, 5))
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function offsetAroundMe()
        Return New Vector3(rand.Next(-500, 500), rand.Next(-500, 500), 0)
    End Function

    Protected Overrides Sub DoEvent(script As PedScript)
        If script.ToggleAttacks Then
            Dim model As Model = script.NextModel
            Dim position = Game.Player.Character.GetOffsetInWorldCoords(offsetAroundMe)
            Dim ped As Ped = World.CreatePed(model, position)
            Dim weapon = WaifuScript.favoriteWeapons(rand.Next(0, WaifuScript.favoriteWeapons.Length))

            Call ped.Weapons.Give(weapon, 9999, True, True)
            Call ped.Task.FightAgainst(Game.Player.Character)
        End If
    End Sub
End Class
