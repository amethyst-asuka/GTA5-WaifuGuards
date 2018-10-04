Imports System.Runtime.CompilerServices
Imports GTA5.Multiplex
Imports Microsoft.VisualBasic.Net
Imports Microsoft.VisualBasic.Net.Protocols
Imports Microsoft.VisualBasic.Net.Protocols.Reflection

''' <summary>
''' User Manager
''' </summary>
''' 
<Protocol(GetType(CSNetwork.Protocols))>
Public Class UsersMgr

    ReadOnly socket As TcpSynchronizationServicesSocket

    Sub New(Optional port% = 22336)
        socket = New TcpSynchronizationServicesSocket(port, AddressOf LogException) With {
            .Responsehandler = New ProtocolHandler(Me)
        }
    End Sub

    Private Shared Sub LogException(ex As Exception)

    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function Run() As Integer
        Return socket.Run
    End Function

    <Protocol(CSNetwork.Protocols.Ping)>
    Public Function Ping(request As RequestStream, RemoteAddress As System.Net.IPEndPoint) As RequestStream

    End Function

    <Protocol(CSNetwork.Protocols.LogIn)>
    Public Function LogIn(request As RequestStream, RemoteAddress As System.Net.IPEndPoint) As RequestStream

    End Function

    <Protocol(CSNetwork.Protocols.LogOut)>
    Public Function LogOut(request As RequestStream, RemoteAddress As System.Net.IPEndPoint) As RequestStream

    End Function
End Class
