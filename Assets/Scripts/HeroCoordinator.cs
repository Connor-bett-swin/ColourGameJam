using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class HeroCoordinator : MonoBehaviour
{
	[SerializeField]
	private Character m_Hero;
	[SerializeField]
	private GameObject m_Player;
	[SerializeField]
	private float m_JumpAngle = 45;
	[SerializeField]
	private float m_DropAngle = 45;

	private Seeker m_Seeker;
	private List<Vector2> m_Path;

	private void Awake()
	{
		m_Seeker = GetComponent<Seeker>();
	}

	private void Start()
	{
		StartCoroutine(Seek());
	}

	private void Update()
	{
		if (m_Path == null || m_Path.Count == 0)
		{
			return;
		}

		var nextNode = m_Path.First();

		if (Vector2.Distance((Vector2)m_Hero.transform.position, nextNode) < 5)
		{
			m_Path.RemoveAt(0);
		}

		if (!m_Hero.Grounded)
		{
			return;
		}

		//m_Hero.Move(nextNode.x - m_Hero.transform.position.x);
		m_Hero.Move(nextNode.x < m_Hero.transform.position.x ? -1 : 1);

		var direction = (nextNode - (Vector2)m_Hero.transform.position).normalized;

		if (Vector2.Angle(Vector2.up, direction) < m_JumpAngle)
		{
			m_Hero.Jump();
		}

		if (Vector2.Angle(Vector2.down, direction) < m_DropAngle)
		{
			m_Hero.Drop();
		}
	}

	private IEnumerator Seek()
	{
		while (true)
		{
			var path = m_Seeker.StartPath(m_Hero.transform.position, m_Player.transform.position);

			yield return StartCoroutine(path.WaitForPath());

			//Debug.Log(path.GetTotalLength());
			m_Path = path.vectorPath.Select(x => (Vector2)x).ToList();

			yield return new WaitForSeconds(0.5f);
		}
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
