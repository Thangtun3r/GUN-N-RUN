using UnityEngine;
    
    public class Door : MonoBehaviour
    {
        public Transform objectToMove; // Reference to the object to move
        public float moveLength = 2f;  // Length to move down
        public float moveSpeed = 1f;   // Speed of movement
        private Vector3 targetPosition;
        private bool isMoving = false;
    
        private void OnEnable()
        {
            if (objectToMove != null)
            {
                // Calculate the target position for the specified object
                targetPosition = objectToMove.position - new Vector3(0, moveLength, 0);
                isMoving = true;
            }
            else
            {
                Debug.LogWarning("No object assigned to move.");
            }
        }
    
        private void Update()
        {
            if (isMoving && objectToMove != null)
            {
                // Move the specified object towards the target position
                objectToMove.position = Vector3.MoveTowards(objectToMove.position, targetPosition, moveSpeed * Time.deltaTime);
    
                // Stop moving when the target position is reached
                if (objectToMove.position == targetPosition)
                {
                    isMoving = false;
                }
            }
        }
    }