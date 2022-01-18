using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTip : MonoBehaviour
{
    public string s_text;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameController.ChangeTip(s_text);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameController.HideTip();
        }
    }
}
