using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BossController : MonoBehaviour
{
	[SerializeField]
	private Color[] m_Colors;
	[SerializeField]
	private float m_MoveSpeed = 8;
	[SerializeField]
	private float m_MoveDamping = 0.05f;
	[SerializeField]
	private float m_JumpSpeed = 10;
	[SerializeField]
	private float m_CoyoteTime = 0.3f;
	[SerializeField]
	private float m_SlimeHealth = 10;
	[SerializeField]
	private float m_SquishDamping = 0.1f;
	[SerializeField]
	private float m_SquishIntensity = 0.2f;
	[SerializeField]
	private float m_SquishFromVelocity = 0.1f;
	[SerializeField]
	private float m_InvertDamageSize = 200;
	[SerializeField]
	private float m_WinSize = 300;
	[SerializeField]
	private GameObject m_Sprites;
	[SerializeField]
	private SpriteRenderer[] m_ColorizedSprites;
	[SerializeField]
	private Animator m_Animator;
	private Health m_Health;
	private int m_ColorIndex;
	private bool m_Drop;
	private float m_Squish;
	private float m_SquishVelocity;
	private bool m_FacingRight;
	private bool m_Grounded;
	private float m_AirTime;
	private Vector2 m_Velocity;
	private ContactPoint2D[] m_Contacts = new ContactPoint2D[8];
    private Rigidbody2D m_Rigidbody;
	private Collider2D m_Collider;
	private PlayerInput m_PlayerInput;
	private InputAction m_MoveAction;

	public int ColorIndex => m_ColorIndex;

	private void Awake()
	{
		m_Health = GetComponent<Health>();
		m_Rigidbody = GetComponent<Rigidbody2D>();
		m_Collider = GetComponent<Collider2D>();
		m_PlayerInput = GetComponent<PlayerInput>();

		m_MoveAction = m_PlayerInput.actions["Move"];

		OnChangeColor();
	}

	private void FixedUpdate()
	{
		var contactFilter = new ContactFilter2D();
		contactFilter.SetNormalAngle(5, 175);

		m_Grounded = m_Rigidbody.GetContacts(contactFilter, m_Contacts) > 0;
	}

	private void Update()
	{
		if (m_Health.Value > m_InvertDamageSize)
		{
			m_Health.InvertDamage = true;
		}

		if (m_Health.Value > m_WinSize)
		{
			SceneManager.LoadScene("GameWin");
		}

		transform.localScale = Vector3.one * m_Health.Value / m_Health.InitialValue;

		var moveInput = m_MoveAction.ReadValue<Vector2>();

		var targetVelocity = new Vector2(moveInput.x * m_MoveSpeed, m_Rigidbody.velocity.y);
		m_Rigidbody.velocity = Vector2.SmoothDamp(m_Rigidbody.velocity, targetVelocity, ref m_Velocity, m_MoveDamping);

		var targetSquish = math.tanh(m_Rigidbody.velocity.y * m_SquishFromVelocity);
		m_Squish = Mathf.SmoothDamp(m_Squish, targetSquish, ref m_SquishVelocity, m_SquishDamping);
		m_Sprites.transform.localScale = new Vector3(Mathf.LerpUnclamped(1, 1 + m_SquishIntensity, m_Squish),
			Mathf.LerpUnclamped(1, 1 - m_SquishIntensity, m_Squish), 1);

		//m_Sprites.transform.localScale = new Vector3(m_Sprites.transform.localScale.x * (m_FacingRight ? -1 : 1), m_Sprites.transform.localScale.y, 1);

		if (Mathf.Abs(moveInput.x) > 0.1f)
		{
			m_FacingRight = moveInput.x > 0;
		}

		if (m_Grounded)
		{
			m_AirTime = 0;
		}
		else
		{
			m_AirTime += Time.deltaTime;
		}

		m_Drop = moveInput.y < 0;

		//Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Boss"), LayerMask.NameToLayer("Platform"), moveInput.y < 0);
	}

	private void OnJump()
	{
		if (m_Grounded || m_AirTime < m_CoyoteTime)
		{
			m_AirTime = m_CoyoteTime;
			m_Grounded = false;
			m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, m_JumpSpeed);
		}
	}

	private void OnChangeColor()
	{
		m_ColorIndex = (m_ColorIndex + 1) % m_Colors.Length;

		foreach (var sprite in m_ColorizedSprites)
		{
			var color = m_Colors[m_ColorIndex];
			color.a = sprite.color.a;
			sprite.color = color;
		}
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.layer != LayerMask.NameToLayer("Platform"))
		{
			return;
		}

		if (!m_Drop)
		{
			return;
		}

		Physics2D.IgnoreCollision(m_Collider, collision.collider, true);
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
		{
			Physics2D.IgnoreCollision(m_Collider, collision, false);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (!collision.gameObject.CompareTag("Slime"))
		{
			return;
		}

		var smileController = collision.gameObject.GetComponent<SlimeController>();

		if (smileController.ColorIndex != m_ColorIndex)
		{
			return;
		}

		Destroy(collision.gameObject);

		m_Health.Heal(m_SlimeHealth);

	}
}
