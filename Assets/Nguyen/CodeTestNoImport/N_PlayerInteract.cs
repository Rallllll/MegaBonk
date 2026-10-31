using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerInteract : MonoBehaviour
{
    public float khoangCach = 5f;
    public TextMeshProUGUI chuGoiThang;
    public Animator animThangMay;

    [Header("Cài đặt Di chuyển")]
    public Transform cucThangMay;
    public Vector3 toaDoTren;  // Tọa độ lúc thang ở trên cao
    public Vector3 toaDoDuoi;  // Tọa độ lúc thang ở dưới đất
    public float tocDoThang = 5f;

    private bool dangXuLy = false;

    void Update()
    {
        if (dangXuLy) return;

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        int layerMask = ~(1 << 2);

        if (Physics.Raycast(ray, out hit, khoangCach, layerMask))
        {
            if (hit.collider.CompareTag("NutThangMay"))
            {
                chuGoiThang.gameObject.SetActive(true);
                if (Input.GetKey(KeyCode.E))
                {
                    StartCoroutine(QuyTrinhThangMay());
                }
            }
            else chuGoiThang.gameObject.SetActive(false);
        }
        else chuGoiThang.gameObject.SetActive(false);
    }

    IEnumerator QuyTrinhThangMay()
    {
        dangXuLy = true;
        chuGoiThang.gameObject.SetActive(false);

        // 1. Dùng localPosition thay vì position để khớp với số trong Inspector
        while (Vector3.Distance(cucThangMay.localPosition, toaDoDuoi) > 0.01f)
        {
            cucThangMay.localPosition = Vector3.MoveTowards(cucThangMay.localPosition, toaDoDuoi, tocDoThang * Time.deltaTime);
            yield return null;
        }

        animThangMay.SetTrigger("Open");
        yield return new WaitForSeconds(3f);

        animThangMay.SetTrigger("Close");
        yield return new WaitForSeconds(1.5f);

        // 2. Đi lên cũng dùng localPosition
        while (Vector3.Distance(cucThangMay.localPosition, toaDoTren) > 0.01f)
        {
            cucThangMay.localPosition = Vector3.MoveTowards(cucThangMay.localPosition, toaDoTren, tocDoThang * Time.deltaTime);
            yield return null;
        }

        animThangMay.SetTrigger("Open");
        yield return new WaitForSeconds(3f);

        animThangMay.SetTrigger("Close");
        yield return new WaitForSeconds(1.5f);

        dangXuLy = false;
    }
}