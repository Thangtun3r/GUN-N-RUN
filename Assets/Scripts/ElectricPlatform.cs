using UnityEngine;

public class ElectricPlatform : MonoBehaviour
{
    public enum MovementDirection { Left, Right } // Enum for movement direction
    public MovementDirection direction = MovementDirection.Right;

    public Transform platform; // Reference to the platform to move
    public float moveLength = 2f;  // Length to move
    public float moveSpeed = 1f;   // Speed of movement
    public bool isMovementEnabled = true; // Bool to enable or disable movement

    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private bool isReversed = false;

    private void OnEnable()
    {
        Movement.OnShoot += ToggleMovement; // Subscribe to the event
        if (platform != null)
        {
            initialPosition = platform.position;
        }
    }

    private void OnDisable()
    {
        Movement.OnShoot -= ToggleMovement; // Unsubscribe from the event
    }

    private void Update()
    {
        if (isMoving && platform != null)
        {
            // Move the platform towards the target position
            platform.position = Vector3.MoveTowards(platform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Stop moving when the target position is reached
            if (platform.position == targetPosition)
            {
                isMoving = false;
            }
        }
    }

    private void ToggleMovement()
    {
        if (!isMovementEnabled || platform == null) return; // Check if movement is enabled

        float movement = moveLength * 2; // Double the movement length
        Vector3 offset = direction == MovementDirection.Right ? Vector3.right : Vector3.left;

        if (isReversed)
        {
            targetPosition = initialPosition; // Move back to the initial position
        }
        else
        {
            targetPosition = initialPosition + offset * movement; // Move in the specified direction
        }

        isReversed = !isReversed; // Toggle the reversed state
        isMoving = true;

        // Toggle the movement direction for the next activation
        direction = direction == MovementDirection.Right ? MovementDirection.Left : MovementDirection.Right;
    }
}