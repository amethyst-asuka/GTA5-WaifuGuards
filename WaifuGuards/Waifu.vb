Imports System.Runtime.CompilerServices
Imports System.Windows.Forms
Imports GTA.Math
Imports GTA.Native

Public Class Waifus : Inherits Script

    ReadOnly names$() = WaifuList.LoadNames
    ReadOnly rand As New Random

    Shared ReadOnly defaultWeapons As WeaponHash() = {
        WeaponHash.HeavySniper,
        WeaponHash.Railgun,
        WeaponHash.MicroSMG,
        WeaponHash.SpecialCarbine,
        WeaponHash.CombatPDW,
        WeaponHash.SMG
    }

    Friend ReadOnly waifuGuards As New List(Of Ped)
    Friend ReadOnly events As New List(Of TickEvent)

    Sub New()
        events.Add(New CleanupDeath)
        events.Add(New FollowPlayer)
    End Sub

    Private Sub spawnWaifu(name As String)
        Dim pos = Game.Player.Character.GetOffsetInWorldCoords(offsetAroundMe)
        Dim waifu As Ped = World.CreatePed(New Model(name), pos)
        Dim randWeapon As WeaponHash = defaultWeapons(rand.Next(defaultWeapons.Length))

        waifu.Weapons.Give(randWeapon, 9999, True, True)
        waifu.RelationshipGroup = Game.Player.Character.RelationshipGroup
        waifu.MaxHealth = 10000
        waifu.Armor = 10000
        waifu.IsInvincible = True

        Call waifuGuards.Add(waifu)
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Friend Function offsetAroundMe()
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
        For Each [event] As TickEvent In events
            Call [event].Tick(Me)
        Next

        For Each waifu As Ped In waifuGuards
            If Not waifu.IsDead Then
                If Game.Player.Character.IsShooting AndAlso Game.Player.IsTargetting(waifu) Then
                    Call waifu.Kill()
                End If
            End If
        Next
    End Sub
End Class
