using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject panel;
    public GameObject mission;
    public GameObject congratulation;
    public GameObject BlastRainbow;

    private bool missionShown = false;
    private bool congratulationShown = false;

    void Start()
    {
        panel.SetActive(false);
        mission.SetActive(false);
        congratulation.SetActive(false);
        BlastRainbow.SetActive(false);
    }

    void Update()
    {
        gameManager.ShowPlayerScore();

        if (gameManager.score <= 0 && !missionShown)
        {
            missionShown = true;
            StartCoroutine(ShowMission());
        }

        if (gameManager.score == 20 && !congratulationShown)
        {
            congratulationShown = true;
            StartCoroutine(ShowCongratulation());
        }
    }

    IEnumerator ShowMission()
    {
        panel.SetActive(true);
        mission.SetActive(true);

        yield return new WaitForSeconds(5f);

        mission.SetActive(false);
        panel.SetActive(false);
    }

    IEnumerator ShowCongratulation()
    {
        Vector3 player = gameManager.player.transform.position + Vector3.up * 2f;

        BlastRainbow.SetActive(true);
        BlastRainbow.transform.position = player;

        panel.SetActive(true);
        congratulation.SetActive(true);

        yield return new WaitForSeconds(5f);

        BlastRainbow.SetActive(false);
        congratulation.SetActive(false);
        panel.SetActive(false);
    }
}