using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class ArrowShooter : MonoBehaviour
{
    public GameObject SniperArrow;
    public LineRenderer warningLine;
    public Transform shootPoint; 
    public float warningDuration = 2f; // Duration for which the warning line is shown
    public float arrowSpeed = 5f; // Speed of the arrow

    private GameObject player;
    private bool isShooting = false; //coroutine toggle
    public AudioSource Charge_Sfx;
    public AudioSource Shot_Sfx;



    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

	private void Update()
	{
        if (isShooting)
        {
            StartCoroutine(ShootAtPlayer());
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && !isShooting)
        {
            Fire();
		}
	}

    public void Fire()
    {
		Charge_Sfx.Play();
		isShooting = true;
		StartCoroutine(ShootArrow());
	}

    private IEnumerator ShootAtPlayer()
    {
        Shot_Sfx.Play();
        ShowWarningLine(player.transform.position);
        Vector2 targetPosition = player.transform.position;

        yield return new WaitForSeconds(warningDuration);

        HideWarningLine();
        //ShootArrow(targetPosition);
        isShooting = false;
    }
    private IEnumerator ShootArrow()
    {
        yield return new WaitForSeconds(warningDuration);
        Vector2 targetPosition = player.transform.position;

        GameObject arrow = Instantiate(SniperArrow, shootPoint.position, Quaternion.identity);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        Vector2 direction = (targetPosition - (Vector2)shootPoint.position).normalized;
        
        // Calculate the angle and rotate the arrow
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + -90;
        arrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        rb.velocity = direction * arrowSpeed;
    }

    private void ShowWarningLine(Vector2 targetPosition)
    {
        warningLine.enabled = true;
        warningLine.SetPosition(0, shootPoint.position);
        warningLine.SetPosition(1, targetPosition);
    }

    private void HideWarningLine()
    {
        warningLine.enabled = false;
    }
}
