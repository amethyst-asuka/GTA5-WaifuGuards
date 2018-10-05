Imports GTA

Module GamePlays

    Public ReadOnly Friends As Dictionary(Of String, [Friend])

    ''' <summary>
    ''' Each server response will bring a new checksum code avoid a 
    ''' fake request from other player disturb your game playing.
    ''' 
    ''' Only you and the server knowns this checksum code
    ''' </summary>
    Dim checksum As Integer

End Module

Public Class [Friend] : Inherits NetworkUser

    Public Ped As Ped

End Class

