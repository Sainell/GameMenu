using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FishData", menuName = "Fishes/new Fish", order = 1)]
public class FishData : ScriptableObject
{
    public FishType FishType;
    public GameObject FishPrefab;
    public float FishSpeed;
    public float FishSpawnDelay;
    public int FishCatchedPoint;

    public InteractableBehaviour InteractableBehaviour;
}
