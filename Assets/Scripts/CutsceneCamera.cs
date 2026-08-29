using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

/// <summary>
/// Điều phối intro camera trên cao lượn quanh map, sau đó blend mượt
/// (nhờ CinemachineBrain) về camera third-person sau lưng nhân vật,
/// rồi mới bật điều khiển cho người chơi.
///
/// Setup:
/// - introPivot: object rỗng đặt giữa map, IntroCamera là con của nó.
/// - introCamera: CinemachineCamera đặt trên cao, nhìn xuống map.
/// - gameplayCamera: chính là FreeLook Camera hiện có (third-person).
/// - controlsToDisable: kéo joystick GameObject, script di chuyển nhân vật,
///   script CameraOrbitDragPanel... vào đây - tất cả sẽ bị tắt trong lúc intro.
/// - brain: CinemachineBrain trên Main Camera (để biết khi nào blend xong).
/// </summary>
public class CutsceneCamera : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] private Transform introPivot;
    [SerializeField] private CinemachineCamera introCamera;
    [SerializeField] private CinemachineCamera gameplayCamera;
    [SerializeField] private CinemachineBrain brain;

    [Header("Intro settings")]
    [SerializeField] private float introDuration = 4f;
    [SerializeField] private float introRotationSpeed = 20f; // độ/giây

    [Header("Điều khiển cần tắt lúc intro")]
    [SerializeField] private Behaviour[] controlsToDisable; // script (joystick, orbit, player controller...)
    [SerializeField] private GameObject[] uiToHide;          // ví dụ: canvas joystick, nút Attack/Jump

    private void Start()
    {
        // StartCoroutine(PlayIntro());
    }

    public IEnumerator PlayIntro()
    {
        SetControlsEnabled(false);

        // IntroCamera live trước
        introCamera.Priority = 20;
        gameplayCamera.Priority = 10;

        float t = 0f;
        while (t < introDuration)
        {
            if (introPivot != null)
                introPivot.Rotate(Vector3.up, introRotationSpeed * Time.deltaTime, Space.World);

            t += Time.deltaTime;
            yield return null;
        }

        // Kích hoạt blend mượt sang gameplay camera
        gameplayCamera.Priority = 30;

        // Đợi Cinemachine Brain blend xong hẳn rồi mới cho điều khiển
        yield return new WaitUntil(() => !brain.IsBlending);

        SetControlsEnabled(true);
    }

    private void SetControlsEnabled(bool enabled)
    {
        foreach (var c in controlsToDisable)
            if (c != null) c.enabled = enabled;

        foreach (var go in uiToHide)
            if (go != null) go.SetActive(enabled);
    }
}