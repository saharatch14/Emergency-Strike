using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public enum ResourceTypes { Money}
    public ResourceTypes resourceType;

    public float harvestTime;
    public float availableResorce;

    public int gatherers;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ResourceTick());
    }

    // Update is called once per frame
    void Update()
    {
        if (availableResorce <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void ResourceGather()
    {
        if (gatherers != 0)
        {
            availableResorce -= gatherers;
        }
    }

    IEnumerator ResourceTick()
    {
        while(true)
        {
            yield return new WaitForSeconds(1);
            ResourceGather();
        }
    }
}
