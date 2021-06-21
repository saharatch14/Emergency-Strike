using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarAi_Script : MonoBehaviour
{
    private Image HealthBar;
    public float CurrentHealth;
    private float MaxHealth = 100f;
    Enemy enemy;
    //UnitStats Unit;

    // Start is called before the first frame update
    void Start()
    {
        HealthBar = GetComponent<Image>();
        enemy = FindObjectOfType<Enemy>();
        //Unit = FindObjectOfType<UnitStats>();
        //CurrentHealth = Player.Health;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentHealth = enemy.health;
        HealthBar.fillAmount = CurrentHealth / MaxHealth;
    }
}
