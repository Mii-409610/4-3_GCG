using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField, Header("マウスの感度")]
    public float mouseSensitivity = 100.0f;

    [SerializeField, Header("追従する対象")]
    public Transform cameraObject;

    // 上下の視点回転を制御するための変数
    float xRotation = 0.0f;

    // 左右の視点回転を制御するための変数
    float yRotation = 0.0f;

    //// 横回転角度制限(最小)
    //float xAngleLimitMin = -90.0f;

    //// 横回転角度制限(最大)
    //float xAngleLimitMax = 90.0f;

    //// 縦回転角度制限(最小)
    //float yAngleLimitMin = -90.0f;

    //// 縦回転角度制限(最大)
    //float yAngleLimitMax = 90.0f;

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

        //// カメラ角度のリセット
        //xRotation = 0.0f;
        //yRotation = cameraObject.eulerAngles.y;
        //transform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        cameraObject.position = transform.position + Vector3.up;

        // マウスの横方向の移動量を取得し、感度と時間でスケーリング
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        // マウスの縦方向の移動量を取得し、感度と時間でスケーリング
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //// 上下の回転
        //xRotation -= mouseY;
        //xRotation = Mathf.Clamp(xRotation, yAngleLimitMin, yAngleLimitMax); // 回転の制限

        //// 左右の回転
        //yRotation += mouseX;
        //yRotation=Mathf.Clamp(yRotation, xAngleLimitMin, xAngleLimitMax);　// 回転の制限

        //// カメラの上下回転
        //transform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);

        //// プレイヤーの左右回転
        //cameraObject.rotation = Quaternion.Euler(0.0f, yRotation, 0.0f);

        xRotation -= mouseY;
        yRotation += mouseX;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraObject.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
