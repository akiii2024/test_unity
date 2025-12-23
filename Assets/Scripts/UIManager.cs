using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private GameObject healthPanel;
    [SerializeField] private Image[] healthIcons;
    [SerializeField] private GameObject helpPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI gameOverScoreText;
    [SerializeField] private GameObject startPrompt;
    
    private void Awake()
    {
        // 参照が設定されていない場合は自動検索
        if (scoreText == null)
            scoreText = GameObject.Find("ScoreText")?.GetComponent<TextMeshProUGUI>();
        if (distanceText == null)
            distanceText = GameObject.Find("DistanceText")?.GetComponent<TextMeshProUGUI>();
        if (coinText == null)
            coinText = GameObject.Find("CoinText")?.GetComponent<TextMeshProUGUI>();
        if (healthPanel == null)
            healthPanel = GameObject.Find("HealthPanel");
        if (helpPanel == null)
            helpPanel = GameObject.Find("HelpPanel");
        if (gameOverPanel == null)
            gameOverPanel = GameObject.Find("GameOverPanel");
        if (gameOverScoreText == null)
            gameOverScoreText = GameObject.Find("GameOverScoreText")?.GetComponent<TextMeshProUGUI>();
        if (startPrompt == null)
            startPrompt = GameObject.Find("StartPrompt");
        
        // 体力アイコンを自動検索
        if (healthIcons == null || healthIcons.Length == 0)
        {
            if (healthPanel != null)
            {
                Image[] foundIcons = healthPanel.GetComponentsInChildren<Image>();
                if (foundIcons.Length > 0)
                {
                    healthIcons = foundIcons;
                }
            }
        }
    }
    
    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnScoreUpdated += UpdateScore;
            GameManager.Instance.OnHealthChanged += UpdateHealth;
            GameManager.Instance.OnGameOver += ShowGameOver;
            GameManager.Instance.OnGameStart += HideStartPrompt;
        }
        
        if (helpPanel != null)
        {
            helpPanel.SetActive(false);
        }
        
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        
        if (startPrompt != null)
        {
            startPrompt.SetActive(true);
        }
    }
    
    private void Update()
    {
        // Hキーでヘルプパネル表示/非表示
        if (Input.GetKeyDown(KeyCode.H))
        {
            ToggleHelpPanel();
        }
        
        // ゲームオーバー時にRキーでリスタート
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver())
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartGame();
            }
        }
    }
    
    private void UpdateScore(float distance, int coinCount)
    {
        if (scoreText != null)
        {
            int totalScore = Mathf.RoundToInt(distance) + (coinCount * 10);
            scoreText.text = $"Score: {totalScore}";
        }
        
        if (distanceText != null)
        {
            distanceText.text = $"Distance: {Mathf.RoundToInt(distance)}m";
        }
        
        if (coinText != null)
        {
            coinText.text = $"Coins: {coinCount}";
        }
    }
    
    private void UpdateHealth(int health)
    {
        if (healthIcons == null || healthIcons.Length == 0) return;
        
        for (int i = 0; i < healthIcons.Length; i++)
        {
            if (healthIcons[i] != null)
            {
                healthIcons[i].gameObject.SetActive(i < health);
            }
        }
    }
    
    private void ToggleHelpPanel()
    {
        if (helpPanel != null)
        {
            helpPanel.SetActive(!helpPanel.activeSelf);
        }
    }
    
    private void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        
        if (gameOverScoreText != null && GameManager.Instance != null)
        {
            int totalScore = GameManager.Instance.GetTotalScore();
            gameOverScoreText.text = $"Final Score: {totalScore}\nDistance: {Mathf.RoundToInt(GameManager.Instance.GetDistance())}m\nCoins: {GameManager.Instance.GetCoinCount()}";
        }
    }
    
    private void HideStartPrompt()
    {
        if (startPrompt != null)
        {
            startPrompt.SetActive(false);
        }
    }
    
    private void RestartGame()
    {
        if (GameManager.Instance != null)
        {
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(false);
            }
            
            // 障害物をクリア
            ObstacleSpawner spawner = FindObjectOfType<ObstacleSpawner>();
            if (spawner != null)
            {
                spawner.ClearAllObstacles();
            }
            
            GameManager.Instance.StartGame();
        }
    }
    
    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnScoreUpdated -= UpdateScore;
            GameManager.Instance.OnHealthChanged -= UpdateHealth;
            GameManager.Instance.OnGameOver -= ShowGameOver;
            GameManager.Instance.OnGameStart -= HideStartPrompt;
        }
    }
}

