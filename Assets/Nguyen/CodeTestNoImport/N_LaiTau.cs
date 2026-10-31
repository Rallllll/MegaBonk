using UnityEngine;
using Unity.Cinemachine; // Bắt buộc dùng thư viện này ở Unity 6

public class N_LaiTau : MonoBehaviour
{
    public CinemachineSplineCart cart;
    public float tocDo = 15f;

    void Start()
    {
        // Tự động tìm xe trượt nếu bạn quên kéo thả
        if (cart == null) cart = GetComponentInParent<CinemachineSplineCart>();
    }

    void Update()
    {
        float input = Input.GetAxis("Vertical");
        if (input != 0)
        {
            // Tăng giảm vị trí theo phím bấm. Xe sẽ tự bẻ lái theo ray.
            cart.SplinePosition += input * tocDo * Time.deltaTime;
        }
    }
}