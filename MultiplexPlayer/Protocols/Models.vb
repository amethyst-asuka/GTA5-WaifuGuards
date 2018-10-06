Imports Microsoft.VisualBasic.ComponentModel.Collection.Generic
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.Repository

Public Class Message(Of T)

    Public Property CheckSum As Integer
    Public Property Guid As String
    Public Property Msg As T

End Class

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

Public Class ServiceRegister

    Public Property Guid As String
    Public Property Socket As Integer

End Class