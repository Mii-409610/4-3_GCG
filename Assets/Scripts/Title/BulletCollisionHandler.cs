using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾がオブジェクトに衝突したときの処理を行うクラス
/// </summary>
public class BulletCollisionHandler : MonoBehaviour
{
    public enum ButtonType
    {
        Start,
        Exit
    }

    public ButtonType type;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            //Debug.Log($"弾が{type}ボタンに当たった");

            switch (type)
            {
                case ButtonType.Start:
                    FindObjectOfType<SceneChanger>()?.ChangeScene("StageSelect");
                    break;
                case ButtonType.Exit:
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
                    break;
            }

            Destroy(collision.gameObject);
        }
    }
}
