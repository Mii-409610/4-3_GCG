using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField, Header("マウスの感度")]
    public float mouseSensitivity = 100.0f;

    [SerializeField, Header("追従するカメラオブジェクト")]
    public Transform cameraObject;

    // 上下の視点回転を制御するための変数
    float xRotation = 0.0f;

    // 左右の視点回転を制御するための変数
    float yRotation = 0.0f;

    // 縦回転角度制限(最小)
    float yAngleLimitMin = -90.0f;

    // 縦回転角度制限(最大)
    float yAngleLimitMax = 90.0f;

    void Awake()
    {
        // =========================================
        //  エラー処理
        // =========================================
        if (GameObject.FindWithTag("MainCamera") == null)
        {
            Debug.LogError("MainCameraが見つかりません。タグを確認してください。");
            return;
        }
        cameraObject = GameObject.FindWithTag("MainCamera").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        // マウスカーソルを画面中央にロックして、見えないようにする
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        cameraObject.position = transform.position + Vector3.up;

        // マウスの移動量を取得
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        yRotation += mouseX;

        xRotation = Mathf.Clamp(xRotation, yAngleLimitMin, yAngleLimitMax);

        cameraObject.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }
}
