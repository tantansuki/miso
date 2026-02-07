using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class tekimove : MonoBehaviour
{
    [Header("追跡設定")]
    [SerializeField] private float moveSpeed = 2f;  // 移動速度
    [SerializeField] private bool flipSprite = true;  // スプライトの向きを反転するか
    
    private Transform player;  // プレイヤーのTransform
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    
    void Start()
    {
        // プレイヤーをタグで検索
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("プレイヤーが見つかりません。Playerタグが設定されているか確認してください。");
        }
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        
        // Rigidbody2Dの設定
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;  // 回転を固定
    }

    void FixedUpdate()
    {
        if (player == null) return;
        
        // プレイヤーを常に追跡
        ChasePlayer();
    }
    
    private void ChasePlayer()
    {
        // プレイヤーの方向を計算
        Vector2 direction = (player.position - transform.position).normalized;
        
        // Rigidbody2Dを使って移動
        Vector2 newPosition = Vector2.MoveTowards(
            rb.position,
            player.position,
            moveSpeed * Time.fixedDeltaTime
        );
        rb.MovePosition(newPosition);
        
        // スプライトの向きを反転
        if (flipSprite && spriteRenderer != null)
        {
            if (direction.x > 0)
            {
                spriteRenderer.flipX = false;  // 右向き
            }
            else if (direction.x < 0)
            {
                spriteRenderer.flipX = true;   // 左向き
            }
        }
    }
}
