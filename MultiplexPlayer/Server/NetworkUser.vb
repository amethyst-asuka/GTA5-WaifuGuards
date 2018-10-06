Public Class NetworkUser : Inherits GTA5.Multiplex.NetworkUser

    Public ReadOnly Property CheckSum As Integer
    Public Property SocketId As Integer

    Sub New()
        CheckSum = NextCheckSum()
    End Sub

    ''' <summary>
    ''' Update checksum to a new random number for the validation
    ''' </summary>
    Public Sub UpdateCheckSum()
        _CheckSum = NextCheckSum()
    End Sub

    Private Function NextCheckSum() As Integer
        Static rand As New Random

        SyncLock rand
            Return rand.Next(0, 999999)
        End SyncLock
    End Function

End Class