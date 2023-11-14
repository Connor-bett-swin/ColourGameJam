using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class BossController : MonoBehaviour
{
	[SerializeField]
	private float m_MoveSpeed = 8;
	[SerializeField]
	private float m_MoveDamping = 0.05f;
	[SerializeField]
	private float m_JumpSpeed = 10;
	[SerializeField]
	private float m_CoyoteTime = 0.3f;
	[SerializeField]
	private float m_SquishDamping = 0.1f;
	[SerializeField]
	private float m_SquishIntensity = 0.2f;
	[SerializeField]
	private float m_SquishFromVelocity = 0.1f;
	[SerializeField]
	private GameObject m_Sprites;
	[SerializeField]
	private SpriteRenderer m_BodySprite;
	[SerializeField]
	private Animator m_Animator;
	private float m_Squish;
	private float m_SquishVelocity;
	private bool m_FacingRight;
	private bool m_Grounded;
	private float m_AirTime;
	private Vector2 m_Velocity;
	private ContactPoint2D[] m_Contacts = new ContactPoint2D[8];
    private Rigidbody2D m_Rigidbody;
	private PlayerInput m_PlayerInput;
	private InputAction m_MoveAction;

	private void Awake()
	{
		m_Rigidbody = GetComponent<Rigidbody2D>();
		m_PlayerInput = GetComponent<PlayerInput>();

		m_MoveAction = m_PlayerInput.actions["Move"];

		m_BodySprite.color = UnityEngine.Random.ColorHSV(0, 1, 1, 1, 1, 1);
	}

	private void FixedUpdate()
	{
		var contactFilter = new ContactFilter2D();
		contactFilter.SetNormalAngle(5, 175);

		m_Grounded = m_Rigidbody.GetContacts(contactFilter, m_Contacts) > 0;
	}

	private void Update()
	{
		var moveInput = m_MoveAction.ReadValue<Vector2>().x;

		var targetVelocity = new Vector2(moveInput * m_MoveSpeed, m_Rigidbody.velocity.y);
		m_Rigidbody.velocity = Vector2.SmoothDamp(m_Rigidbody.velocity, targetVelocity, ref m_Velocity, m_MoveDamping);

		var targetSquish = math.tanh(m_Rigidbody.velocity.y * m_SquishFromVelocity);
		m_Squish = Mathf.SmoothDamp(m_Squish, targetSquish, ref m_SquishVelocity, m_SquishDamping);
		m_Sprites.transform.localScale = new Vector3(Mathf.LerpUnclamped(1, 1 + m_SquishIntensity, m_Squish),
			Mathf.LerpUnclamped(1, 1 - m_SquishIntensity, m_Squish), 1);

		m_Sprites.transform.localScale = new Vector3(m_Sprites.transform.localScale.x * (m_FacingRight ? -1 : 1), m_Sprites.transform.localScale.y, 1);

		if (Mathf.Abs(moveInput) > 0.1f)
		{
			m_FacingRight = moveInput > 0;
		}

		if (m_Grounded)
		{
			m_AirTime = 0;
		}
		else
		{
			m_AirTime += Time.deltaTime;
		}
	}

	private void OnJump()
	{
		if (m_Grounded || m_AirTime < m_CoyoteTime)
		{
			m_Grounded = false;
			m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, m_JumpSpeed);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Slime"))
		{
			Destroy(collision.gameObject);

			transform.localScale += new Vector3(0.2f, 0.2f, 0);
		}
	}
}
