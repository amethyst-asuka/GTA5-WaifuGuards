'Imports System.Windows.Forms
'Imports GTA
'Imports GTA.Math

'Public Class CameraScript : Inherits Script

'    Public ReadOnly splineCam As New SplineCamera With {
'        .InterpToPlayer = True
'    }

'    Dim pointPicker As New PointPicker
'    Dim toggleOn As Boolean = False

'    Private Sub CameraScript_Tick(sender As Object, e As EventArgs) Handles Me.Tick
'        Call splineCam.Update()

'        If toggleOn Then
'            Call pointPicker.Tick(Me)
'        End If
'    End Sub

'    Private Sub CameraScript_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
'        If e.KeyCode = Keys.O Then
'            toggleOn = Not toggleOn

'            If toggleOn Then
'                Call splineCam.EnterCameraView(Game.Player.Character.GetOffsetInWorldCoords(New Vector3(0, 0, 10.0F)))
'            Else
'                Call splineCam.ExitCameraView()
'            End If
'        End If
'    End Sub
'End Class
