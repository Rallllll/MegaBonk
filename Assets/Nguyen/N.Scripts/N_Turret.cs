using UnityEngine;

public class N_Turret : MonoBehaviour
{
    private Transform target;
    public float rotationSpeed = 5f; // Tốc độ xoay của nòng súng

    void Start()
    {
        // Tự động quét trên bản đồ tìm object có tag "Player"
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            target = playerObj.transform;
        }
    }

    void Update()
    {
        if (target == null) return;

        // Tính hướng từ Turret đến Player
        Vector3 direction = target.position - transform.position;

        // BỎ COMMENT dòng dưới nếu bạn chỉ muốn Turret xoay ngang (trục Y), không bị chúi đầu xuống đất
        // direction.y = 0; 

        // Tính toán góc quay
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        // Nội suy để Turret xoay từ từ (không bị giật cục)
        Vector3 rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles;

        // Áp dụng góc xoay mới
        transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
    }
}