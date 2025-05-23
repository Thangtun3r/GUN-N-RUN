using UnityEngine;

public class BulletParticlePool : MonoBehaviour
{
    [Header("Particle Pool Settings")]
    public GameObject particlePrefab;
    public int poolSize = 10;

    private GameObject[] particlePool;
    private int currentIndex = 0;

    void Start()
    {
        particlePool = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            particlePool[i] = Instantiate(particlePrefab, transform);
            particlePool[i].SetActive(false);
        }
    }

    public void PlayParticle(Vector3 position)
    {
        GameObject particle = particlePool[currentIndex];
        particle.transform.position = position;
        particle.SetActive(true);

        var ps = particle.GetComponent<ParticleSystem>();
        if (ps)
        {
            ps.Play();
            StartCoroutine(DeactivateAfterSeconds(particle, ps.main.duration));
        }
        else
        {
            // fallback if not using ParticleSystem
            StartCoroutine(DeactivateAfterSeconds(particle, 1f));
        }

        currentIndex = (currentIndex + 1) % poolSize;
    }

    private System.Collections.IEnumerator DeactivateAfterSeconds(GameObject obj, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        obj.SetActive(false);
    }
}