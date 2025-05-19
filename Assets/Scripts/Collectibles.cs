using System;
using System.Collections;
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

    private Coroutine collectTimerCoroutine; // <--- Store coroutine reference

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

            // Start the timer only once
            if (collectTimerCoroutine == null)
                collectTimerCoroutine = StartCoroutine(StartTime());
        }
    }

    private void Update()
    {
        if (!isCollecting || playerPosition == null) return;
        FollowPlayer();
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
        Destroy(gameObject);
    }

    private void ResetCollectible()
    {
        if (!isCollecting) return;

        if (collectTimerCoroutine != null)
        {
            StopCoroutine(collectTimerCoroutine);
            collectTimerCoroutine = null;
        }

        isCollecting = false;
        playerPosition = null;
        transform.position = initialPosition;
    }

    IEnumerator StartTime()
    {
        float timer = 0f;
        float requiredTime = 0.5f;

        while (true)
        {
            if (Movement.isOnSafeGround)
            {
                timer += Time.deltaTime;
                if (timer >= requiredTime)
                {
                    ConfirmCollect();
                    yield break;
                }
            }
            else
            {
                timer = 0f; // Reset if not on safe ground
            }

            yield return null;
        }
    }
}
