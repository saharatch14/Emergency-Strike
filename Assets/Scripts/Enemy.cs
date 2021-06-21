using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

	[HideInInspector]

	public float startHealth = 100;
	public float health;

	[Header("Unity Stuff")]
	public Image healthBar;

	private bool isDead = false;

	void Start()
	{
		health = startHealth;
	}

	public void TakingDamage(float amount)
	{
		health -= amount;

		healthBar.fillAmount = health / startHealth;

		if (health <= 0 && !isDead)
		{
			Die();
		}
	}

	void Die()
	{
		isDead = true;

		Destroy(gameObject);
	}

}