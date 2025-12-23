using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    [Header("Audio Sources")]
    [SerializeField] private AudioSource coinSource;
    [SerializeField] private AudioSource collisionSource;
    [SerializeField] private AudioSource gameStartSource;
    [SerializeField] private AudioSource gameOverSource;
    
    [Header("Audio Clips")]
    [SerializeField] private AudioClip coinSound;
    [SerializeField] private AudioClip collisionSound;
    [SerializeField] private AudioClip gameStartSound;
    [SerializeField] private AudioClip gameOverSound;
    
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
        // AudioSourceが設定されていない場合は自動生成
        if (coinSource == null)
        {
            coinSource = gameObject.AddComponent<AudioSource>();
        }
        
        if (collisionSource == null)
        {
            collisionSource = gameObject.AddComponent<AudioSource>();
        }
        
        if (gameStartSource == null)
        {
            gameStartSource = gameObject.AddComponent<AudioSource>();
        }
        
        if (gameOverSource == null)
        {
            gameOverSource = gameObject.AddComponent<AudioSource>();
        }
        
        // ゲーム開始時にSE再生
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStart += PlayGameStartSound;
            GameManager.Instance.OnGameOver += PlayGameOverSound;
        }
    }
    
    public void PlayCoinSound()
    {
        if (coinSource != null && coinSound != null)
        {
            coinSource.PlayOneShot(coinSound);
        }
    }
    
    public void PlayCollisionSound()
    {
        if (collisionSource != null && collisionSound != null)
        {
            collisionSource.PlayOneShot(collisionSound);
        }
    }
    
    public void PlayGameStartSound()
    {
        if (gameStartSource != null && gameStartSound != null)
        {
            gameStartSource.PlayOneShot(gameStartSound);
        }
    }
    
    public void PlayGameOverSound()
    {
        if (gameOverSource != null && gameOverSound != null)
        {
            gameOverSource.PlayOneShot(gameOverSound);
        }
    }
    
    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStart -= PlayGameStartSound;
            GameManager.Instance.OnGameOver -= PlayGameOverSound;
        }
    }
}

