using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombThrowAttack : MonoBehaviour
{
	[SerializeField]
	private float m_Cooldown = 8;
	[SerializeField]
	private float m_MinThrowVelocity = 10;
	[SerializeField]
	private float m_MaxThrowVelocity = 30;
	[SerializeField]
	private GameObject m_BombPrefab;

	private GameObject m_Player;

	private void Start()
	{
		m_Player = GameObject.FindGameObjectWithTag("Player");
	}

	public Vector2 CalculateInitialVelocity(Vector2 startPosition, Vector2 targetPosition, float minSpeed, float maxSpeed)
	{
		var displacement = targetPosition - startPosition;

		var distance = displacement.magnitude;
		var timeToReach = distance / maxSpeed;

		var gravity = Physics2D.gravity.y * 2;
		var vy = (displacement.y - 0.5f * gravity * timeToReach * timeToReach) / timeToReach;
		var vx = displacement.x / timeToReach;

		var initialSpeed = Mathf.Clamp(Mathf.Sqrt(vx * vx + vy * vy), minSpeed, maxSpeed);
		var initialVelocity = new Vector2(vx, vy).normalized * initialSpeed;

		return initialVelocity;
	}

	public void Activate()
	{
		var bomb = Instantiate(m_BombPrefab, transform.position, Quaternion.identity)
			.GetComponent<Rigidbody2D>();

		bomb.velocity = CalculateInitialVelocity(transform.position, m_Player.transform.position, m_MinThrowVelocity, m_MaxThrowVelocity);
	}
}
