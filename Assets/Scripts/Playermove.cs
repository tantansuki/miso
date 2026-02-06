using UnityEngine;
using UnityEngine.InputSystem;

public class Playermove : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField] public float moveSpeed = 5f;
    
    private Rigidbody2D rb;
    private Vector2 moveInput;
    
    void Start()
    {
        // Rigidbody2Dコンポーネントを取得
        rb = GetComponent<Rigidbody2D>();
        
        if (rb == null)
        {
            Debug.LogError("Rigidbody2Dコンポーネントが見つかりません！");
        }
    }

    void Update()
    {
        // キー入力を取得（新Input System使用）
        float horizontal = 0f;
        float vertical = 0f;
        
        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            // W: 上移動
            if (keyboard.wKey.isPressed)
            {
                vertical = 1f;
            }
            // S: 下移動
            if (keyboard.sKey.isPressed)
            {
                vertical = -1f;
            }
            // A: 左移動
            if (keyboard.aKey.isPressed)
            {
                horizontal = -1f;
            }
            // D: 右移動
            if (keyboard.dKey.isPressed)
            {
                horizontal = 1f;
            }
        }
        
        // 入力値を保存
        moveInput = new Vector2(horizontal, vertical);
        
        // 移動ベクトルを正規化（斜め移動時の速度が速くなりすぎないように）
        if (moveInput.magnitude > 1f)
        {
            moveInput.Normalize();
        }
    }
    
    void FixedUpdate()
    {
        // プレイヤーを移動（物理演算の更新タイミングで実行）
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }
}
