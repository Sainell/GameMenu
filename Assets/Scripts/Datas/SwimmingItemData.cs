using UnityEngine;

public class SwimmingItemData : ScriptableObject
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private float _horizontalSpeed;
    [SerializeField] private float _verticalSpeed;
    [SerializeField] private float _spawnDelayMin;
    [SerializeField] private float _spawnDelayMax;
    [SerializeField] private int _catchedPoint;

    public GameObject Prefab => _prefab;
    public float HorizontalSpeed => _horizontalSpeed;
    public float VerticalSpeed => _verticalSpeed;
    public float SpawnDelayMin => _spawnDelayMin;
    public float SpawnDelayMax => _spawnDelayMax;
    public int CatchedPoint => _catchedPoint;

    public float GetRandomSpawnDelayTime()
    {
        var delayTime = Random.Range(_spawnDelayMin, _spawnDelayMax);
        return delayTime;
    }
}
