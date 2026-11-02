using UnityEngine;

public class N_Turret_Lon : MonoBehaviour
{
    [Header("Gắn các bộ phận Turret")]
    public Transform mountPart;
    public Transform headPart;

    [Header("Gắn điểm bắn và Hiệu ứng")]
    public Transform[] firePoints;
    public bool shootAllAtOnce = false;
    public GameObject muzzleFlashPrefab;

    [Header("Cài đặt Di chuyển")]
    public float turnSpeed = 5f;
    public float minPitch = -90f;
    public float maxPitch = 90f;

    [Header("Cài đặt Tấn công")]
    public float fireRange = 20f;
    public float fireRate = 0.15f;
    private float nextFireTime = 0f;
    private int currentFirePointIndex = 0;

    private Transform target;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
    }

    void Update()
    {
        if (target == null) return;

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        Vector3 aimTarget = target.position + Vector3.up * 1.0f;
        Vector3 dirToTarget = aimTarget - mountPart.position;
        dirToTarget.y = 0;

        if (dirToTarget != Vector3.zero)
        {
            Quaternion mountLook = Quaternion.LookRotation(dirToTarget);
            mountPart.rotation = Quaternion.Slerp(mountPart.rotation, mountLook, Time.deltaTime * turnSpeed);
        }

        Vector3 dirFromHead = aimTarget - headPart.position;
        Vector3 localDir = mountPart.InverseTransformDirection(dirFromHead);

        if (localDir != Vector3.zero)
        {
            Quaternion localLook = Quaternion.LookRotation(localDir);
            float pitch = localLook.eulerAngles.x;
            if (pitch > 180f) pitch -= 360f;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            Quaternion finalHeadRot = Quaternion.Euler(pitch, 0, 0);
            headPart.localRotation = Quaternion.Slerp(headPart.localRotation, finalHeadRot, Time.deltaTime * turnSpeed);
        }

        if (distanceToTarget <= fireRange)
        {
            float angleToTarget = Vector3.Angle(headPart.forward, dirFromHead);
            if (angleToTarget < 5f)
            {
                if (Time.time >= nextFireTime)
                {
                    Shoot();
                    nextFireTime = Time.time + fireRate;
                }
            }
        }
    }

    void Shoot()
    {
        if (muzzleFlashPrefab == null || firePoints.Length == 0) return;

        if (shootAllAtOnce)
        {
            foreach (Transform fp in firePoints)
            {
                if (fp != null)
                {
                    // Tham số thứ 4 (fp) ép tia lửa làm con của nòng súng
                    GameObject flash = Instantiate(muzzleFlashPrefab, fp.position, fp.rotation, fp);
                    Destroy(flash, 0.05f);
                }
            }
        }
        else
        {
            Transform fp = firePoints[currentFirePointIndex];
            if (fp != null)
            {
                GameObject flash = Instantiate(muzzleFlashPrefab, fp.position, fp.rotation, fp);
                Destroy(flash, 0.05f);
            }

            currentFirePointIndex++;
            if (currentFirePointIndex >= firePoints.Length)
            {
                currentFirePointIndex = 0;
            }
        }
    }
}