Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms

Public Module Utils

    ''' <summary>
    ''' The directory location of the current script dll file.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property AssemblyLocation As String
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Get
            Return Path.GetDirectoryName(Application.ExecutablePath) & "/scripts"
        End Get
    End Property

    <Extension>
    Public Function Append(list As IEnumerable(Of String), add As IEnumerable(Of String)) As String()
        With New List(Of String)
            If Not list Is Nothing Then
                Call .AddRange(list)
            End If
            If Not add Is Nothing Then
                Call .AddRange(add)
            End If

            Return .ToArray
        End With
    End Function
End Module
