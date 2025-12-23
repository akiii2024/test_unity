using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float rotationSpeed = 90f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private GameObject collectEffectPrefab;
    [SerializeField] private float despawnDistance = -15f;
    
    private bool isCollected = false;
    private Transform playerTransform;
    
    private void Start()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }
    
    private void Update()
    {
        if (isCollected) return;
        
        // コインを回転
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        
        // コインを後ろに移動
        transform.position += Vector3.back * moveSpeed * Time.deltaTime;
        
        // 画面外に出たら削除
        if (playerTransform != null)
        {
            float distanceBehind = playerTransform.position.z - transform.position.z;
            if (distanceBehind > Mathf.Abs(despawnDistance))
            {
                Destroy(gameObject);
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            CollectCoin();
        }
    }
    
    private void CollectCoin()
    {
        isCollected = true;
        
        // エフェクト再生
        if (collectEffectPrefab != null)
        {
            Instantiate(collectEffectPrefab, transform.position, Quaternion.identity);
        }
        
        // スコア加算
        if (GameManager.Instance != null)
        {
            GameManager.Instance.CollectCoin();
        }
        
        // 音響効果
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayCoinSound();
        }
        
        // コインを削除
        Destroy(gameObject);
    }
}

