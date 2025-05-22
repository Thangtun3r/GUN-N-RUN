using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Vector3 checkpointPosition;
    public Transform checkpointTransform;
    public GameObject door;
 
    
    private void OnEnable()
    {
        LosingEvent.onPlayerDeath += ResetCheckpoint;
    }
    private void OnDisable()
    {
        LosingEvent.onPlayerDeath -= ResetCheckpoint;
    }


    void ResetCheckpoint()
    {
        
    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            checkpointPosition = checkpointTransform.position;
            other.GetComponent<ICheckpoint>()?.SaveCheckpoint(checkpointPosition);
            Debug.Log("Checkpoint saved at: " + checkpointPosition);
            
            door.SetActive(true);
        }
    }
}
