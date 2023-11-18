using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float MoveVelocity = 8;
    public float MoveAcceleration = 30;
    public float JumpVelocity = 10;
    public float CoyoteTime = 0.3f;
    public LayerMask GroundMask;

    public bool Grounded => m_Grounded;

	[SerializeField]
    private Rigidbody2D m_Rigidbody;
    [SerializeField]
    private Collider2D m_Collider;
    private ContactPoint2D[] m_ContactPoints = new ContactPoint2D[16];
    private float m_CurrentMoveVelocity;
	private float m_AirTime;
	private bool m_Grounded;
    private float m_TargetMoveVelocity;

	public void Move(float direction)
	{
        m_TargetMoveVelocity = Mathf.Clamp(direction, -1, 1) * MoveVelocity;
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

    public void Drop()
    {
        var contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(LayerMask.GetMask("Platform"));
        
		var count = m_Rigidbody.GetContacts(contactFilter, m_ContactPoints);

        if (count == 0)
        {
            return;
        }

        var platformCollider = m_ContactPoints.First().collider;

		Physics2D.IgnoreCollision(m_Collider, platformCollider, true);
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
		{
			Physics2D.IgnoreCollision(m_Collider, collision, false);
		}
	}

	private void FixedUpdate()
	{
        m_Grounded = Physics2D.OverlapCircle(m_Collider.bounds.center - new Vector3(0, m_Collider.bounds.extents.y), 0.5f, GroundMask) != null;
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
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(m_Collider.bounds.center - new Vector3(0, m_Collider.bounds.extents.y), 0.5f);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(m_TargetMoveVelocity, 0));
	}
}
