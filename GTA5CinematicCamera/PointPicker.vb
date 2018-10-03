Imports GTA
Imports GTA.Math
Imports GTA.WaifuGuards

Public Class PointPicker : Inherits TickEvent(Of CameraScript)

    Dim rand As New Random

    Public Sub New()
        MyBase.New(New TimeSpan(0, 0, 1))
    End Sub

    Protected Overrides Sub DoEvent(script As CameraScript)
        Dim nextDbl = rand.NextDouble

        'If nextDbl > 0.5 Then
        Call SplineAbove(script.splineCam)
        'Else
        'Call FirstPerson()
        ' End If
    End Sub

    '          [B]
    '          +y
    '           ^
    '           |
    ' [C] -x -- + ----> +x [A]
    '           |
    '          -y 
    '          [D]

    Private Sub SplineAbove(camera As SplineCamera)
        Dim p = PickRandom.Position
        Dim A = New Vector3(p.X + 2, p.Y, p.Z + 5)
        Dim B = New Vector3(p.X, p.Y + 2, p.Z + 5)
        Dim C = New Vector3(p.X - 2, p.Y, p.Z + 5)
        Dim D = New Vector3(p.X, p.Y - 2, p.Z + 5)

        Call camera.AddNode(A, Utils.GetLookRotation(p, A).Axis, 250)
        Call camera.AddNode(B, Utils.GetLookRotation(p, B).Axis, 250)
        Call camera.AddNode(C, Utils.GetLookRotation(p, C).Axis, 250)
        Call camera.AddNode(D, Utils.GetLookRotation(p, D).Axis, 250)
    End Sub

    Private Sub FirstPerson(camera As SplineCamera)

    End Sub

    Public Function PickRandom() As Ped
        Dim peds = World.GetNearbyPeds(Game.Player.Character.Position, 30) _
            .Where(Function(p)
                       Return p.RelationshipGroup = Game.Player.Character.RelationshipGroup
                   End Function) _
            .ToArray
        Return peds(rand.Next(0, peds.Length))
    End Function
End Class
