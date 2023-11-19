using CleverCrow.Fluid.BTs.Decorators;
using CleverCrow.Fluid.BTs.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repeat : DecoratorBase
{
	public override string IconPath { get; } = $"{PACKAGE_ROOT}/Repeat.png";

	public int Times;

	private int m_Count;

	protected override TaskStatus OnUpdate()
	{
		return Child.Update();
	}
}
