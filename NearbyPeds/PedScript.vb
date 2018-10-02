Imports System.Runtime.CompilerServices
Imports System.Windows.Forms

Public Class PedScript : Inherits Script

    Public Shared ReadOnly Skins$() = {
        "tonacho",
        "ByStaxx",
        "Capitan_america",
        "DeadPool",
        "ElRubius",
        "empollon",
        "LuzuGames",
        "perxittaa",
        "Tigre",
        "Waluigi",
        "Luigi_MTUS",
        "Mario_MTUS",
        "BabyLuigi",
        "BabyMario",
        "Wario"
    }
    ReadOnly rand As New Random
    ReadOnly player As Integer = Game.Player.Character.RelationshipGroup

    Friend nearbyPeds As List(Of Ped)
    Friend events As New List(Of TickEvent(Of PedScript))

    Public ReadOnly Property NextModel As (name As String, model As Model)
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Get
            Dim name$ = Skins(rand.Next(0, Skins.Length))
            Return (name, New Model(name))
        End Get
    End Property

    Public ReadOnly Property ToggleAttacks As Boolean = False
    Public ReadOnly Property ToggleChangeModel As Boolean = False

    Sub New()
        events.Add(New AttackEvent)
    End Sub

    Private Sub PedScript_Tick(sender As Object, e As EventArgs) Handles Me.Tick
        For Each [event] As TickEvent(Of PedScript) In events
            Call [event].Tick(Me)
        Next
    End Sub

    Private Sub PedScript_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.NumPad1 Then
            _ToggleAttacks = Not ToggleAttacks
        ElseIf e.KeyCode = Keys.I Then
            _ToggleChangeModel = Not ToggleChangeModel
        End If
    End Sub
End Class
