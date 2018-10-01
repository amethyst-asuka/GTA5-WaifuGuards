Imports System.Runtime.CompilerServices
Imports System.Windows.Forms
Imports GTA.Math
Imports GTA.Native

Public Class WaifuScript : Inherits Script

    ReadOnly names$() = WaifuList.LoadNames
    ReadOnly rand As New Random

    Public Shared ReadOnly favoriteWeapons As WeaponHash() = {
        WeaponHash.HeavySniper,
        WeaponHash.Railgun,
        WeaponHash.MicroSMG,
        WeaponHash.SpecialCarbine,
        WeaponHash.CombatPDW,
        WeaponHash.SMG
    }

    Friend ReadOnly waifuGuards As New List(Of Waifu)
    Friend ReadOnly events As New List(Of TickEvent(Of WaifuScript))
    Friend ReadOnly pendings As New List(Of PendingEvent)

    Sub New()
        If WaifuList.IsWaifusMegaPackInstalled Then
            events.Add(New FollowPlayer)
        Else
            ' Given warning message
            ' UI.Notify("[Waifus mega pack] not found, you can download this mod from: https://zh.gta5-mods.com/player/lolis-and-waifus-mega-pack-blz")
        End If
    End Sub

    Private Sub spawnWaifu(name As String)
        Dim waifu As New Waifu(name, Me)
        Dim nextHash = rand.Next(favoriteWeapons.Length)
        Dim randWeapon As WeaponHash = favoriteWeapons(nextHash)

        Call waifu.TakeAction(
            Sub(waifuPed As Ped)
                waifuPed.Weapons.Give(randWeapon, 9999, True, True)
                waifuPed.RelationshipGroup = Game.Player.Character.RelationshipGroup
                waifuPed.NeverLeavesGroup = True
                waifuPed.MaxHealth = 10000
                waifuPed.Armor = 10000
                waifuPed.IsInvincible = True
            End Sub)

        Call waifuGuards.Add(waifu)
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function offsetAroundMe()
        Return New Vector3(rand.Next(-10, 10), rand.Next(-10, 10), 0)
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Sub Pending(action As PendingEvent)
        pendings.Add(action)
    End Sub

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
        ElseIf e.KeyCode = Keys.U Then
            ' union all your waifus
            ' force waifu stop current task and guard player immediately
            Call FollowPlayer.PlayerUnion(Me, Function() False)
        End If
    End Sub

    Private Sub Waifus_Tick(sender As Object, e As EventArgs) Handles Me.Tick
        For Each [event] As TickEvent(Of WaifuScript) In events
            Call [event].Tick(Me)
        Next

        For Each waifu As Waifu In waifuGuards.ToArray
            If Not waifu.IsDead Then
                If waifu.IsShootByPlayer Then
                    Call waifu.Kill()
                End If

                ' try to prevent kill each other
                For Each partner As Waifu In waifuGuards _
                    .Where(Function(ped)
                               Return Not ped Is waifu AndAlso Not ped.IsDead
                           End Function)

                    Call waifu.StopAttack(partner)
                Next

                Call waifu.StopAttack(Game.Player.Character)
            End If

            ' removes too far away peds for release memory
            If waifu.DistanceToPlayer > 1000 Then
                ' Call UI.Notify($"Delete [{waifu.Name}] due to she is too far away from you.")
                Call waifu.Delete()
            End If
        Next

        Dim actives As PendingEvent() = pendings _
            .Where(Function(task) task.IsReady) _
            .ToArray

        For Each task As PendingEvent In actives
            Call task.Tick(Me)
            Call pendings.Remove(task)
        Next
    End Sub
End Class
