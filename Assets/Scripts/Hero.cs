using CleverCrow.Fluid.BTs.Trees;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField]
    private BehaviorTree m_BehaviorTree;
	[SerializeField]
	private float m_JumpAngle = 45;
	[SerializeField]
	private float m_DropAngle = 45;
	[SerializeField]
	private Animator m_Animator;
	[SerializeField]
	private ArrowRainSequence m_ArrowRainSequence;
	[SerializeField]
	private LaserGridSequence m_LaserGridSequence;
	private GameObject m_Player;
	private Seeker m_Seeker;
	private Character m_Character;
	private List<Vector2> m_Path;

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
		m_Character = GetComponent<Character>();
	}

	private void Start()
	{
		m_Player = GameObject.FindGameObjectWithTag("Player");

		StartCoroutine(Seek());
	}

	private void Update()
	{
		m_BehaviorTree.Tick();

		m_Character.Move(0);

		if (m_Path == null || m_Path.Count == 0)
		{
			return;
		}

		var nextNode = m_Path.First();

		if (Vector2.Distance((Vector2)transform.position, nextNode) < 5)
		{
			m_Path.RemoveAt(0);
		}

		if (!m_Character.Grounded)
		{
			return;
		}

		//m_Hero.Move(nextNode.x - m_Hero.transform.position.x);
		m_Character.Move(nextNode.x < transform.position.x ? -1 : 1);

		var direction = (nextNode - (Vector2)transform.position).normalized;

		if (Vector2.Angle(Vector2.up, direction) < m_JumpAngle)
		{
			m_Character.Jump();
		}

		if (Vector2.Angle(Vector2.down, direction) < m_DropAngle)
		{
			m_Character.Drop();
		}
	}

	private IEnumerator Seek()
	{
		while (true)
		{
			var path = m_Seeker.StartPath(transform.position, m_Player.transform.position);

			yield return StartCoroutine(path.WaitForPath());

			m_Path = path.vectorPath.Select(x => (Vector2)x).ToList();

			yield return new WaitForSeconds(0.5f);
		}
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
		if (m_Path == null || m_Path.Count == 0)
		{
			return;
		}

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(m_Path.First(), 1);
	}
}
