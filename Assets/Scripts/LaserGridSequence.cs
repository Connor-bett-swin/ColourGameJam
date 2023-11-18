using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LaserGridSequence : MonoBehaviour
{
	[SerializeField]
	private float m_SpinChance = 0.2f;
	[SerializeField]
	private int m_MinSpins = 2;
	[SerializeField]
	private int m_MaxSpins = 2;
	[SerializeField]
	private float m_SpinTime = 2.5f;
	[SerializeField]
	private Vector2 m_Size;
	[SerializeField]
	private GameObject m_LaserPrefab;
    public AudioSource LaserStart;


	private void Update()
	{
		if (Keyboard.current.digit2Key.wasPressedThisFrame)
		{
			Activate();
		}
	}

	public void Activate()
	{
		LaserStart.Play();
		if (Random.value > m_SpinChance)
		{
			Sweep(Random.value > 0.5f, Random.value > 0.5f);
		}
		else
		{
			Spin(Random.Range(m_MinSpins, m_MaxSpins), m_SpinTime);
		}
	}

	private void Sweep(bool horizontal, bool top)
	{
		var laser = Instantiate(m_LaserPrefab, transform).GetComponent<Laser>();
		laser.transform.localPosition = (horizontal ? new Vector2(0, m_Size.y) : new Vector2(m_Size.x, 0)) / 2;
		laser.transform.localPosition *= top ? 1 : -1;
		laser.transform.eulerAngles = new Vector3(0, 0, horizontal ? 0 : 90);
		laser.Separation = (horizontal ? m_Size.x : m_Size.y) / 2;

		var finalPosition = -laser.transform.localPosition;

		var sequence = LeanTween.sequence();
		sequence.append(LeanTween.alpha(laser.gameObject, 1, 0.5f).setFrom(0));
		sequence.append(0.5f);
		sequence.append(() => laser.Activated = true);
		sequence.append(1);

		sequence.append(LeanTween.moveLocal(laser.gameObject, finalPosition, 4).setEaseInOutQuad());

		sequence.append(0.5f);
		sequence.append(() => laser.Activated = false);
		sequence.append(LeanTween.alpha(laser.gameObject, 0, 0.5f).setDestroyOnComplete(true));
	}

	private void Spin(int spins, float revolutionTime)
	{
		var laser = Instantiate(m_LaserPrefab, transform).GetComponent<Laser>();
		laser.Separation = Mathf.Min(m_Size.x, m_Size.y) / 2;

		var sequence = LeanTween.sequence();
		sequence.append(LeanTween.alpha(laser.gameObject, 1, 0.5f).setFrom(0));
		sequence.append(0.5f);
		sequence.append(() => laser.Activated = true);
		sequence.append(1);

		sequence.append(LeanTween.rotateLocal(laser.gameObject, new Vector3(0, 0, 360 * spins), spins * revolutionTime).setEaseInOutQuad());

		sequence.append(0.5f);
		sequence.append(() => laser.Activated = false);
		sequence.append(LeanTween.alpha(laser.gameObject, 0, 0.5f).setDestroyOnComplete(true));
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(transform.position, m_Size);
	}
}
