''' <summary>
''' Event happens in periodically
''' </summary>
Public MustInherit Class TickEvent(Of TScript As Script)

    ''' <summary>
    ''' The time interval of the period
    ''' </summary>
    Protected timeSpan As TimeSpan
    Protected lastCheck As Date

    Sub New(length As TimeSpan)
        timeSpan = length
        lastCheck = Now
    End Sub

    Public Overridable Sub Tick(script As TScript)
        If Now - lastCheck >= timeSpan Then
            Call DoEvent(script)
            lastCheck = Now
        End If
    End Sub

    Protected MustOverride Sub DoEvent(script As TScript)

End Class

Public Class PendingEvent : Inherits TickEvent(Of WaifuScript)

    ReadOnly action As Action(Of WaifuScript)

    Public ReadOnly Property IsReady As Boolean
        Get
            Return Now - lastCheck >= timeSpan
        End Get
    End Property

    Public Sub New(length As TimeSpan, [event] As Action(Of WaifuScript))
        MyBase.New(length)
        Me.action = [event]
    End Sub

    Protected Overrides Sub DoEvent(script As WaifuScript)
        Call action(script)
    End Sub
End Class