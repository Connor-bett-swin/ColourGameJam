using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Tasks.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAttackAction : ActionBase
{
	protected override TaskStatus OnUpdate()
	{
		Owner.SendMessage("OnLaserAttack");

		return TaskStatus.Success;
	}
}
