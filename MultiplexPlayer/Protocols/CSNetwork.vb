Imports System.Runtime.CompilerServices
Imports System.Text
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