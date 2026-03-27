using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgressTracker : MonoBehaviour
{
    [SerializeField] private Transform carTransform;
    [SerializeField] private Image distanceFillImage;
    [SerializeField] private TMP_Text distanceText;
    [SerializeField] private GameObject progressRoot;

    private float _startZ;
    private float _plannedDistance;
    private bool _isInitialized;
    private bool _isCompleted;

    public void Initialize(float startZ, float plannedDistance)
    {
        _startZ = startZ;
        _plannedDistance = Mathf.Max(1f, plannedDistance);
        _isInitialized = true;
        _isCompleted = false;

        SetVisible(true);
        RefreshView();
    }

    private void Update()
    {
        if (!_isInitialized || _isCompleted || carTransform == null)
            return;

        RefreshView();
    }

    private void RefreshView()
    {
        float traveledDistance = Mathf.Clamp(carTransform.position.z - _startZ, 0f, _plannedDistance);
        float progress = traveledDistance / _plannedDistance;

        if (distanceFillImage != null)
            distanceFillImage.fillAmount = progress;

        if (distanceText != null)
            distanceText.text = $"{Mathf.FloorToInt(traveledDistance)} m";

        if (traveledDistance < _plannedDistance)
            return;

        _isCompleted = true;
        SetVisible(false);
    }

    private void SetVisible(bool visible)
    {
        if (progressRoot != null)
            progressRoot.SetActive(visible);
        else
            gameObject.SetActive(visible);
    }
}