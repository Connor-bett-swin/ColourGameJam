using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class LaserGridSequence : MonoBehaviour
{
	[SerializeField]
	private Vector2 m_Size;
	[SerializeField]
	private GameObject m_LaserPrefab;

	private void Update()
	{
		if (Keyboard.current.digit2Key.wasPressedThisFrame)
		{
			Activate();
		}
	}

	public void Activate()
	{
		if (Random.value > 0.2f)
		{
			Sweep(Random.value > 0.5f, Random.value > 0.5f);
		}
		else
		{
			Spin(2, 5, 5);
		}
	}

	private void Sweep(bool horizontal, bool top)
	{
		var laser = Instantiate(m_LaserPrefab).GetComponent<Laser>();
		laser.transform.position = (horizontal ? new Vector2(0, m_Size.y) : new Vector2(m_Size.x, 0)) / 2;
		laser.transform.position *= top ? 1 : -1;
		laser.transform.eulerAngles = new Vector3(0, 0, horizontal ? 0 : 90);
		laser.Separation = (horizontal ? m_Size.x : m_Size.y) / 2;

		var finalPosition = -laser.transform.position;

		var sequence = LeanTween.sequence();
		sequence.append(LeanTween.alpha(laser.gameObject, 1, 0.5f).setFrom(0));
		sequence.append(0.5f);
		sequence.append(() => laser.Activated = true);
		sequence.append(1);

		sequence.append(LeanTween.move(laser.gameObject, finalPosition, 4).setEaseInOutQuad());

		sequence.append(0.5f);
		sequence.append(() => laser.Activated = false);
		sequence.append(LeanTween.alpha(laser.gameObject, 0, 0.5f).setDestroyOnComplete(true));
	}

	private void Spin(int spins, float duration, float offset)
	{
		var laser = Instantiate(m_LaserPrefab).GetComponent<Laser>();
		//laser.transform.position = new Vector3(-offset, 0, 0);
		laser.Separation = Mathf.Min(m_Size.x, m_Size.y) / 2;

		var sequence = LeanTween.sequence();
		sequence.append(LeanTween.alpha(laser.gameObject, 1, 0.5f).setFrom(0));
		sequence.append(0.5f);
		sequence.append(() => laser.Activated = true);
		sequence.append(1);

		sequence.append(LeanTween.rotate(laser.gameObject, new Vector3(0, 0, 360 * spins), duration).setEaseInOutQuad());
		//sequence.append(() =>
		//{
		//	LeanTween.rotate(laser.gameObject, new Vector3(0, 0, 360 * spins), duration).setEaseInOutQuad();
		//	LeanTween.moveX(laser.gameObject, offset, duration / spins).setEaseInOutQuad().setLoopPingPong(spins / 2);
		//});
		//sequence.append(duration);

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
