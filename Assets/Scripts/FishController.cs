using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : BaseController
{
    private List<GameObject> _fishes;
    private List<GameObject> _fishesSpawnPoints;
    private InteractableBehaviour _interactableBehaviour;
    public override void Initialise()
    {
        _fishes = new List<GameObject>(GameObject.FindGameObjectsWithTag("Fish")); //TODO data.fishes
        _fishesSpawnPoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("FishSpawnPoint"));
        foreach(var fish in _fishes)
        {
            fish.GetComponent<InteractableBehaviour>().CatchedEvent += OnCatched;
        }
    }

    public override void Execute()
    {
        MoveFish();
    }
    public override void Dispose()
    {
        foreach (var fish in _fishes)
        {
            fish.GetComponent<InteractableBehaviour>().CatchedEvent -= OnCatched;
        }
    }

    private void MoveFish()
    {
        foreach(var fish in _fishes)
        {
            fish.transform.Translate(Vector3.right * 0.5f * Time.deltaTime);
            CheckFishOutOfScreen(fish);
        }
    }

    private void CheckFishOutOfScreen(GameObject fish)
    {
        if(Mathf.Abs(fish.transform.position.x) > 10f)
        {
            var spawnPoint = _fishesSpawnPoints[Random.Range(0, _fishesSpawnPoints.Count)];
            fish.transform.position = spawnPoint.transform.position;
            fish.transform.rotation = spawnPoint.transform.rotation;
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
