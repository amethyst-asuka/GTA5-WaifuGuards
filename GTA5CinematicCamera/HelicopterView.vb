Imports System.Windows.Forms
Imports GTA
Imports GTA.Math
Imports GTA.Native

Public Class HelicopterView : Inherits Script

    Dim camera As Camera
    Dim helicopter As Vehicle, pilot As Ped

    Sub New()
        camera = New Camera([Function].[Call](Of Integer)(Hash.CREATE_CAM, "DEFAULT_SPLINE_CAMERA", 0))
        camera.IsActive = False
    End Sub

    Private Sub HelicopterView_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.O Then
            If Not camera.IsActive Then
                ' toggle on
                Dim model As New Model("buzzard")
                Dim position As Vector3 = Game.Player.Character.Position + New Vector3(30, 30, 100)

                helicopter = World.CreateVehicle(model, position)
                model = New Model("rmiku2016")
                pilot = helicopter.CreatePedOnSeat(VehicleSeat.Driver, model)
                pilot.AlwaysKeepTask = True
                pilot.BlockPermanentEvents = True
                pilot.DrivingSpeed = 50
                pilot.DrivingStyle = DrivingStyle.Rushed

                pilot.AddBlip()

                With pilot.CurrentBlip
                    .Color = BlipColor.Yellow
                    .IsFlashing = True
                    .Scale = 1
                    .Name = "Camera"
                End With

                ' pilot.Task.ClearAllImmediately()
            Else
                ' toggle off
                Call camera.ExitCameraView
                Call pilot.Delete()
                Call helicopter.Delete()
            End If
        End If
    End Sub

    Dim last As Date = Now

    Private Sub HelicopterView_Tick(sender As Object, e As EventArgs) Handles Me.Tick
        If (Not pilot Is Nothing) AndAlso last - Now > New TimeSpan(0, 0, 3) Then
            Dim abovePlayer As Vector3 = Game.Player.Character.Position + New Vector3(15, 15, 140)
            Dim followPlayer As InputArgument() = New InputArgument() {
                pilot, helicopter, 0, 0,
                abovePlayer.X, abovePlayer.Y, abovePlayer.Z,
                4.0!, 100.0!, 0!, 90.0!, 0, CSng(-200)
            }

            [Function].Call(Hash._0x23703CD154E83B88, followPlayer)
            ' pilot.Task.DriveTo(helicopter, abovePlayer, 10, 50)

            last = Now
        End If
    End Sub
End Class
