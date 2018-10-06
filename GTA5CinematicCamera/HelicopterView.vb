Imports System.Windows.Forms
Imports GTA
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
                helicopter = World.CreateVehicle
                helicopter.AttachTo(pilot, 0)

                Call camera.EnterCameraView()
            Else
                ' toggle off
                Call camera.ExitCameraView
                Call helicopter.Delete()
            End If
        End If
    End Sub
End Class
