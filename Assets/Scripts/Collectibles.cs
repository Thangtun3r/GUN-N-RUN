using System;
using UnityEngine;

public class Collectibles : MonoBehaviour
{
    private Transform playerPosition;
    private bool isCollecting = false;

    private Vector3 initialPosition;
    private Vector3 velocity = Vector3.zero;

    [Header("Follow Settings")]
    public Vector3 followOffset = new Vector3(0.5f, 1f, 0f);
    public float followSpeed = 5f;

    private void Awake()
    {
        initialPosition = transform.position;
    }

    private void OnEnable()
    {
        LosingEvent.onPlayerDeath += ResetCollectible;
    }

    private void OnDisable()
    {
        LosingEvent.onPlayerDeath -= ResetCollectible;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCollecting)
        {
            Debug.Log("Collectible collected!");
            playerPosition = other.transform;
            isCollecting = true;
        }
    }

    private void Update()
    {
        if (!isCollecting || playerPosition == null) return;

        FollowPlayer();

        if (Movement.isOnSafeGround)
        {
            ConfirmCollect();
        }
    }

    private void FollowPlayer()
    {
        Vector3 targetPos = playerPosition.position + followOffset;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPos,
            ref velocity,
            1f / followSpeed
        );
    }

    private void ConfirmCollect()
    {
        Debug.Log("Collectible confirmed!");
        Destroy(gameObject);

        // Optional: Add to score, play sound, spawn particles, etc.
    }

    private void ResetCollectible()
    {
        if (!isCollecting) return;

        Debug.Log("Player died — collectible reset.");
        isCollecting = false;
        playerPosition = null;

        // Snap back or animate to original position
        transform.position = initialPosition;

        // Optional: Reset visuals/sound here
    }
}