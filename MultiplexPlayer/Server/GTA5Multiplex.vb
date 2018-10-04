Imports GTA5.Multiplex
Imports Microsoft.VisualBasic.Net
Imports Microsoft.VisualBasic.Net.Protocols
Imports Microsoft.VisualBasic.Net.Protocols.Reflection

<Protocol(GetType(PlayerControls.Protocols))>
Public Class GTA5Multiplex

    ReadOnly socket As TcpSynchronizationServicesSocket

    Sub New(Optional port% = 22335)
        socket = New TcpSynchronizationServicesSocket(port, AddressOf LogException) With {
            .Responsehandler = New ProtocolHandler(Me)
        }
    End Sub

    Private Shared Sub LogException(ex As Exception)

    End Sub

    Public Function Run() As Integer
        Return socket.Run
    End Function

    <Protocol(PlayerControls.Protocols.ShootAt)>
    Public Function ShootAt(request As RequestStream, RemoteAddress As System.Net.IPEndPoint) As RequestStream

    End Function
End Class
