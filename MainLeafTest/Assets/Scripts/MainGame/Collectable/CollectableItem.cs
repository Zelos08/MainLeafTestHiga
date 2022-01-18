using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollectableItem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Action(other);
        }
    }

    abstract public void Action(Collider player);
}
