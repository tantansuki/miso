using UnityEngine;
using TMPro;

public class TekiHealth : MonoBehaviour
{
    [SerializeField] private float maxHP = 50f;
    [SerializeField] private TextMeshPro hpText;
    public float CurrentHP { get; private set; }

    void Awake()
    {
        CurrentHP = maxHP;
        UpdateHPText();
    }

    public void ApplyDamage(float amount)
    {
        CurrentHP -= amount;
        if (CurrentHP <= 0f)
        {
            CurrentHP = 0f;
            Destroy(gameObject);
        }
        UpdateHPText();
    }

    void UpdateHPText()
    {
        if (hpText != null)
        {
            hpText.text = $"{CurrentHP:F0}";
        }
    }
}
