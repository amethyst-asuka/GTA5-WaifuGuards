Imports GTA
Imports GTA.Math
Imports GTA.Native

Public Class SplineCamera

    Dim _renderSceneTimer As Timer
    Dim _replayTimer As Timer
    Dim _startPos As Vector3, _previousPos As Vector3

    Public ReadOnly Property MainCamera() As Camera
    Public Property InterpToPlayer As Boolean

    Public WriteOnly Property Speed() As Integer
        Set
            [Function].[Call](Hash.SET_CAM_SPLINE_DURATION, _mainCamera.Handle, (100 \ Value) * 1000)
        End Set
    End Property

    Public Sub New()
        Me._mainCamera = New Camera([Function].[Call](Of Integer)(Hash.CREATE_CAM, "DEFAULT_SPLINE_CAMERA", 0))
        Me._replayTimer = New Timer(1100)
        Me._renderSceneTimer = New Timer(5000)
        Me._renderSceneTimer.Start()
    End Sub

    Public Sub AddNode(position As Vector3, rotation As Vector3, duration As Integer)
        [Function].[Call](Hash.ADD_CAM_SPLINE_NODE, _MainCamera.Handle,
                          position.X, position.Y, position.Z,
                          rotation.X, rotation.Y, rotation.Z,
                          duration,
                          3, 2)
    End Sub

    Public Sub Update()
        If MainCamera.IsActive Then
            If _renderSceneTimer.Enabled AndAlso Game.GameTime > _renderSceneTimer.Waiter Then
                [Function].[Call](Hash._0x0923DBF87DFF735E, _MainCamera.Position.X, _MainCamera.Position.Y, _MainCamera.Position.Z)
                _renderSceneTimer.Reset()
            End If

            [Function].[Call](Hash.HIDE_HUD_AND_RADAR_THIS_FRAME)
            [Function].[Call](Hash.HIDE_HUD_COMPONENT_THIS_FRAME, 18)

            _previousPos = _MainCamera.Position

            If _replayTimer.Enabled AndAlso Game.GameTime > _replayTimer.Waiter Then
                [Function].[Call](Hash.SET_CAM_SPLINE_PHASE, _MainCamera.Handle, 0F)
                _replayTimer.Enabled = False
            End If

            If Not _MainCamera.IsInterpolating Then
                If [Function].[Call](Of Single)(Hash.GET_CAM_SPLINE_PHASE, _MainCamera.Handle) > 0.001F Then
                    If InterpToPlayer Then
                        [Function].[Call](Hash.RENDER_SCRIPT_CAMS, 0, 1, 3000, 1, 1,
                            1)

                        [Function].[Call](Hash.CLEAR_FOCUS)
                        MainCamera.IsActive = False
                    End If
                End If

                If Not _replayTimer.Enabled Then
                    _replayTimer.Start()
                End If
            Else
                ' render local scene
                Dim lastPos = Vector3.Subtract(_MainCamera.Position, _previousPos)

                [Function].[Call](Hash._SET_FOCUS_AREA,
                                  _MainCamera.Position.X, _MainCamera.Position.Y, _MainCamera.Position.Z,
                                  lastPos.X, lastPos.Y, lastPos.Z)
            End If
        End If
    End Sub
End Class

