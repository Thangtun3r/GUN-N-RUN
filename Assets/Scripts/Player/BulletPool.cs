using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [Header("Bullet Pool Settings")]
    public GameObject bulletPrefab;
    public int poolSize = 10;

    private GameObject[] bulletPool;
    private int currentIndex = 0;

    void Start()
    {
        bulletPool = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            bulletPool[i] = Instantiate(bulletPrefab);
            bulletPool[i].SetActive(false);
        }
    }

    public GameObject GetBullet()
    {
        GameObject bullet = bulletPool[currentIndex];
        currentIndex = (currentIndex + 1) % poolSize;
        return bullet;
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
    }
}
