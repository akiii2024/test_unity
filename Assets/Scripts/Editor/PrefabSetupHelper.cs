using UnityEngine;
using UnityEditor;

public class PrefabSetupHelper
{
    [MenuItem("Tools/Create Obstacle Prefab")]
    public static void CreateObstaclePrefab()
    {
        // 障害物オブジェクトを作成
        GameObject obstacle = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obstacle.name = "Obstacle";
        obstacle.transform.localScale = new Vector3(1, 1, 1);
        
        // マテリアルを設定（赤色）
        Renderer renderer = obstacle.GetComponent<Renderer>();
        Material material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        material.color = Color.red;
        renderer.material = material;
        
        // ColliderをTriggerに設定
        BoxCollider collider = obstacle.GetComponent<BoxCollider>();
        collider.isTrigger = true;
        
        // Obstacleスクリプトを追加
        obstacle.AddComponent<Obstacle>();
        
        // Prefabとして保存
        string prefabPath = "Assets/Prefabs/Obstacle.prefab";
        EnsurePrefabDirectoryExists();
        
        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(obstacle, prefabPath);
        Object.DestroyImmediate(obstacle);
        
        Debug.Log($"Obstacle prefab created at {prefabPath}");
    }
    
    [MenuItem("Tools/Create Coin Prefab")]
    public static void CreateCoinPrefab()
    {
        // コインオブジェクトを作成
        GameObject coin = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        coin.name = "Coin";
        coin.transform.localScale = new Vector3(0.5f, 0.1f, 0.5f);
        
        // マテリアルを設定（金色）
        Renderer renderer = coin.GetComponent<Renderer>();
        Material material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        material.color = new Color(1f, 0.84f, 0f); // 金色
        material.SetFloat("_Metallic", 0.8f);
        material.SetFloat("_Smoothness", 0.9f);
        renderer.material = material;
        
        // ColliderをTriggerに設定
        SphereCollider collider = coin.GetComponent<SphereCollider>();
        collider.isTrigger = true;
        collider.radius = 0.5f;
        
        // Coinスクリプトを追加
        coin.AddComponent<Coin>();
        
        // パーティクルシステムを追加（収集エフェクト用）
        GameObject effectObj = new GameObject("CollectEffect");
        effectObj.transform.SetParent(coin.transform);
        effectObj.transform.localPosition = Vector3.zero;
        
        ParticleSystem particles = effectObj.AddComponent<ParticleSystem>();
        var main = particles.main;
        main.startColor = new Color(1f, 0.84f, 0f);
        main.startSize = 0.2f;
        main.startLifetime = 0.5f;
        main.maxParticles = 20;
        main.simulationSpace = ParticleSystemSimulationSpace.Local;
        
        var emission = particles.emission;
        emission.enabled = false; // 手動で再生
        
        var shape = particles.shape;
        shape.enabled = true;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.5f;
        
        // Prefabとして保存
        string prefabPath = "Assets/Prefabs/Coin.prefab";
        EnsurePrefabDirectoryExists();
        
        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(coin, prefabPath);
        Object.DestroyImmediate(coin);
        
        Debug.Log($"Coin prefab created at {prefabPath}");
    }
    
    [MenuItem("Tools/Create All Prefabs")]
    public static void CreateAllPrefabs()
    {
        CreateObstaclePrefab();
        CreateCoinPrefab();
        Debug.Log("All prefabs created!");
    }
    
    private static void EnsurePrefabDirectoryExists()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Prefabs"))
        {
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        }
    }
}

