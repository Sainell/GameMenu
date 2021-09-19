using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour
{
    public event Action LoadGameLevel; // send LevelData for all controllers if need
    public event Action ClearGameLevel;

    [SerializeField] public GameObject MainMenu;
    [SerializeField] public GameObject SettingsMenu;

    [SerializeField] public Button StartGameButton;
    [SerializeField] public Button SettingsButton;
    [SerializeField] public Button ExitButton;
    [SerializeField] public Button BackButton;
    [SerializeField] public Toggle MusicToggle;
    [SerializeField] public Toggle SoundToggle;

    [SerializeField] public GameObject Game;
    [SerializeField] public GameObject GameUI;
    [SerializeField] public GameObject Menu;

    [SerializeField] public Button PauseButton;
    [SerializeField] public GameObject PauseMenu;
    [SerializeField] public Button ReturnPlayButton;
    [SerializeField] public Button BackInMainButton;
    [SerializeField] public Button NextLevelButton;
    [SerializeField] public Button RetryButton;

    [SerializeField] public GameObject TimeScale;
    [SerializeField] public GameObject ScoreScale;

    [SerializeField] private GameController _gameController;


    private void Awake()
    {
        _gameController.MenuController = this;
        StartGameButton.onClick.AddListener(PlayGame);
        SettingsButton.onClick.AddListener(OpenSettingsMenu);
        ExitButton.onClick.AddListener(ExitGame);
        BackButton.onClick.AddListener(Back);
        PauseButton.onClick.AddListener(() => OpenPauseMenu(false,false));
        ReturnPlayButton.onClick.AddListener(ReturnToPlay);
        BackInMainButton.onClick.AddListener(OpenMainMenu);
        RetryButton.onClick.AddListener(RetryLevel);
        NextLevelButton.onClick.AddListener(StartNextLevel);
    }

    private void Start()
    {
        OpenMainMenu();
    }

    private void PlayGame()
    {
        ClearGameLevel?.Invoke();
        Time.timeScale = 1;
        Game.SetActive(true);
        GameUI.SetActive(true);
        MainMenu.SetActive(false);
        PauseMenu.SetActive(false);

        LoadGameLevel?.Invoke();
        
    }

    private void OpenSettingsMenu()
    {
        MainMenu.SetActive(false);
        SettingsMenu.SetActive(true);
    }

    private void ExitGame()
    {

    }

    private void Back()
    {
        MainMenu.SetActive(true);
        SettingsMenu.SetActive(false);
    }

    public void OpenPauseMenu(bool isWin = false, bool isFail = false)
    {
        Time.timeScale = 0;
        PauseMenu.SetActive(true);
        if (isWin)
        {
            NextLevelButton.gameObject.SetActive(true);
            ReturnPlayButton.gameObject.SetActive(false);
        }
        else if (isFail)
        {
            RetryButton.gameObject.SetActive(true);
            ReturnPlayButton.gameObject.SetActive(false);
        }
        else
        {
            NextLevelButton.gameObject.SetActive(false);
            RetryButton.gameObject.SetActive(false);
            ReturnPlayButton.gameObject.SetActive(true);
        }
    }
    
    private void ReturnToPlay()
    {
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
    }
    private void OpenMainMenu()
    {
       // ClearGameLevel?.Invoke();
        PauseMenu.SetActive(false);
        Game.SetActive(false);
        GameUI.SetActive(false);
        MainMenu.SetActive(true);
    }

    private void StartNextLevel()
    {
        PlayGame();
    }

    private void RetryLevel()
    {
        PlayGame();
    }
}
