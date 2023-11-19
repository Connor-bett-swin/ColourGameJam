using CleverCrow.Fluid.BTs.Trees;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Hero : MonoBehaviour
{
    [SerializeField]
    private BehaviorTree m_BehaviorTree;
	[SerializeField]
	private float m_JumpCooldown = 0.5f;
	[SerializeField]
	private float m_JumpAngle = 45;
	[SerializeField]
	private float m_DropAngle = 60;
	[SerializeField]
	private Collider2D m_Collider;
	[SerializeField]
	private Animator m_Animator;
	[SerializeField]
	private SpriteRenderer m_Arm;
	[SerializeField]
	private BombThrowAttack m_BombThrowAttack;
	[SerializeField]
	private ArrowRainSequence m_ArrowRainSequence;
	[SerializeField]
	private LaserGridSequence m_LaserGridSequence;
	[SerializeField]
	private ArrowShooter m_ChargedShot;
	[SerializeField]
	private BasicShot m_BasicShot;
	private GameObject m_Player;
	private Health m_PlayerHealth;
	private Seeker m_Seeker;
	private Character m_Character;
	private List<Vector2> m_Path = new List<Vector2>();
	private float m_JumpCooldownTimer;

	public bool IsAttacking()
	{
		return !m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Jump") &&
			!m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") &&
			!m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Run");
	}

	private void Awake()
	{
		m_BehaviorTree = new BehaviorTreeBuilder(gameObject)
			.Sequence()
				.WaitTime(Random.Range(3, 6))
				.SelectorRandom()
					//.ThrowBombAction()
					//.CastLaserAction()
					//.CastFireballAction()
					.ChargedShotAction()
					.BasicShotAction()
			.End()
			.Build();

		m_Seeker = GetComponent<Seeker>();
		m_Seeker.pathCallback = OnPathComplete;

		InvokeRepeating(nameof(UpdatePath), 0, 1.5f);

		m_Character = GetComponent<Character>();
	}

	private void OnPathComplete(Path path)
	{
		m_Path = path.vectorPath.Select(x => (Vector2)x).ToList();
		m_Path.RemoveAt(0);
	}

	private void Start()
	{
		m_Player = GameObject.FindGameObjectWithTag("Player");
		m_PlayerHealth = m_Player.GetComponent<Health>();
	}

	private Vector2 GetTarget()
	{
		//var pathNodes = AstarPath.active.data.pointGraph.nodes;
		//return (Vector3)pathNodes[Random.Range(0, pathNodes.Length)].position;

		var pathNodes = AstarPath.active.data.pointGraph.nodes;
		return (Vector3)pathNodes.OrderByDescending(x => GetTargetScore((Vector3)x.position)).ElementAt(Random.Range(0, 3)).position;
	}

	private float GetTargetScore(Vector2 targetPosition)
	{
		return Mathf.Abs(targetPosition.x - m_Player.transform.position.x) -
			3.5f * Mathf.Abs(targetPosition.y - m_Player.transform.position.y);
	}

	private void Update()
	{
		m_BehaviorTree.Tick();

		UpdateAim();

		UpdateMovement();

		if (IsAttacking())
		{
			m_Character.LookAt(m_Player.transform.position);
		}

		if (m_JumpCooldownTimer > 0)
		{
			m_JumpCooldownTimer -= Time.deltaTime;
		}
	}

	private void UpdateAim()
	{
		var direction = ((Vector2)m_Player.transform.position - (Vector2)m_Character.transform.position).normalized;
		var angle = Vector2.SignedAngle(Vector2.right, direction);

		m_Arm.transform.localEulerAngles = new Vector3(0, 0, angle);
		m_Arm.transform.localPosition = new Vector3(m_Character.FacingRight ? 0.1f : -0.1f, 0, 0);
		m_Arm.flipY = angle < -90 || angle > 90;
	}

	private void UpdateMovement()
	{
		if (m_Path.Count == 0 || IsAttacking())
		{
			return;
		}

		var nextNode = m_Path.First();

		if (m_Character.Grounded && m_Collider.OverlapPoint(nextNode))
		{
			m_Path.RemoveAt(0);
		}

		if (m_Character.Grounded)
		{
			m_Character.Move(nextNode.x < transform.position.x ? -1 : 1);
		}
		else
		{
			m_Character.Move(nextNode.x - transform.position.x);
		}

		var direction = (nextNode - (Vector2)transform.position).normalized;

		if (Vector2.Distance(nextNode, transform.position) < 1.5f)
		{
			return;
		}

		if (m_JumpCooldownTimer <= 0 && Vector2.Angle(Vector2.up, direction) < m_JumpAngle)
		{
			m_Character.JumpTo(nextNode);
			m_JumpCooldownTimer = m_JumpCooldown;
		}

		if (Vector2.Angle(Vector2.down, direction) < m_DropAngle)
		{
			m_Character.Drop();
		}
	}

	private void UpdatePath()
	{
		var target = GetTarget();
		m_Seeker.StartPath(transform.position, target); 
	}

	private void OnLaserAttack()
	{
		LeanTween.delayedCall(0.5f, m_LaserGridSequence.Activate);
		m_Animator.SetTrigger("LaserSpell");
	}

	private void OnArrowAttack()
	{
		LeanTween.delayedCall(0.5f, m_ArrowRainSequence.Activate);
		m_Animator.SetTrigger("FireballSpell");
	}

	private void OnBombAttack()
	{
		LeanTween.delayedCall(0.5f, m_BombThrowAttack.Activate);
		m_Character.LookAt(m_Player.transform.position);
		m_Animator.SetTrigger("ThrowBomb");
	}

	private void OnBasicShot()
	{
		m_Animator.SetBool("Aiming", true);
		m_Arm.enabled = true;

		LeanTween.delayedCall(0.1f, () =>
		{
			m_BasicShot.Fire();
			m_Animator.SetBool("Aiming", false);
			m_Arm.enabled = false;
		});
	}

	private void OnChargedShot()
	{
		m_Animator.SetBool("Aiming", true);
		m_Arm.enabled = true;

		m_ChargedShot.Fire();

		LeanTween.delayedCall(1, () => 
		{
			m_Animator.SetBool("Aiming", false);
			m_Arm.enabled = false;
		});
	}

	private void OnDrawGizmos()
	{
		//Gizmos.color = Color.green;
		//Gizmos.DrawWireSphere((Vector3)GetFurthestNode().position, 1);

		if (m_Path.Count == 0)
		{
			return;
		}

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(m_Path.First(), 1);

		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(m_Path.Last(), 1);
	}
}
