using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableBehaviour : MonoBehaviour
{
    public event Action<GameObject,GameObject> CatchedEvent;
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        CatchedEvent?.Invoke(collision.gameObject, this.gameObject);
    }
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {

    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        
    }
}
