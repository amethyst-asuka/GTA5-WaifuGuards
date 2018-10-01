Imports System.Runtime.CompilerServices
Imports GTA.Math

Public Class PedScript : Inherits Script

    ReadOnly Skins$() = {
        "tonacho",
        "ByStaxx",
        "Capitan_america",
        "DeadPool",
        "ElRubius",
        "empollon",
        "LuzuGames",
        "perxittaa",
        "Tigre"
    }
    ReadOnly rand As New Random
    ReadOnly player As Integer = Game.Player.Character.RelationshipGroup

    Dim nearbyPeds As List(Of Ped)
    Dim distance As Double = 500

    Public ReadOnly Property NextModel As Model
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Get
            Dim name$ = Skins(rand.Next(0, Skins.Length))
            Return New Model(name)
        End Get
    End Property

    Private Sub PedScript_Tick(sender As Object, e As EventArgs) Handles Me.Tick
        Dim nearby As Ped() = World _
            .GetNearbyPeds(Game.Player.Character.Position, distance) _
            .Where(Function(p)
                       Return nearbyPeds.IndexOf(p) = -1 AndAlso Not Game.Player.Character Is p
                   End Function)

        For Each ped As Ped In nearby
            If Not ped.IsDead Then
                Call nearbyPeds.Add(PedChangeModel(ped))
            End If
        Next

        For Each ped As Ped In nearbyPeds.ToArray
            If ped.IsDead Then
                nearbyPeds.Remove(ped)
            End If
        Next
    End Sub

    Private Function PedChangeModel(nearbyPed As Ped) As Ped
        Dim location As Vector3 = nearbyPed.Position
        Dim ped As Ped = World.CreatePed(NextModel, location)

        With nearbyPed
            ped.Accuracy = .Accuracy
            ped.Alpha = .Alpha
            ped.Armor = .Armor
            ped.CanFlyThroughWindscreen = .CanFlyThroughWindscreen
            ped.CanRagdoll = .CanRagdoll
            ped.CanSufferCriticalHits = .CanSufferCriticalHits
            ped.CanWrithe = .CanWrithe
            ped.DropsWeaponsOnDeath = .DropsWeaponsOnDeath
            ped.FreezePosition = .FreezePosition
            ped.HasCollision = .HasCollision
            ped.Heading = .Heading
            ped.Health = .Health
            ped.IsBulletProof = .IsBulletProof
            ped.IsCollisionProof = .IsCollisionProof
            ped.IsDucking = .IsDucking
            ped.IsExplosionProof = .IsExplosionProof
            ped.IsFireProof = .IsFireProof
            ped.IsInvincible = .IsInvincible
            ped.IsMeleeProof = .IsMeleeProof
            ped.IsOnlyDamagedByPlayer = .IsOnlyDamagedByPlayer
            ped.IsPersistent = .IsPersistent
            ped.IsVisible = .IsVisible
            ped.LodDistance = .LodDistance
            ped.MaxHealth = .MaxHealth
            ped.Money = .Money
            ped.RelationshipGroup = .RelationshipGroup
        End With

        Call nearbyPed.Delete()

        Return ped
    End Function
End Class
