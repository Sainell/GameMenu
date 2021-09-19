using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseController
{
    private PlayerData _playerData;
    private List<GameObject> _playerSpawnPoints;
    private GameObject _player;
    public override void Initialise()
    {
        _playerData = Resources.Load<PlayerData>($"Data/PlayerData");
        _playerSpawnPoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("PlayerSpawnPoint")); //todo spawncontroller 
        PlayerSpawn();
    }

    public override void Execute()
    {

    }

    public override void Dispose()
    {

    }
    public override void Clear()
    {
        GameObject.Destroy(_player);
    }
    private Transform GetSpawnPoint()
    {
        return _playerSpawnPoints[Random.Range(0, _playerSpawnPoints.Count)].transform;
    }
    
    private void PlayerSpawn()
    {
        var spawnPoint = GetSpawnPoint();
        _player = GameObject.Instantiate(_playerData.PlayerPrefab, spawnPoint.position, Quaternion.identity);
        var playerModel = _player.transform.GetChild(0);
        playerModel.rotation = spawnPoint.rotation;
    }
}
