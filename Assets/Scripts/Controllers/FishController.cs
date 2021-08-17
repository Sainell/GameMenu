using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : BaseController
{
    private List<FishData> _fishesData;
    private List<GameObject> _fishes;
    private List<GameObject> _fishesSpawnPoints;
    private Dictionary<GameObject, FishData> _fishesDic;

    public override void Initialise()
    {
        _fishesData = new List<FishData>(Resources.LoadAll<FishData>($"Data/Fishes"));
        _fishesDic = new Dictionary<GameObject, FishData>();
        _fishesSpawnPoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("FishSpawnPoint")); //
        SpawnFishes();
    }

    public override void Execute()
    {
        MoveFish();
    }
    public override void Dispose()
    {
        foreach (var fish in _fishesData)
        {
            fish.InteractableBehaviour.CatchedEvent -= OnCatched;
        }
    }

    private void SpawnFishes()
    {
        foreach (var fish in _fishesData)
        {
            var spawnPoint = _fishesSpawnPoints[Random.Range(0, _fishesSpawnPoints.Count)];
            var newFish = GameObject.Instantiate(fish.FishPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
            newFish.name = fish.FishType.ToString();

            if (_fishes == null)
            {
                _fishes = new List<GameObject>();
            }
            _fishes.Add(newFish);
            _fishesDic.Add(newFish, fish);
            fish.InteractableBehaviour = newFish.GetComponent<InteractableBehaviour>();
            fish.InteractableBehaviour.CatchedEvent += OnCatched;
        }
    }
    private void MoveFish()
    {
        foreach(var fish in _fishes)
        {
            fish.transform.Translate(Vector3.right * _fishesDic[fish].FishSpeed * Time.deltaTime);
            CheckFishOutOfScreen(fish);
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
        if (collision.tag.Equals("Hook"))
        {
            catchedFish.transform.SetParent(collision.transform);
            _fishes.Remove(catchedFish);
        }
    }
}
