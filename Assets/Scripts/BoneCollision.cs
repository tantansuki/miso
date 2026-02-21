using UnityEngine;

public class BoneCollision : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("teki"))
            GetComponentInParent<Player>().TakeDamage();
    }
    
    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("teki"))
            GetComponentInParent<Player>().TakeDamage();
    }
}