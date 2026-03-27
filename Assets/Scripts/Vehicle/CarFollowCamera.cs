using UnityEngine;

public class CarFollowCamera : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Offset")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 6f, -8f);

    [Header("Look")]
    [SerializeField] private Vector3 lookOffset = new Vector3(0f, 2f, 8f);

    [Header("Win Look")]
    [SerializeField] private Vector3 winLookOffset = new Vector3(0f, 5f, 16f);
    [SerializeField] private float winRotationLerpSpeed = 2f;

    [Header("Position Smoothing")]
    [SerializeField] private float smoothTimeX = 0.15f;
    [SerializeField] private float smoothTimeY = 0.3f;
    [SerializeField] private float smoothTimeZ = 0.12f;

    [Header("Rotation")]
    [SerializeField] private float rotationLerpSpeed = 8f;

    [Header("Vertical Filtering")]
    [SerializeField] private bool limitYFollow = true;
    [SerializeField] private float maxYStepPerFrame = 0.08f;

    private float _velocityX;
    private float _velocityY;
    private float _velocityZ;
    private bool _isWinLook;

    private void LateUpdate()
    {
        if (target == null)
            return;

        UpdatePosition();
        UpdateRotation();
    }

    private void UpdatePosition()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 currentPosition = transform.position;

        float nextX = Mathf.SmoothDamp(currentPosition.x, desiredPosition.x, ref _velocityX, smoothTimeX);
        float nextY = Mathf.SmoothDamp(currentPosition.y, desiredPosition.y, ref _velocityY, smoothTimeY);
        float nextZ = Mathf.SmoothDamp(currentPosition.z, desiredPosition.z, ref _velocityZ, smoothTimeZ);

        if (limitYFollow)
        {
            float minY = currentPosition.y - maxYStepPerFrame;
            float maxY = currentPosition.y + maxYStepPerFrame;
            nextY = Mathf.Clamp(nextY, minY, maxY);
        }

        transform.position = new Vector3(nextX, nextY, nextZ);
    }

    private void UpdateRotation()
    {
        Vector3 currentLookOffset = _isWinLook ? winLookOffset : lookOffset;
        float currentLerpSpeed = _isWinLook ? winRotationLerpSpeed : rotationLerpSpeed;

        Vector3 lookTarget = target.position + currentLookOffset;
        Vector3 direction = (lookTarget - transform.position).normalized;

        if (direction.sqrMagnitude < 0.0001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, currentLerpSpeed * Time.deltaTime);
    }

    public void PlayWinLook()
    {
        _isWinLook = true;
    }
}