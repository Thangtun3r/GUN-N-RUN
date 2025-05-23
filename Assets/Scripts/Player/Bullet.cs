using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed = 10f;
    private Vector2 direction;
    private BulletPool pool;
    private Rigidbody2D rb;

    [Header("Particle Settings")]
    public BulletParticlePool particlePool; // Assign in Inspector or via script
    public LayerMask hitMask; // Assign to Tilemap/Solid layer in Inspector if you want raycast backup

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (particlePool == null)
            particlePool = FindObjectOfType<BulletParticlePool>();
    }

    public void SetDirection(Vector2 dir) => direction = dir.normalized;
    public void SetPool(BulletPool bulletPool) => pool = bulletPool;
    public void SetParticlePool(BulletParticlePool pool) => particlePool = pool;

    void FixedUpdate()
    {
        // Optional: Raycast backup. Remove if you don't want it.
        Vector2 moveDelta = direction * speed * Time.fixedDeltaTime;
        RaycastHit2D hit = Physics2D.Raycast(rb.position, direction, moveDelta.magnitude, hitMask);
        if (hit.collider != null)
        {
            if (particlePool != null) particlePool.PlayParticle(hit.point);
            if (pool != null) pool.ReturnBullet(gameObject);
            else gameObject.SetActive(false);
        }
        else
        {
            rb.MovePosition(rb.position + moveDelta);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (particlePool != null)
        {
            Vector3 hitPoint = other.ClosestPoint(transform.position);
            particlePool.PlayParticle(hitPoint);
        }

        if (pool != null)
            pool.ReturnBullet(gameObject);
        else
            gameObject.SetActive(false);
    }
}