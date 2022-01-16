using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    public Vector3 startPos;
    public Vector3 FinalPos;

    private void Start()
    {
        if (PlayerPrefs.GetInt("resolved") == 0)
            transform.parent.localPosition = startPos;
        else
            transform.parent.localPosition = FinalPos;

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().CanPushABox(true,gameObject.GetComponentInParent<Rigidbody>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().CanPushABox(false);
        }
    }
}
