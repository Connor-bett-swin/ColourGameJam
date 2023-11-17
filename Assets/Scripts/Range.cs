using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range : MonoBehaviour
{
    public float MinValue;
    public float MaxValue = 1000;
	public float InitialValue = 100;
	public float Value
    {
        get => m_Value;

		set => m_Value = Mathf.Clamp(value, MinValue, MaxValue);
    }
    public float Percentage => (Value - MinValue) / (MaxValue - MinValue);

    private float m_Value;

	private void Start()
	{
		Value = InitialValue;
	}
}
