Imports GTA

Public Class Timer

#Region "Public Variables"

    Public Property Enabled() As Boolean
    Public Property Interval() As Integer
    Public Property Waiter() As Integer

#End Region

    Public Sub New(interval As Integer)
        Me.Interval = interval
        Me.Waiter = 0
        Me.Enabled = False
    End Sub

    Public Sub New()
        Me.Interval = 0
        Me.Waiter = 0
        Me.Enabled = False
    End Sub

    Public Sub Start()
        Me.Waiter = Game.GameTime + Interval
        Me.Enabled = True
    End Sub

    Public Sub Reset()
        Me.Waiter = Game.GameTime + Interval
    End Sub

End Class
