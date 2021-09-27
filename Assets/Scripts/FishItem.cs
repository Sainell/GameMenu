using UnityEngine;
using System;

[Serializable]
public class FishItem
{
    [SerializeField] private FishData _fishData;
    [SerializeField] private int _count;

    public FishData FishData => _fishData;
    public FishType FishType => _fishData.FishType;
    public int Count => _count;

    public Vector3 VerticalDirection { get; set; }
    public bool IsWaitRespawn { get; set; }
    public FishItem()
    {

    }
    public FishItem(FishItem fishItem)
    {
        _fishData = fishItem._fishData;
        _count = fishItem._count;
    }
}
