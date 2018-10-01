Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms

Module WaifuList

    ReadOnly waifusMegaPack$() = {
        "ram",
        "rem",
        "rmiku2015",
        "rmiku2016",
        "tohka",
        "tohru",
        "yoshino",
        "beatrice",
        "kanna",
        "kotori"
    }
    ReadOnly defaultWaifus$() = waifusMegaPack.Append({
        "22",
        "33",
        "sora"
    })

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
    ''' You can load custom names from a ``waifus.txt`` file which is located at script dir.
    ''' </summary>
    ''' <returns></returns>
    Public Function LoadNames() As String()
        Static waifus$ = Path.GetFullPath($"{AssemblyLocation}/waifus.txt")

        If File.Exists(waifus) Then
            Return File.ReadAllLines(waifus) _
                .Where(Function(s) Not String.IsNullOrWhiteSpace(s)) _
                .ToArray
        Else
            Return defaultWaifus
        End If
    End Function

    ''' <summary>
    ''' Check if the ``waifus mega pack`` mod is installed
    ''' </summary>
    ''' <returns></returns>
    ''' 
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function IsWaifusMegaPackInstalled() As Boolean
        Return waifusMegaPack _
            .Any(Function(name)
                     With New Model(name)
                         Return .IsInCdImage AndAlso .IsValid
                     End With
                 End Function)
    End Function
End Module
