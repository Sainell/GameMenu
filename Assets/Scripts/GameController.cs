using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public MenuController MenuController { get; set; }
    public WaterController WaterController => _waterController;
    public FishController FishController => _fishController;
    public PlayerController PlayerController => _playerController;
    public HookController HookController => _hookController;
    public ScoreController ScoreController => _scoreController;
    public LevelController LevelController => _levelController;

    private WaterController _waterController;
    private FishController _fishController;
    private PlayerController _playerController;
    private HookController _hookController;
    private ScoreController _scoreController;
    private LevelController _levelController;

    private List<BaseController> _controllers = new List<BaseController>();

    private void Awake()
    {
        Instance = this;
        _controllers.Add(_waterController = new WaterController());
        _controllers.Add(_fishController = new FishController());
        _controllers.Add(_playerController = new PlayerController());
        _controllers.Add(_hookController = new HookController());
        _controllers.Add(_scoreController = new ScoreController());
        _levelController = new LevelController();
        _levelController.Initialise(null);

        MenuController.LoadGameLevel += Initialise;
        MenuController.ClearGameLevel += Clear;
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
        MenuController.LoadGameLevel -= Initialise;
        MenuController.ClearGameLevel -= Clear;
    }

    private void Initialise(LevelData levelData)
    {
        foreach (var controller in _controllers)
        {
            controller.Initialise(levelData);
        }
    }
    private void Clear()
    {
        foreach (var controller in _controllers)
        {
            controller.Clear();
        }
    }
}
