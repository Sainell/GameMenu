using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Level/new Level", order = 1)]
public class LevelData : ScriptableObject
{
    [SerializeField] private int _levelId;
    [SerializeField] private float _levelTime;
    [SerializeField] private int _levelWinScore;

    [SerializeField] private List<FishItem> _fishList;

    public int LevelId => _levelId;
    public float LevelTime => _levelTime;
    public int LevelWinScore => _levelWinScore;
    public List<FishItem> FishList => _fishList;
}
