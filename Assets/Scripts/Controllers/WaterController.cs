using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : BaseController
{
    private GameObject _waterPrefab;
    private GameObject _waterPartPrefab;
    private float _waterSpriteWidth;
    private List<Transform> _waterParts;
    public override void Initialise(LevelData levelData)
    {
        _waterPrefab = GameObject.Find("water"); // TODO: Data.Prefab; 
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
            if(water.position.x > _waterPrefab.transform.position.x+_waterSpriteWidth)
            {
                water.position -= new Vector3(_waterSpriteWidth * _waterParts.Count, 0, 0);
            }
        }
    }
    private void CreaterWaterPlane()
    {
        for(int i=0; i<CalculateWaterPartCount();i++)
        {
            _waterParts.Add(GameObject.Instantiate(_waterPartPrefab,
                _waterPrefab.transform.position - new Vector3(i *_waterSpriteWidth, 0),
                Quaternion.identity).transform);
        }
    }

    private float CalculateWaterPartCount()
    {
        var cameraWidth = GameController.Instance.CameraController.CameraWidth;
        var partCount = cameraWidth / _waterSpriteWidth +1;
        return partCount;
    }
}
