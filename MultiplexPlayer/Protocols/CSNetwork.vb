Imports System.Runtime.CompilerServices
Imports System.Text
Imports Microsoft.VisualBasic.ComponentModel.Collection.Generic
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.Repository
Imports Microsoft.VisualBasic.Net.Protocols
Imports Microsoft.VisualBasic.Net.Protocols.Reflection
Imports Microsoft.VisualBasic.Serialization.JSON
Imports Microsoft.VisualBasic.Text

Public Module CSNetwork

    Public Enum Protocols As Long
        ''' <summary>
        ''' The very first step, ping the server and gets the uid from server as user id
        ''' </summary>
        Ping = 100
        RegisterService
        LogIn = 500
        LogOut
    End Enum

    Public ReadOnly Property EntryPoint As Long = New Protocol(GetType(Protocols)).EntryPoint

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function Ping() As RequestStream
        Return New RequestStream(EntryPoint, Protocols.Ping)
    End Function

    ''' <summary>
    ''' Display name may contains non-ascii character, this function using utf8 encoding.
    ''' </summary>
    ''' <param name="user"></param>
    ''' <returns></returns>
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    <Extension>
    Public Function LogIn(user As NetworkUser) As RequestStream
        Static utf8 As Encoding = Encodings.UTF8WithoutBOM.CodePage
        Return New RequestStream(EntryPoint, Protocols.LogIn, user.GetJson, utf8)
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function LogOut(guid As String) As RequestStream
        Return New RequestStream(EntryPoint, Protocols.LogOut, Encoding.ASCII.GetBytes(guid))
    End Function
End Module

Public Class NetworkUser : Implements INamedValue

    ''' <summary>
    ''' Unique identify you from your friends.
    ''' </summary>
    ''' <returns></returns>
    Public Property Guid As String Implements IKeyedEntity(Of String).Key

    ''' <summary>
    ''' Your display name, this name maybe duplicated with your friend's
    ''' Just as the real world.
    ''' </summary>
    ''' <returns></returns>
    Public Property Name As String

    ''' <summary>
    ''' Character model name
    ''' 
    ''' If character model is missing in your friend's game client, then will 
    ''' display the default character model ``Michael``.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' If your friend's patched another character model to this model name, 
    ''' then will display different character model between your client and your 
    ''' friends' client.
    ''' </remarks>
    Public Property ModelName As String

End Class