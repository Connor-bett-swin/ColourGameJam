using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArrowRainSequence : MonoBehaviour
{
	[SerializeField]
	private float m_FireArrowsDelay;
	[SerializeField]
	private float m_ArrowDensity;
	[SerializeField]
	private float m_Width;
	[SerializeField]
	private float m_Ground;
	[SerializeField]
	private float m_GapWidth;
	[SerializeField]
	private GameObject m_ShadowArrowPrefab;
	[SerializeField]
	private GameObject m_ArrowPrefab;

	private void Update()
	{
		if (Keyboard.current.digit1Key.wasPressedThisFrame)
		{
			Fire();
		}
	}

	private void Fire()
	{
		var gapX = Random.Range(0, m_Width - m_GapWidth);

		var sequence = LeanTween.sequence();
		sequence.append(() => FireShadowArrows(gapX));
		sequence.append(m_FireArrowsDelay);
		sequence.append(() => FireArrows(gapX));
	}

	private void FireShadowArrows(float gapX)
	{
		var shadowArrows = new GameObject("ShadowArrows");
		shadowArrows.transform.position = new Vector3(transform.position.x, transform.position.y - m_Ground);

		for (float x = 0; x < m_Width; x += 1 / m_ArrowDensity)
		{
			if (x > gapX && x < gapX + m_GapWidth)
			{
				continue;
			}

			var shadowArrow = Instantiate(m_ShadowArrowPrefab, shadowArrows.transform);
			shadowArrow.transform.localPosition = new Vector3((x - m_Width / 2) * 0.5f, 0);
		}

		LeanTween.moveY(shadowArrows, transform.position.y, m_FireArrowsDelay)
			.setEaseOutQuad()
			.setDestroyOnComplete(true);
	}

	private void FireArrows(float gapX)
	{
		for (float x = 0; x < m_Width; x += 1 / m_ArrowDensity)
		{
			if (x > gapX && x < gapX + m_GapWidth)
			{
				continue;
			}

			var arrow = Instantiate(m_ArrowPrefab);
			arrow.transform.position = new Vector3(transform.position.x + x - m_Width / 2 + Random.Range(-0.5f, 0.5f), transform.position.y);
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawLine(transform.position - new Vector3(m_Width / 2, 0), transform.position + new Vector3(m_Width / 2, 0));

		Gizmos.color = Color.blue;
		Gizmos.DrawLine(transform.position + new Vector3(-m_Width / 2, -m_Ground), transform.position + new Vector3(m_Width / 2, -m_Ground));
	}
}
