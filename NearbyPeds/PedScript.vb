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

    Private Sub PedScript_Tick(sender As Object, e As EventArgs) Handles Me.Tick
        Dim nearby As Ped() = World _
            .GetNearbyPeds(Game.Player.Character.Position, distance) _
            .Where(Function(p) p.RelationshipGroup <> player) _
            .ToArray
    End Sub
End Class
