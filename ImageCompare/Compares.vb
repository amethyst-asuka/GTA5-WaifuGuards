Imports System.Drawing
Imports Microsoft.VisualBasic.DataMining.Framework.DynamicProgramming
Imports Microsoft.VisualBasic.Linq

''' <summary>
''' 得到两个图片之后，进行动态规划比较差异
''' 1. 首先进行逐行扫描，
''' </summary>
Public Module Compares

    Public Function Compares(seq1 As ImageSequence, seq2 As ImageSequence) As Rectangle()
        Dim MAT1 As Single()() = seq1.Rows.ToArray(Function(x) x.ToArray(Function(cl) cl.GetBrightness))
        Dim MAT2 As Single()() = seq2.Rows.ToArray(Function(x) x.ToArray(Function(cl) cl.GetBrightness))
        Dim gsw As New GSW(Of Single())(MAT1, MAT2, AddressOf Similarity, Function(x) Chr(x.Average))

    End Function

    Private Function Similarity(a As Single(), b As Single()) As Integer
        Dim gsw As New GSW(Of Single)(a, b, Function(x, y) 255 - (Math.Abs(x - y)), Function(x) Chr(x))
        Return gsw.AlignmentScore
    End Function

    Public Function Compares(seq1 As Bitmap, seq2 As Bitmap) As Rectangle()
        Return Compares(New ImageSequence(seq1), New ImageSequence(seq2))
    End Function
End Module
