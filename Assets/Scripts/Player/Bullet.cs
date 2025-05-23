using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed = 10f;
    private Vector2 direction;
    private Collider2D bulletCollider;
    private BulletPool pool;

    [Header("Particle Settings")]
    public BulletParticlePool particlePool; // Assign this in Inspector or via script

    void Awake()
    {
        bulletCollider = GetComponent<Collider2D>();
        // Optionally auto-find the particle pool if not set
        if (particlePool == null)
            particlePool = FindObjectOfType<BulletParticlePool>();
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    public void SetPool(BulletPool bulletPool)
    {
        pool = bulletPool;
    }

    public void SetParticlePool(BulletParticlePool particlePool)
    {
        this.particlePool = particlePool;
    }

    void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Spawn particle effect at collision point
        if (particlePool != null)
        {
            // Get contact point (for more precise collision, otherwise use transform.position)
            Vector3 hitPoint = other.ClosestPoint(transform.position);
            particlePool.PlayParticle(hitPoint);
        }

        if (pool != null)
            pool.ReturnBullet(gameObject);
        else
            gameObject.SetActive(false);
    }
}