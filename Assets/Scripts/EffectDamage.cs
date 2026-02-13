using UnityEngine;

/// <summary>
/// エフェクトPrefabにアタッチして使用
/// Collider2D (isTrigger=true) とセットで使う
/// </summary>
public class EffectDamage : MonoBehaviour
{
    public enum EffectType { Place, Shoot }

    [Header("ダメージ設定")]
    [SerializeField] public float damage = 10f;
    [SerializeField] public float hitDuration = 0.2f; // 当たり判定の有効時間

    [Header("エフェクト挙動")]
    [SerializeField] public EffectType effectType = EffectType.Place; // 配置 or 射出
    [SerializeField] private float shootSpeed = 8f;                    // 射出速度

    private bool canHit = true;
    private bool isMoving = false;
    private Vector2 moveDir = Vector2.right;
    private Rigidbody2D rb;

    // 外部から方向を設定（生成直後に呼ぶ）
    public void SetDirection(Vector2 direction)
    {
        moveDir = direction.normalized;
    }

    void Start()
    {
        // 指定時間後に当たり判定を無効化
        Invoke(nameof(DisableHit), hitDuration);

        // 射出タイプなら移動開始
        if (effectType == EffectType.Shoot)
        {
            rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = moveDir * shootSpeed;
            }
            else
            {
                isMoving = true;
            }
        }
    }

    void Update()
    {
        // Rigidbody2Dがない場合の簡易移動
        if (isMoving)
        {
            transform.Translate(moveDir * shootSpeed * Time.deltaTime, Space.World);
        }
    }

    void DisableHit()
    {
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
