using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FishController : BaseController
{
    private List<FishData> _fishesData;
    private List<GameObject> _fishesSpawnPoints;
    private List<GameObject> _startSpawnPoints;
    private Dictionary<GameObject, FishData> _fishesDic;
    private float _spawnTime;
    private bool _isPulledOut;
    private bool _isCatched;
    private GameObject _catchedFish;
    private bool _isFirstSpawn = true;
    private bool _isNeedSpawnTimer;


    public override void Initialise()
    {
        _fishesData = new List<FishData>(Resources.LoadAll<FishData>($"Data/Fishes"));
        _fishesDic = new Dictionary<GameObject, FishData>();
        _fishesSpawnPoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("FishSpawnPoint")); //todo
        _startSpawnPoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("startSpawnPoint"));//todo
        GameController.Instance.HookController.CatchedSmthEvent += OnCatch;
        GameController.Instance.HookController.PulledOutEvent += OnPulledOut;
    }

    public override void Execute()
    {
        FishesSpawn();
        MoveFish();
        SpawnTimer();
    }
    public override void Dispose()
    {
        GameController.Instance.HookController.CatchedSmthEvent -= OnCatch;
        GameController.Instance.HookController.PulledOutEvent -= OnPulledOut;
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
            for (int i = 0; i < fish.CurrentSpawnCount; i++)
            {
                if (!_isFirstSpawn && _spawnTime < fish.GetRandomSpawnDelayTime())
                {                   
                    --i;
                    return;
                }
                var spawnPoint = GetSpawnPoint();
                var newFish = GameObject.Instantiate(fish.FishPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
                newFish.name = $"{fish.FishType}{newFish.GetInstanceID()}";

                _fishesDic.Add(newFish, fish);          
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

    private bool CheskIsNeedSpawn(FishData fishData)
    {
        var dataCount = _fishesDic.Count(x => x.Value.Equals(fishData));
        if (dataCount != 0 && dataCount >= fishData.GetSpawnCount())
        {
            return false;
        }
        else
        {
            fishData.CurrentSpawnCount = fishData.GetSpawnCount() - dataCount;
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
    }

    private void MoveFish()
    {
        foreach (var fish in _fishesDic)
        {
            if (CheckFishOutOfScreen(fish.Key))
                fish.Key.transform.Translate(Vector3.right * _fishesDic[fish.Key].FishSpeed * Time.deltaTime);
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
        _fishesDic.Remove(catchedFish);
    }

}
