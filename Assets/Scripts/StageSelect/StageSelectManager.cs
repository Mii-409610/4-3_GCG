using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectManager : MonoBehaviour
{
    [SerializeField] GameObject stage1To2Line; // 線オブジェクト
    [SerializeField] CloudClearEffect cloudClearEffect; // 雲の演出スクリプト

    void Start()
    {
        if (PlayerPrefs.GetInt("Stage1Cleared", 0) == 0)
        {
            // ステージ1と2の線を非表示
            if (stage1To2Line != null)
                stage1To2Line.SetActive(false);

            // 雲の両方を確実に表示（CloudClearEffect内で管理される前提）
            if (cloudClearEffect != null)
                cloudClearEffect.ResetClouds(); // 元の位置に戻して表示
        }
        else if (PlayerPrefs.GetInt("Stage1Cleared", 0) == 1)
        {
            // ステージ1と2の線を表示して伸ばす演出
            if (stage1To2Line != null)
                stage1To2Line.SetActive(true);
            FindObjectOfType<LineGrowEffect>()?.StartGrow();

            // 雲を左右に分けて消す演出
            if (cloudClearEffect != null)
                cloudClearEffect.StartClearEffect();
        }
    }

#if UNITY_EDITOR
    void Update()
    {
        // テスト用：Tabキーでセーブデータ初期化
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("PlayerPrefs 初期化！");
            SceneManager.LoadScene("StageSelect");
        }
    }
#endif
}
