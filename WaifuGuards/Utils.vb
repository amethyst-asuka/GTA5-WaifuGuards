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

    ''' <summary>
    ''' Join two collection as an array.
    ''' </summary>
    ''' <param name="list"></param>
    ''' <param name="add"></param>
    ''' <returns></returns>
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

    <Extension>
    Public Function IsNothing(Of T As Class)(x As T) As Boolean
        Return x Is Nothing
    End Function
End Module
