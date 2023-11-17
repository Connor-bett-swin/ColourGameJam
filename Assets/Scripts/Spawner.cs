using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	[SerializeField]
	private float m_StartDelay = 3;
    [SerializeField]
    private float m_MinInterval = 4;
	[SerializeField]
	private float m_MaxInterval = 8;
	[SerializeField]
	private int m_MaxCount = 15;
	[SerializeField]
    private GameObject m_SpawnPrefab;
	private List<GameObject> m_Spawned = new List<GameObject>();

	private void Start()
	{
		StartCoroutine(Spawn());
	}

	private IEnumerator Spawn()
	{
		yield return new WaitForSeconds(m_StartDelay);

		while (true)
		{
			yield return new WaitUntil(() => m_Spawned.Count(x => x != null) < m_MaxCount);

			yield return new WaitForSeconds(Random.Range(m_MinInterval, m_MaxInterval));

			var spawn = Instantiate(m_SpawnPrefab, transform.position, Quaternion.identity);
			m_Spawned.Add(spawn);
		}
	}
}
