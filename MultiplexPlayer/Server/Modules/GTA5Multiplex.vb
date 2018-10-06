Imports System.Runtime.CompilerServices
Imports System.Threading
Imports GTA5.Multiplex
Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.VisualBasic.Net.Protocols
Imports Microsoft.VisualBasic.Net.Protocols.Reflection
Imports Microsoft.VisualBasic.Parallel

<Protocol(GetType(PlayerControls.Protocols))>
Public Class GTA5Multiplex : Inherits ServerModule

    ReadOnly users As UsersMgr

    Sub New(Optional port% = 22335, Optional userPort% = 22336)
        Call MyBase.New(port)

        users = New UsersMgr(userPort)
    End Sub

    Protected Overrides Sub LogException(ex As Exception)

    End Sub

    Protected Overrides Function ProtocolHandler() As ProtocolHandler
        Return New ProtocolHandler(Me)
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Overrides Function Run() As Integer
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
