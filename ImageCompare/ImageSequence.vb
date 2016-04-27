Imports System.Drawing
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Serialization

Public Class ImageSequence
    Implements IEnumerable(Of Color)

    ''' <summary>
    ''' 这个图片对象在原始图片上面的位置
    ''' </summary>
    Public ReadOnly RECT As Rectangle
    Public ReadOnly Property Raw As Color()
    Public ReadOnly Property Rows As IEnumerable(Of Color())

    Sub New(bitmap As hBitmap, rect As Rectangle)
        Me.RECT = rect

        Dim index As Integer = bitmap.GetIndex(rect.X, rect.Y)
        Dim len As Integer = rect.ByteLength
        Dim buf As Byte() = New Byte(len - 1) {}

        Array.ConstrainedCopy(bitmap.Raw, index, buf, Scan0, buf.Length)
        Raw = buf.Colors.ToArray
        Rows = Raw.Split(rect.Width).ToArray
    End Sub

    Sub New(bitmap As hBitmap)
        Call Me.New(bitmap, New Rectangle(New Point, bitmap.Size))
    End Sub

    Sub New(bitmap As Bitmap)
        Call Me.New(hBitmap.FromBitmap(bitmap))
    End Sub

    Public Function GetIndex(i As Integer) As Point
        Dim y As Integer = i / RECT.Width
        Dim x As Integer = i Mod RECT.Width
        Return New Point(x, y)
    End Function

    Public Overrides Function ToString() As String
        Return RECT.GetJson
    End Function

    Public Iterator Function GetEnumerator() As IEnumerator(Of Color) Implements IEnumerable(Of Color).GetEnumerator
        For Each x As Color In Raw
            Yield x
        Next
    End Function

    Private Iterator Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Yield GetEnumerator()
    End Function
End Class
