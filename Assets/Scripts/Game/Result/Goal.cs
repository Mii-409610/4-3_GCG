using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class Goal : MonoBehaviour
{
    [SerializeField] GameObject resultUI;  // InspectorでResult UIパネルを入れる
    [SerializeField] GameObject scoreUI;   // スコア表示したいときに使う

    public bool test = false;

    private void Start()
    {
        test = false;

        // 念のため非表示にしておく
        if (resultUI != null)
        {
            resultUI.SetActive(false);

        }

        // SE停止
        AudioManager.Instance.StopAllSE();

        // BGM停止
        AudioManager.Instance.StopBGM();
    }

    void Update()
    {
        //if (test == true && Input.GetKeyDown(KeyCode.Return))
        //{
        //    PlayerPrefs.SetInt("Stage1Cleared", 1);
        //    PlayerPrefs.Save();

        //    // 物理挙動の再開
        //    Time.timeScale = 1f;

        //    // ストップオフ
        //    IsGameManager.isGameEnded = false;
        //}
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {

            // 念のため非表示にしておく
            if (scoreUI != null)
            {
                scoreUI.SetActive(false);
            }

            Debug.Log("ゴールした");

            // スコアを保存
            int score = ScoreManager.Instance.GetScore();
            PlayerPrefs.SetInt("LastScore", score);
            PlayerPrefs.Save();

            // リザルトUI表示
            if (resultUI != null) resultUI.SetActive(true);

            // ゲーム停止（個別処理）
            IsGameManager.isGameEnded = true;

            // 全部の物理挙動を止める
            //Time.timeScale = 0f;

            // プレイヤーのSplineAnimateを止める
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true; // 必要なら物理シミュレーションも止める
            }

            StopAllBullets();

            // ステージクリア情報保存
            PlayerPrefs.SetInt("Stage1Cleared", 1);
            PlayerPrefs.Save();

            test = true;

            // BGM再生
            AudioManager.Instance.PlayBGM(BGMID.BGM_Clear);
        }
    }

    void StopAllBullets()
    {
        // タグの物理挙動と慣性を止める
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");

        foreach (GameObject bullet in bullets)
        {
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;
            }
        }
    }
}
