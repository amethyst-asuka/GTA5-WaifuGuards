Imports System.Runtime.CompilerServices
Imports System.Windows.Forms
Imports GTA
Imports GTA.Math

Public Class Waifus : Inherits Script

    ReadOnly names$() = WaifuList.LoadNames
    ReadOnly rand As New Random
    ReadOnly waifuGuards As New List(Of Ped)

    Dim lastCheck As DateTime = Now

    Shared ReadOnly twoSecond As New TimeSpan(0, 0, 2)

    Private Sub spawnWaifu(name As String)
        Dim pos = Game.Player.Character.GetOffsetInWorldCoords(offsetAroundMe)
        Dim waifu As Ped = World.CreatePed(New Model(name), pos)

        waifu.Weapons.Give(Native.WeaponHash.SMG, 9999, True, True)
        waifu.RelationshipGroup = Game.Player.Character.RelationshipGroup
        waifu.MaxHealth = 10000
        waifu.Armor = 10000

        Call waifuGuards.Add(waifu)
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Private Function offsetAroundMe()
        Return New Vector3(rand.Next(-10, 10), rand.Next(-10, 10), 0)
    End Function

    ''' <summary>
    ''' Press key ``Y`` for spawn a waifu.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Waifus_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Y Then
            Call spawnWaifu(names(rand.Next(0, names.Length)))
        ElseIf e.KeyCode = Keys.NumPad9 Then
            ' spawn all
            For Each name As String In names
                If waifuGuards.Count < 10 Then
                    ' too many peds will makes GTAV crashed.
                    Call spawnWaifu(name)
                Else
                    Exit For
                End If
            Next
        End If
    End Sub

    Private Sub Waifus_Tick(sender As Object, e As EventArgs) Handles Me.Tick
        If (Now - lastCheck) >= twoSecond Then
            For Each waifu In waifuGuards.ToArray
                If waifu.IsDead Then
                    Call waifu.Delete()
                    Call waifuGuards.Remove(waifu)
                Else
                    Dim offset As Vector3 = offsetAroundMe()

                    ' If the player is running, then your waifus will running to you
                    ' else walking
                    If Game.Player.Character.IsRunning Then
                        Call waifu.Task.RunTo(Game.Player.Character.Position, False)
                    Else
                        Call waifu.Task.GoTo(Game.Player.Character, offset)
                    End If
                End If
            Next

            lastCheck = Now
        End If

        ' Keeps alive
        For Each waifu In waifuGuards
            If Not waifu.IsDead Then
                waifu.Health += 100

                If Game.Player.Character.IsShooting Then
                    waifu.Task.ShootAt(Game.Player.Character.GetJackTarget)
                End If
            End If
        Next
    End Sub
End Class
