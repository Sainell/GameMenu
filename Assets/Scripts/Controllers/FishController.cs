using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class FishController : BaseController
{
    public event Action<int> CatchedData;
    private List<FishItem> _fishesData;
    private List<GameObject> _fishesSpawnPoints;
    private List<GameObject> _startSpawnPoints;
    private Dictionary<GameObject, FishData> _fishesDic;
    private float _spawnTime;
    private bool _isPulledOut;
    private bool _isFirstSpawn = true;
    private bool _isNeedSpawnTimer;
    private int _catchedFishPoint;


    public override void Initialise(LevelData levelData)
    {
        _fishesData = levelData.FishList;
        _fishesDic = new Dictionary<GameObject, FishData>();
        _fishesSpawnPoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("FishSpawnPoint")); //todo spawncontroller 
        _startSpawnPoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("startSpawnPoint"));//todo spawncontroller 
        GameController.Instance.HookController.CatchedSmthEvent += OnCatch;
        GameController.Instance.HookController.PulledOutEvent += OnPulledOut;
        _isFirstSpawn = true;
        base.Initialise(levelData);
    }

    public override void Execute()
    {
        if (!IsInitialised)
            return;
        FishesSpawn();
        MoveFish();
        SpawnTimer();
    }
    public override void Dispose()
    {
        Clear();
    }
    public override void Clear()
    {
        GameController.Instance.HookController.CatchedSmthEvent -= OnCatch;
        GameController.Instance.HookController.PulledOutEvent -= OnPulledOut;
        if (_fishesDic != null && _fishesDic.Count != 0)
        {
            foreach (var fish in _fishesDic)
            {
                GameObject.Destroy(fish.Key);
            }
            _fishesDic.Clear();
        }
    }

    private void FishesSpawn()
    {
        if (!_isFirstSpawn)
        {
            if (!CheckIsPulledOut())
                return;
        }
        foreach (var fish in _fishesData)
        {
            if (!CheskIsNeedSpawn(fish))
            {
                continue;
            }
            for (int i = 0; i < fish.Count; i++)
            {
                if (!_isFirstSpawn && _spawnTime < fish.FishData.GetRandomSpawnDelayTime())
                {                   
                    --i;
                    return;
                }
                var spawnPoint = GetSpawnPoint();
                var newFish = GameObject.Instantiate(fish.FishData.Prefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
                newFish.name = $"{fish.FishData.FishType}{newFish.GetInstanceID()}";

                _fishesDic.Add(newFish, fish.FishData);          
                _spawnTime = 0;
            }
        }
        _isPulledOut = false;
        _isFirstSpawn = false;
    }

    private void SpawnTimer()
    {
        if (_isNeedSpawnTimer)
        {
            _spawnTime += Time.deltaTime;
        }
    }

    private GameObject GetSpawnPoint()
    {
        return _isFirstSpawn ? _startSpawnPoints[Random.Range(0, _startSpawnPoints.Count)] : _fishesSpawnPoints[Random.Range(0, _fishesSpawnPoints.Count)];
    }

    private bool CheskIsNeedSpawn(FishItem fishItem)
    {
        
        var dataCount = _fishesDic.Count(x => x.Value.FishType.Equals(fishItem.FishType));
        if (dataCount != 0 && dataCount >= fishItem.Count)
        {
            return false;
        }
        else
        {
            _isNeedSpawnTimer = true;
            return true;
        }
    }

    private bool CheckIsPulledOut()
    {
        if (!_isPulledOut)
        {
            _isNeedSpawnTimer = false;
            _spawnTime = 0;
        }
        return _isPulledOut;
    }

    private void OnPulledOut()
    {
        _isPulledOut = true;
        CatchedData?.Invoke(_catchedFishPoint);
        _catchedFishPoint = 0;
    }

    private void MoveFish()
    {
        foreach (var fish in _fishesDic)
        {
            if (CheckFishOutOfScreen(fish.Key))
                fish.Key.transform.Translate(Vector3.right * _fishesDic[fish.Key].Speed * Time.deltaTime);
        }
    }

    private bool CheckFishOutOfScreen(GameObject fish)
    {
        if (Mathf.Abs(fish.transform.position.x) > 15f)
        {
            var spawnPoint = _fishesSpawnPoints[Random.Range(0, _fishesSpawnPoints.Count)];
            fish.transform.SetPositionAndRotation(spawnPoint.transform.position, spawnPoint.transform.rotation);
            return false;
        }
        return true;
    }

    private void OnCatch(GameObject catchedFish)
    {
        _catchedFishPoint = _fishesDic[catchedFish].CatchedPoint;
        _fishesDic.Remove(catchedFish);
    }

}
