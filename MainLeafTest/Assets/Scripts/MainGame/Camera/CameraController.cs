using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera[] CameraList;
 

    public void ChangeCamera(int x)
    {
        foreach (var item in CameraList)
        {
            item.Priority = 0;
        }

        CameraList[x].Priority = 10;
    }
}
