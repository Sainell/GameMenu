using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : BaseController
{
    private const float WATER_Y_OFFSET = -1f;
    private GameObject _waterPartPrefab;
    private float _waterSpriteWidth;
    private List<Transform> _waterParts;
    public override void Initialise(LevelData levelData)
    {
        _waterPartPrefab = levelData.WaterPartPrefab;
        _waterSpriteWidth = _waterPartPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        _waterParts = new List<Transform>();
        CreaterWaterPlane();
        base.Initialise(levelData);
    }

    public override void Execute()
    {
        if (!IsInitialised)
            return;
        WaterMoving();
    }
    public override void Dispose()
    {
        
    }
    public override void Clear()
    {

    }

    private void WaterMoving()
    {
        foreach(var water in _waterParts)
        {
            water.Translate(Vector3.right * Time.deltaTime);
            if(water.position.x > _waterSpriteWidth)
            {
                water.position -= new Vector3(_waterSpriteWidth * _waterParts.Count, 0, 0);
            }
        }
    }
    private void CreaterWaterPlane()
    {
        for (int i = 0; i < CalculateWaterPartCount(); i++)
        {
            var waterPart = GameObject.Instantiate(_waterPartPrefab,
                   Vector3.zero - new Vector3(i * _waterSpriteWidth, WATER_Y_OFFSET),
                   Quaternion.identity).transform;
            _waterParts.Add(waterPart);
            if (i % 2 != 0)
            {
                waterPart.GetComponent<SpriteRenderer>().flipX = true;
            }
           
        }
    }

    private float CalculateWaterPartCount()
    {
        var cameraWidth = GameController.Instance.CameraController.CameraWidth;
        var partCount = cameraWidth / _waterSpriteWidth +1;
        return partCount;
    }
}
