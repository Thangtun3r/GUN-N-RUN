using Unity.Cinemachine;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    [SerializeField] private float bounceForce;
    private Vector2 bounceDirection = Vector2.up;
    
    void OnCollisionEnter2D(Collision2D collision) 
    { 
        collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * bounceForce,ForceMode2D.Impulse);
        
    } 
}
