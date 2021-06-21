using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitController : MonoBehaviour
{

    private NavMeshAgent navAgent;
    private Transform currentTarget;
    private float attackTimer;
    Animator anim;
    public UnitStats unitStats;

    private Enemy targetEnemy;


    private void Start()
    {
        anim = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        attackTimer = unitStats.attackSpeed;
    }

    private void Update()
    {
        attackTimer += Time.deltaTime;
        if (currentTarget != null)
        {
            navAgent.destination = currentTarget.position;

            var distance = (transform.position - currentTarget.position).magnitude;

            if (distance <= unitStats.attackRange)
            {
                Attack();
            }
        }
    }

    public void MoveUnit(Vector3 dest)
    {
        currentTarget = null;
        anim.SetBool("IsWalk", true);
        anim.SetBool("IsIdel", false);
        navAgent.destination = dest;
    }

    public void MoveColleterUnit(Vector3 dest)
    {
        currentTarget = null;
        //Debug.Log("Harvesting");
        navAgent.destination = dest;

    }

    public void SetSelected(bool isSelected)
    {
        transform.Find("Highlight").gameObject.SetActive(isSelected);
        transform.Find("HealthBar").gameObject.SetActive(isSelected);
    }

    public void SetNewTarget(Transform enemy)
    {
        currentTarget = enemy;
    }

    public void Attack()
    {
        //Debug.Log("Attack");
        if (attackTimer >= unitStats.attackSpeed)
        {
            RTSGameManager.UnitTakeDamage(this, currentTarget.GetComponent<UnitController>());
            attackTimer = 0;
        }

    }

    public void TakeDamage(UnitController enemy, float damage)
    {
        StartCoroutine(Flasher(GetComponent<Renderer>().material.color));
        /*AiUnitController.Health -= damage;
        if(AiUnitController.Health <= 0)
        {
            Destroy(gameObject);
        }*/
        Enemy e = enemy.GetComponent<Enemy>();

        if (e != null)
        {
            e.TakingDamage(damage);
        }
    }

    IEnumerator Flasher(Color defaultColor)
    {
        var renderer = GetComponent<Renderer>();
        for (int i = 0; i < 2; i++)
        {
            renderer.material.color = Color.gray;
            yield return new WaitForSeconds(.05f);
            renderer.material.color = defaultColor;
            yield return new WaitForSeconds(.05f);
        }
        //Debug.Log("Attack");
    }
}