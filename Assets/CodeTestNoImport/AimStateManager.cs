using UnityEngine;

public class AimStateManager : MonoBehaviour
{
    [Header("Cài đặt Chuột")]
    public float mouseSensitivity = 3f;
    [SerializeField] private Transform camFollowPos;

    private float xAxis;
    private float yAxis;
    private Transform myTransform; // Cache transform để tối ưu hiệu suất

    void Start()
    {
        myTransform = transform;

        // Khóa con trỏ chuột vào giữa màn hình và ẩn nó đi để dễ điều khiển
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        // 1. Nhận tín hiệu di chuyển của chuột
        xAxis += Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        yAxis -= Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        // 2. Giới hạn góc nhìn ngước lên / cúi xuống (giống hệt Min/Max của AxisState)
        yAxis = Mathf.Clamp(yAxis, -60f, 60f);

        // 3. Xoay góc nhìn lên/xuống của Camera (áp dụng vào cục CameraFollowPos)
        Vector3 camAngles = camFollowPos.localEulerAngles;
        camAngles.x = yAxis;
        camFollowPos.localEulerAngles = camAngles;

        // 4. Xoay nhân vật sang trái/phải
        Vector3 playerAngles = myTransform.eulerAngles;
        playerAngles.y = xAxis;
        myTransform.eulerAngles = playerAngles;
    }
}