using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LaserGridSequence : MonoBehaviour
{
	[SerializeField]
	private Vector2 m_Size;
	[SerializeField]
	private GameObject m_LaserPrefab;

	private void Update()
	{
		if (Keyboard.current.digit2Key.wasPressedThisFrame)
		{
			Activate();
		}
	}

	public void Activate()
	{

	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(transform.position, m_Size);
	}
}
