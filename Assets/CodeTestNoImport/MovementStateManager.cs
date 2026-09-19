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

    void Start()
    {
        controller = GetComponent<CharacterController>();
        myTransform = transform;
    }

    void Update()
    {
        ApplyGravity();
        Move();
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
        // Tối ưu 2: Không dùng new Vector3 để tránh xả rác bộ nhớ
        Vector3 spherePos = myTransform.position;
        spherePos.y -= groundYOffset;
        return Physics.CheckSphere(spherePos, controller.radius - 0.05f, groundMask);
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