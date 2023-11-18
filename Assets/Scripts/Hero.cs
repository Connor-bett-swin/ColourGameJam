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
	private Animator m_Animator;
	[SerializeField]
	private Collider2D m_Collider;
	[SerializeField]
	private ArrowRainSequence m_ArrowRainSequence;
	[SerializeField]
	private LaserGridSequence m_LaserGridSequence;
	private GameObject m_Player;
	private Health m_PlayerHealth;
	private Seeker m_Seeker;
	private Character m_Character;
	private List<Vector2> m_Path = new List<Vector2>();
	private Vector2 m_Target;
	private float m_JumpCooldownTimer;

	private void Awake()
	{
		m_BehaviorTree = new BehaviorTreeBuilder(gameObject)
			.Sequence()
				.WaitTime(Random.Range(3, 6))
				.SelectorRandom()
					.ThrowBombAction()
					.LaserAttackAction()
					.ArrowAttackAction()
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

		//StartCoroutine(Seek());
	}

	//private PointNode GetFurthestNode()
	//{
	//	var pathNodes = AstarPath.active.data.pointGraph.nodes;
	//	return pathNodes.OrderByDescending(x => Vector2.Distance((Vector3)x.position, m_Player.transform.position)).First();
	//}

	private PointNode GetFurthestNode()
	{
		var pathNodes = AstarPath.active.data.pointGraph.nodes;
		return pathNodes.OrderByDescending(x => Mathf.Abs(((Vector3)x.position).x - m_Player.transform.position.x) - 3.5f * Mathf.Abs(((Vector3)x.position).y - m_Player.transform.position.y)).First();
	}

	private void Update()
	{
		//m_BehaviorTree.Tick();

		UpdateMovement();

		if (m_JumpCooldownTimer > 0)
		{
			m_JumpCooldownTimer -= Time.deltaTime;
		}
	}

	private void UpdateMovement()
	{
		if (m_Path.Count == 0)
		{
			var pathNodes = AstarPath.active.data.pointGraph.nodes;
			m_Target = (Vector3)pathNodes[Random.Range(0, pathNodes.Length)].position;
			return;
		}

		var nextNode = m_Path.First();

		if (m_Character.Grounded && m_Collider.OverlapPoint(nextNode))
		{
			m_Path.RemoveAt(0);
		}

		if (m_Character.Grounded)
		{
			m_Character.Move(nextNode.x < m_Character.transform.position.x ? -1 : 1);
		}
		else
		{
			m_Character.Move(nextNode.x - m_Character.transform.position.x);
		}

		var direction = (nextNode - (Vector2)m_Character.transform.position).normalized;

		if (Vector2.Distance(nextNode, m_Character.transform.position) < 1.5f)
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
		m_Seeker.StartPath(transform.position, m_Target); 
	}

	private void OnLaserAttack()
	{
		m_LaserGridSequence.Activate();
	}

	private void OnArrowAttack()
	{
		m_ArrowRainSequence.Activate();
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
