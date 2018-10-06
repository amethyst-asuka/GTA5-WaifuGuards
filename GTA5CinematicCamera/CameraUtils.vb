Imports System.Runtime.CompilerServices
Imports GTA
Imports GTA.Math
Imports GTA.Native

Module CameraUtils

    <Extension>
    Public Sub EnterCameraView(mainCamera As Camera, position As Vector3)
        [Function].[Call](Hash.DO_SCREEN_FADE_OUT, 1200)
        Script.Wait(1100)
        mainCamera.Position = position
        mainCamera.IsActive = True
        World.RenderingCamera = mainCamera
        Script.Wait(100)
        [Function].[Call](Hash.DO_SCREEN_FADE_IN, 800)
    End Sub

    <Extension> Public Sub ExitCameraView(camera As Camera)
        [Function].[Call](Hash.DO_SCREEN_FADE_OUT, 1200)
        Script.Wait(1100)
        camera.IsActive = False
        World.RenderingCamera = Nothing
        Script.Wait(100)
        [Function].[Call](Hash.CLEAR_FOCUS)
        [Function].[Call](Hash.DO_SCREEN_FADE_IN, 800)
    End Sub
End Module
