using UnityEngine;
using UnityEngine.InputSystem;

public class Playermove : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] private float positionCorrectionStrength = 0.5f; // 位置補正の強さ（0-1）
    
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector2 moveInput;
    private Rigidbody2D[] childRigidbodies; // 子のRigidbody2D配列
    private Vector2[] childInitialOffsets; // 子の初期オフセット
    
    void Start()
    {
        // Rigidbody2Dを取得または追加（Kinematic設定）
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // 回転のみ固定、位置は自由
        
        // 子のRigidbody2Dを全て取得（親は除外）
        childRigidbodies = GetComponentsInChildren<Rigidbody2D>();
        var list = new System.Collections.Generic.List<Rigidbody2D>(childRigidbodies);
        list.Remove(rb);
        childRigidbodies = list.ToArray();
        
        // 子の初期オフセット（親からの相対位置）を保存
        childInitialOffsets = new Vector2[childRigidbodies.Length];
        for (int i = 0; i < childRigidbodies.Length; i++)
        {
            if (childRigidbodies[i] != null)
            {
                childInitialOffsets[i] = (Vector2)childRigidbodies[i].transform.position - rb.position;
            }
        }
        
        // SpriteRendererコンポーネントを取得（ボーンにある場合は子から取得）
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRendererコンポーネントが見つかりません！");
        }
    }

    void Update()
    {
        // New Input Systemでキー入力を取得
        moveInput = Vector2.zero;
        
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) moveInput.y = 1f;
            if (Keyboard.current.sKey.isPressed) moveInput.y = -1f;
            if (Keyboard.current.aKey.isPressed) moveInput.x = -1f;
            if (Keyboard.current.dKey.isPressed) moveInput.x = 1f;
        }
        
        // 移動ベクトルを正規化（斜め移動時の速度が速くなりすぎないように）
        if (moveInput.magnitude > 1f)
        {
            moveInput.Normalize();
        }
        
        // スプライトの向きを変更（右に移動するときは反転）
        if (spriteRenderer != null)
        {
            if (moveInput.x > 0)
            {
                // 右方向に移動：反転
                spriteRenderer.flipX = true;
            }
            else if (moveInput.x < 0)
            {
                // 左方向に移動：通常
                spriteRenderer.flipX = false;
            }
        }
    }
    
    void FixedUpdate()
    {
        if (rb == null) return;
        
        // 移動量を計算
        Vector2 movement = moveInput * moveSpeed * Time.fixedDeltaTime;
        
        if (movement.magnitude > 0)
        {
            // 親のRigidbody2Dを移動
            rb.MovePosition(rb.position + movement);
            
            // 子のRigidbody2Dも同じ量だけ移動（ソフトボディが親についていく）
            for (int i = 0; i < childRigidbodies.Length; i++)
            {
                if (childRigidbodies[i] != null)
                {
                    childRigidbodies[i].MovePosition(childRigidbodies[i].position + movement);
                }
            }
        }
        
        // 衝突でずれた子の位置を補正（プルプルを保ちながら親についていく）
        for (int i = 0; i < childRigidbodies.Length; i++)
        {
            if (childRigidbodies[i] != null)
            {
                // 本来あるべき位置
                Vector2 expectedPosition = rb.position + childInitialOffsets[i];
                
                // 現在位置から本来の位置に少しずつ補正（プルプルを残すため）
                Vector2 correctedPosition = Vector2.Lerp(
                    childRigidbodies[i].position,
                    expectedPosition,
                    positionCorrectionStrength
                );
                
                childRigidbodies[i].MovePosition(correctedPosition);
            }
        }
    }
}
