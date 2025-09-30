using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    enum OptionType
    {
        Continue,
        Restart,
        Setting,
        Title
    }
    int move;
    int position;
    RectTransform RectTransform_get;

    OptionType ctype;
    public static bool setting;

    public GameObject pauseMenuUI; // ポーズ時に表示するUI
    public GameObject optionUI; // ポーズ時に表示するUI

    private void Start()
    {
        RectTransform_get = gameObject.GetComponent<RectTransform>();
        ctype = OptionType.Continue;
        setting = false;
        position = 0;
        move = 0;
    }

    void Update()
    {
        //float unscaledDelta = Time.unscaledDeltaTime;
        if (position == move)
        {
            if (PouseDisplay.volume == false && PouseDisplay.help == false)
            {
                if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("Vertical1") <= -1 || Input.GetAxis("Vertical2") <= -1)
                {
                    if (ctype != OptionType.Title)
                    {
                        ctype++;
                        move -= 40;
                    }
                    else
                    {
                        ctype -= 3;
                        move += 120;
                    }
                }

                if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("Vertical1") >= 1 || Input.GetAxis("Vertical2") >= 1))
                {
                    if (ctype != OptionType.Continue)
                    {
                        ctype--;
                        move += 40;
                    }
                    else
                    {
                        ctype += 3;
                        move -= 120;
                    }
                }
            }


            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 0"))
            {

                if (setting == false)// タイトル
                {
                    switch (ctype)
                    {
                        case OptionType.Continue: FindObjectOfType<PouseManager>()?.Resume(); break; // 続ける
                        case OptionType.Restart: FindObjectOfType<Fade>()?.REChange(); break; // リスタート
                        case OptionType.Setting: setting = true;
                            ctype = OptionType.Continue;
                            Vector3 pos = RectTransform_get.position;
                            pos.y += 400.0f;
                            RectTransform_get.position = pos;
                            pauseMenuUI.SetActive(false);
                            optionUI.SetActive(true);
                            FindObjectOfType<PouseDisplay>()?.OperationChange();
                            break; // ポーズ→オプション
                        case OptionType.Title: FindObjectOfType<Fade>()?.TiChange(); ; break;//タイトルへ

                        default: break;
                    }
                }
                else
                {
                    switch (ctype) // オプション
                    {
                        case OptionType.Continue: FindObjectOfType<PouseDisplay>()?.VolumeChange(); break; // 音量調整
                        case OptionType.Restart: FindObjectOfType<CorM>()?.OperatorChange();
                            FindObjectOfType<PouseDisplay>()?.OperationChange();
                            break; // 操作変更
                        case OptionType.Setting: FindObjectOfType<PouseDisplay>()?.HelpChange(); break; // 操作説明
                        case OptionType.Title: setting = false;
                            ctype = OptionType.Continue;
                            Vector3 pos = RectTransform_get.position;
                            pos.y += 600.0f;
                            RectTransform_get.position = pos;
                            pauseMenuUI.SetActive(true);
                            optionUI.SetActive(false);
                            break; // オプション→ポーズ
                        default: break;
                    }
                }

                // SE再生
                AudioManager.Instance.PlaySE(SEID.SE_ButtonDecision);
            }
        }
        else
        {
            Vector3 pos = RectTransform_get.position;
            if (position < move)
            {
                pos.y += 5.0f;
                position++;
            }
            else
            {
                pos.y -= 5.0f;
                position--;
            }
            RectTransform_get.position = pos;


        }

    }

}
