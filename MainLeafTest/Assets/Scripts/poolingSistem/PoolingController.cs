using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingController : MonoBehaviour
{
    public List<GameObject>[] pools;
    public GameObject[] PoolingElements;

    void Start()
    {
        pools = new List<GameObject>[PoolingElements.Length];
        for (var i = 0; i < PoolingElements.Length; i++)
            pools[i] = new List<GameObject>();
    }
 


    public GameObject findElement(string type, Vector3 position)
    {
        GameObject element = null;
        int numberInPools = 0;
        for (var i = 0; i < PoolingElements.Length; i++)
        {
            if (PoolingElements[i].GetComponent<PoolingElements>().type == type)
            {
                numberInPools = i;
                break;
            }
        }

        foreach (GameObject shot in pools[numberInPools])
        {
            if (shot != null && shot.GetComponent<PoolingElements>().getIngame() == false)
            {
                element = shot;
                
            }
        }

        if (element == null)
            element = criate(type);

        element.GetComponent<PoolingElements>().putInGame();
        element.transform.position = position;
        return element;
    }

    public GameObject criate(string type)
    {

        int numberInPools = 0;
        GameObject element = null;
        for (var i = 0; i < PoolingElements.Length; i++)
        {
            if (PoolingElements[i].GetComponent<PoolingElements>().type == type)
            {
                numberInPools = i;
                element = PoolingElements[i];
                break;
            }
        }

        if (element == null)
        {
            Debug.Log("Cannot find PoolingElements type");
            return null;
        }

        Vector3 spawnPosition = new Vector3(0, -1000, 0);
        Quaternion spawnRotation = Quaternion.identity;
        pools[numberInPools].Add(Instantiate(element, spawnPosition, spawnRotation));

        return pools[numberInPools][pools[numberInPools].Count - 1];
    }
}
