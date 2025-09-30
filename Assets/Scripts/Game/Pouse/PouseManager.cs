using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PouseManager : MonoBehaviour
{
    public GameObject pauseMenuUI; // ポーズ時に表示するUI
    public GameObject pausedeleteUI; // ポーズ時に表示するUI

    public static bool isPaused = false;


    void Update()
    {
        if (IsGameManager.isGameEnded) return;

        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("joystick button 7")) && CursorManager.setting == false)
        {
            if (!isPaused)
                Pause();
            else
                Resume();
        }
    }

    public void Resume()// リスタート
    {
        pauseMenuUI.SetActive(false); // UI 表示
        pausedeleteUI.SetActive(true); // UI 非表示
        isPaused = false;
        //Physics.autoSimulation = !PouseManager.isPaused;
        Time.timeScale = 1f;

        // SE再生
        AudioManager.Instance.PlaySE(SEID.SE_OpenMenu);
    }

    public void Pause()// ポーズ中
    {
        pauseMenuUI.SetActive(true); // UI 表示
        pausedeleteUI.SetActive(false); // UI 非表示
        isPaused = true;
        //Physics.autoSimulation = !PouseManager.isPaused;
        Time.timeScale = 0f;

        // SE再生
        AudioManager.Instance.PlaySE(SEID.SE_OpenMenu);
    }
}
