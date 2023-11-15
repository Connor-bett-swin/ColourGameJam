using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float MoveVelocity = 8;
    public float MoveAcceleration = 30;
    public float JumpVelocity = 10;
    public float CoyoteTime = 0.3f;
    public float Feet = 1;
    public LayerMask GroundMask;

	[SerializeField]
    private Rigidbody2D m_Rigidbody;
    private float m_CurrentMoveVelocity;
	private float m_AirTime;
	private bool m_Grounded;
    private float m_TargetMoveVelocity;

	public void Move(float direction)
	{
        m_TargetMoveVelocity = Mathf.Clamp01(direction) * MoveVelocity;
	}

    public void Jump()
    {
        if (m_Grounded || m_AirTime < CoyoteTime)
        {
            m_AirTime = CoyoteTime;
			m_Grounded = false;
            m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, JumpVelocity);
        }
    }

	private void FixedUpdate()
	{
        m_Grounded = Physics2D.OverlapCircle((Vector2)transform.position + Vector2.down * Feet, 0.5f, GroundMask) != null;
	}

	private void Update()
	{
		if (m_Grounded)
        {
            m_AirTime = 0;
        }
        else
        {
            m_AirTime += Time.deltaTime;
        }

		m_Rigidbody.velocity = new Vector2(Mathf.SmoothDamp(m_Rigidbody.velocity.x, m_TargetMoveVelocity, ref m_CurrentMoveVelocity, 1 / MoveAcceleration), m_Rigidbody.velocity.y);

		Move(1);
        Jump();
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + Vector2.down * Feet, 0.5f);
	}
}
