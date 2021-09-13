using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : BaseController
{
    private GameObject _waterPrefab;
    private float _waterSpriteWidth;
    private List<Transform> _waterParts;
    public override void Initialise()
    {
        _waterPrefab = GameObject.Find("water"); // TODO: Data.Prefab; 
        _waterSpriteWidth = _waterPrefab.GetComponentInChildren<SpriteRenderer>().bounds.size.x;
        _waterParts = new List<Transform>();
        _waterParts.Add(_waterPrefab.transform.GetChild(0).transform);
        _waterParts.Add(_waterPrefab.transform.GetChild(1).transform);
    }

    public override void Execute()
    {
        WaterMoving();
    }
    public override void Dispose()
    {
        
    }

    private void WaterMoving()
    {
        foreach(var water in _waterParts)
        {
            water.Translate(Vector3.right * Time.deltaTime);
            if(water.transform.position.x > _waterPrefab.transform.position.x+_waterSpriteWidth)
            {
                water.transform.position -= new Vector3(_waterSpriteWidth * _waterParts.Count, 0, 0);
            }
        }
    }
}
