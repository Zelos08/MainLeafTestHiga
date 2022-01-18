using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardTriggerArea : MonoBehaviour
{
    public Transform _head;
    public float f_reach = 3;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _head.LookAt(other.gameObject.GetComponent<BoxCollider>().bounds.center);
            RaycastHit hit;
            Ray ray = new Ray(_head.position, _head.forward * f_reach);

            if (Physics.Raycast(ray, out hit, f_reach))
            {
                if (hit.collider.CompareTag("Player"))
                    Debug.Log("acho");
            }
        }
    }   
}
