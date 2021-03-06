﻿Imports System.Runtime.CompilerServices

Public Class Waifu

    ReadOnly obj As Ped
    ReadOnly script As WaifuScript

    Public ReadOnly Property MarkDeletePending As Boolean = False

    ''' <summary>
    ''' The model name
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Name As String

    Public ReadOnly Property IsInCombat As Boolean
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Get
            Return obj.IsInCombat
        End Get
    End Property

    Public ReadOnly Property Target As Ped
        Get
            Return World.GetAllPeds _
                .Where(Function(p) obj.IsInCombatAgainst(p)) _
                .FirstOrDefault
        End Get
    End Property

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function DistanceTo(target As Ped) As Double
        Return obj.Position.DistanceTo(target.Position)
    End Function

    Public ReadOnly Property DistanceToPlayer As Double
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Get
            Return Game.Player.Character.Position.DistanceTo(obj.Position)
        End Get
    End Property

    Public ReadOnly Property IsDead As Boolean
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Get
            Return obj.IsDead
        End Get
    End Property

    Public ReadOnly Property IsShootByPlayer As Boolean
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Get
            Return Game.Player.Character.IsShooting AndAlso Game.Player.IsTargetting(obj)
        End Get
    End Property

    ''' <summary>
    ''' Not dead and not in combat
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property IsAvailable As Boolean
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Get
            Return Not obj.IsDead AndAlso Not obj.IsInCombat
        End Get
    End Property

    Public Function IsInVehicle(Optional vehicle As Vehicle = Nothing) As Boolean
        If vehicle Is Nothing Then
            Return obj.IsInVehicle
        Else
            Return obj.IsInVehicle(vehicle)
        End If
    End Function

    Sub New(modelName$, host As WaifuScript)
        Dim model As New Model(modelName)
        model.Request(500)

        If model.IsInCdImage AndAlso model.IsValid Then
            Do While Not model.IsLoaded
                Call GTA.Script.Wait(100)
            Loop

            Dim pos = Game.Player.Character.GetOffsetInWorldCoords(host.offsetAroundMe)
            Dim waifu As Ped = World.CreatePed(model, pos)

            Name = modelName
            obj = waifu
            script = host

            Call model.MarkAsNoLongerNeeded()
        Else
            MarkDeletePending = True
        End If
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Sub TakeAction(action As Action(Of Ped))
        Call action(obj)
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Sub TakeAction(action As Action(Of Tasks))
        Call action(obj.Task)
    End Sub

    Public Function IsFightAgainst(partner As Waifu) As Boolean
        Return obj.IsInCombatAgainst(partner.obj)
    End Function

    Public Sub StopAttack()
        Call obj.Task.ClearAll()
        Call obj.Task.HandsUp(3000)
    End Sub

    Public Sub StopAttack(target As Ped)
        If obj.IsInCombatAgainst(target) Then
            Call obj.Task.ClearAllImmediately()
        End If
    End Sub

    ''' <summary>
    ''' Kill this waifu and pending to delete after 30 seconds
    ''' </summary>
    Public Sub Kill()
        Dim task As New PendingEvent(Of WaifuScript)(New TimeSpan(0, 0, 30), AddressOf Delete)

        Call obj.Kill()
        Call script.Pending(task)

        _MarkDeletePending = True
    End Sub

    Public Sub Delete()
        Call script.waifuGuards.Remove(Me)
        Call obj.Delete()
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Shared Operator =(waifu As Waifu, ped As Ped) As Boolean
        Return waifu.obj Is ped OrElse waifu.obj.Handle = ped.Handle
    End Operator

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Shared Operator <>(waifu As Waifu, ped As Ped) As Boolean
        Return Not waifu = ped
    End Operator
End Class
