using CleverCrow.Fluid.BTs.Trees;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField]
    private BehaviorTree m_BehaviorTree;

	private void Awake()
	{
		m_BehaviorTree = new BehaviorTreeBuilder(gameObject)
			.Build();
	}

	private void Update()
	{
		m_BehaviorTree.Tick();
	}
}
