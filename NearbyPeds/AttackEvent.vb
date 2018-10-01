Imports System.Runtime.CompilerServices
Imports GTA.Math
Imports GTA.Native

Public Class AttackEvent : Inherits TickEvent(Of PedScript)

    Dim rand As New Random
    Dim peds As New List(Of Ped)
    Dim plus10 As Boolean = False

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
            Dim weapon As WeaponHash = WeaponHash.Hatchet

            Call ped.Weapons.Give(weapon, 9999, True, True)
            Call ped.Task.FightAgainst(Game.Player.Character)
            Call peds.Add(ped)
        End If

        If plus10 Then
            For Each dead As Ped In peds.Where(Function(p) p.IsDead).ToArray
                Call dead.Delete()
            Next
        End If

        plus10 = Not plus10
    End Sub
End Class
