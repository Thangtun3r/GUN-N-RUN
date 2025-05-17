using UnityEngine;

public class Player : MonoBehaviour,IDeathAble,ICheckpoint
{
   private Vector3 currentSpawnPoint;

   public void Die()
   {
      LosingEvent.RaisePlayerDeath();
      Respawn();
   }
   
   public void Respawn()
   {
      transform.position = currentSpawnPoint;
      Rigidbody2D rb = GetComponent<Rigidbody2D>();
      rb.linearVelocity = Vector2.zero;
      rb.angularVelocity = 0f;
      
   }
   public void SaveCheckpoint(Vector3 currentSpawnPoint)
   {
      this.currentSpawnPoint = currentSpawnPoint;
   }
}
