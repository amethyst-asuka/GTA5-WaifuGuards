Imports Microsoft.VisualBasic.Net


Public Class GTA5Multiplex

    ReadOnly socket As TcpSynchronizationServicesSocket

    Sub New(Optional port% = 22335)
        socket = New TcpSynchronizationServicesSocket(port, AddressOf LogException)
    End Sub

    Private Shared Sub LogException(ex As Exception)

    End Sub

    Public Function Run() As Integer

    End Function

    Private Function HandleProtocol()

    End Function
End Class
