using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class FishController : BaseController
{
    private const float CAMERA_BORDER_OFFSET = 1.3f;
    private const float Y_OFFSET = 0.75f;

    public event Action<int> CatchedData;
    private List<FishItem> _fishesData;
    private List<Vector3> _fishesSpawnPoints;
    private List<Vector3> _startSpawnPoints;
    private Dictionary<GameObject, FishItem> _fishesDic; // mb list? GO in fishItem
    private float _spawnTime;
    private bool _isPulledOut;
    private bool _isFirstSpawn = true;
    private bool _isNeedSpawnTimer;
    private int _catchedFishPoint;

    private CameraController _cameraController => GameController.Instance.CameraController;


    public override void Initialise(LevelData levelData)
    {
        _fishesData = levelData.FishList;
        _fishesDic = new Dictionary<GameObject, FishItem>();
        _fishesSpawnPoints = GameController.Instance.SpawnPointController.RespawnPoints;
        _startSpawnPoints = GameController.Instance.SpawnPointController.StartSpawnPoints;
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
        MoveFishHorizontal();
        MoveFishVertical();
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
                var quaternion = spawnPoint.x < 0 ? Quaternion.identity : Quaternion.Euler(Vector3.down * 180);
                var newFish = GameObject.Instantiate(fish.FishData.Prefab, spawnPoint, quaternion);
                newFish.name = $"{fish.FishData.FishType}{newFish.GetInstanceID()}";

                _fishesDic.Add(newFish, new FishItem(fish));
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

    private Vector3 GetSpawnPoint()
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

    private void MoveFishHorizontal()
    {
        foreach (var fish in _fishesDic)
        {
            if (!fish.Value.IsWaitRespawn && CheckFishOutOfHorizontalScreen(fish.Key))
                fish.Key.transform.Translate(Vector3.right * fish.Value.FishData.HorizontalSpeed * Time.deltaTime);
        }
    }

    private void MoveFishVertical()
    {
        foreach (var fish in _fishesDic)
        {
            var forbiddenDirection = Vector3.one;
            if (IsNeedChangeVerticalDirection())
            {
                fish.Value.VerticalDirection = ChooseVerticalDirection(forbiddenDirection);
            }
            if (CheckFishOutOfVerticalScreen(fish.Key, out forbiddenDirection))
            {
                fish.Value.VerticalDirection = ChooseVerticalDirection(forbiddenDirection);
            }

            fish.Key.transform.Translate(fish.Value.VerticalDirection * fish.Value.FishData.VerticalSpeed * Time.deltaTime);
        }
    }

    private bool IsNeedChangeVerticalDirection()
    {
        var rn = Random.Range(0f, 100f);
        var isNeed = rn < 0.1f ? true : false;
        return isNeed;
    }

    private Vector3 ChooseVerticalDirection(Vector3 direction)
    {
        var random = Random.Range(0f, 15f);
        if (random < 5f && direction!= Vector3.up)
            return Vector3.up;
        else if (random > 5f && random < 10 && direction != Vector3.down)
            return Vector3.down;
        else
            return Vector3.zero;;
    }

    private bool CheckFishOutOfVerticalScreen(GameObject fish, out Vector3 forbiddenDirection)
    {
        if (fish.transform.position.y < -_cameraController.CameraHalfHeight + Y_OFFSET)
        {
            forbiddenDirection = Vector3.down;
            return true;
        }
        if (fish.transform.position.y > 0 - Y_OFFSET)
        {
          forbiddenDirection = Vector3.up;
            return true;
        }
        else
        {
            forbiddenDirection = Vector3.one;
            return false;
        }
    }

    private bool CheckFishOutOfHorizontalScreen(GameObject fish)
    {
        if (Mathf.Abs(fish.transform.position.x) > _cameraController.CameraWidth / CAMERA_BORDER_OFFSET)
        {
            var spawnPoint = _fishesSpawnPoints[Random.Range(0, _fishesSpawnPoints.Count)];
            var quaternion = spawnPoint.x < 0 ? Quaternion.identity : Quaternion.Euler(Vector3.down * 180);
            _fishesDic[fish].IsWaitRespawn = true;
            DOVirtual.DelayedCall(Random.Range(0, 5), () => SetFishInOtherSpawnPoint(fish, spawnPoint, quaternion));

            return false;
        }
        return true;
    }

    private void SetFishInOtherSpawnPoint(GameObject fish, Vector3 spawnPoint, Quaternion quaternion )
    {
        _fishesDic[fish].IsWaitRespawn = false;
        fish.transform.SetPositionAndRotation(spawnPoint, quaternion);
    }

    private void OnCatch(GameObject catchedFish)
    {
        _catchedFishPoint = _fishesDic[catchedFish].FishData.CatchedPoint;
        _fishesDic.Remove(catchedFish);
    }

}
