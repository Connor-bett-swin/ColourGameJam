using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
	public float Damage = 10;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		var health = collision.GetComponentInParent<Health>();

		if (health != null)
		{
			health.Hurt(Damage);
		}
	}
}
