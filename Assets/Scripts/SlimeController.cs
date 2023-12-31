using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
	[SerializeField]
	private float m_MoveSpeed = 3;
	[SerializeField]
	private SpriteRenderer m_BodySprite;
	[SerializeField]
	private ColorScheme m_Colors;
	private SpriteRenderer[] m_Sprites;
    private Rigidbody2D m_Rigidbody;
	private bool m_FacingRight;

	public int ColorIndex;

	public Vector2 MoveDirection => m_FacingRight ? Vector2.right : Vector2.left;

	private void Awake()
	{
		m_Rigidbody = GetComponent<Rigidbody2D>();
		m_Sprites = GetComponentsInChildren<SpriteRenderer>();
	}

	private void Start()
	{
		m_FacingRight = Random.value > 0.5f;

		ColorIndex = Random.Range(0, m_Colors.Length);
		m_BodySprite.color = m_Colors[ColorIndex];
	}

	private void Update()
	{
		m_Rigidbody.velocity = new Vector2((m_FacingRight ? 1 : -1) * m_MoveSpeed, m_Rigidbody.velocity.y);

		foreach (var sprite in m_Sprites)
		{
			sprite.flipX = m_FacingRight;
		}

		//if (!Physics2D.Raycast((Vector2)transform.position + MoveDirection * 0.2f, Vector2.down, 1.5f, LayerMask.GetMask("Default", "Platform")))
		//{
		//	m_FacingRight = !m_FacingRight;
		//}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.contactCount > 0 && Vector3.Dot(collision.contacts[0].normal, MoveDirection) < -0.5f)
		{
			m_FacingRight = !m_FacingRight;
		}
	}
}
