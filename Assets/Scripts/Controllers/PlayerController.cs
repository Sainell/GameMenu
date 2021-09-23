using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseController
{
    private PlayerData _playerData;
    private List<Vector3> _playerSpawnPoints;
    private GameObject _player;
    public override void Initialise(LevelData levelData)
    {
        _playerData = Resources.Load<PlayerData>($"Data/PlayerData");
        _playerSpawnPoints = GameController.Instance.SpawnPointController.PlayerSpawnPoints;
        PlayerSpawn();
        base.Initialise(levelData);
    }

    public override void Execute()
    {
        if (!IsInitialised)
            return;
    }

    public override void Dispose()
    {

    }
    public override void Clear()
    {
        GameObject.Destroy(_player);
    }
    private Vector3 GetSpawnPoint()
    {
        return _playerSpawnPoints[Random.Range(0, _playerSpawnPoints.Count)];
    }
    
    private void PlayerSpawn()
    {
        var spawnPoint = GetSpawnPoint();
        var quaternion = spawnPoint.x < 0 ? Quaternion.identity : Quaternion.Euler(Vector3.down * 180);
        _player = GameObject.Instantiate(_playerData.PlayerPrefab, spawnPoint, Quaternion.identity);
        var playerModel = _player.transform.GetChild(0);
        playerModel.rotation = quaternion;
    }
}
