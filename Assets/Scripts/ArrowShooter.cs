using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class ArrowShooter : MonoBehaviour
{
    public GameObject SniperArrow;
    public LineRenderer warningLine; // I HAVENT ATTACHED THE LINE COMPONENT YET*******
    public Transform shootPoint; 
    public float warningDuration = 2f; // Duration for which the warning line is shown
    public float arrowSpeed = 5f; // Speed of the arrow

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

	private void Update()
	{
		if (Keyboard.current.digit3Key.wasPressedThisFrame)
		{
			StartCoroutine(ShootAtPlayer());
		}
	}

    private IEnumerator ShootAtPlayer()
    {
        Vector2 targetPosition = player.transform.position;
        ShowWarningLine(targetPosition);

        yield return new WaitForSeconds(warningDuration);

        HideWarningLine();
        ShootArrow(targetPosition);
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

    private void ShootArrow(Vector2 targetPosition)
    {
        GameObject arrow = Instantiate(SniperArrow, shootPoint.position, Quaternion.identity);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        Vector2 direction = (targetPosition - (Vector2)shootPoint.position).normalized;
        rb.velocity = direction * arrowSpeed;
    }
}
