Public MustInherit Class TickEvent

    Dim timeSpan As TimeSpan
    Dim lastCheck As Date

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
