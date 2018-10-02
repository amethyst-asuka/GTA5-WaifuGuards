Imports System.Runtime.CompilerServices
Imports GTA.Math
Imports GTA.Native

Public Class AttackEvent : Inherits TickEvent(Of PedScript)

    Dim rand As New Random
    Dim peds As New List(Of Ped)
    Dim plus10 As Boolean = False
    Dim explodeds As New List(Of Ped)

    Const MaxAttacks% = 5
    Const SpawnRadius% = 60

    Public Sub New()
        MyBase.New(New TimeSpan(0, 0, 10))
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function offsetAroundMe()
        Dim x = rand.Next(-SpawnRadius, SpawnRadius)
        Dim y = rand.Next(-SpawnRadius, SpawnRadius)

        Return New Vector3(x, y, 0)
    End Function

    Protected Overrides Sub DoEvent(script As PedScript)
        If script.ToggleAttacks AndAlso peds.Count < MaxAttacks Then
            Dim model = script.NextModel

            If Not (model.model.IsInCdImage AndAlso model.model.IsValid) Then
                Call UI.ShowSubtitle($"Missing model [{model.name}]...")
                Return
            Else
                Call UI.ShowSubtitle($"[{model.name}] incomming! ({peds.Count + 1}/{MaxAttacks})")
            End If

            Dim position = Game.Player.Character.GetOffsetInWorldCoords(offsetAroundMe)
            Dim ped As Ped = World.CreatePed(model.model, position)
            Dim weapon As WeaponHash = WeaponHash.Hatchet

            Call ped.Weapons.Give(weapon, 9999, True, True)
            Call ped.Task.FightAgainst(Game.Player.Character)
            Call peds.Add(ped)

            ped.AddBlip()

            With ped.CurrentBlip
                .Scale = 0.7!
                .Name = "Incomming Attack!"
                .Color = BlipColor.Yellow
            End With

            If model.name = "ByStaxx" Then
                explodeds.Add(ped)
            End If
        End If

        If plus10 Then
            For Each dead As Ped In peds.Where(Function(p) p.IsDead).ToArray
                If explodeds.IndexOf(dead) > -1 Then
                    explodeds.Remove(dead)
                    World.AddExplosion(dead.Position, ExplosionType.GasCanister, 50, 5)
                End If

                Call peds.Remove(dead)
                Call dead.Delete()
            Next
        End If

        plus10 = Not plus10

        For Each ped As Ped In peds
            If Not ped.IsInCombat Then
                Call ped.Task.FightAgainst(Game.Player.Character)
            ElseIf Not ped.IsDead Then
                Dim distance = Game.Player.Character.Position.DistanceTo(ped.Position)

                If distance >= 200 Then
                    Call ped.Kill()
                ElseIf distance <= 10 AndAlso explodeds.IndexOf(ped) > -1 Then
                    Call ped.Kill()
                    Call World.AddExplosion(ped.Position, ExplosionType.GasCanister, 50, 5)
                    Call peds.Remove(ped)
                    Call explodeds.Remove(ped)
                    Call ped.Delete()
                End If
            End If
        Next
    End Sub
End Class
