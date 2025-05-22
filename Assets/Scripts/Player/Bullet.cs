using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    private Vector2 direction;
    private Collider2D bulletCollider;
    private BulletPool pool;

    void Awake()
    {
        bulletCollider = GetComponent<Collider2D>();
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    public void SetPool(BulletPool bulletPool)
    {
        pool = bulletPool;
    }

    void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (pool != null)
            pool.ReturnBullet(gameObject);
        else
            gameObject.SetActive(false);
    }

}