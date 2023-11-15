using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float Width;

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position + new Vector3(-Width / 2, 0, -5), transform.position + new Vector3(Width / 2, 0, -5));
	}
}
