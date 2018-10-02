Imports System.Runtime.CompilerServices

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

    ''' <summary>
    ''' Not dead and not in combat
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property IsAvailable As Boolean
        Get
            Return Not obj.IsDead AndAlso obj.IsInCombat
        End Get
    End Property

    Sub New(modelName$, host As WaifuScript)
        Dim model As New Model(modelName)

        If model.IsInCdImage AndAlso model.IsValid Then
            Dim pos = Game.Player.Character.GetOffsetInWorldCoords(host.offsetAroundMe)
            Dim waifu As Ped = World.CreatePed(model, pos)

            Name = modelName
            obj = waifu
            script = host
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
            ' Call obj.Task.AimAt(target, 1)
        End If
    End Sub

    ''' <summary>
    ''' Kill this waifu and pending to delete after 30 seconds
    ''' </summary>
    Public Sub Kill()
        Dim task As New PendingEvent(New TimeSpan(0, 0, 30), AddressOf Delete)

        Call obj.Kill()
        Call script.Pending(task)

        _MarkDeletePending = True
    End Sub

    Public Sub Delete()
        Call script.waifuGuards.Remove(Me)
        Call script.guards.Remove(obj)
        Call obj.Delete()
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Shared Operator =(waifu As Waifu, ped As Ped) As Boolean
        Return waifu.obj Is ped
    End Operator

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Shared Operator <>(waifu As Waifu, ped As Ped) As Boolean
        Return Not waifu = ped
    End Operator
End Class
