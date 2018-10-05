Imports System.Runtime.CompilerServices
Imports System.Threading
Imports GTA5.Multiplex
Imports Microsoft.VisualBasic.Net.Protocols
Imports Microsoft.VisualBasic.Net.Protocols.Reflection
Imports Microsoft.VisualBasic.Net.Tcp
Imports Microsoft.VisualBasic.Parallel

<Protocol(GetType(PlayerControls.Protocols))>
Public Class GTA5Multiplex

    ReadOnly socket As TcpServicesSocket
    ReadOnly users As UsersMgr

    Sub New(Optional port% = 22335, Optional userPort% = 22336)
        socket = New TcpServicesSocket(port, AddressOf LogException) With {
            .Responsehandler = New ProtocolHandler(Me)
        }
        users = New UsersMgr(userPort)
    End Sub

    Private Shared Sub LogException(ex As Exception)

    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function Run() As Integer
        Call New ThreadStart(AddressOf users.Run).RunTask
        Return socket.Run
    End Function

    <Protocol(PlayerControls.Protocols.CreatePlayer)>
    Public Function CreatePlayer(request As RequestStream, RemoteAddress As System.Net.IPEndPoint) As RequestStream

    End Function

    <Protocol(PlayerControls.Protocols.PlayerMessage)>
    Public Function PlayerMessage(request As RequestStream, RemoteAddress As System.Net.IPEndPoint) As RequestStream

    End Function

    <Protocol(PlayerControls.Protocols.ShootAt)>
    Public Function ShootAt(request As RequestStream, RemoteAddress As System.Net.IPEndPoint) As RequestStream

    End Function
End Class
