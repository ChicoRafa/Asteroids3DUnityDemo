using UnityEngine;

public class WrapMovement : MonoBehaviour
{
    Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        // Wrap the object around the screen vertically
        Vector3 wrapPosition = Vector3.zero;
        WrapPosition(viewportPosition, wrapPosition);
    }

    private void WrapPosition(Vector3 viewportPosition, Vector3 wrapPosition)
    {
        // Check if the object is outside the screen
        if (viewportPosition.x < -0.05f)
        {
            // add 1.05f to the x position to wrap the object around the screen
            wrapPosition.x += 1.04f;
        }
        else if (viewportPosition.x > 1.05f)
        {
            wrapPosition.x -= 1.04f;
        }

        // Check if the object is outside the screen vertically
        if (viewportPosition.y < -0.1f)
        {
            // add 1.05f to the y position to wrap the object around the screen
            wrapPosition.y += 1.05f;
        }
        else if (viewportPosition.y > 1.1f)
        {
            wrapPosition.y -= 1.05f;
        }

        Vector3 worldPoint = mainCamera.ViewportToWorldPoint(viewportPosition + wrapPosition);
        // Move the object to the new position
        transform.position = worldPoint;
    }
}