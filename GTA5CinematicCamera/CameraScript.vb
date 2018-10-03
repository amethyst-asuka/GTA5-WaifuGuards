Imports System.Windows.Forms
Imports GTA
Imports GTA.Math

Public Class CameraScript : Inherits Script

    Dim rand As New Random
    Dim splineCam As SplineCamera

    Private Sub CameraScript_Tick(sender As Object, e As EventArgs) Handles Me.Tick
        Call splineCam.Update()
    End Sub

    Private Sub SplineAbove()

    End Sub

    Private Sub FirstPerson()

    End Sub

    Public Function PickRandom() As Ped

    End Function

    Private Sub CameraScript_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.O Then
            splineCam.EnterCameraView(Game.Player.Character.GetOffsetInWorldCoords(New Vector3(0, 0, 10.0F)))
        End If
    End Sub
End Class
