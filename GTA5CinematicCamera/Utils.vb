Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports GTA.Math

Module Utils

    <Extension>
    Public Function ToRadians(val As Single) As Double
        Return (Math.PI / 180) * val
    End Function

    Public Function GetLookRotation(lookPosition As Vector3, up As Vector3) As Quaternion
        OrthoNormalize(lookPosition, up)
        Dim right As Vector3 = Vector3.Cross(up, lookPosition)
        Dim w = Math.Sqrt(1.0F + right.X + up.Y + lookPosition.Z) * 0.5F
        Dim val = 1.0F / (4.0F * w)
        Dim x = (up.Z - lookPosition.Y) * val
        Dim y = (lookPosition.X - right.Z) * val
        Dim z = (right.Y - up.X) * val
        Return New Quaternion(CSng(x), CSng(y), CSng(z), CSng(w))
    End Function

    Public Function RotationToDirection(rotation As Vector3) As Vector3
        Dim retZ As Double = rotation.Z * 0.01745329F
        Dim retX As Double = rotation.X * 0.01745329F
        Dim absX As Double = Math.Abs(Math.Cos(retX))
        Return New Vector3(CSng(-(Math.Sin(retZ) * absX)), CSng(Math.Cos(retZ) * absX), CSng(Math.Sin(retX)))
    End Function

    <Extension>
    Public Function RightVector(position As Vector3, up As Vector3) As Vector3
        position.Normalize()
        up.Normalize()
        Return Vector3.Cross(position, up)
    End Function

    <Extension>
    Public Function LeftVector(position As Vector3, up As Vector3) As Vector3
        position.Normalize()
        up.Normalize()
        Return -(Vector3.Cross(position, up))
    End Function

    Public Sub OrthoNormalize(ByRef normal As Vector3, ByRef tangent As Vector3)
        Vector3.Normalize(normal)
        Dim vec = Vector3.Multiply(normal, Vector3.Dot(tangent, normal))
        tangent = Vector3.Subtract(tangent, vec)
        tangent.Normalize()
    End Sub

    Const height As Single = 1080.0F

    Public Function GetScreenResolutionMantainRatio() As SizeF
        Dim screenw As Integer = GTA.Game.ScreenResolution.Width
        Dim screenh As Integer = GTA.Game.ScreenResolution.Height
        Dim ratio As Single = CSng(screenw) / screenh
        Dim width = height * ratio

        Return New SizeF(width, height)
    End Function
End Module
