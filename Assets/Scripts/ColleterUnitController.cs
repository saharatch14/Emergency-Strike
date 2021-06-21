using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ColleterUnitController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform currentTarget;
    private float attackTimer;
    Animator anim;
    public UnitStats unitStats;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {

    }


    public void MoveColleterUnit(Vector3 dest)
    {
        agent.destination = dest;
    }

    public void SetSelected(bool isSelected)
    {
        transform.Find("Highlight").gameObject.SetActive(isSelected);
        transform.Find("HealthBar").gameObject.SetActive(isSelected);
    }

}
