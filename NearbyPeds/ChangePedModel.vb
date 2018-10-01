Imports GTA.Math

Public Class ChangePedModel : Inherits TickEvent(Of PedScript)

    Dim distance As Double = 500

    Public Sub New()
        MyBase.New(New TimeSpan(0, 0, 1))
    End Sub

    Protected Overrides Sub DoEvent(script As PedScript)
        If Not script.ToggleChangeModel Then
            Return
        End If

        Dim nearby As Ped() = World _
           .GetNearbyPeds(Game.Player.Character.Position, distance) _
           .Where(Function(p)
                      Return script.nearbyPeds.IndexOf(p) = -1 AndAlso Not Game.Player.Character Is p
                  End Function)

        For Each ped As Ped In nearby
            If Not ped.IsDead Then
                Call script.nearbyPeds.Add(PedChangeModel(ped, script))
            End If
        Next

        For Each ped As Ped In script.nearbyPeds.ToArray
            If ped.IsDead Then
                Call script.nearbyPeds.Remove(ped)
            End If
        Next
    End Sub

    Private Function PedChangeModel(nearbyPed As Ped, script As PedScript) As Ped
        Dim location As Vector3 = nearbyPed.Position
        Dim ped As Ped = World.CreatePed(script.NextModel.model, location)

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
