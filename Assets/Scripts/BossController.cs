using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class BossController : MonoBehaviour
{
	[SerializeField]
	private float m_MaxSpeed = 8;
	[SerializeField]
	private float m_MovementDamping = 0.1f;
	[SerializeField]
	private float m_JumpForce = 400;
	[SerializeField]
	private float m_CoyoteTime = 0.3f;
	[SerializeField]
	private float m_SquishDamping = 0.1f;
	[SerializeField]
	private float m_SquishIntensity = 0.2f;
	[SerializeField]
	private float m_SquishFromVelocity = 0.1f;
	[SerializeField]
	private SpriteRenderer m_SpriteRenderer;
	[SerializeField]
	private Animator m_Animator;
	private float m_Squish;
	private float m_SquishVelocity;
	private bool m_Grounded = true;
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
	}

	private void FixedUpdate()
	{
		var contactFilter = new ContactFilter2D();
		contactFilter.SetNormalAngle(1, 179);

		m_Grounded = m_Rigidbody.GetContacts(contactFilter, m_Contacts) > 0;
	}

	private void Update()
	{
		var moveInput = m_MoveAction.ReadValue<Vector2>().x;

		var targetVelocity = new Vector2(moveInput * m_MaxSpeed, m_Rigidbody.velocity.y);
		m_Rigidbody.velocity = Vector2.SmoothDamp(m_Rigidbody.velocity, targetVelocity, ref m_Velocity, m_MovementDamping);

		var targetSquish = math.tanh(m_Rigidbody.velocity.y * m_SquishFromVelocity);
		m_Squish = Mathf.SmoothDamp(m_Squish, targetSquish, ref m_SquishVelocity, m_SquishDamping);
		m_SpriteRenderer.transform.localScale = new Vector3(Mathf.LerpUnclamped(1, 1 + m_SquishIntensity, m_Squish),
			Mathf.LerpUnclamped(1, 1 - m_SquishIntensity, m_Squish), 1);

		if (moveInput > 0.1f)
		{
			m_SpriteRenderer.flipX = true;
		}
		else if (moveInput < -0.1f)
		{
			m_SpriteRenderer.flipX = false;
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
			m_Rigidbody.AddForce(new Vector2(0, m_JumpForce));
		}
	}
}
