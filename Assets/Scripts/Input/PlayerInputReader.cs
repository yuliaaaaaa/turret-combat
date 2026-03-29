using UnityEngine;

public class PlayerInputReader : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 0.02f;
    [SerializeField] private float keyboardWeight = 1f;

    private GameInputActions _inputActions;

    public float AimInput { get; private set; }

    private void Awake()
    {
        _inputActions = new GameInputActions();
    }

    private void OnEnable()
    {
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
        AimInput = 0f;
    }

    private void Update()
    {
        float keyboardInput = _inputActions.Gameplay.Aim.ReadValue<float>() * keyboardWeight;
        Vector2 mouseDelta = _inputActions.Gameplay.LookDelta.ReadValue<Vector2>();
        float mouseInput = mouseDelta.x * mouseSensitivity;

        float combinedInput = keyboardInput + mouseInput;
        AimInput = Mathf.Clamp(combinedInput, -1f, 1f);
    }
}