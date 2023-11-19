using CleverCrow.Fluid.BTs.Trees;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BehaviorTreeBuilderExtensions
{
    public static BehaviorTreeBuilder CastLaserAction(this BehaviorTreeBuilder builder, string name = "Cast Laser")
    {
        return builder.AddNode(new CastLaserAction { Name = name });
    }

	public static BehaviorTreeBuilder CastFireballAction(this BehaviorTreeBuilder builder, string name = "Cast Fireball")
	{
		return builder.AddNode(new CastFireballAction { Name = name });
	}

	public static BehaviorTreeBuilder ThrowBombAction(this BehaviorTreeBuilder builder, string name = "Throw Bomb")
	{
		return builder.AddNode(new ThrowBombAction { Name = name });
	}

	public static BehaviorTreeBuilder BasicShotAction(this BehaviorTreeBuilder builder, string name = "Basic Shot")
	{
		return builder.AddNode(new BasicShotAction { Name = name });
	}

	public static BehaviorTreeBuilder ChargedShotAction(this BehaviorTreeBuilder builder, string name = "Charged Shot")
	{
		return builder.AddNode(new ChargedShotAction { Name = name });
	}
}
