using Unity.Cinemachine;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    [SerializeField] private float bounceForce;
    private Vector2 bounceDirection = Vector2.up;
    
    void OnTriggerStay2D(Collider2D collider)
    {
        Rigidbody2D rb = collider.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(transform.up * bounceForce, ForceMode2D.Impulse);
        }
    }
}
