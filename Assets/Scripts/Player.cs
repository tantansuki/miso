using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("HP設定")]
    [SerializeField] private float maxHP = 100f;
    [SerializeField] private float damageAmount = 1f;
    [SerializeField] private float damageCooldown = 1f;
    
    [Header("UI設定")]
    [SerializeField] private Image hpBarImage;
    [SerializeField] private bool useScale = true;
    
    private float currentHP;
    private float lastDamageTime = -999f;
    private RectTransform hpBarRect;
    private Vector3 hpBarOriginalScale;
    
    void Start()
    {
        currentHP = maxHP;
        
        if (hpBarImage != null)
        {
            hpBarRect = hpBarImage.GetComponent<RectTransform>();
            hpBarOriginalScale = hpBarRect.localScale;
        }
        
        UpdateHPBar();
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("teki"))
        {
            TakeDamage();
        }
    }
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("teki"))
        {
            if (Time.time >= lastDamageTime + damageCooldown)
            {
                TakeDamage();
            }
        }
    }
    
    // ボーンのColliderから呼べるようにpublicに変更
    public void TakeDamage()
    {
        // クールダウンチェック
        if (Time.time < lastDamageTime + damageCooldown)
        {
            return;
        }
        
        lastDamageTime = Time.time;
        currentHP -= damageAmount;
        
        if (currentHP < 0)
        {
            currentHP = 0;
        }
        
        UpdateHPBar();
        
        if (currentHP <= 0)
        {
            Die();
        }
    }
    
    private void UpdateHPBar()
    {
        if (hpBarImage == null) return;
        
        float hpPercentage = currentHP / maxHP;
        
        if (useScale)
        {
            if (hpBarRect != null)
            {
                Vector3 newScale = hpBarOriginalScale;
                newScale.x = hpPercentage;
                hpBarRect.localScale = newScale;
            }
        }
        else
        {
            hpBarImage.fillAmount = hpPercentage;
        }
    }
    
    private void Die()
    {
        // 死亡処理をここに追加
    }
}
