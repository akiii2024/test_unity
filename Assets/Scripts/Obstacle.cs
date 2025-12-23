using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float moveSpeed = 5f;
    
    private bool hasHitPlayer = false;
    
    private void Update()
    {
        // 障害物を後ろに移動（プレイヤーが前進するため、相対的に後ろに移動）
        transform.position += Vector3.back * moveSpeed * Time.deltaTime;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasHitPlayer)
        {
            hasHitPlayer = true;
            
            if (GameManager.Instance != null)
            {
                GameManager.Instance.TakeDamage();
                
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlayCollisionSound();
                }
            }
            
            // 衝突後は非アクティブ化（オブジェクトプールの代わり）
            gameObject.SetActive(false);
        }
    }
    
    private void OnEnable()
    {
        hasHitPlayer = false;
    }
}

