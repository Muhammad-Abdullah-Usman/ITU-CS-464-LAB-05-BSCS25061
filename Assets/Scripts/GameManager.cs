using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] int targetScore = 20;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text bonusCoinsText;
    [SerializeField] GameObject winPanel;
    [SerializeField] Spawner spawner;
    int score;
    int bonusCoins;
    int redCoinScore;
    bool redCoinTriggered;
    void Awake() => Instance = this;
    void Start()
    {
        redCoinScore = Random.Range(1, 11);

        Debug.Log("Red coin will spawn at score: " + redCoinScore);
        bonusCoinsText.gameObject.SetActive(false);

        UpdateUI();
    }
    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
        if (!redCoinTriggered && score == redCoinScore)
        {
            redCoinTriggered = true;
            spawner.SpawnRedCoin();
        }

        if (score >= targetScore) Win();
    }

    public void CollectRedCoin(Transform player)
    {
        spawner.SpawnBlueCoins(player.position);
    }

    public void AddBonusCoin()
    {
        bonusCoins++;
        if (!bonusCoinsText.gameObject.activeSelf)
        {
            bonusCoinsText.gameObject.SetActive(true);
        }

        bonusCoinsText.text = $"BonusCoins: {bonusCoins}/50";
    }
    void UpdateUI() => scoreText.text = $"Score: {score} / {targetScore}";
    void Win() { winPanel.SetActive(true); Time.timeScale = 0f; }
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
