Imports System.Drawing
Imports Microsoft.VisualBasic.Linq

''' <summary>
''' 得到两个图片之后，进行动态规划比较差异
''' 1. 首先进行逐行扫描，
''' </summary>
Public Module Compares

    Public Function Compares(seq1 As ImageSequence, seq2 As ImageSequence) As Rectangle()
        Dim MAT1 As Single()() = seq1.Rows.ToArray(Function(x) x.ToArray(Function(cl) cl.GetBrightness))
        Dim MAT2 As Single()() = seq2.Rows.ToArray(Function(x) x.ToArray(Function(cl) cl.GetBrightness))
    End Function

    Public Function Compares(seq1 As Bitmap, seq2 As Bitmap) As Rectangle()
        Return Compares(New ImageSequence(seq1), New ImageSequence(seq2))
    End Function
End Module
