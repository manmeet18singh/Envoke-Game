using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthUIManager : MonoBehaviour
{
    [SerializeField] Image mFillImage = null;
    [SerializeField] TextMeshProUGUI healthTMP = null;

    private void Awake()
    {
        PlayerHealth.mPlayerDamaged += UpdateHealth;
        PlayerHealth.mPlayerHealed += UpdateHealth;
    }

    private void OnDestroy()
    {
        PlayerHealth.mPlayerDamaged -= UpdateHealth;
        PlayerHealth.mPlayerHealed -= UpdateHealth;
    }

    void UpdateHealth(int _currentHealth, int _maxHealth)
    {
        mFillImage.fillAmount = _currentHealth / (float)_maxHealth;
        healthTMP.text = $"{_currentHealth} / {_maxHealth}";
    }
}
