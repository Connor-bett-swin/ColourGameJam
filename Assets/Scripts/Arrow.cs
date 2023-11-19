using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
	[SerializeField]
	private ColorScheme m_Colors;
	[SerializeField]
	private float m_FadeDelay = 1;
	[SerializeField]
	private float m_FadeDuration = 2;
	[SerializeField]
	private Collider2D m_HitboxCollider;
	[SerializeField]
	private Hitbox m_Hitbox;
	[SerializeField]
	private SpriteRenderer m_Sprite;
	[SerializeField]
	private GameObject m_ExplosionPrefab;
    private Animator m_Animator;

	public int ColorIndex = -1;

	private void Awake()
	{
		m_Animator = GetComponent<Animator>();
	}

	private void Start()
	{
		if (ColorIndex >= 0)
		{
			m_Sprite.color = m_Colors[ColorIndex];
			m_Hitbox.Colored = true;
			m_Hitbox.ColorIndex = ColorIndex;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		Explode();
		m_Animator.SetTrigger("Hit");

		m_HitboxCollider.enabled = false;

		LeanTween.alpha(gameObject, 0, m_FadeDuration)
			.setDelay(m_FadeDelay)
			.setDestroyOnComplete(true);
	}
	private void Explode()
	{
		Instantiate(m_ExplosionPrefab, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}
}
