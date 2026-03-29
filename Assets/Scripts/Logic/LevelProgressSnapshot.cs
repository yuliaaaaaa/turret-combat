using System;
using UnityEngine;

public readonly struct LevelProgressSnapshot
{
    public readonly float Progress;
    public readonly float TraveledDistance;
    public readonly float PlannedDistance;
    public readonly bool IsCompleted;

    public LevelProgressSnapshot(float progress, float traveledDistance, float plannedDistance, bool isCompleted)
    {
        Progress = progress;
        TraveledDistance = traveledDistance;
        PlannedDistance = plannedDistance;
        IsCompleted = isCompleted;
    }
}

public class LevelProgressService
{
    public bool IsInitialized { get; private set; }
    public bool IsCompleted { get; private set; }

    private float _startZ;
    private float _plannedDistance;

    public event Action<LevelProgressSnapshot> Changed;
    public event Action Completed;

    public void Initialize(float startZ, float plannedDistance)
    {
        _startZ = startZ;
        _plannedDistance = Mathf.Max(1f, plannedDistance);
        IsInitialized = true;
        IsCompleted = false;

        Publish(startZ);
    }

    public void UpdateProgress(float currentZ)
    {
        if (!IsInitialized || IsCompleted)
            return;

        Publish(currentZ);

        float traveledDistance = Mathf.Clamp(currentZ - _startZ, 0f, _plannedDistance);
        if (traveledDistance < _plannedDistance)
            return;

        IsCompleted = true;
        Completed?.Invoke();
    }

    private void Publish(float currentZ)
    {
        float traveledDistance = Mathf.Clamp(currentZ - _startZ, 0f, _plannedDistance);
        float progress = traveledDistance / _plannedDistance;

        Changed?.Invoke(new LevelProgressSnapshot(
            progress,
            traveledDistance,
            _plannedDistance,
            IsCompleted));
    }
}