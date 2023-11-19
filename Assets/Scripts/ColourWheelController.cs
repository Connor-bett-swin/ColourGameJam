using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourWheelController : MonoBehaviour
{
    public GameObject colorWheel; // Assign the color wheel GameObject in the Inspector
    public BossController bossController; // Assign the BossController script here
    public float rotationDuration = 0.5f; // Duration of the rotation animation

    private RectTransform colorWheelRectTransform;
    private int previousColorIndex = -1; // To track color index changes

    private void Start()
    {
        if (colorWheel != null)
        {
            colorWheelRectTransform = colorWheel.GetComponent<RectTransform>();
        }

        if (bossController == null)
        {
            Debug.LogError("BossController is not assigned!");
            return;
        }
    }

    private void Update()
    {
        int currentColorIndex = bossController.ColorIndex;
        if (currentColorIndex != previousColorIndex)
        {
            RotateColorWheel(currentColorIndex);
            previousColorIndex = currentColorIndex;
        }
    }

    private void RotateColorWheel(int colorIndex)
    {
        float rotationAngle = CalculateRotationAngle(colorIndex);
        LeanTween.rotateZ(colorWheel, rotationAngle, rotationDuration)
            .setEase(LeanTweenType.easeInOutCubic);
    }

    private float CalculateRotationAngle(int colorIndex)
    {
        // Assuming you have 3 colors and the wheel should rotate 120 degrees per color
        return colorIndex * -120f; 
    }
}
