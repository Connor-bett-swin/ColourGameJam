using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class BasicShot : MonoBehaviour
{
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
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ShootArrow();
        }
    }

    private void ShootArrow()
    {
        BasicShotSFX.Play();
        Vector2 targetPosition = player.transform.position;

        GameObject arrow = Instantiate(SniperArrow, shootPoint.position, Quaternion.identity);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        Vector2 direction = (targetPosition - (Vector2)shootPoint.position).normalized;
        
        // Calculate the angle and rotate the arrow
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + -90;
        arrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        rb.velocity = direction * arrowSpeed;

    }
}
