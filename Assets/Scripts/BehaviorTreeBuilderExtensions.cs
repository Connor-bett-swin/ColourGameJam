using CleverCrow.Fluid.BTs.Trees;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BehaviorTreeBuilderExtensions
{
    public static BehaviorTreeBuilder LaserAttackAction(this BehaviorTreeBuilder builder, string name = "Laser Attack")
    {
        return builder.AddNode(new LaserAttackAction { Name = name });
    }

	public static BehaviorTreeBuilder ArrowAttackAction(this BehaviorTreeBuilder builder, string name = "Laser Attack")
	{
		return builder.AddNode(new ArrowAttackAction { Name = name });
	}

	public static BehaviorTreeBuilder ThrowBombAction(this BehaviorTreeBuilder builder, string name = "Laser Attack")
	{
		return builder.AddNode(new ThrowBombAction { Name = name });
	}

	public static BehaviorTreeBuilder FireAction(this BehaviorTreeBuilder builder, string name = "Laser Attack")
	{
		return builder.AddNode(new ArrowAttackAction { Name = name });
	}
}
