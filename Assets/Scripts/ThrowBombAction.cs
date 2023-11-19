using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Tasks.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBombAction : ActionBase
{
	protected override TaskStatus OnUpdate()
	{
		//var bombThrow = Owner.GetComponent<BombThrowAttack>();

		//bombThrow.Activate();

		Owner.SendMessage("OnBombAttack");

		return TaskStatus.Success;
	}
}
