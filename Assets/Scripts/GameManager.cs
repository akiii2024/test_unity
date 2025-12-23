using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("Game Settings")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int coinScoreValue = 10;
    
    private int currentHealth;
    private int coinCount = 0;
    private float distance = 0f;
    private bool isGameOver = false;
    private bool isGameStarted = false;
    
    private PlayerController playerController;
    
    public event Action<int> OnHealthChanged;
    public event Action<int> OnCoinCollected;
    public event Action<float, int> OnScoreUpdated;
    public event Action OnGameOver;
    public event Action OnGameStart;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth);
    }
    
    private void Update()
    {
        if (!isGameStarted)
        {
            if (Input.anyKeyDown)
            {
                StartGame();
            }
            return;
        }
        
        if (!isGameOver)
        {
            // 距離計算
            if (playerController != null)
            {
                distance = playerController.GetDistance();
            }
            else
            {
                distance += Time.deltaTime * 5f; // フォールバック
            }
            
            OnScoreUpdated?.Invoke(distance, coinCount);
        }
    }
    
    public void StartGame()
    {
        if (isGameStarted) return;
        
        isGameStarted = true;
        isGameOver = false;
        currentHealth = maxHealth;
        coinCount = 0;
        distance = 0f;
        
        if (playerController != null)
        {
            playerController.ResetPosition();
        }
        
        OnGameStart?.Invoke();
        OnHealthChanged?.Invoke(currentHealth);
        OnScoreUpdated?.Invoke(distance, coinCount);
    }
    
    public void TakeDamage()
    {
        if (isGameOver) return;
        
        currentHealth--;
        OnHealthChanged?.Invoke(currentHealth);
        
        if (currentHealth <= 0)
        {
            GameOver();
        }
    }
    
    public void CollectCoin()
    {
        if (isGameOver) return;
        
        coinCount++;
        OnCoinCollected?.Invoke(coinCount);
        OnScoreUpdated?.Invoke(distance, coinCount);
    }
    
    private void GameOver()
    {
        isGameOver = true;
        OnGameOver?.Invoke();
    }
    
    public bool IsGameOver()
    {
        return isGameOver;
    }
    
    public bool IsGameStarted()
    {
        return isGameStarted;
    }
    
    public int GetTotalScore()
    {
        return Mathf.RoundToInt(distance) + (coinCount * coinScoreValue);
    }
    
    public float GetDistance()
    {
        return distance;
    }
    
    public int GetCoinCount()
    {
        return coinCount;
    }
    
    public int GetHealth()
    {
        return currentHealth;
    }
}

