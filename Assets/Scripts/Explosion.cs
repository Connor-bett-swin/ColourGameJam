using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    private CinemachineImpulseSource m_ImpulseSource;

	private void Start()
	{
		m_ImpulseSource.GenerateImpulse();
	}
}
