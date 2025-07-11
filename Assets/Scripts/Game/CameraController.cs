using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField, Header("カメラの感度")]
    [Range(0.1f, 10f)]
    public float lookSensitivity = 5f;

    [SerializeField, Header("カメラの上限方向の回転角制限")]
    public Vector2 MinMaxAngle = new Vector2(-65, 65);

    // カメラの回転速度
    private float lookSmooth = 0.1f;

    private float yRotation;    // 左右回転角度
    private float xRotation;    // 上下回転角度

    private float currentYRot;  // 現在の左右回転角度
    private float currentXRot;  // 現在の上下回転角度

    private float yRotationVelocity;    // 左右の加速度
    private float xRotationVelocity;    // 上下の加速度

    void Update()
    {
        // マウス入力の取得
        yRotation += Input.GetAxis("Mouse X") * lookSensitivity; //マウスの移動.
        xRotation -= Input.GetAxis("Mouse Y") * lookSensitivity; //マウスの移動.

        // 上下の回転角度を制限
        xRotation = Mathf.Clamp(xRotation, MinMaxAngle.x, MinMaxAngle.y);//上下の角度移動の最大、最小.

        // 現在のx,y回転角度を目標角度に向けて、徐々に近づける
        currentXRot = Mathf.SmoothDamp(currentXRot, xRotation, ref xRotationVelocity, lookSmooth);
        currentYRot = Mathf.SmoothDamp(currentYRot, yRotation, ref yRotationVelocity, lookSmooth);

        // 回転
        transform.rotation = Quaternion.Euler(currentXRot, currentYRot, 0);
    }
}
