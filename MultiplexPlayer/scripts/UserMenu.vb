Imports GTA

Public Class UserMenu : Inherits Script

    Const GameServerPort% = 22335
    Const UserServerPort% = 22336

    Dim gamePort%, userPort%

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
