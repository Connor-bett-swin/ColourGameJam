using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Health : Range
{
    public AudioSource HurtSfx;
    public AudioSource HealSfx;

	public bool InvertDamage;

	public void Hurt(float amount)
	{
		if (InvertDamage)
		{
			amount *= -1;
		}
		
		Debug.Log(Value);
		
		HurtSfx.Play();

		Value -= amount;
		
		if (Value <= 0)
		{
			SceneManager.LoadScene("GameOver");
		}
	}

	public void Heal(float amount)
	{
		HealSfx.Play();
		Value += amount;
	}
}