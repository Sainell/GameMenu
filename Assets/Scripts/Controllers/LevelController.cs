using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : BaseController
{
    private List<LevelData> LevelList;
    private Dictionary<int, LevelData> LevelDictionary;
    private int _currentLevelId; // save \ load progress
    private int _nextLevelId => _currentLevelId + 1;
    public override void Initialise(LevelData levelData)
    {
        LevelList = new List<LevelData>(Resources.LoadAll<LevelData>($"Data/Levels"));
        LevelDictionary = new Dictionary<int, LevelData>();
        foreach(var level in LevelList)
        {
            LevelDictionary.Add(level.LevelId, level);
        }
        _currentLevelId = 0;
        base.Initialise(levelData);
    }
    public override void Execute()
    {

    }
    public override void Clear()
    {

    }

    public override void Dispose()
    {

    }

    public LevelData GetCurrentLevelData()
    {
        return GetLevelData(_currentLevelId);
    }

    public LevelData GetNextLevelData()
    {
        return GetLevelData(_nextLevelId);
    }
    private LevelData GetLevelData(int levelId)
    {
        if (LevelDictionary.ContainsKey(levelId))
        {
            return LevelDictionary[levelId];
        }
        else
        {
            Debug.LogException(new Exception($"ERROR:Level with ID <{levelId}> does not exist "));
            return null;
        }
    }
}
