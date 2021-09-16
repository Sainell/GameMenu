using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour
{
    [SerializeField] public GameObject MainMenu;
    [SerializeField] public GameObject SettingsMenu;

    [SerializeField] public Button PlayButton;
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




    private void Awake()
    {
        PlayButton.onClick.AddListener(PlayGame);
        SettingsButton.onClick.AddListener(OpenSettingsMenu);
        ExitButton.onClick.AddListener(ExitGame);
        BackButton.onClick.AddListener(Back);
        PauseButton.onClick.AddListener(() => OpenPauseMenu(false,false));
        ReturnPlayButton.onClick.AddListener(ReturnToPlay);
        BackInMainButton.onClick.AddListener(OpenMainMenu);

        //  Time.timeScale = 0;
        OpenMainMenu();
    }


    private void PlayGame()
    {
        Time.timeScale = 1;
        Game.SetActive(true);
        GameUI.SetActive(true);
        MainMenu.SetActive(false);
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
            PlayButton.gameObject.SetActive(false);
            NextLevelButton.onClick.AddListener(StartNextLevel);
        }
        else if (isFail)
        {
            RetryButton.gameObject.SetActive(true);
            PlayButton.gameObject.SetActive(false);
            RetryButton.onClick.AddListener(RetryLevel);
        }
        else
        {
            NextLevelButton.gameObject.SetActive(false);
            RetryButton.gameObject.SetActive(false);
            PlayButton.gameObject.SetActive(true);
        }
    }
    
    private void ReturnToPlay()
    {
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
    }
    private void OpenMainMenu()
    {
        PauseMenu.SetActive(false);
        Game.SetActive(false);
        GameUI.SetActive(false);
        MainMenu.SetActive(true);
    }

    private void StartNextLevel()
    {

    }

    private void RetryLevel()
    {

    }
}
