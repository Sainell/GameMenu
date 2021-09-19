using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController
{
    public bool IsInitialised;
    public virtual void Initialise()
    {
        IsInitialised = true;
    }

    public abstract void Execute();

    public abstract void Dispose();

    public abstract void Clear();


}
