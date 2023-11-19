using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class BasicShot : MonoBehaviour
{
	[SerializeField]
	private ColorScheme m_Colors;
	public GameObject SniperArrow;
    public Transform shootPoint; 
    public float arrowSpeed = 5f; // Speed of the arrow

    private GameObject player;
    public AudioSource BasicShotSFX;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
    }

    void Update()
    {
#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.Alpha4))
        {
			Fire();
        }
#endif
    }

    public void Fire()
    {
		var colorIndex = -1;

		if (Random.value > 0.2f)
		{
			colorIndex = Random.Range(0, m_Colors.Length);
		}

		BasicShotSFX.Play();
        Vector2 targetPosition = player.transform.position;

        GameObject arrow = Instantiate(SniperArrow, shootPoint.position, Quaternion.identity);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        Vector2 direction = (targetPosition - (Vector2)shootPoint.position).normalized;

		var colorized = arrow.GetComponent<Colorized>();
		colorized.ColorIndex = colorIndex;

        var hitbox = arrow.GetComponentInChildren<Hitbox>();
        hitbox.ColorIndex = colorIndex;

		// Calculate the angle and rotate the arrow
		float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + -90;
        arrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        rb.velocity = direction * arrowSpeed;

    }
}
