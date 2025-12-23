using UnityEngine;
using System.Collections.Generic;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float spawnDistance = 20f;
    [SerializeField] private float despawnDistance = -10f;
    [SerializeField] private float coinSpawnChance = 0.3f; // 30%の確率でコイン生成
    
    [Header("Lane Positions")]
    [SerializeField] private float leftLaneX = -2f;
    [SerializeField] private float centerLaneX = 0f;
    [SerializeField] private float rightLaneX = 2f;
    
    private float nextSpawnTime = 0f;
    private List<GameObject> activeObstacles = new List<GameObject>();
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
        if (GameManager.Instance == null || !GameManager.Instance.IsGameStarted() || GameManager.Instance.IsGameOver())
        {
            return;
        }
        
        // 障害物生成
        if (Time.time >= nextSpawnTime)
        {
            SpawnObstacle();
            
            // コインもランダムに生成
            if (coinPrefab != null && Random.value < coinSpawnChance)
            {
                SpawnCoin();
            }
            
            nextSpawnTime = Time.time + spawnInterval;
        }
        
        // 画面外の障害物を削除
        CleanupObstacles();
    }
    
    private void SpawnObstacle()
    {
        if (obstaclePrefab == null) return;
        
        // ランダムなレーンを選択（0, 1, 2）
        int randomLane = Random.Range(0, 3);
        float laneX = GetLaneX(randomLane);
        
        Vector3 spawnPosition = new Vector3(laneX, 0.5f, spawnDistance);
        GameObject obstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
        activeObstacles.Add(obstacle);
    }
    
    private void SpawnCoin()
    {
        if (coinPrefab == null) return;
        
        // 障害物と異なるレーンにコインを配置
        int randomLane = Random.Range(0, 3);
        float laneX = GetLaneX(randomLane);
        
        Vector3 spawnPosition = new Vector3(laneX, 0.5f, spawnDistance);
        Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
    }
    
    private float GetLaneX(int lane)
    {
        switch (lane)
        {
            case 0:
                return leftLaneX;
            case 1:
                return centerLaneX;
            case 2:
                return rightLaneX;
            default:
                return centerLaneX;
        }
    }
    
    private void CleanupObstacles()
    {
        if (playerTransform == null) return;
        
        for (int i = activeObstacles.Count - 1; i >= 0; i--)
        {
            if (activeObstacles[i] == null)
            {
                activeObstacles.RemoveAt(i);
                continue;
            }
            
            float distanceBehind = playerTransform.position.z - activeObstacles[i].transform.position.z;
            
            if (distanceBehind > Mathf.Abs(despawnDistance))
            {
                Destroy(activeObstacles[i]);
                activeObstacles.RemoveAt(i);
            }
        }
    }
    
    public void ClearAllObstacles()
    {
        foreach (GameObject obstacle in activeObstacles)
        {
            if (obstacle != null)
            {
                Destroy(obstacle);
            }
        }
        activeObstacles.Clear();
    }
}

