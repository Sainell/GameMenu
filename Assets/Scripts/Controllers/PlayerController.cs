using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseController
{
    private PlayerData _playerData;
    private List<Vector3> _playerSpawnPoints;
    private GameObject _playerPrefabRoot;

    public GameObject Player { get; private set; }
    public GameObject Boat{ get; private set; }
    public GameObject Rod { get; private set; }
    public Transform RodEnd { get; private set; }
    public GameObject Hook { get; private set; }
    
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
        GameObject.Destroy(_playerPrefabRoot);
    }
    private Vector3 GetSpawnPoint()
    {
        return _playerSpawnPoints[Random.Range(0, _playerSpawnPoints.Count)];
    }
    
    private void PlayerSpawn()
    {
        var spawnPoint = GetSpawnPoint();
        var quaternion = spawnPoint.x < 0 ? Quaternion.identity : Quaternion.Euler(Vector3.down * 180);
        _playerPrefabRoot = GameObject.Instantiate(_playerData.PlayerPrefab, spawnPoint, Quaternion.identity);
        Player = _playerPrefabRoot.transform.GetChild(0).gameObject;
        Boat = _playerPrefabRoot.transform.GetChild(1).gameObject;
        Hook = _playerPrefabRoot.transform.GetChild(2).gameObject;
        Rod = Player.transform.GetChild(0).gameObject;
        RodEnd = Rod.transform.GetChild(0);
        Player.transform.rotation = quaternion;
    }
}
