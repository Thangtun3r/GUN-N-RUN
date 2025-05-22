using UnityEngine;

public class LaserTurret : MonoBehaviour
{
    [Header("Turret Settings")]
    public Transform nozzle;                     // Rotating part
    public Transform muzzlePoint;               // Tip of the turret (child of nozzle)
    public float detectionRadius = 5f;           // Radius to detect player
    public string playerTag = "Player";          // Player tag
    public float fireRate = 1f;                  // Damage per second
    [Tooltip("Rotation speed in degrees/second")]
    public float rotationSpeed = 90f;

    [Header("Laser Settings")]
    public LineRenderer lineRenderer;
    public float maxLaserDistance = 100f;
    public LayerMask hitLayers = default;        // Should be set to "Default" only

    private float nextDamageTime;

    void Start()
    {
        lineRenderer.enabled = false;
    }

    void Update()
    {
        // Check for player within detection radius
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

            // Fire direction from nozzle's right (local X+)
            Vector3 startPos = muzzlePoint.position;
            Vector2 fireDir = nozzle.right;

            // Show laser
            if (!lineRenderer.enabled)
                lineRenderer.enabled = true;

            // Raycast through all hits
            RaycastHit2D[] hits = Physics2D.RaycastAll(startPos, fireDir, maxLaserDistance);
            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            Vector3 endPos = startPos + (Vector3)fireDir * maxLaserDistance;

            foreach (var hit in hits)
            {
                if (hit.collider == null) continue;

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
                    continue;
                }



                // Hit a real obstacle → stop here
                if (((1 << hit.collider.gameObject.layer) & hitLayers) != 0)
                {
                    endPos = hit.point;
                    break;
                }
            }

            // Update beam
            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, endPos);
        }
        else
        {
            if (lineRenderer.enabled)
                lineRenderer.enabled = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
