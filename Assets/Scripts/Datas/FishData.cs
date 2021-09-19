using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FishData", menuName = "Fishes/new Fish", order = 1)]
public class FishData : SwimmingItemData
{
    public FishType FishType;

    public int SpawnCountMin;
    public int SpawnCountMax;

    private int _spawnCount;

    public int CurrentSpawnCount { get; set; }

    private void OnEnable()
    {
        _spawnCount = 0;
    }
    public int GetSpawnCount()
    {
        if (_spawnCount == 0)
            _spawnCount = Random.Range(SpawnCountMin, SpawnCountMax);
        return _spawnCount;
    }
}
