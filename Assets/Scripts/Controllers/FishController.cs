using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FishController : BaseController
{
    private List<FishData> _fishesData;
    private List<GameObject> _fishesSpawnPoints;
    private Dictionary<GameObject, FishData> _fishesDic;
    private float _spawnTime;
    private bool _isPulledOut;
    private bool _isCatched;
    private GameObject _catchedFish;
    private bool _isFirstSpawn = true;

    public override void Initialise()
    {
        _fishesData = new List<FishData>(Resources.LoadAll<FishData>($"Data/Fishes"));
        _fishesDic = new Dictionary<GameObject, FishData>();
        _fishesSpawnPoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("FishSpawnPoint")); //todo
    }

    public override void Execute()
    {
        FishesSpawn();
        MoveFish();
    }
    public override void Dispose()
    {
        foreach (var fish in _fishesData)
        {
            fish.InteractableBehaviour.CatchedEvent -= OnCatched;
        }
    }


    private void FishesSpawn()
    {
        if (!_isFirstSpawn)
        {
            if (!CheckIsPulledOut(_catchedFish))
                return;
        }
        _isCatched = false;
        _isFirstSpawn = false;
        _catchedFish = null;
        foreach (var fish in _fishesData)
        {
            if (!CheskIsNeedSpawn(fish))
            {
                continue;
            }
            for (int i = 0; i < fish.GetSpawnCount(); i++)
            {
                if (_spawnTime < fish.GetRandomSpawnDelayTime())
                {
                    SpawnTimer();
                    --i;
                    continue;
                }
                var spawnPoint = _fishesSpawnPoints[Random.Range(0, _fishesSpawnPoints.Count)];
                var newFish = GameObject.Instantiate(fish.FishPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
                newFish.name = fish.FishType.ToString();
                _fishesDic.Add(newFish, fish);
                fish.InteractableBehaviour = newFish.GetComponent<InteractableBehaviour>();
                fish.InteractableBehaviour.CatchedEvent += OnCatched;
                _spawnTime = 0;
            }
        }
    }

    private void SpawnTimer()
    {
        _spawnTime += Time.deltaTime;
    }

    private bool CheskIsNeedSpawn(FishData fishData)
    {
        var dataCount = _fishesDic.Count(x => x.Value.Equals(fishData));
        if (dataCount != 0 && dataCount >= fishData.GetSpawnCount())
            return false;
        else
            return true;
    }

    private bool CheckIsPulledOut(GameObject fish)
    {
        return _isPulledOut = fish == null ? false : fish.activeSelf == true ? false : true;
    }

    private void MoveFish()
    {
        foreach(var fish in _fishesDic)
        {
            fish.Key.transform.Translate(Vector3.right * _fishesDic[fish.Key].FishSpeed * Time.deltaTime);
            CheckFishOutOfScreen(fish.Key);
        }
    }

    private void CheckFishOutOfScreen(GameObject fish)
    {
        if(Mathf.Abs(fish.transform.position.x) > 10f)
        {
            var spawnPoint = _fishesSpawnPoints[Random.Range(0, _fishesSpawnPoints.Count)];
            fish.transform.SetPositionAndRotation(spawnPoint.transform.position, spawnPoint.transform.rotation);
        }
    }

    private void OnCatched(GameObject collision, GameObject catchedFish)
    {
        if (!_isCatched && collision.tag.Equals("Hook"))
        {
            catchedFish.transform.SetParent(collision.transform);
            _isCatched = true;
            _catchedFish = catchedFish;
            _fishesDic[catchedFish].InteractableBehaviour.CatchedEvent -= OnCatched;
            _fishesDic.Remove(catchedFish);
        }
    }
}
