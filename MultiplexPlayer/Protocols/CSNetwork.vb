Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Net.Protocols
Imports Microsoft.VisualBasic.Net.Protocols.Reflection

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

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="uid"></param>
    ''' <param name="name$">Your display name</param>
    ''' <param name="modelName$">Character model name</param>
    ''' <returns></returns>
    ''' 
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function LogIn(uid As Long, name$, modelName$) As RequestStream
        Return New RequestStream(EntryPoint, Protocols.LogIn)
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function LogOut(uid As Long) As RequestStream
        Return New RequestStream(EntryPoint, Protocols.LogOut, BitConverter.GetBytes(uid))
    End Function
End Module
