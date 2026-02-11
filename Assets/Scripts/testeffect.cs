using UnityEngine;
using UnityEngine.InputSystem;

public class testeffect : MonoBehaviour
{
    [SerializeField] private GameObject slashEffectPrefab;
    [SerializeField] private Vector2 spawnOffset = Vector2.zero;
    [SerializeField] private bool usePlayerDirection = true;
    
    void Update()
    {
        // Spaceキーで再生テスト
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            PlaySlashEffect();
        }
    }
    
    public void PlaySlashEffect()
    {
        if (slashEffectPrefab != null)
        {
            Vector3 spawnPosition = (Vector2)transform.position + spawnOffset;
            
            // 2D用の回転（プレイヤーの向きを考慮）
            Quaternion rotation = Quaternion.identity;
            if (usePlayerDirection)
            {
                // プレイヤーが左を向いている場合は反転
                float direction = transform.localScale.x;
                if (direction < 0)
                {
                    rotation = Quaternion.Euler(0, 180, 0);
                }
            }
            
            GameObject effect = Instantiate(slashEffectPrefab, spawnPosition, rotation);
            
            // 自動削除（エフェクトの長さ+余裕）
            Destroy(effect, 2f);
        }
    }
    
    // 特定の位置でエフェクトを再生
    public void PlaySlashEffectAt(Vector2 position)
    {
        if (slashEffectPrefab != null)
        {
            GameObject effect = Instantiate(slashEffectPrefab, position, Quaternion.identity);
            Destroy(effect, 2f);
        }
    }
    
    // 特定の位置と角度でエフェクトを再生
    public void PlaySlashEffectAt(Vector2 position, float angle)
    {
        if (slashEffectPrefab != null)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            GameObject effect = Instantiate(slashEffectPrefab, position, rotation);
            Destroy(effect, 2f);
        }
    }
}
