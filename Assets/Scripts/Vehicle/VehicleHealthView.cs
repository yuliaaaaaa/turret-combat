using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class VehicleHealthView : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private TMP_Text healthText;

    private Health _health;

    [Inject]
    public void Construct(VehicleHealth vehicleHealth)
    {
        if (vehicleHealth != null)
            _health = vehicleHealth.Health;
    }

    private void Start()
    {
        if (_health == null)
            return;

        _health.Changed += OnHealthChanged;
        Refresh(_health.Current, _health.Max);
    }

    private void OnDestroy()
    {
        if (_health != null)
            _health.Changed -= OnHealthChanged;
    }

    private void OnHealthChanged(int current, int max)
    {
        Refresh(current, max);
    }

    private void Refresh(int current, int max)
    {
        float normalized = max > 0 ? (float)current / max : 0f;

        if (fillImage != null)
            fillImage.fillAmount = normalized;

        if (healthText != null)
            healthText.text = $"{current}/{max}";
    }
}