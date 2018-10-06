Imports System.Windows.Forms
Imports GTA
Imports GTA.Math
Imports GTA.Native

Public Class HelicopterView : Inherits Script

    Dim camera As Camera
    Dim helicopter As Entity, pilot As Ped

    Sub New()
        camera = New Camera([Function].[Call](Of Integer)(Hash.CREATE_CAM, "DEFAULT_SPLINE_CAMERA", 0))
        camera.IsActive = False
    End Sub

    Private Sub HelicopterView_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.J Then
            If Not camera.IsActive Then
                ' toggle on
                Dim model As New Model("annihilator")
                Dim abovePlayer As Vector3 = Game.Player.Character.Position + New Vector3(0, 0, 20)

                helicopter = World.CreateVehicle(model, abovePlayer)
                helicopter.AttachTo(pilot, 0)

                Call camera.EnterCameraView(abovePlayer)
                Call camera.AttachTo(helicopter, New Vector3(0, 0, -2))
                Call camera.PointAt(Game.Player.Character)
            Else
                ' toggle off
                Call camera.ExitCameraView
                Call helicopter.Delete()
            End If
        End If
    End Sub
End Class
