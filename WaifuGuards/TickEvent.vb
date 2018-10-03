''' <summary>
''' Event happens in periodically
''' </summary>
Public MustInherit Class TickEvent

    ''' <summary>
    ''' The time interval of the period
    ''' </summary>
    Protected timeSpan As TimeSpan
    Protected lastCheck As Date

    Sub New(length As TimeSpan)
        timeSpan = length
        lastCheck = Now
    End Sub

    Public Sub Tick(script As WaifuScript)
        If Now - lastCheck >= timeSpan Then
            Call DoEvent(script)
            lastCheck = Now
        End If
    End Sub

    Protected MustOverride Sub DoEvent(script As WaifuScript)

End Class

''' <summary>
''' Event happens after a given length time span.
''' </summary>
Public Class PendingEvent : Inherits TickEvent

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