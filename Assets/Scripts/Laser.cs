using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
	[SerializeField]
	private ColorScheme m_Colors;
	[SerializeField]
	private Material[] m_Materials;
	[SerializeField]
	private GameObject m_LeftEmitter;
	[SerializeField]
	private GameObject m_RightEmitter;
	[SerializeField]
	private SpriteRenderer m_LeftEmitterGlass;
	[SerializeField]
	private SpriteRenderer m_RightEmitterGlass;
	[SerializeField]
	private LineRenderer m_Beam;
	[SerializeField]
	private BoxCollider2D m_Collider;

	public int ColorIndex;
	public float Separation = 10;
	public bool Activated;

	private void Start()
	{
		ColorIndex = Random.Range(0, m_Colors.Length);

		m_Beam.startColor = m_Colors[ColorIndex];
		m_Beam.endColor = m_Colors[ColorIndex];

		//m_Beam.material = m_Materials[ColorIndex];

		m_Beam.SetPosition(0, new Vector3(-Separation, 0, 0));
		m_Beam.SetPosition(1, new Vector3(Separation, 0, 0));

		m_LeftEmitter.transform.localPosition = new Vector3(-Separation, 0, 0);
		m_RightEmitter.transform.localPosition = new Vector3(Separation, 0, 0);

		m_LeftEmitterGlass.color = m_Colors[ColorIndex];
		m_RightEmitterGlass.color = m_Colors[ColorIndex];

		m_Collider.size = new Vector2(Separation * 2, m_Collider.size.y);
	}

	private void Update()
	{
		m_Beam.enabled = Activated;
		m_Collider.enabled = Activated;
	}
}
