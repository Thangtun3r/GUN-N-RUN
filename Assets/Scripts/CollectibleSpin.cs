using UnityEngine;

public class CollectibleSpin : MonoBehaviour
{
    [Header("Spin Settings")]
    public float spinSpeed = 90f; // degrees per second

    [Header("Float Settings")]
    public float floatAmplitude = 0.25f; // how high it floats
    public float floatFrequency = 1f;    // how fast it floats

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Spin
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.World);

        // Float
        float newY = startPos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}