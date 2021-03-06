﻿Imports System.Text
Imports GTA
Imports GTA5.Multiplex
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Net.Protocols
Imports Microsoft.VisualBasic.Net.Tcp

Public Class UserMenu : Inherits Script

    Const GameServerPort% = 22335
    Const UserServerPort% = 22336

    Dim gameServer As TcpRequest, userServer As TcpRequest
    Dim user As NetworkUser

    Sub New()
#If DEBUG Then
        gameServer = New TcpRequest("127.0.0.1", 22335)
        userServer = New TcpRequest("127.0.0.1", 22336)
        user = New NetworkUser With {
            .ModelName = "testxxx",
            .Name = "12345"
        }
#End If
    End Sub

#Region "Menu Item"

    ''' <summary>
    ''' Config game server port, user server port and the game server ip address.
    ''' </summary>
    ''' <returns></returns>
    Public Function ConfigServer() As Boolean

    End Function

    ''' <summary>
    ''' Do user login through menu item invoke
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Screen will fade in and your will enter the online mode
    ''' </remarks>
    Public Function LogIn() As Boolean
        ' 1. ping server to get guid
        Dim response As RequestStream = userServer.SendMessage(CSNetwork.Ping)
        Dim guid$ = Encoding.ASCII.GetString(response.ChunkBuffer)
        Dim logInRequest As RequestStream = user.LogIn
        Dim result = userServer.SendMessage(logInRequest)
        Dim code As int = -1

        If (code = BitConverter.ToInt32(result.ChunkBuffer, Scan0)) = 200 Then
            ' success
            ' do screen fade in and request download your friends' game playing data.
        Else
            ' error
        End If
    End Function

    ''' <summary>
    ''' Do user logout through menu item invoke
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Do user logout, means your ware offline from server now. But your game is still running unless you quite the game.
    ''' So you still can controlling your character, but other player's character will become a normal pedestrian 
    ''' and flee away from you.
    ''' </remarks>
    Public Function LogOut() As Boolean

    End Function
#End Region
End Class
