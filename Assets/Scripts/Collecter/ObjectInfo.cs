using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectInfo : MonoBehaviour
{
    public TaskList task;
    public ResourceManager RM;

    GameObject targetNode;

    public NodeManager.ResourceTypes heldResourceType;

    public bool isSelected = false;
    public bool isGathering = false;

    public string objectName;

    private NavMeshAgent agent;

    public int heldResource;
    public int maxHeldResource;

    public GameObject[] drops;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GatherTick());
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {

        if(targetNode == null)
        {
            if(heldResource != 0)
            {
                drops = GameObject.FindGameObjectsWithTag("Drops");
                agent.destination = GetClosestDropOff(drops).transform.position;
                drops = null;
                task = TaskList.Delivering;
            }
            else
            {
                task = TaskList.Idle;
            }
        }
        if (heldResource >= maxHeldResource)
        {
            drops = GameObject.FindGameObjectsWithTag("Drops");
            agent.destination = GetClosestDropOff(drops).transform.position;
            drops = null;
            task = TaskList.Delivering;
        }
        if (Input.GetMouseButtonDown(1) && isSelected)
        {
            RightClick();
        }
    }

    GameObject GetClosestDropOff(GameObject[] dropOffs)
    {
        GameObject closestDrop = null;
        float closestDistance = Mathf.Infinity;
        Vector3 position = transform.position;

        foreach(GameObject targetDrop in dropOffs)
        {
            Vector3 direction = targetDrop.transform.position - position;
            float distance = direction.sqrMagnitude;
            if(distance < closestDistance)
            {
                closestDistance = distance;
                closestDrop = targetDrop;
            }
        }
        return closestDrop;
    }

    public void RightClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            if(hit.collider.tag == "Ground")
            {
                agent.destination = hit.point;
                Debug.Log("Moving");
                task = TaskList.Moving;
            }
            else if (hit.collider.tag == "Resource")
            {
                agent.destination = hit.collider.gameObject.transform.position;
                Debug.Log("Harvesting");
                task = TaskList.Gathering;
                targetNode = hit.collider.gameObject;
            }
        }

    }

    public void OnTriggerEnter(Collider other)
    {
        GameObject hitObject = other.gameObject;

        if(hitObject.tag == "Resource" && task == TaskList.Gathering)
        {
            isGathering = true;
            hitObject.GetComponent<NodeManager>().gatherers++;
            heldResourceType = hitObject.GetComponent<NodeManager>().resourceType;
        }  
        else if (hitObject.tag == "Drops" && task == TaskList.Delivering)
        {
            if(RM.money >= RM.maxMoney)
            {
                task = TaskList.Idle;
            }
            else
            {
                RM.money += heldResource;
                heldResource = 0;
                task = TaskList.Gathering;
                agent.destination = targetNode.transform.position;
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        GameObject hitObject = other.gameObject;

        if (hitObject.tag == "Resource")
        {
            hitObject.GetComponent<NodeManager>().gatherers--;
            isGathering = false;
        }
    }

    IEnumerator GatherTick()
    {
        while(true)
        {
            yield return new WaitForSeconds(1);
            if(isGathering)
            {
                heldResource++;
            }
        }
    }
}
