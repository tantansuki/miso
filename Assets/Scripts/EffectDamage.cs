using UnityEngine;

/// <summary>
/// エフェクトPrefabにアタッチして使用
/// Collider2D (isTrigger=true) とセットで使う
/// </summary>
public class EffectDamage : MonoBehaviour
{
    [SerializeField] public float damage = 10f;
    [SerializeField] public float hitDuration = 0.2f; // 当たり判定の有効時間
    
    private bool canHit = true;
    
    void Start()
    {
        // 指定時間後に当たり判定を無効化
        Invoke(nameof(DisableHit), hitDuration);
    }
    
    void DisableHit()
    {
        Debug.Log("EffectDamage: Hit disabled");
        canHit = false;
        
        // Colliderを無効化
        var col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!canHit) return;
        if (!other.CompareTag("teki")) return;
        
        var health = other.GetComponent<TekiHealth>();
        if (health != null)
        {
            health.ApplyDamage(damage);
        }
    }
}
