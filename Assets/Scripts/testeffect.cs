using UnityEngine;
using UnityEngine.InputSystem;

public class testeffect : MonoBehaviour
{
    [Header("エフェクト設定")]
    [SerializeField] private GameObject slashEffectPrefab;
    [SerializeField] private Vector2 spawnOffset = Vector2.zero;
    [SerializeField] private float effectLifetime = 2f;
    [SerializeField] private bool autoRotateToMouse = true; // マウス方向に回転（射出時）
    
    private Camera mainCam;
    
    void Start()
    {
        mainCam = Camera.main;
    }
    
    void Update()
    {
        // Spaceキー：エフェクトタイプに応じて配置or射出
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            PlayEffectByType();
        }
    }
    
    // エフェクトタイプに応じて配置または射出
    private void PlayEffectByType()
    {
        if (slashEffectPrefab == null) return;
        
        // プレハブからEffectDamageを確認
        var effectDmg = slashEffectPrefab.GetComponent<EffectDamage>();
        if (effectDmg != null && effectDmg.effectType == EffectDamage.EffectType.Shoot)
        {
            // 射出タイプ→マウス方向へ射出
            ShootTowardsMouseWithPrefab(slashEffectPrefab);
        }
        else
        {
            // 配置タイプ→プレイヤー位置に配置
            Vector3 spawnPosition = (Vector2)transform.position + spawnOffset;
            GameObject effect = Instantiate(slashEffectPrefab, spawnPosition, Quaternion.identity);
            Destroy(effect, effectLifetime);
        }
    }
    
    // 指定プレハブをマウス方向へ射出
    private void ShootTowardsMouseWithPrefab(GameObject prefab)
    {
        if (prefab == null || mainCam == null) return;
        
        Vector2 mouseWorldPos = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 direction = (mouseWorldPos - (Vector2)transform.position).normalized;
        
        Vector3 spawnPos = (Vector2)transform.position + spawnOffset;
        Quaternion rotation = Quaternion.identity;
        
        if (autoRotateToMouse)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rotation = Quaternion.Euler(0, 0, angle);
        }
        
        GameObject effect = Instantiate(prefab, spawnPos, rotation);
        
        // EffectDamageがあれば方向を設定
        var effectDmg = effect.GetComponent<EffectDamage>();
        if (effectDmg != null)
        {
            effectDmg.SetDirection(direction);
        }
        
        Destroy(effect, effectLifetime);
    }
}
