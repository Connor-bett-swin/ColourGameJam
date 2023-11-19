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
	private SpriteRenderer m_LeftEmitter;
	[SerializeField]
	private SpriteRenderer m_RightEmitter;
	[SerializeField]
	private LineRenderer m_Beam;
	[SerializeField]
	private Hitbox m_Hitbox;
	private BoxCollider2D m_HitboxCollider;

	public int ColorIndex;
	public float Separation = 10;
	public bool Activated;

	private void Awake()
	{
		m_HitboxCollider = m_Hitbox.GetComponent<BoxCollider2D>();
	}

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

		m_LeftEmitter.color = m_Colors[ColorIndex];
		m_RightEmitter.color = m_Colors[ColorIndex];

		m_Hitbox.ColorIndex = ColorIndex;

		m_HitboxCollider.size = new Vector2(Separation * 2, m_HitboxCollider.size.y);
	}

	private void Update()
	{
		m_Beam.enabled = Activated;
		m_HitboxCollider.enabled = Activated;
	}
}
