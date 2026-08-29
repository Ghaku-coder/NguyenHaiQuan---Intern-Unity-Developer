using UnityEngine;

public class Gem : MonoBehaviour
{
    [Header("Gia tri cua gem")]
    public int gemValue = 1;

    private void OnEnable()
    {
        GameManager.Instance.RegisterGems(this);
    }

    private void OnDisable()
    {
        GameManager.Instance.UnRegisterGems(this);   
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.Instance.player.isAttacking == true)
        {
            GameManager.Instance.AddScore(gemValue);
            GemPool.Instance.ReturnGem(gameObject);
        }

        
    }
}
