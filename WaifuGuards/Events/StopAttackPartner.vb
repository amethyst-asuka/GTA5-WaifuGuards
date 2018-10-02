Public Class StopAttackPartner : Inherits TickEvent(Of WaifuScript)

    Public Sub New()
        MyBase.New(New TimeSpan(0, 0, 2))
    End Sub

    Protected Overrides Sub DoEvent(script As WaifuScript)
        For Each waifu As Waifu In script.waifuGuards _
            .Where(Function(w) Not w.IsDead AndAlso w.IsInCombat) _
            .ToArray

            ' try to prevent kill each other
            ' no nearby engaged ped but waifu is incombat
            ' means an internal war in this guard group
            ' stop it
            For Each partner In script.waifuGuards.Where(Function(p) Not p Is waifu)
                If waifu.IsFightAgainst(partner) Then
                    Call waifu.StopAttack()
                    Call partner.StopAttack()
                End If
            Next
        Next
    End Sub
End Class
