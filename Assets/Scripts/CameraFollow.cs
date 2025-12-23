using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Follow Settings")]
    [SerializeField] private Transform target; // 追尾するターゲット（プレイヤー）
    [SerializeField] private Vector3 offset = new Vector3(0f, 1f, -10f); // カメラのオフセット
    [SerializeField] private float followSpeed = 5f; // 追尾の速度
    
    [Header("Follow Options")]
    [SerializeField] private bool followX = false; // X軸を追尾するか
    [SerializeField] private bool followY = true; // Y軸を追尾するか
    [SerializeField] private bool followZ = true; // Z軸を追尾するか
    
    private Vector3 initialPosition;
    
    private void Start()
    {
        // ターゲットが設定されていない場合は、PlayerControllerを探す
        if (target == null)
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            if (player != null)
            {
                target = player.transform;
            }
        }
        
        initialPosition = transform.position;
    }
    
    private void LateUpdate()
    {
        if (target == null) return;
        
        // ターゲットの位置を取得
        Vector3 targetPosition = target.position;
        
        // 各軸の追尾設定に応じて位置を計算
        float newX = followX ? targetPosition.x + offset.x : initialPosition.x + offset.x;
        float newY = followY ? targetPosition.y + offset.y : initialPosition.y + offset.y;
        float newZ = followZ ? targetPosition.z + offset.z : initialPosition.z + offset.z;
        
        Vector3 desiredPosition = new Vector3(newX, newY, newZ);
        
        // スムーズに追尾
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
    }
}



