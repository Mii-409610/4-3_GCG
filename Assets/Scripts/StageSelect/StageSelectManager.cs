using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectManager : MonoBehaviour
{
    [SerializeField] GameObject stage1To2Line;      // 線オブジェクト
    [SerializeField] GameObject stage1To2Effect;    // 新しい演出オブジェクト
    [SerializeField] CloudClearEffect cloudClearEffect; // 雲の演出スクリプト

    void Start()
    {
        bool isCleared = PlayerPrefs.GetInt("Stage1Cleared", 0) == 1;

        // 線の表示・非表示
        if (stage1To2Line != null)
            stage1To2Line.SetActive(isCleared);

        // 新しい演出オブジェクトも同じ条件で表示
        if (stage1To2Effect != null)
            stage1To2Effect.SetActive(isCleared);

        // 線の伸びる演出はクリア時のみ
        if (isCleared)
            FindObjectOfType<LineGrowEffect>()?.StartGrow();

        // 雲のクリア演出もクリア時のみ
        if (cloudClearEffect != null)
        {
            if (isCleared)
                cloudClearEffect.StartClearEffect();
            else
                cloudClearEffect.ResetClouds(); // 元の位置に戻して表示
        }

        // BGM再生
        AudioManager.Instance.PlayBGM(BGMID.BGM_StageSelect);
    }

#if UNITY_EDITOR
    void Update()
    {
        // テスト用：Tabキーでセーブデータ初期化
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            PlayerPrefs.SetInt("Stage1Cleared", 1);
            PlayerPrefs.Save();
            Debug.Log("PlayerPrefs Stage1クリア！");
            SceneManager.LoadScene("StageSelect");
        }

        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("PlayerPrefs 初期化！");
            SceneManager.LoadScene("StageSelect");
        }

    }
#endif
}
