using UnityEngine;

public class LaserTurret : MonoBehaviour
{
    [Header("Turret Settings")]
    public Transform nozzle;                     
    public Transform muzzlePoint;            
    public float detectionRadius = 5f;         
    public string playerTag = "Player";        
    public float fireRate = 1f;             
    [Tooltip("Rotation speed in degrees/second")]
    public float rotationSpeed = 90f;

    [Header("Laser Settings")]
    public LineRenderer lineRenderer;
    public float maxLaserDistance = 100f;
    public LayerMask hitLayers = default;        

    private float nextDamageTime;

    // Store initial state
    private Quaternion initialNozzleRotation;
    private Quaternion initialTurretRotation;
    private Vector3 initialTurretPosition;

    void Start()
    {
        lineRenderer.enabled = false;

        if (nozzle != null)
            initialNozzleRotation = nozzle.rotation;
        initialTurretRotation = transform.rotation;
        initialTurretPosition = transform.position;
    }

    void OnEnable()
    {
        LosingEvent.onPlayerDeath += ResetTurret;
    }

    void OnDisable()
    {
        LosingEvent.onPlayerDeath -= ResetTurret;
    }

    void Update()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(
            transform.position,
            detectionRadius,
            LayerMask.GetMask(playerTag)
        );

        if (playerCollider != null && playerCollider.CompareTag(playerTag))
        {
            // Smoothly rotate toward player
            Vector2 toPlayer = playerCollider.transform.position - nozzle.position;
            float targetAngle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
            nozzle.rotation = Quaternion.RotateTowards(
                nozzle.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
            
            Vector3 startPos = muzzlePoint.position;
            Vector2 fireDir = nozzle.right;

            // Prepare ContactFilter2D to ignore triggers and filter layers
            ContactFilter2D filter = new ContactFilter2D();
            filter.SetLayerMask(hitLayers);
            filter.useTriggers = false; // Ignores isTrigger colliders

            RaycastHit2D[] hits = new RaycastHit2D[16];
            int hitCount = Physics2D.Raycast(startPos, fireDir, filter, hits, maxLaserDistance);

            Vector3 endPos = startPos + (Vector3)fireDir * maxLaserDistance;

            for (int i = 0; i < hitCount; i++)
            {
                var hit = hits[i];
                if (hit.collider == null) continue;

                // If hit player
                if (hit.collider.CompareTag(playerTag))
                {
                    if (Time.time >= nextDamageTime)
                    {
                        Player player = hit.collider.GetComponent<Player>();
                        if (player != null)
                        {
                            player.Die();
                        }
                        nextDamageTime = Time.time + 1f / fireRate;
                    }
                    // Don't break, keep checking if there's another collider closer
                }

                // Block laser at first valid (non-trigger) collider in hitLayers (including player)
                if (((1 << hit.collider.gameObject.layer) & hitLayers.value) != 0)
                {
                    endPos = hit.point;
                    break;
                }
            }

            if (!lineRenderer.enabled)
                lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, endPos);
        }
        else
        {
            if (lineRenderer.enabled)
                lineRenderer.enabled = false;
        }
    }

    // -- RESET FUNCTION --
    private void ResetTurret()
    {
        // Restore original rotation and position
        if (nozzle != null)
            nozzle.rotation = initialNozzleRotation;
        transform.rotation = initialTurretRotation;
        transform.position = initialTurretPosition;

        // Reset timers, laser, etc
        nextDamageTime = 0f;
        if (lineRenderer != null)
            lineRenderer.enabled = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
