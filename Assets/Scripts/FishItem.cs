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
}