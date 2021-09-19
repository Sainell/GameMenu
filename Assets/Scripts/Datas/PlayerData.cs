using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player", order = 1)]
public class PlayerData : ScriptableObject
{
    public GameObject PlayerPrefab;
    public GameObject BoatPrefab;
    public GameObject FishingRodPrefab;
    public GameObject HookPrefab;

}
