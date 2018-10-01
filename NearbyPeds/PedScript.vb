Imports System.Runtime.CompilerServices

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
                       Return p.RelationshipGroup <> player AndAlso nearbyPeds.IndexOf(p) = -1
                   End Function)

        For Each ped As Ped In nearby
            If Not ped.IsDead Then
                Dim location = ped.Position
                ped.Delete()
                ped = World.CreatePed(NextModel, location)
                nearbyPeds.Add(ped)
            End If
        Next

        For Each ped As Ped In nearbyPeds.ToArray
            If ped.IsDead Then
                nearbyPeds.Remove(ped)
            End If
        Next
    End Sub
End Class
