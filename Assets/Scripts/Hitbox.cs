using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
	public float Damage = 10;
	public float Growth;
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

		if (boss != null && Colored && boss.ColorIndex == GetColorIndex())
        {
			if (Growth > 0)
			{
				health.Heal(Growth);
			}

			return;
        }

        health.Hurt(Damage);
	}

	private int GetColorIndex()
	{
		var colorized = GetComponentInParent<Colorized>();

		if (colorized != null)
		{
			return colorized.ColorIndex;
		}

		return ColorIndex;
	}
}
