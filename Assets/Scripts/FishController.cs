using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : BaseController
{
    private List<GameObject> _fishes;
    public override void Initialise()
    {
        _fishes = new List<GameObject>(GameObject.FindGameObjectsWithTag("Fish")); //TODO data.fishes
    }

    public override void Execute()
    {
        MoveFish();
    }
    public override void Dispose()
    {

    }

    private void MoveFish()
    {
        foreach(var fish in _fishes)
        {
            fish.transform.Translate(Vector3.right * 0.5f * Time.deltaTime);
            if(!fish.GetComponent<SpriteRenderer>().isVisible)
            {
                fish.transform.position -= Vector3.right;
            }
        }
    }
}
