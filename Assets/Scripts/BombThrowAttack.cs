using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombThrowAttack : MonoBehaviour
{
	[SerializeField]
	private float m_Cooldown = 8;
	[SerializeField]
	private float m_ThrowVelocity = 40;
	[SerializeField]
	private GameObject m_BombPrefab;

	private GameObject m_Player;

	private void Start()
	{
		m_Player = GameObject.FindGameObjectWithTag("Player");
	}

	public void Activate()
	{
		var direction = (m_Player.transform.position - transform.position).normalized;

		var bomb = Instantiate(m_BombPrefab, transform.position, Quaternion.identity)
			.GetComponent<Rigidbody2D>();

		bomb.velocity = direction * m_ThrowVelocity;
	}
}
