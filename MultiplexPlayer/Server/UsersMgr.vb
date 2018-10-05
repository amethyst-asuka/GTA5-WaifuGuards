Imports System.Runtime.CompilerServices
Imports GTA5.Multiplex
Imports Microsoft.VisualBasic.Net
Imports Microsoft.VisualBasic.Net.Http
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
        messageServer = New ServicesSocket(messageChannel, AddressOf LogException) With {
            .AcceptCallbackHandleInvoke = AddressOf RegisterService
        }
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

    Public Sub RegisterService(socket As WorkSocket)
        Dim registerMsg As New Message(Of Integer) With {
            .Msg = socket.GetHashCode,
            .CheckSum = HTTP_RFC.RFC_OK
        }
        Call socket.PushMessage(RequestStream.CreatePackage(registerMsg))
    End Sub

    ''' <summary>
    ''' Write link between the user object and its service socket
    ''' </summary>
    ''' <param name="request"></param>
    ''' <param name="RemoteAddress"></param>
    ''' <returns></returns>
    <Protocol(CSNetwork.Protocols.RegisterService)>
    Public Function RegisterService(request As RequestStream, RemoteAddress As System.Net.IPEndPoint) As RequestStream
        Dim msg As Message(Of ServiceRegister) = request.LoadObject(AddressOf LoadJSON(Of Message(Of ServiceRegister)))

        If msg.CheckSum <> users(msg.Guid).CheckSum Then
            Return RequestStream.SystemProtocol(RequestStream.Protocols.InvalidCertificates, "Mismatched checksum")
        Else
            With users(msg.Guid)
                ' Establish links between user and the services socket
                .SocketId = msg.Msg.Socket
                .UpdateCheckSum()
            End With
        End If

        Dim OK As New Message(Of String) With {
            .CheckSum = users(msg.Guid).CheckSum,
            .Msg = "OK!"
        }

        Return RequestStream.CreatePackage(OK)
    End Function

    ''' <summary>
    ''' The multiplex network protocol initiator
    ''' </summary>
    ''' <param name="request"></param>
    ''' <param name="RemoteAddress"></param>
    ''' <returns></returns>
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
                         Call userSocket.PushMessage(loginBroadCast)
                     End Sub)

        Return RequestStream.CreatePackage(New Message(Of String) With {
            .CheckSum = users(msg.Guid).CheckSum,
            .Msg = "OK!"
        })
    End Function

    ''' <summary>
    ''' Broadcast message to other player and let system take over the player character 
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
                         Call userSocket.PushMessage(logoutBroadCast)
                     End Sub)

        Return RequestStream.SystemProtocol(RequestStream.Protocols.OK, "OK!")
    End Function
End Class