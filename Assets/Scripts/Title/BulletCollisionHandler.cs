using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

/// <summary>
/// 弾がオブジェクトに衝突したときの処理を行うクラス
/// </summary>
public class BulletCollisionHandler : MonoBehaviour
{
    enum OptionType
    {
        Start,
        Continue,
        Setting,
        Exit
    }
    int move;
    int position;
    RectTransform RectTransform_get;

    OptionType ctype;

    private void Start()
    {
        RectTransform_get = gameObject.GetComponent<RectTransform>();
        ctype = OptionType.Start;
        position = 0;
        move = 0;
    }

    void Update()
    {
        if (position == move)
        {
            if (Ondisplay.volume == false && Ondisplay.help == false)
            {
                if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("Vertical1") <= -1 || Input.GetAxis("Vertical2") <= -1)
                {
                    if (ctype != OptionType.Exit)
                    {
                        ctype++;
                        move -= 20;
                    }
                    else
                    {
                        ctype -= 3;
                        move += 60;
                    }
                }

                if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("Vertical1") >= 1 || Input.GetAxis("Vertical2") >= 1))
                {
                    if (ctype != OptionType.Start)
                    {
                        ctype--;
                        move += 20;
                    }
                    else
                    {
                        ctype += 3;
                        move -= 60;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 0"))
            {

                if (Ondisplay.display == false)// タイトル
                {
                    switch (ctype)
                    {
                        case OptionType.Start: FindObjectOfType<Fade>()?.Tchange();
                            PlayerPrefs.DeleteAll();
                            break; // 初めから
                        case OptionType.Continue: FindObjectOfType<Fade>()?.Tchange(); break; // 続きから
                        case OptionType.Setting: FindObjectOfType<Fade>()?.SChange(); break; // タイトル→オプション
                        case OptionType.Exit:
#if UNITY_EDITOR
                            UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
                            break;// ゲームを終了
                        default: break;
                    }
                }
                else
                {
                    switch (ctype) // オプション
                    {
                        case OptionType.Start: FindObjectOfType<Ondisplay>()?.VolumeChange(); break; // 音量調整
                        case OptionType.Continue: FindObjectOfType<CorM>()?.OperatorChange(); break; // 操作変更
                        case OptionType.Setting: FindObjectOfType<Ondisplay>()?.HelpChange(); break; // 操作説明
                        case OptionType.Exit: FindObjectOfType<Fade>()?.SChange(); break; // オプション→タイトル
                        default: break;
                    }
                }

            }
        }
        else
        {
            Vector3 pos = RectTransform_get.position;
            if (position < move)
            {
                pos.y += 0.4f;
                position++;
            }
            else
            {
                pos.y -= 0.4f;
                position--;
            }
            RectTransform_get.position = pos;


        }

    }

    public void CursorReset()
    {
        //Vector3 pos = RectTransform_get.position;
        //pos.y = 0f;
        //RectTransform_get.position = pos;
        //ctype = OptionType.Start;
        Vector3 pos = RectTransform_get.position;
        for (;ctype > 0;)
        {
            ctype--;
            pos.y += 8.0f;
        }
        RectTransform_get.position = pos;
    }


    void OnCollisionEnter(Collision collision)
    {
        /*if (collision.gameObject.CompareTag("Bullet"))
        {
            //Debug.Log($"弾が{type}ボタンに当たった");
            if (Ondisplay.display == false)
            {
                switch (type)// タイトル
                {
                    case ButtonType.Start:
                        FindObjectOfType<Fade>()?.Tchange();// 初めから
                        break;
                    case ButtonType.Continue:
                        // 続きから
                        break;
                    case ButtonType.Setting:
                        FindObjectOfType<Fade>()?.SChange();// オプション
                        break;
                    case ButtonType.Exit:
#if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;// ゲームを終了
#else
                    Application.Quit();
#endif
                        break;
                }
            }
            else
            {
                switch (type) // オプション
                {
                    case ButtonType.Start:
                        // 音量調整
                        break;
                    case ButtonType.Continue:
                        FindObjectOfType<CorM>()?.OperatorChange();// 操作変更
                        break;
                    case ButtonType.Setting:
                        // 遊び方
                        break;
                    case ButtonType.Exit:
                        FindObjectOfType<Fade>()?.SChange();// 戻る
                        break;
                }
            }

            Destroy(collision.gameObject);
        }*/
    }
}
