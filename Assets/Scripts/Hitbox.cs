using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
	public float Damage = 10;
	public bool Grow;
	public bool Colored;
	public int ColorIndex;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		var health = collision.GetComponentInParent<Health>();

		if (health == null)
		{
			return;
		}

		var boss = collision.GetComponentInParent<BossController>();

		if (boss != null && Colored && boss.ColorIndex == ColorIndex)
        {
			if (Grow)
			{
				health.Heal(Damage);
			}

			return;
        }

        health.Hurt(Damage);
	}
}
