using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : BaseController
{
    public float CameraWidth => _cameraWidth;
    public float CameraHeight => _cameraHeight;

    private float _cameraWidth;
    private float _cameraHeight;
    public override void Initialise(LevelData levelData)
    {
        CalculateCameraFrustum();
        base.Initialise(levelData);
    }
    public override void Execute()
    {
    }
    public override void Clear()
    {
    }
    public override void Dispose()
    {
    }

    private void CalculateCameraFrustum()
    {
       var frustumCameraPlanes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        _cameraHeight = frustumCameraPlanes[2].distance + frustumCameraPlanes[3].distance;
        _cameraWidth = frustumCameraPlanes[0].distance + frustumCameraPlanes[1].distance;
    }
}
