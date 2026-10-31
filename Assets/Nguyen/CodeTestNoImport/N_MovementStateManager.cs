using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovementStateManager : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3f;
    public float gravity = -9.81f;

    [Header("Ground Check")]
    [SerializeField] private float groundYOffset = 0.6f;
    [SerializeField] private LayerMask groundMask;

    private CharacterController controller;
    private Vector3 velocity;
    private Transform myTransform; // Tối ưu Cache

    // --- CÁC BIẾN MỚI DÙNG CHO THANG MÁY ---
    private Transform currentPlatform;
    private Vector3 lastPlatformPosition;
    private Collider[] groundHits = new Collider[1]; // Tối ưu: Không xả rác bộ nhớ

    void Start()
    {
        controller = GetComponent<CharacterController>();
        myTransform = transform;
    }

    void Update()
    {
        ApplyGravity();
        Move();
        TheoThangMay(); // Gọi hàm bám sàn
    }

    void Move()
    {
        // Tối ưu 1: Dùng GetAxisRaw và Normalize để di chuyển chuẩn xác
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 inputDir = new Vector3(h, 0f, v).normalized;

        // Tính hướng dựa trên hướng xoay của nhân vật
        Vector3 moveDir = myTransform.forward * inputDir.z + myTransform.right * inputDir.x;

        controller.Move(moveDir * moveSpeed * Time.deltaTime);
    }

    void ApplyGravity()
    {
        if (!IsGrounded())
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else if (velocity.y < 0)
        {
            velocity.y = -2f; // Ép sát xuống đất khi chạm nền
        }

        controller.Move(velocity * Time.deltaTime);
    }

    bool IsGrounded()
    {
        Vector3 spherePos = myTransform.position;
        spherePos.y -= groundYOffset;

        // Tối ưu 2: Dùng OverlapSphereNonAlloc để lấy thông tin vật đang dẫm lên
        int hits = Physics.OverlapSphereNonAlloc(spherePos, controller.radius - 0.05f, groundHits, groundMask);

        if (hits > 0)
        {
            Transform hitTransform = groundHits[0].transform;

            // Cập nhật dữ liệu nếu bước lên một bệ đỡ/thang máy mới
            if (currentPlatform != hitTransform)
            {
                currentPlatform = hitTransform;
                lastPlatformPosition = currentPlatform.position;
            }
            return true;
        }
        else
        {
            currentPlatform = null;
            return false;
        }
    }

    // --- HÀM MỚI: TỰ ĐỘNG CUỐN THEO THANG MÁY ---
    void TheoThangMay()
    {
        if (currentPlatform != null)
        {
            // Tính xem thang máy vừa nhích đi bao nhiêu mét
            Vector3 delta = currentPlatform.position - lastPlatformPosition;

            if (delta.magnitude > 0.0001f)
            {
                // Ép CharacterController di chuyển theo đúng khoảng cách đó
                controller.Move(delta);
            }

            // Lưu lại vị trí để tính cho khung hình sau
            lastPlatformPosition = currentPlatform.position;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 spherePos = transform.position;
        spherePos.y -= groundYOffset;

        float rad = GetComponent<CharacterController>() != null ? GetComponent<CharacterController>().radius : 0.5f;
        Gizmos.DrawWireSphere(spherePos, rad - 0.05f);
    }
}