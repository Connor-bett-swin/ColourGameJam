using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Tasks.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicShotAction : ActionBase
{
	protected override TaskStatus OnUpdate()
	{
		Owner.SendMessage("OnBasicShot");

		return TaskStatus.Success;
	}
}
