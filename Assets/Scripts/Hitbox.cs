using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
	public float Damage = 10;
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
			return;
        }

        health.Hurt(Damage);
	}
}
