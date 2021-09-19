using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController
{
    protected bool IsInitialised;
    protected LevelData LevelData;
    public virtual void Initialise(LevelData levelData)
    {
        IsInitialised = true;
        LevelData = levelData;
    }

    public abstract void Execute();

    public abstract void Dispose();

    public abstract void Clear();


}
