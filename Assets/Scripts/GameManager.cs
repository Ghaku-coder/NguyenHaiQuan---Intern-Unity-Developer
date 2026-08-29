using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<Gem> gems = new List<Gem>();
    public Player player;
    public int score = 0;
    public TextMeshProUGUI ShowScore;

    public Camera mainCamera;
    public RectTransform scoreIconTarget;

    private const string SCORE_KEY = "PlayerScore";

    void Awake()
    {
        Instance = this;
        ShowScore.text = string.Empty;

        LoadScore(); // load điểm cũ ngay khi scene chạy
    }

    public void RegisterGems(Gem g) => gems.Add(g);
    public void UnRegisterGems(Gem g) => gems.Remove(g);

    public void AddScore(int value)
    {
        score += value;
        SaveScore(); // lưu ngay mỗi lần ăn gem
    }

    public void ShowPlayerScore()
    {
        ShowScore.text = $"Score: {score}";
    }

    private void SaveScore()
    {
        PlayerPrefs.SetInt(SCORE_KEY, score);
        PlayerPrefs.Save();
    }

    private void LoadScore()
    {
        score = PlayerPrefs.GetInt(SCORE_KEY, 0);
    }

    // Gọi khi bấm nút Reset
    public void ResetScore()
    {
        score = 0;
        SaveScore(); // ghi đè 0 xuống PlayerPrefs
    }
}