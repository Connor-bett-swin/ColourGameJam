using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Tasks.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedShotAction : ActionBase
{
	protected override TaskStatus OnUpdate()
	{
		Owner.SendMessage("OnChargedShot");

		return TaskStatus.Success;
	}
}
