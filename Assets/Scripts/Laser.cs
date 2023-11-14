using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
	[SerializeField]
	private ColorScheme m_Colors;
	[SerializeField]
	private SpriteRenderer m_LeftEmitter;
	[SerializeField]
	private SpriteRenderer m_RightEmitter;
	[SerializeField]
	private LineRenderer m_Beam;
	[SerializeField]
	private BoxCollider2D m_Collider;

	public int ColorIndex;
	public bool Activated;
	public float Separation = 10;

	private void Start()
	{
		m_Beam.startColor = m_Colors[ColorIndex];
		m_Beam.endColor = m_Colors[ColorIndex];
		m_Beam.SetPosition(0, new Vector3(-Separation, 0, 0));
		m_Beam.SetPosition(1, new Vector3(Separation, 0, 0));

		m_LeftEmitter.transform.localPosition = new Vector3(-Separation, 0, 0);
		m_RightEmitter.transform.localPosition = new Vector3(Separation, 0, 0);

		m_Collider.size = new Vector2(Separation * 2, m_Collider.size.y);
	}
}
