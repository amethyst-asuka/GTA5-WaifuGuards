Imports System.Runtime.CompilerServices
Imports GTA5.Multiplex
Imports Microsoft.VisualBasic.Net
Imports Microsoft.VisualBasic.Net.Protocols
Imports Microsoft.VisualBasic.Net.Protocols.Reflection
Imports Microsoft.VisualBasic.Serialization.JSON

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
        Dim user = New IPEndPoint(RemoteAddress).GetJson.MD5
    End Function

    <Protocol(CSNetwork.Protocols.LogIn)>
    Public Function LogIn(request As RequestStream, RemoteAddress As System.Net.IPEndPoint) As RequestStream

    End Function

    ''' <summary>
    ''' Brocast message to other player and let system take over the player character 
    ''' and make it as a normal pedestrian?
    ''' </summary>
    ''' <param name="request"></param>
    ''' <param name="RemoteAddress"></param>
    ''' <returns></returns>
    <Protocol(CSNetwork.Protocols.LogOut)>
    Public Function LogOut(request As RequestStream, RemoteAddress As System.Net.IPEndPoint) As RequestStream

    End Function
End Class
