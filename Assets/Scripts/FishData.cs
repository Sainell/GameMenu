using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FishData", menuName = "Fishes/new Fish", order = 1)]
public class FishData : ScriptableObject
{
    public FishType FishType;
    public GameObject FishPrefab;
    public float FishSpeed;
    public float FishSpawnDelayMin;
    public float FishSpawnDelayMax;
    public int FishCatchedPoint;
    public int SpawnCountMin;
    public int SpawnCountMax;
    
    private int _spawnCount;

    public InteractableBehaviour InteractableBehaviour { get; set; }

    public int GetSpawnCount()
    {
        if (_spawnCount == 0)
            _spawnCount = Random.Range(SpawnCountMin, SpawnCountMax);
        return _spawnCount;
    }
    public float GetRandomSpawnDelayTime()
    {
        var delayTime = Random.Range(FishSpawnDelayMin, FishSpawnDelayMax);
        return delayTime;
    }
}
