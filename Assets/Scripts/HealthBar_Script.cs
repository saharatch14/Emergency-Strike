using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar_Script : MonoBehaviour
{
    private Image HealthBar;
    public float CurrentHealth = 100f;
    private float MaxHealth = 100f;
    AiUnitController Player;
    //UnitStats Unit;

    // Start is called before the first frame update
    void Start()
    {
        HealthBar = GetComponent<Image>();
        Player = FindObjectOfType<AiUnitController>();
        //Unit = FindObjectOfType<UnitStats>();
        //CurrentHealth = Player.Health;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentHealth = Player.Health;
        HealthBar.fillAmount = CurrentHealth / MaxHealth;
    }

    void check()
    {

    }

}
