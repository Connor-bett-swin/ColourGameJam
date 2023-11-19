using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bomb : MonoBehaviour
{
	public UnityEvent Exploded;

	[SerializeField]
	private float m_FuseTime;
	[SerializeField]
	private Collider2D m_Collider;
	[SerializeField]
	private GameObject m_ExplosionPrefab;
    public AudioSource ThrowSfx;
	

	private void Start()
	{
		//m_Collider.excludeLayers = LayerMask.GetMask("Platform");
		//LeanTween.delayedCall(m_FuseTime / 3, () => m_Collider.excludeLayers = 0);

		LeanTween.delayedCall(m_FuseTime, Explode);
		ThrowSfx.Play();
	}

	private void Explode()
	{
		Exploded?.Invoke();
		Instantiate(m_ExplosionPrefab, transform.position, Quaternion.identity);
		Destroy(gameObject);
		

	}
}
