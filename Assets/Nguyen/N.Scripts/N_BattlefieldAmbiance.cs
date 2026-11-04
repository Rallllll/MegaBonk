using UnityEngine;
using System.Collections;

public class N_BattlefieldAmbiance : MonoBehaviour
{
    [Header("Thời gian chờ giữa các vụ nổ (Giây)")]
    public float minWaitTime = 5f;
    public float maxWaitTime = 15f;

    private AudioSource audioSource;
    private MonoBehaviour impulseSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // Tự động tìm Component rung của Unity 6
        impulseSource = GetComponent("CinemachineImpulseSource") as MonoBehaviour;

        StartCoroutine(SimulateWarzone());
    }

    IEnumerator SimulateWarzone()
    {
        while (true)
        {
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);

            // Đổi nhẹ cao độ âm thanh mỗi lần nổ
            audioSource.pitch = Random.Range(0.85f, 1.15f);
            audioSource.Play();

            // Kích hoạt cú rung màn hình
            if (impulseSource != null)
            {
                impulseSource.SendMessage("GenerateImpulse", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}