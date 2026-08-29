using UnityEngine;

public class Gem : MonoBehaviour
{
    [Header("Gia tri cua gem")]
    public int gemValue = 1;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //other.GetComponent<Player>()?.AddScore(gemValue);
        }

        //GemPool.Instance.ReturnGem(gameObject);
    }
}
