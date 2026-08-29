using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraOrbitDragPanel : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("Refs")]
    [SerializeField] private CinemachineOrbitalFollow orbitalFollow;

    [Header("Sensitivity")]
    [SerializeField] private float sensitivityX = 3f;
    [SerializeField] private float sensitivityY = 3f;
    [SerializeField] private bool invertY = false;

    private int _activePointerId = int.MinValue;
    private Vector2 _lastPos;

    public void OnPointerDown(PointerEventData eventData)
    {
        // Chỉ nhận 1 ngón điều khiển camera tại 1 thời điểm
        if (_activePointerId != int.MinValue) return;

        _activePointerId = eventData.pointerId;
        _lastPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerId != _activePointerId) return;

        Vector2 delta = eventData.position - _lastPos;
        _lastPos = eventData.position;
        ApplyDelta(delta);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerId == _activePointerId)
            _activePointerId = int.MinValue;
    }

    private void ApplyDelta(Vector2 delta)
    {
        if (orbitalFollow == null) return;

        orbitalFollow.HorizontalAxis.Value += delta.x * sensitivityX * Time.deltaTime;

        float yDelta = delta.y * sensitivityY * Time.deltaTime;
        orbitalFollow.VerticalAxis.Value += invertY ? yDelta : -yDelta;
    }
}   