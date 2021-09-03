using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public WaterController WaterController => _waterController;
    public FishController FishController => _fishController;
    public PlayerController PlayerController => _playerController;
    public HookController HookController => _hookController;

    private WaterController _waterController;
    private FishController _fishController;
    private PlayerController _playerController;
    private HookController _hookController;

    private List<BaseController> _controllers = new List<BaseController>();

    private void Awake()
    {
        Instance = this;
        _controllers.Add(_waterController = new WaterController());
        _controllers.Add(_fishController = new FishController());
        _controllers.Add(_playerController = new PlayerController());
        _controllers.Add(_hookController = new HookController());

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
