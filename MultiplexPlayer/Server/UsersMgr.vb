Imports System.Runtime.CompilerServices
Imports GTA5.Multiplex
Imports Microsoft.VisualBasic.Net
Imports Microsoft.VisualBasic.Net.Persistent.Socket
Imports Microsoft.VisualBasic.Net.Protocols
Imports Microsoft.VisualBasic.Net.Protocols.Reflection
Imports Microsoft.VisualBasic.Serialization.JSON

''' <summary>
''' User Manager and message server
''' </summary>
''' 
<Protocol(GetType(CSNetwork.Protocols))>
Public Class UsersMgr

    ReadOnly socket As TcpSynchronizationServicesSocket
    ''' <summary>
    ''' For message broadcast and message push
    ''' </summary>
    ReadOnly messageServer As ServicesSocket
    ReadOnly users As Dictionary(Of String, NetworkUser)

    Sub New(Optional port% = 22336, Optional messageChannel% = 22337)
        socket = New TcpSynchronizationServicesSocket(port, AddressOf LogException) With {
            .Responsehandler = New ProtocolHandler(Me)
        }
        messageServer = New ServicesSocket(messageChannel, AddressOf LogException)
    End Sub

    Private Shared Sub LogException(ex As Exception)

    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function Run() As Integer
        Return socket.Run
    End Function

    Private Function requestNewGuid() As String
        SyncLock users
            Do While True
                With Guid.NewGuid.ToString
                    If Not users.ContainsKey(.ToString) Then
                        users(.ToString) = Nothing
                        Return .ToString
                    End If
                End With
            Loop
        End SyncLock

        Throw New Exception("This exception will never happends.")
    End Function

    <Protocol(CSNetwork.Protocols.Ping)>
    Public Function Ping(request As RequestStream, RemoteAddress As System.Net.IPEndPoint) As RequestStream
        Dim userId As String = requestNewGuid()
        Dim user As New NetworkUser With {
            .Guid = userId
        }
        Dim msg As New Message(Of String) With {
            .CheckSum = user.CheckSum,
            .Guid = userId,
            .Msg = userId
        }

        SyncLock users
            users(userId) = user
        End SyncLock

        Return RequestStream.CreatePackage(msg)
    End Function

    <Protocol(CSNetwork.Protocols.LogIn)>
    Public Function LogIn(request As RequestStream, RemoteAddress As System.Net.IPEndPoint) As RequestStream
        Dim msg As Message(Of GTA5.Multiplex.NetworkUser) = request.LoadObject(AddressOf LoadJSON(Of Message(Of GTA5.Multiplex.NetworkUser)))

        If msg.CheckSum <> users(msg.Guid).CheckSum Then
            Return RequestStream.SystemProtocol(RequestStream.Protocols.InvalidCertificates, "Mismatched checksum")
        Else
            With users(msg.Guid)
                .ModelName = msg.Msg.ModelName
                .Name = msg.Msg.Name

                Call .UpdateCheckSum()
            End With
        End If

        Dim loginMsg As New Message(Of GTA5.Multiplex.NetworkUser) With {
            .Guid = msg.Guid,
            .Msg = msg.Msg
        }
        Dim loginBroadCast As New RequestStream(
            CSNetwork.EntryPoint,
            CSNetwork.Protocols.LogIn,
            loginMsg.GetJson
        )

        ' message broadcast to all users
        Call messageServer _
            .Connections _
            .ForEach(Sub(userSocket, i)
                         Call userSocket.SendMessage(loginBroadCast)
                     End Sub)

        Return RequestStream.CreatePackage(New Message(Of String) With {
            .CheckSum = users(msg.Guid).CheckSum,
            .Msg = "OK!"
        })
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
        Dim msg As Message(Of String) = request.LoadObject(AddressOf LoadJSON(Of Message(Of String)))

        If msg.CheckSum <> users(msg.Guid).CheckSum Then
            Return RequestStream.SystemProtocol(RequestStream.Protocols.InvalidCertificates, "Mismatched checksum")
        Else
            ' delete user from server memory
            SyncLock users
                Call users.Remove(msg.Guid)
            End SyncLock
        End If

        Dim logoutMsg As New Message(Of String) With {
            .Guid = msg.Guid,
            .Msg = msg.Guid
        }
        Dim logoutBroadCast As New RequestStream(
            CSNetwork.EntryPoint,
            CSNetwork.Protocols.LogOut,
            logoutMsg.GetJson
        )

        ' message broadcast to all users
        Call messageServer _
            .Connections _
            .ForEach(Sub(userSocket, i)
                         Call userSocket.SendMessage(logoutBroadCast)
                     End Sub)

        Return RequestStream.SystemProtocol(RequestStream.Protocols.OK, "OK!")
    End Function
End Class

Public Class NetworkUser : Inherits GTA5.Multiplex.NetworkUser

    Public ReadOnly Property CheckSum As Integer

    Sub New()
        CheckSum = NextCheckSum()
    End Sub

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