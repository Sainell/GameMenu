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

    private void Awake()
    {
        PlayButton.onClick.AddListener(PlayGame);
        SettingsButton.onClick.AddListener(OpenSettingsMenu);
        ExitButton.onClick.AddListener(ExitGame);
        BackButton.onClick.AddListener(Back);
    }


    private void PlayGame()
    {

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
}
