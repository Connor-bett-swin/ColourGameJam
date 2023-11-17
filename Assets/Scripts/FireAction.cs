using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Tasks.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAction : ActionBase
{
	protected override TaskStatus OnUpdate()
	{
		return TaskStatus.Success;
	}
}
