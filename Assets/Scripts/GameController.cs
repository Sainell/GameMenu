using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public WaterController WaterController;
    public FishController FishController;
    public HookController HookController;

    private List<BaseController> _controllers = new List<BaseController>();

    private void Awake()
    {
        Instance = this;
        _controllers.Add(WaterController = new WaterController());
        _controllers.Add(FishController = new FishController());
        _controllers.Add(HookController = new HookController());

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
