using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
	public Platform From;
	public Platform To;

	private void OnDrawGizmos()
	{
		if (From == null || To == null)
		{
			return;
		}

		Gizmos.color = Color.green;
		Gizmos.DrawLine(From.transform.position, To.transform.position);
	}
}
