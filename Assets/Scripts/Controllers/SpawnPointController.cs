using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointController : BaseController
{
    public List<Vector3> RespawnPoints => _respawnPoints;
    public List<Vector3> StartSpawnPoints => _startSpawnPoints;
    public List<Vector3> PlayerSpawnPoints => _playerSpawnPoints;

    private const float X_OFFSET = 2f;
    private const float Y_OFFSET = 1.25f;
    private const float PLAYER_Y_OFFSET = 1.75f;
    private const int SPAWN_LINE_COUNT = 3;
    private CameraController _cameraController => GameController.Instance.CameraController;
    private List<Vector3> _respawnPoints;
    private List<Vector3> _startSpawnPoints;
    private List<Vector3> _playerSpawnPoints;

    public override void Initialise(LevelData levelData)
    {
        _respawnPoints = new List<Vector3>();
        _startSpawnPoints = new List<Vector3>();
        _playerSpawnPoints = new List<Vector3>();

        CreateAllSpawnPoints(_cameraController.CameraHalfHeight * -1 + Y_OFFSET, SPAWN_LINE_COUNT);
      //  CreateTestObjectOnPoints();
        base.Initialise(levelData);
    }
    public override void Execute()
    {

    }
    public override void Clear()
    {

    }

    public override void Dispose()
    {

    }

    private void CreateRespawnPoint(float posY)
    {
        var rightPoint = new Vector3(_cameraController.CameraHalfWidth + X_OFFSET, posY);
        var leftPoint = new Vector3((_cameraController.CameraHalfWidth + X_OFFSET) * -1, posY);
        _respawnPoints.Add(rightPoint);
        _respawnPoints.Add(leftPoint);
    }

    private void CreateStartSpawnPoint(float posY)
    {
        var pointCount = _cameraController.CameraWidth / X_OFFSET - 1;

        for (int i = 0; i < pointCount; i++)
        {
            var point = new Vector3(-1 * _cameraController.CameraHalfWidth + X_OFFSET + (X_OFFSET * i), posY);
            _startSpawnPoints.Add(point);
        }
    }

    private void CreatePlayerSpawnPoints(float posY)
    {
        var rightPoint = new Vector3(_cameraController.CameraHalfWidth / X_OFFSET, posY);
        var leftPoint = new Vector3((_cameraController.CameraHalfWidth / X_OFFSET) * -1, posY);
        _playerSpawnPoints.Add(rightPoint);
        _playerSpawnPoints.Add(leftPoint);
    }

    private void CreateAllSpawnPoints(float posY, float lineCount)
    {
        for (int i = 0; i < lineCount; i++)
        {
            CreateRespawnPoint(posY + Y_OFFSET * i);
            CreateStartSpawnPoint(posY + Y_OFFSET * i);
        }
        CreatePlayerSpawnPoints(posY + PLAYER_Y_OFFSET * lineCount);
    }

    //for test spawnpoints
    private void CreateTestObjectOnPoints()
    {
        var respawn = Resources.Load<GameObject>("fishSpawnPoint");
        var spawn = Resources.Load<GameObject>("startSpawnPoint");
        var player = Resources.Load<GameObject>("playerSpawnPoint");
        var i = 0;
        foreach (var point in _startSpawnPoints)
        {
            GameObject.Instantiate(spawn, point, Quaternion.identity);
        }
        foreach (var point in _respawnPoints)
        {
            GameObject.Instantiate(respawn, point, Quaternion.identity);
        }
        foreach (var point in _playerSpawnPoints)
        {
            var quaternion = i % 2 != 0 ? Quaternion.identity : Quaternion.Euler(Vector3.down * 180);
            GameObject.Instantiate(player, point, quaternion);
            i++;

        }
    }
}
