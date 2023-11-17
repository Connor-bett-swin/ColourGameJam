using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
	[SerializeField]
	private float m_FadeDelay = 1;
	[SerializeField]
	private float m_FadeDuration = 2;
	[SerializeField]
	private Collider2D m_Hitbox;
    private Animator m_Animator;

	private void Awake()
	{
		m_Animator = GetComponent<Animator>();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		m_Animator.SetTrigger("Hit");

		m_Hitbox.enabled = false;

		LeanTween.alpha(gameObject, 0, m_FadeDuration)
			.setDelay(m_FadeDelay)
			.setDestroyOnComplete(true);
	}
}
