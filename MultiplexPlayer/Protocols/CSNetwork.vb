Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Net.Protocols
Imports Microsoft.VisualBasic.Net.Protocols.Reflection
Imports Microsoft.VisualBasic.Serialization.JSON

Public Module CSNetwork

    Public Enum Protocols As Long
        ''' <summary>
        ''' The very first step, ping the server and gets the uid from server as user id
        ''' </summary>
        Ping = 100
        LogIn = 500
        LogOut
    End Enum

    Public ReadOnly Property EntryPoint As Long = New Protocol(GetType(Protocols)).EntryPoint

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function Ping() As RequestStream
        Return New RequestStream(EntryPoint, Protocols.Ping)
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    <Extension>
    Public Function LogIn(user As NetworkUser) As RequestStream
        Return New RequestStream(EntryPoint, Protocols.LogIn, user.GetJson)
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function LogOut(uid As Long) As RequestStream
        Return New RequestStream(EntryPoint, Protocols.LogOut, BitConverter.GetBytes(uid))
    End Function
End Module

Public Class NetworkUser

    Public Property Uid As Long
    ''' <summary>
    ''' Your display name
    ''' </summary>
    ''' <returns></returns>
    Public Property Name As String

    ''' <summary>
    ''' Character model name
    ''' 
    ''' If character model is missing in your friend's game, then will 
    ''' display the default character model ``Michael``.
    ''' </summary>
    ''' <returns></returns>
    Public Property ModelName As String

End Class