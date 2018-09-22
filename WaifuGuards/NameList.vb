Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms

Module NameList

    ReadOnly defaultWaifus$() = {
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
End Module
