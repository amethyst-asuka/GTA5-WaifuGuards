Imports System.Drawing
Imports Microsoft.VisualBasic.DataMining.Framework.DynamicProgramming
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Serialization
Imports System.Runtime.CompilerServices

Public Structure ImageMatch

    Public Property QFrom As Point
    Public Property QTo As Point
    Public Property SFrom As Point
    Public Property STo As Point

    Public ReadOnly Property QFrame As Rectangle
        Get
            Return GetFrame(QFrom, QTo)
        End Get
    End Property

    Public ReadOnly Property SFrame As Rectangle
        Get
            Return GetFrame(SFrom, STo)
        End Get
    End Property

    Private Shared Function GetFrame(QFrom As Point, QTo As Point) As Rectangle
        If (QFrom.X + QFrom.Y) <= (QTo.X + QTo.Y) Then
            Return New Rectangle(QFrom, New Size(QTo.X - QFrom.X, QTo.Y - QFrom.Y))
        Else
            Return New Rectangle(QTo, New Size(QFrom.X - QTo.X, QFrom.Y - QTo.Y))
        End If
    End Function

    Sub New(m As Match, a As ImageSequence, b As ImageSequence)
        QFrom = a.GetIndex(m.FromA)
        QTo = a.GetIndex(m.ToA)
        SFrom = b.GetIndex(m.FromB)
        STo = b.GetIndex(m.ToB)
    End Sub

    Public Overrides Function ToString() As String
        Return New With {QFrame, SFrame}.GetJson
    End Function
End Structure

''' <summary>
''' 得到两个图片之后，进行动态规划比较差异
''' 1. 首先进行逐行扫描，
''' </summary>
Public Module Compares

    Public Function Compares(seq1 As ImageSequence, seq2 As ImageSequence) As ImageMatch()
        Dim gsw As New GSW(Of Color)(seq1.Raw, seq2.Raw, AddressOf Similarity, Nothing)
        Dim ms As Match() = gsw.GetMatches
        Dim result As ImageMatch() = ms.ToArray(Function(x) New ImageMatch(x, seq1, seq2))
        Return result
    End Function

    ReadOnly MAXD As Double = Math.Sqrt(255 * 255 + 255 * 255 + 255 * 255)

    <Extension>
    Public Sub DrawFrame(ByRef res As Image, frames As Rectangle())
        Using gdi As Graphics = Graphics.FromImage(res)
            Dim pen As New Pen(Color.Black, 1)

            For Each x In frames
                Call gdi.DrawRectangle(pen, x)
            Next
        End Using
    End Sub

    Public Function DrawQuery(res As Image, matches As ImageMatch()) As Image
        Dim bmp As New Bitmap(DirectCast(res.Clone, Image))
        Call bmp.DrawFrame(matches.ToArray(Function(x) x.QFrame))
        Return bmp
    End Function

    Public Function DrawHit(res As Image, matches As ImageMatch()) As Image
        Dim bmp As New Bitmap(DirectCast(res.Clone, Image))
        Call bmp.DrawFrame(matches.ToArray(Function(x) x.SFrame))
        Return bmp
    End Function

    Private Function Similarity(a As Color, b As Color) As Integer
        Dim d As Double = Math.Sqrt(CInt(a.R) * CInt(a.R) + CInt(a.G) * CInt(a.G) + CInt(a.B) * CInt(a.B))
        d = d / MAXD
        If d > 0.5 Then
            Return 10
        Else
            Return -10
        End If
    End Function

    Public Function Compares(seq1 As Bitmap, seq2 As Bitmap) As ImageMatch()
        '   Call seq1.Binarization
        '  Call seq2.Binarization
        Return Compares(New ImageSequence(seq1), New ImageSequence(seq2))
    End Function
End Module
