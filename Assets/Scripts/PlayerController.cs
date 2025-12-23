using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float forwardSpeed = 5f;
    [SerializeField] private float laneSwitchSpeed = 10f;
    
    [Header("Lane Positions")]
    [SerializeField] private float leftLaneX = -2f;
    [SerializeField] private float centerLaneX = 0f;
    [SerializeField] private float rightLaneX = 2f;
    
    private int currentLane = 1; // 0 = left, 1 = center, 2 = right
    private bool isSwitchingLanes = false;
    private float targetX;
    private Vector3 startPosition;
    
    private void Start()
    {
        targetX = centerLaneX;
        startPosition = transform.position;
    }
    
    private void Update()
    {
        // 自動前進
        transform.position += Vector3.forward * forwardSpeed * Time.deltaTime;
        
        // レーン切り替え入力
        if (!isSwitchingLanes)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                SwitchLane(-1);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                SwitchLane(1);
            }
        }
        
        // スムーズなレーン移動
        float currentX = transform.position.x;
        if (Mathf.Abs(currentX - targetX) > 0.01f)
        {
            float newX = Mathf.Lerp(currentX, targetX, laneSwitchSpeed * Time.deltaTime);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
            isSwitchingLanes = false;
        }
    }
    
    private void SwitchLane(int direction)
    {
        int newLane = currentLane + direction;
        
        if (newLane >= 0 && newLane <= 2)
        {
            currentLane = newLane;
            isSwitchingLanes = true;
            
            switch (currentLane)
            {
                case 0:
                    targetX = leftLaneX;
                    break;
                case 1:
                    targetX = centerLaneX;
                    break;
                case 2:
                    targetX = rightLaneX;
                    break;
            }
        }
    }
    
    public float GetDistance()
    {
        return transform.position.z - startPosition.z;
    }
    
    public void ResetPosition()
    {
        transform.position = startPosition;
        currentLane = 1;
        targetX = centerLaneX;
        isSwitchingLanes = false;
    }
    
    // レーンの位置を取得するメソッド（LaneVisualizer用）
    public float GetLeftLaneX() { return leftLaneX; }
    public float GetCenterLaneX() { return centerLaneX; }
    public float GetRightLaneX() { return rightLaneX; }
}

