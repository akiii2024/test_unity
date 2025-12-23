using UnityEngine;

public class LaneVisualizer : MonoBehaviour
{
    [Header("Lane Positions")]
    [SerializeField] private bool autoDetectFromPlayer = true; // PlayerControllerから自動取得
    [SerializeField] private float leftLaneX = -2f;
    [SerializeField] private float centerLaneX = 0f;
    [SerializeField] private float rightLaneX = 2f;
    
    [Header("Line Settings")]
    [SerializeField] private float lineLength = 100f; // 線の長さ（Z方向）
    [SerializeField] private float lineHeight = 0.1f; // 線の高さ（Y位置）
    [SerializeField] private float lineWidth = 0.05f; // 線の太さ
    [SerializeField] private Color lineColor = Color.yellow; // 線の色
    [SerializeField] private Material lineMaterial; // 線のマテリアル（オプション）
    
    private LineRenderer[] laneLines;
    
    private void Start()
    {
        // PlayerControllerからレーンの位置を自動取得
        if (autoDetectFromPlayer)
        {
            PlayerController player = FindFirstObjectByType<PlayerController>();
            if (player != null)
            {
                leftLaneX = player.GetLeftLaneX();
                centerLaneX = player.GetCenterLaneX();
                rightLaneX = player.GetRightLaneX();
            }
        }
        
        CreateLaneLines();
    }
    
    private void CreateLaneLines()
    {
        // 3本の線を作成（左、中央、右）
        laneLines = new LineRenderer[3];
        float[] lanePositions = { leftLaneX, centerLaneX, rightLaneX };
        
        for (int i = 0; i < 3; i++)
        {
            GameObject lineObject = new GameObject($"LaneLine_{i}");
            lineObject.transform.SetParent(transform);
            
            LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
            laneLines[i] = lineRenderer;
            
            // 線の設定
            lineRenderer.positionCount = 2;
            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;
            lineRenderer.material = lineMaterial != null ? lineMaterial : CreateDefaultMaterial();
            lineRenderer.startColor = lineColor;
            lineRenderer.endColor = lineColor;
            lineRenderer.useWorldSpace = true;
            
            // 線の位置を設定（Z方向に長い線）
            Vector3 startPos = new Vector3(lanePositions[i], lineHeight, -lineLength / 2f);
            Vector3 endPos = new Vector3(lanePositions[i], lineHeight, lineLength / 2f);
            
            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, endPos);
        }
    }
    
    private Material CreateDefaultMaterial()
    {
        // デフォルトのマテリアルを作成
        Material mat = new Material(Shader.Find("Sprites/Default"));
        mat.color = lineColor;
        return mat;
    }
    
    private void Update()
    {
        // プレイヤーの位置に合わせて線を更新（オプション）
        UpdateLinePositions();
    }
    
    private void UpdateLinePositions()
    {
        if (laneLines == null) return;
        
        PlayerController player = FindFirstObjectByType<PlayerController>();
        if (player == null) return;
        
        float playerZ = player.transform.position.z;
        float[] lanePositions = { leftLaneX, centerLaneX, rightLaneX };
        
        for (int i = 0; i < 3; i++)
        {
            if (laneLines[i] == null) continue;
            
            // プレイヤーの前後を表示範囲とする
            Vector3 startPos = new Vector3(lanePositions[i], lineHeight, playerZ - lineLength / 2f);
            Vector3 endPos = new Vector3(lanePositions[i], lineHeight, playerZ + lineLength / 2f);
            
            laneLines[i].SetPosition(0, startPos);
            laneLines[i].SetPosition(1, endPos);
        }
    }
    
    // レーンの位置を更新する場合に使用
    public void UpdateLanePositions(float left, float center, float right)
    {
        leftLaneX = left;
        centerLaneX = center;
        rightLaneX = right;
        
        if (laneLines != null)
        {
            DestroyLaneLines();
            CreateLaneLines();
        }
    }
    
    private void DestroyLaneLines()
    {
        if (laneLines == null) return;
        
        foreach (LineRenderer line in laneLines)
        {
            if (line != null && line.gameObject != null)
            {
                Destroy(line.gameObject);
            }
        }
        
        laneLines = null;
    }
    
    private void OnDestroy()
    {
        DestroyLaneLines();
    }
}

