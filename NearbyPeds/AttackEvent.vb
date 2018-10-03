Imports System.Runtime.CompilerServices
Imports GTA.Math
Imports GTA.Native

Public Class AttackEvent : Inherits TickEvent(Of PedScript)

    Dim rand As New Random
    Dim peds As New List(Of Ped)
    Dim plus10 As Boolean = False
    Dim explodeds As New List(Of Ped)

    Const MaxAttacks% = 20
    Const SpawnRadius% = 30

    Public Sub New()
        MyBase.New(New TimeSpan(0, 0, 1))

#If DEBUG Then
        ' Call Add("ByStaxx", New Model("ByStaxx"))
#End If
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function offsetAroundMe()
        Dim x = rand.Next(SpawnRadius / 2, SpawnRadius)
        Dim y = rand.Next(SpawnRadius / 2, SpawnRadius)

        ' avoid spawn nearby player
        If rand.NextDouble > 0.5 Then
            x = -x
        End If
        If rand.NextDouble > 0.5 Then
            y = -y
        End If

        Return New Vector3(x, y, 0)
    End Function

    Private Sub Add(modelName$, model As Model)
        Call model.Request(500)

        If Not (model.IsInCdImage AndAlso model.IsValid) Then
            Call UI.ShowSubtitle($"Missing model [{modelName}]...")
            Return
        Else
            Do While Not model.IsLoaded
                Call GTA.Script.Wait(100)
            Loop
        End If

        Dim position = Game.Player.Character.GetOffsetInWorldCoords(offsetAroundMe)
        Dim ped As Ped = World.CreatePed(model, position)
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

        If modelName = "Creeper" Then
            Call explodeds.Add(ped)
            Call UI.ShowSubtitle($"Warning: [{modelName}] incomming! ({peds.Count}/{MaxAttacks})")
        Else
            Call UI.ShowSubtitle($"[{modelName}] incomming! ({peds.Count}/{MaxAttacks})")
        End If

        Call model.MarkAsNoLongerNeeded()
    End Sub

    Protected Overrides Sub DoEvent(script As PedScript)
        If script.ToggleAttacks AndAlso peds.Count < MaxAttacks Then
            With script.NextModel
                Call Add(.name, .model)
            End With
        End If

        If plus10 Then
            For Each dead As Ped In peds.Where(Function(p) p.IsDead).ToArray
                If explodeds.IndexOf(dead) > -1 Then
                    explodeds.Remove(dead)
                    World.AddExplosion(dead.Position, ExplosionType.GasTank, 30, 20)
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
                End If
            End If
        Next
    End Sub

    Public Overrides Sub Tick(script As PedScript)
        Call MyBase.Tick(script)
        Call explosionNearbyPlayerImmediately()
    End Sub

    Private Sub explosionNearbyPlayerImmediately()
        For Each ped As Ped In peds.Where(Function(p) Not p.IsDead)
            Dim distance = Game.Player.Character.Position.DistanceTo(ped.Position)

            If distance <= 10 AndAlso explodeds.IndexOf(ped) > -1 Then
                Call ped.Kill()
                Call World.AddExplosion(ped.Position, ExplosionType.GasTank, 30, 20)
                Call peds.Remove(ped)
                Call explodeds.Remove(ped)
                Call ped.Delete()
            End If
        Next
    End Sub
End Class