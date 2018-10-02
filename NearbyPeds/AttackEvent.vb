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
        Return New Vector3(rand.Next(-300, 300), rand.Next(-300, 300), 0)
    End Function

    Protected Overrides Sub DoEvent(script As PedScript)
        If script.ToggleAttacks AndAlso peds.Count < 10 Then
            Dim model = script.NextModel
            Dim position = Game.Player.Character.GetOffsetInWorldCoords(offsetAroundMe)
            Dim ped As Ped = World.CreatePed(model.model, position)
            Dim weapon As WeaponHash = WeaponHash.Hatchet

            Call ped.Weapons.Give(weapon, 9999, True, True)
            Call ped.Task.FightAgainst(Game.Player.Character)
            Call peds.Add(ped)

            Call UI.ShowSubtitle($"'{model.name}' incomming!")
        End If

        If plus10 Then
            For Each dead As Ped In peds.Where(Function(p) p.IsDead).ToArray
                Call dead.Delete()
            Next
        End If

        plus10 = Not plus10

        For Each ped As Ped In peds
            If Not ped.IsInCombat Then
                Call ped.Task.FightAgainst(Game.Player.Character)
            End If
        Next
    End Sub
End Class
