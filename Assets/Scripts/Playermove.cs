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
    }
    
    void FixedUpdate()
    {
        // プレイヤーを移動（物理演算の更新タイミングで実行）
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }
}
