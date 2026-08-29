using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuControl : MonoBehaviour
{
    public GameObject background;
    public UIManager uIManager;
    public CutsceneCamera cutsceneCamera;
    
    public void Play()
    {
        if(uIManager.gameManager.score <= 0)
        {
            background.SetActive(false);
            StartCoroutine(Intro());
        }
    }

    IEnumerator Intro()
    {
        StartCoroutine(cutsceneCamera.PlayIntro());
        yield return new WaitForSeconds(1f);
        StartCoroutine(uIManager.ShowMission());
    }

    public void Reset()
    {
        background.SetActive(false);
        GameManager.Instance.ResetScore();
        StartCoroutine(Intro());
        
        uIManager.missionShown = false;
        uIManager.congratulationShown = false;
    }

    public void openMenu()
    {
        background.SetActive(true);
    }
}
