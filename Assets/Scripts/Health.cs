using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Health : Range
{
	public void Hurt(float amount)
	{
		Debug.Log(Value);

		Value -= amount;
		
		if (Value <= 0)
		{
			SceneManager.LoadScene("GameOver");
		}
	}

	public void Heal(float amount)
	{
		Value += amount;
	}
}