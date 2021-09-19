using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : BaseController
{
    private const float TEST_NEEDED_SCORE = 30;
    private const float TEST_LEVEL_TIME = 3;
    private float _currentScore;
    private float _currentTime;
    private Image _timeScale;
    private Image _scoreScale;

    private MenuController _menuController;

    public override void Initialise()
    {
        _menuController = GameObject.Find("Menu").GetComponent<MenuController>(); //
        _timeScale = _menuController.TimeScale.GetComponent<Image>();//

        _scoreScale = _menuController.ScoreScale.GetComponent<Image>();//
        _currentScore = 0;
        _scoreScale.fillAmount = _currentScore;
        _currentTime = TEST_LEVEL_TIME;
        GameController.Instance.FishController.CatchedData += OnCatch;
    }

    public override void Execute()
    {
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
        _scoreScale.fillAmount = _currentScore / TEST_NEEDED_SCORE;
        CheckScore();
    }

    private void CheckScore()
    {
        if(_currentScore >= TEST_NEEDED_SCORE)
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
        _timeScale.fillAmount = _currentTime / TEST_LEVEL_TIME;
        if (_currentTime <= 0)
        {
            Debug.Log("FAIL_LEVEL");
            _menuController.OpenPauseMenu(isFail: true);
        }
    }

    public void ResetScoreAfterLevelEnd()
    {
        _currentScore = 0;
        _currentTime = TEST_LEVEL_TIME;
    }
}
