using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishInteractableBehaviour : InteractableBehaviour
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
    protected override void OnTriggerExit2D(Collider2D collision)
    {

    }

    protected override void OnTriggerEnter(Collider other)
    {

    }
}
