using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCamera : MonoBehaviour
{
    public CameraController _cameraController;
    public int i_cameraNumber;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _cameraController.ChangeCamera(i_cameraNumber);
        }
    }
}
