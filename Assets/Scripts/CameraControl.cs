using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/// <summary>
/// Giữ chuột trái (Editor) hoặc chạm 1 ngón (Android) rồi kéo để xoay
/// Cinemachine camera quanh nhân vật. Dùng API của package Input System mới
/// (Mouse.current / Touchscreen.current) nên chạy được kể cả khi
/// Project Settings > Player > Active Input Handling = "Input System Package (New)".
/// Gán trực tiếp vào CinemachineOrbitalFollow, không cần Cinemachine Input Axis Controller.
/// </summary>
public class CameraOrbitDragControl : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private CinemachineOrbitalFollow orbitalFollow;

    [Header("Sensitivity")]
    [SerializeField] private float sensitivityX = 3f;
    [SerializeField] private float sensitivityY = 3f;
    [SerializeField] private bool invertY = false;

    private bool _dragging;
    private Vector2 _lastPos;

    private void Reset()
    {
        orbitalFollow = GetComponent<CinemachineOrbitalFollow>();
    }

    private void Awake()
    {
        if (orbitalFollow == null)
            Debug.LogError("[CameraOrbitDragControl] Chưa gán Orbital Follow trong Inspector!", this);
    }

    private void Update()
    {
        if (Touchscreen.current != null && Touchscreen.current.touches.Count > 0)
            HandleTouch();
        else if (Mouse.current != null)
            HandleMouse();
    }

    private void HandleMouse()
    {
        var mouse = Mouse.current;

        if (mouse.leftButton.wasPressedThisFrame)
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return; // đang bấm nút Attack/Jump/joystick UI thì bỏ qua

            _dragging = true;
            _lastPos = mouse.position.ReadValue();
            return;
        }

        if (mouse.leftButton.wasReleasedThisFrame)
        {
            _dragging = false;
            return;
        }

        if (_dragging)
        {
            Vector2 current = mouse.position.ReadValue();
            ApplyDelta(current - _lastPos);
            _lastPos = current;
        }
    }

    private void HandleTouch()
    {
        var touch = Touchscreen.current.primaryTouch;
        Vector2 pos = touch.position.ReadValue();
        var phase = touch.phase.ReadValue();

        bool overUI = EventSystem.current != null &&
                      EventSystem.current.IsPointerOverGameObject();

        switch (phase)
        {
            case UnityEngine.InputSystem.TouchPhase.Began:
                if (overUI) { _dragging = false; break; }
                _dragging = true;
                _lastPos = pos;
                break;

            case UnityEngine.InputSystem.TouchPhase.Moved:
            case UnityEngine.InputSystem.TouchPhase.Stationary:
                if (_dragging)
                {
                    ApplyDelta(pos - _lastPos);
                    _lastPos = pos;
                }
                break;

            case UnityEngine.InputSystem.TouchPhase.Ended:
            case UnityEngine.InputSystem.TouchPhase.Canceled:
                _dragging = false;
                break;
        }
    }

    private void ApplyDelta(Vector2 delta)
    {
        if (orbitalFollow == null) return;

        orbitalFollow.HorizontalAxis.Value += delta.x * sensitivityX * Time.deltaTime;

        float yDelta = delta.y * sensitivityY * Time.deltaTime;
        orbitalFollow.VerticalAxis.Value += invertY ? yDelta : -yDelta;
    }
}