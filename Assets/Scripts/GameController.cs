using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private List<BaseController> _controllers = new List<BaseController>();
    private void Awake()
    {
        _controllers.Add(new WaterController());
        _controllers.Add(new FishController());

       foreach(var controller in _controllers)
        {
            controller.Initialise();
        }    
    }

    private void Update()
    {
        foreach (var controller in _controllers)
        {
            controller.Execute();
        }
    }

    private void OnDestroy()
    {
        foreach (var controller in _controllers)
        {
            controller.Dispose();
        }
    }
}
