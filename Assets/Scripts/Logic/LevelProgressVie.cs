using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LevelProgressView : MonoBehaviour
{
    [SerializeField] private Image distanceFillImage;
    [SerializeField] private TMP_Text distanceText;
    [SerializeField] private GameObject progressRoot;

    private LevelProgressService _progressService;

    [Inject]
    public void Construct(LevelProgressService progressService)
    {
        _progressService = progressService;
    }

    private void OnEnable()
    {
        if (_progressService == null)
            return;

        _progressService.Changed += OnProgressChanged;
        _progressService.Completed += OnCompleted;
    }

    private void OnDisable()
    {
        if (_progressService == null)
            return;

        _progressService.Changed -= OnProgressChanged;
        _progressService.Completed -= OnCompleted;
    }

    private void Start()
    {
        SetVisible(false);
    }

    private void OnProgressChanged(LevelProgressSnapshot snapshot)
    {
        SetVisible(true);

        if (distanceFillImage != null)
            distanceFillImage.fillAmount = snapshot.Progress;

        if (distanceText != null)
            distanceText.text = $"{Mathf.FloorToInt(snapshot.TraveledDistance)} m";
    }

    private void OnCompleted()
    {
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