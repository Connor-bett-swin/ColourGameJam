using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArrowRainSequence : MonoBehaviour
{
	[SerializeField]
	private ColorScheme m_Colors;
	[SerializeField]
	private float m_SpawnFireballsDelay = 1.5f;
	[SerializeField]
	private float m_FireballDensity = 0.5f;
	[SerializeField]
	private float m_Width;
	[SerializeField]
	private float m_Ground;
	[SerializeField]
	private float m_MinGapWidth = 10;
	[SerializeField]
	private float m_SilhouetteFireballScale = 0.75f;
	[SerializeField]
	private GameObject m_SilhouetteFireballPrefab;
	[SerializeField]
	private GameObject m_FireballPrefab;
	private Collider2D m_PlayerCollider;
    public AudioSource AudioWhole;

	private void Awake()
	{
		var player = GameObject.FindGameObjectWithTag("Player");
		m_PlayerCollider = player.GetComponent<Collider2D>();
	}

	private void Update()
	{
		if (Keyboard.current.digit1Key.wasPressedThisFrame)
		{
			Activate();
		}
	}

	private float GetGapWidth()
	{
		return Mathf.Max(m_MinGapWidth, m_PlayerCollider.bounds.size.x + 5);
	}

	public void Activate()
	{
		AudioWhole.Play();

		var gapX = Random.Range(0, m_Width - GetGapWidth());

		Debug.Log(GetGapWidth());

		var sequence = LeanTween.sequence();
		sequence.append(() => SpawnSilhouetteFireballs(gapX));
		sequence.append(m_SpawnFireballsDelay);
		sequence.append(() => SpawnFireballs(gapX));
	}

	private void SpawnSilhouetteFireballs(float gapX)
	{
		var silhouetteFireballs = new GameObject("ShadowFireballs");
		silhouetteFireballs.transform.position = new Vector3(transform.position.x, transform.position.y - m_Ground);

		for (float x = 0; x < m_Width; x += 1 / m_FireballDensity)
		{
			if (x > gapX && x < gapX + GetGapWidth())
			{
				continue;
			}

			var silhouetteFireball = Instantiate(m_SilhouetteFireballPrefab, silhouetteFireballs.transform);
			silhouetteFireball.transform.localPosition = new Vector3((x - m_Width / 2) * m_SilhouetteFireballScale, 0);
		}

		LeanTween.alpha(silhouetteFireballs, 1, 0.25f).setFrom(0);

		LeanTween.moveY(silhouetteFireballs, transform.position.y, m_SpawnFireballsDelay)
			.setEaseOutQuad()
			.setDestroyOnComplete(true);
	}

	private void SpawnFireballs(float gapX)
	{
		var colorIndex = Random.Range(0, m_Colors.Length);

		for (float x = 0; x < m_Width; x += 1 / m_FireballDensity)
		{
			var fireball = Instantiate(m_FireballPrefab).GetComponent<Arrow>();
			fireball.transform.position = new Vector3(transform.position.x + x - m_Width / 2 + Random.Range(-0.5f, 0.5f),
				transform.position.y + Random.Range(-0.5f, 0.5f));
			
			if (x > gapX && x < gapX + GetGapWidth())
			{
				fireball.ColorIndex = colorIndex;
			}
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawLine(transform.position - new Vector3(m_Width / 2, 0), transform.position + new Vector3(m_Width / 2, 0));

		Gizmos.color = Color.blue;
		Gizmos.DrawLine(transform.position + new Vector3(m_SilhouetteFireballScale * -m_Width / 2, -m_Ground),
			transform.position + new Vector3(m_SilhouetteFireballScale * m_Width / 2, -m_Ground));
	}
}
