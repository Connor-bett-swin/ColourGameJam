using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCoordinator : MonoBehaviour
{
	[SerializeField]
	private Character m_Hero;
	[SerializeField]
	private GameObject m_Player;

	private Vector2 m_PlayerNode;

	private void Update()
	{
		var heroInfo = AstarPath.active.GetNearest(m_Hero.transform.position, NNConstraint.Default);
		var playerInfo = AstarPath.active.GetNearest(m_Player.transform.position, NNConstraint.Default);

		m_PlayerNode = playerInfo.position;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(m_PlayerNode, 1);
	}
}
