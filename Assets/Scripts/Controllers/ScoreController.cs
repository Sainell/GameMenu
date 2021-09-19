using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : BaseController
{
    private float _neededScore;
    private float _levelTime;
    private float _currentScore;
    private float _currentTime;
    private Image _timeScale;
    private Image _scoreScale;

    private MenuController _menuController;

    public override void Initialise(LevelData levelData)
    {
        _menuController = GameController.Instance.MenuController;
        _timeScale = _menuController.TimeScale.GetComponent<Image>();
        _scoreScale = _menuController.ScoreScale.GetComponent<Image>();

        _neededScore = levelData.LevelWinScore;
        _levelTime = levelData.LevelTime;
        _currentScore = 0;
        _scoreScale.fillAmount = _currentScore;
        _currentTime = _levelTime;
        GameController.Instance.FishController.CatchedData += OnCatch;
        base.Initialise(levelData);
    }

    public override void Execute()
    {
        if (!IsInitialised)
            return;
        Timer();
    }

    public override void Dispose()
    {
        Clear();
    }

    public override void Clear()
    {
        GameController.Instance.FishController.CatchedData -= OnCatch;
        ResetScoreAfterLevelEnd();
    }

    private void OnCatch(int score)
     {
        _currentScore += score;
        _scoreScale.fillAmount = _currentScore / _neededScore;
        CheckScore();
    }

    private void CheckScore()
    {
        if(_currentScore >= _neededScore)
        {
            Debug.Log("WIN_LEVEL");
            _menuController.OpenPauseMenu(isWin: true);
        }
    }

    private void Timer()
    {
        if (_currentTime <= 0)
            return;
        _currentTime -= Time.deltaTime;
        _timeScale.fillAmount = _currentTime / _levelTime;
        if (_currentTime <= 0)
        {
            Debug.Log("FAIL_LEVEL");
            _menuController.OpenPauseMenu(isFail: true);
        }
    }

    public void ResetScoreAfterLevelEnd()
    {
        _currentScore = 0;
        _currentTime = _levelTime;
    }
}
