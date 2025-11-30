using UnityEngine;

public class Bottle : MonoBehaviour
{
    private const float FALL_ANGLE = 10f; // tilt more than 10� = fallen
    private const float FALL_Y = -0.05f; // below 0 by a bit = fallen

    public bool IsFallen()
    {
        // Check tilt
        float angle = Vector3.Angle(transform.up, Vector3.up);
        bool tiltedOver = angle > FALL_ANGLE;

        // Check if moved downward
        bool fellDown = transform.localPosition.y < FALL_Y;

        return tiltedOver || fellDown;
    }
}
