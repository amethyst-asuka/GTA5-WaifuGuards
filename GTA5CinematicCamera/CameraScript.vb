Imports System.Windows.Forms
Imports GTA

Public Class CameraScript : Inherits Script

    Dim toggleOn As Boolean = False
    Dim rand As New Random

    Private Sub CameraScript_Tick(sender As Object, e As EventArgs) Handles Me.Tick
        If toggleOn Then
            Call RunCamera()
        End If
    End Sub

    Private Sub RunCamera()

    End Sub

    Private Sub SplineAbove()

    End Sub

    Private Sub FirstPerson()

    End Sub

    Public Function PickRandom() As Ped

    End Function

    Private Sub CameraScript_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.O Then
            toggleOn = Not toggleOn
        End If
    End Sub
End Class
