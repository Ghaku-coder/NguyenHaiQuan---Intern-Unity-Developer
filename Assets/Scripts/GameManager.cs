using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<Gem> gems = new List<Gem>();
    //public List<Player> player = new List<Player>();
    public Player player;
    public int score = 0;
    public TextMeshProUGUI ShowScore;

    void Awake()
    {
        Instance = this;
        ShowScore.text = string.Empty; 
    }

    public void RegisterGems(Gem g) => gems.Add(g);
    public void UnRegisterGems(Gem g) => gems.Remove(g);

    // public void RegisterGems(Player p) => player.Add(p);
    // public void UnRegisterGems(Player p) => player.Remove(p);

    public void AddScore(int value)
    {
        score += value;
    }

    public void ShowPlayerScore()
    {
        ShowScore.text = $"Score: {score}";
    }

}
