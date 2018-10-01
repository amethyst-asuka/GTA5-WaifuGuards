''' <summary>
''' Event happens in periodically
''' </summary>
Public MustInherit Class TickEvent

    ''' <summary>
    ''' The time interval of the period
    ''' </summary>
    Dim timeSpan As TimeSpan
    Dim lastCheck As Date

    Sub New(length As TimeSpan)
        timeSpan = length
        lastCheck = Now
    End Sub

    Public Sub Tick(script As Waifus)
        If Now - lastCheck >= timeSpan Then
            Call DoEvent(script)
            lastCheck = Now
        End If
    End Sub

    Protected MustOverride Sub DoEvent(script As Waifus)

End Class
