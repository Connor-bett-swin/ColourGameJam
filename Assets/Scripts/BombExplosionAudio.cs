using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosionAudio : MonoBehaviour
{
    public AudioSource ExplodeSfx;

    // Start is called before the first frame update
    void Start()
    {
		ExplodeSfx.Play();
        
    }
}
