using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : Range
{
	public void Hurt(float amount)
	{
		Debug.Log(Value);

		Value -= amount;
	}

	public void Heal(float amount)
	{
		Value += amount;
	}
}