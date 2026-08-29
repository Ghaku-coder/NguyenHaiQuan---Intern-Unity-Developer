using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Gem : MonoBehaviour
{
    [Header("Gia tri cua gem")]
    public int gemValue = 1;

    [Header("Hieu ung bai ve UI")]
    private float flyDuration = 0.6f;
    public AnimationCurve flyCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    public float uiDistanceFromCamera = 2f;

    public Collider col;
    private Rigidbody rb;
    private bool isCollected;

    void Start()
    {
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();    
    }

    private void OnEnable()
    {
        isCollected = false;
        col.enabled = true;
        transform.localScale = Vector3.one;
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        if (GameManager.Instance != null)
            GameManager.Instance.RegisterGems(this);
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.UnRegisterGems(this);
    }

    void OnTriggerEnter(Collider other)
    {
        if(isCollected) return;

        if (other.CompareTag("Player") && GameManager.Instance.player.isAttacking == true)
        {
            isCollected = true;
            col.enabled = false;

            GameManager.Instance.UnRegisterGems(this);   
            StartCoroutine(FlyToScoreUI());
        }
    }

    IEnumerator FlyToScoreUI()
    {
        Vector3 startPos = transform.position;
        Vector3 StartScale = transform.localScale;

        Camera camera = GameManager.Instance.mainCamera;
        RectTransform target = GameManager.Instance.scoreIconTarget;

        float esclaped = 0f;
        while(esclaped < flyDuration)
        {
            esclaped += Time.deltaTime;
            float t = flyCurve.Evaluate(esclaped / flyDuration);

            Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(null, target.position);
            Vector3 endPos = camera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, uiDistanceFromCamera));

            transform.position = Vector3.Lerp(startPos, endPos, t);
            transform.localScale = Vector3.Lerp(StartScale, Vector3.zero, t);

            yield return null;
        }

        GameManager.Instance.AddScore(gemValue);
        transform.localScale = StartScale;
        GemPool.Instance.ReturnGem(gameObject);
    }
}
