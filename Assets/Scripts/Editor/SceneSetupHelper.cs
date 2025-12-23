using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public class SceneSetupHelper : EditorWindow
{
    [MenuItem("Tools/Setup Runner Game Scene")]
    public static void SetupScene()
    {
        // 新しいシーンを作成
        Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        
        // カメラを調整
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            mainCamera.transform.position = new Vector3(0, 5, -10);
            mainCamera.transform.rotation = Quaternion.Euler(15, 0, 0);
        }
        
        // 地面を作成
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground";
        ground.transform.position = Vector3.zero;
        ground.transform.localScale = new Vector3(10, 1, 50);
        
        // プレイヤーを作成
        GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.name = "Player";
        player.transform.position = new Vector3(0, 1, 0);
        player.tag = "Player";
        
        // PlayerControllerを追加
        PlayerController playerController = player.AddComponent<PlayerController>();
        
        // ColliderをTriggerに設定
        CapsuleCollider playerCollider = player.GetComponent<CapsuleCollider>();
        if (playerCollider != null)
        {
            playerCollider.isTrigger = false;
        }
        
        // Rigidbodyを追加（物理演算用）
        Rigidbody playerRb = player.AddComponent<Rigidbody>();
        playerRb.isKinematic = true;
        playerRb.useGravity = false;
        
        // GameManagerを作成
        GameObject gameManager = new GameObject("GameManager");
        gameManager.AddComponent<GameManager>();
        
        // ObstacleSpawnerを作成
        GameObject obstacleSpawner = new GameObject("ObstacleSpawner");
        ObstacleSpawner spawner = obstacleSpawner.AddComponent<ObstacleSpawner>();
        
        // Canvasを作成
        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
        canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        
        // EventSystemを作成
        GameObject eventSystem = new GameObject("EventSystem");
        eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
        eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        
        // UI要素を配置
        UIReferences uiRefs = CreateUIElements(canvasObj.transform);
        
        // UIManagerを作成（参照はAwakeで自動検索される）
        GameObject uiManager = new GameObject("UIManager");
        uiManager.AddComponent<UIManager>();
        
        // AudioManagerを作成
        GameObject audioManager = new GameObject("AudioManager");
        audioManager.AddComponent<AudioManager>();
        
        // Prefabが存在する場合はObstacleSpawnerに設定
        GameObject obstaclePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Obstacle.prefab");
        GameObject coinPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Coin.prefab");
        if (obstaclePrefab != null || coinPrefab != null)
        {
            SerializedObject spawnerSerialized = new SerializedObject(spawner);
            if (obstaclePrefab != null)
            {
                spawnerSerialized.FindProperty("obstaclePrefab").objectReferenceValue = obstaclePrefab;
            }
            if (coinPrefab != null)
            {
                spawnerSerialized.FindProperty("coinPrefab").objectReferenceValue = coinPrefab;
            }
            spawnerSerialized.ApplyModifiedProperties();
        }
        
        // シーンを保存
        string scenePath = "Assets/Scenes/GameScene.unity";
        EditorSceneManager.SaveScene(newScene, scenePath);
        
        Debug.Log("Scene setup complete! Please assign audio clips to AudioManager if needed.");
    }
    
    private class UIReferences
    {
        public TMPro.TextMeshProUGUI scoreText;
        public TMPro.TextMeshProUGUI distanceText;
        public TMPro.TextMeshProUGUI coinText;
        public GameObject healthPanel;
        public UnityEngine.UI.Image[] healthIcons;
        public GameObject helpPanel;
        public GameObject gameOverPanel;
        public TMPro.TextMeshProUGUI gameOverScoreText;
        public GameObject startPrompt;
    }
    
    private static UIReferences CreateUIElements(Transform canvasParent)
    {
        UIReferences refs = new UIReferences();
        
        // スコアテキスト
        GameObject scoreTextObj = new GameObject("ScoreText");
        scoreTextObj.transform.SetParent(canvasParent, false);
        refs.scoreText = scoreTextObj.AddComponent<TMPro.TextMeshProUGUI>();
        refs.scoreText.text = "Score: 0";
        refs.scoreText.fontSize = 24;
        refs.scoreText.alignment = TMPro.TextAlignmentOptions.TopLeft;
        RectTransform scoreRect = scoreTextObj.GetComponent<RectTransform>();
        scoreRect.anchorMin = new Vector2(0, 1);
        scoreRect.anchorMax = new Vector2(0, 1);
        scoreRect.anchoredPosition = new Vector2(10, -30);
        scoreRect.sizeDelta = new Vector2(200, 30);
        
        // 距離テキスト
        GameObject distanceTextObj = new GameObject("DistanceText");
        distanceTextObj.transform.SetParent(canvasParent, false);
        refs.distanceText = distanceTextObj.AddComponent<TMPro.TextMeshProUGUI>();
        refs.distanceText.text = "Distance: 0m";
        refs.distanceText.fontSize = 20;
        refs.distanceText.alignment = TMPro.TextAlignmentOptions.TopLeft;
        RectTransform distanceRect = distanceTextObj.GetComponent<RectTransform>();
        distanceRect.anchorMin = new Vector2(0, 1);
        distanceRect.anchorMax = new Vector2(0, 1);
        distanceRect.anchoredPosition = new Vector2(10, -60);
        distanceRect.sizeDelta = new Vector2(200, 30);
        
        // コインテキスト
        GameObject coinTextObj = new GameObject("CoinText");
        coinTextObj.transform.SetParent(canvasParent, false);
        refs.coinText = coinTextObj.AddComponent<TMPro.TextMeshProUGUI>();
        refs.coinText.text = "Coins: 0";
        refs.coinText.fontSize = 20;
        refs.coinText.alignment = TMPro.TextAlignmentOptions.TopLeft;
        RectTransform coinRect = coinTextObj.GetComponent<RectTransform>();
        coinRect.anchorMin = new Vector2(0, 1);
        coinRect.anchorMax = new Vector2(0, 1);
        coinRect.anchoredPosition = new Vector2(10, -90);
        coinRect.sizeDelta = new Vector2(200, 30);
        
        // 体力パネル
        GameObject healthPanelObj = new GameObject("HealthPanel");
        healthPanelObj.transform.SetParent(canvasParent, false);
        refs.healthPanel = healthPanelObj;
        RectTransform healthPanelRect = healthPanelObj.AddComponent<RectTransform>();
        healthPanelRect.anchorMin = new Vector2(1, 1);
        healthPanelRect.anchorMax = new Vector2(1, 1);
        healthPanelRect.anchoredPosition = new Vector2(-10, -10);
        healthPanelRect.sizeDelta = new Vector2(150, 50);
        
        // 体力アイコン（3つ）
        refs.healthIcons = new UnityEngine.UI.Image[3];
        for (int i = 0; i < 3; i++)
        {
            GameObject healthIcon = new GameObject($"HealthIcon_{i}");
            healthIcon.transform.SetParent(healthPanelObj.transform, false);
            refs.healthIcons[i] = healthIcon.AddComponent<UnityEngine.UI.Image>();
            refs.healthIcons[i].color = Color.red;
            RectTransform iconRect = healthIcon.GetComponent<RectTransform>();
            iconRect.anchorMin = new Vector2(0, 0.5f);
            iconRect.anchorMax = new Vector2(0, 0.5f);
            iconRect.anchoredPosition = new Vector2(20 + i * 40, 0);
            iconRect.sizeDelta = new Vector2(30, 30);
        }
        
        // ヘルプパネル
        GameObject helpPanelObj = new GameObject("HelpPanel");
        helpPanelObj.transform.SetParent(canvasParent, false);
        refs.helpPanel = helpPanelObj;
        UnityEngine.UI.Image helpPanelBg = helpPanelObj.AddComponent<UnityEngine.UI.Image>();
        helpPanelBg.color = new Color(0, 0, 0, 0.8f);
        RectTransform helpPanelRect = helpPanelObj.GetComponent<RectTransform>();
        helpPanelRect.anchorMin = new Vector2(0.5f, 0.5f);
        helpPanelRect.anchorMax = new Vector2(0.5f, 0.5f);
        helpPanelRect.anchoredPosition = Vector2.zero;
        helpPanelRect.sizeDelta = new Vector2(400, 300);
        helpPanelObj.SetActive(false);
        
        GameObject helpTextObj = new GameObject("HelpText");
        helpTextObj.transform.SetParent(helpPanelObj.transform, false);
        TMPro.TextMeshProUGUI helpText = helpTextObj.AddComponent<TMPro.TextMeshProUGUI>();
        helpText.text = "操作方法\n\n←/→ または A/D: レーン切り替え\nH: ヘルプ表示/非表示\nR: リスタート（ゲームオーバー時）";
        helpText.fontSize = 18;
        helpText.alignment = TMPro.TextAlignmentOptions.Center;
        RectTransform helpTextRect = helpTextObj.GetComponent<RectTransform>();
        helpTextRect.anchorMin = Vector2.zero;
        helpTextRect.anchorMax = Vector2.one;
        helpTextRect.sizeDelta = Vector2.zero;
        helpTextRect.anchoredPosition = Vector2.zero;
        
        // ゲームオーバーパネル
        GameObject gameOverPanelObj = new GameObject("GameOverPanel");
        gameOverPanelObj.transform.SetParent(canvasParent, false);
        refs.gameOverPanel = gameOverPanelObj;
        UnityEngine.UI.Image gameOverPanelBg = gameOverPanelObj.AddComponent<UnityEngine.UI.Image>();
        gameOverPanelBg.color = new Color(0, 0, 0, 0.9f);
        RectTransform gameOverPanelRect = gameOverPanelObj.GetComponent<RectTransform>();
        gameOverPanelRect.anchorMin = Vector2.zero;
        gameOverPanelRect.anchorMax = Vector2.one;
        gameOverPanelRect.sizeDelta = Vector2.zero;
        gameOverPanelObj.SetActive(false);
        
        GameObject gameOverTextObj = new GameObject("GameOverScoreText");
        gameOverTextObj.transform.SetParent(gameOverPanelObj.transform, false);
        refs.gameOverScoreText = gameOverTextObj.AddComponent<TMPro.TextMeshProUGUI>();
        refs.gameOverScoreText.text = "Game Over\n\nFinal Score: 0\n\nPress R to Restart";
        refs.gameOverScoreText.fontSize = 24;
        refs.gameOverScoreText.alignment = TMPro.TextAlignmentOptions.Center;
        RectTransform gameOverTextRect = gameOverTextObj.GetComponent<RectTransform>();
        gameOverTextRect.anchorMin = Vector2.zero;
        gameOverTextRect.anchorMax = Vector2.one;
        gameOverTextRect.sizeDelta = Vector2.zero;
        gameOverTextRect.anchoredPosition = Vector2.zero;
        
        // スタートプロンプト
        GameObject startPromptObj = new GameObject("StartPrompt");
        startPromptObj.transform.SetParent(canvasParent, false);
        refs.startPrompt = startPromptObj;
        TMPro.TextMeshProUGUI startPrompt = startPromptObj.AddComponent<TMPro.TextMeshProUGUI>();
        startPrompt.text = "Press Any Key to Start";
        startPrompt.fontSize = 32;
        startPrompt.alignment = TMPro.TextAlignmentOptions.Center;
        RectTransform startPromptRect = startPromptObj.GetComponent<RectTransform>();
        startPromptRect.anchorMin = new Vector2(0.5f, 0.5f);
        startPromptRect.anchorMax = new Vector2(0.5f, 0.5f);
        startPromptRect.anchoredPosition = Vector2.zero;
        startPromptRect.sizeDelta = new Vector2(400, 50);
        
        return refs;
    }
}

