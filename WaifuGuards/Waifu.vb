Imports System.Runtime.CompilerServices

Public Class Waifu

    ReadOnly obj As Ped
    ReadOnly script As WaifuScript

    ''' <summary>
    ''' The model name
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Name As String
    Public ReadOnly Property IsInCombat As Boolean
        Get
            Return obj.IsInCombat
        End Get
    End Property
    Public ReadOnly Property DistanceToPlayer As Double
        Get
            Return Game.Player.Character.Position.DistanceTo(obj.Position)
        End Get
    End Property

    Public ReadOnly Property IsDead As Boolean
        Get
            Return obj.IsDead
        End Get
    End Property

    Public ReadOnly Property IsShootByPlayer As Boolean
        Get
            Return Game.Player.Character.IsShooting AndAlso Game.Player.IsTargetting(obj)
        End Get
    End Property

    Public ReadOnly Property IsAvailable As Boolean
        Get
            Return Not obj.IsDead AndAlso obj.IsInCombat
        End Get
    End Property

    Sub New(modelName$, host As WaifuScript)
        Dim pos = Game.Player.Character.GetOffsetInWorldCoords(host.offsetAroundMe)
        Dim waifu As Ped = World.CreatePed(New Model(modelName), pos)

        Name = modelName
        obj = waifu
        script = host
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Sub TakeAction(action As Action(Of Ped))
        Call action(obj)
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Sub TakeAction(action As Action(Of Tasks))
        Call action(obj.Task)
    End Sub

    Public Sub StopAttack(target As Ped)
        If obj.IsInCombatAgainst(target) Then
            Call obj.Task.ClearAllImmediately()
        End If
    End Sub

    Public Sub StopAttack(target As Waifu)
        If obj.IsInCombatAgainst(target.obj) Then
            Call obj.Task.ClearAllImmediately()
        End If
    End Sub

    ''' <summary>
    ''' Kill this waifu and pending to delete after 30 seconds
    ''' </summary>
    Public Sub Kill()
        Dim task As New PendingEvent(
            New TimeSpan(0, 0, 30),
            Sub(script)
                Call obj.Delete()
                Call script.waifuGuards.Remove(Me)
            End Sub)

        Call obj.Kill()
        Call script.Pending(task)
    End Sub

    Public Sub Delete()
        Call obj.Delete()
        Call script.waifuGuards.Remove(Me)
    End Sub
End Class
