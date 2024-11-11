using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ForwardContinuousMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 10f;
    private float velocityMultiplier = 100f;
    private Rigidbody gameObjectRigidbody;
    private Camera mainCamera;

    void Start()
    {
        gameObjectRigidbody = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Check if the object is outside the screen, if not, the OnOffScreen method is called
        if (IsOffScreen())
        {
            OnOffScreen();
        }
    }

    private void FixedUpdate()
    {
        //moves the bullet in the direction of the input at a constant speed
        Vector3 newVelocity = gameObjectRigidbody.transform.forward *
                              (movementSpeed * Time.fixedDeltaTime * velocityMultiplier);
        //rotates the bullet to face the mouse
        gameObjectRigidbody.linearVelocity = newVelocity;
    }

    private bool IsOffScreen()
    {
        /* Convert the object's world position to viewport coordinates.
        In viewport coordinates:
        (0,0) represents the bottom-left corner of the screen,
        (1,1) represents the top-right corner.*/
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        /* Returns true if the object is outside the screen boundaries
        with a small margin (-0.1 to 1.1) for X and Y axes.
        This margin allows us to detect when the object is slightly out of view
        before it completely disappears from the screen.*/
        return screenPoint.x < -0.1f || screenPoint.x > 1.1f || screenPoint.y < -0.1f || screenPoint.y > 1.1f;
    }

    /*
     * This method is called when the bullet goes off-screen, it will be overriden by the bullet movement script.
     */
    protected virtual void OnOffScreen()
    {
        Debug.Log("Off Screen");
    }
}