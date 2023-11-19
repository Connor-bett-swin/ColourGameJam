using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Tasks.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastFireballAction : ActionBase
{
	protected override TaskStatus OnUpdate()
	{
		Owner.SendMessage("OnArrowAttack");

		return TaskStatus.Success;
	}
}
