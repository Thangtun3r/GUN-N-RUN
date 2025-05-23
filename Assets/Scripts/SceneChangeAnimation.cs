using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpriteCyclerWithEnd : MonoBehaviour
{
    [Header("List of Sprites to Cycle Through (in order)")]
    public List<Sprite> sprites;

    [Header("Time (seconds) between each sprite swap")]
    public float interval = 0.5f;

    [Header("Total time to cycle before ending")]
    public float endTime = 3f;

    [Header("Object to enable after cycling")]
    public GameObject objectToEnable;

    [Header("Wait time after enabling object before switching scene")]
    public float waitAfterEnd = 2f;

    [Header("Scene to load after wait")]
    public string sceneToLoad = "NextScene";

    private SpriteRenderer spriteRenderer;
    private int currentIndex = 0;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (sprites != null && sprites.Count > 0 && spriteRenderer != null)
        {
            StartCoroutine(CycleSprites());
        }
    }

    IEnumerator CycleSprites()
    {
        float timer = 0f;

        while (timer < endTime)
        {
            spriteRenderer.sprite = sprites[currentIndex];
            currentIndex = (currentIndex + 1) % sprites.Count;

            float delay = Mathf.Min(interval, endTime - timer); // Prevents overshooting endTime
            yield return new WaitForSeconds(delay);
            timer += delay;
        }

        // End: Enable object
        if (objectToEnable != null)
        {
            objectToEnable.SetActive(true);
        }

        // Wait before switching scene
        yield return new WaitForSeconds(waitAfterEnd);

        // Switch scene
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}