using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    [SerializeField] GameObject resultUI;  // InspectorでResult UIパネルを入れる
    //[SerializeField] Score scoreManager;   // スコア表示したいときに使う

    public bool test = false;

    private void Start()
    {
        test = false;

        // 念のため非表示にしておく
        if (resultUI != null)
        {
            resultUI.SetActive(false);
        }
    }

    void Update()
    {
        if (test == true && Input.GetKeyDown(KeyCode.Return))
        {
            // 物理挙動の再開
            Time.timeScale = 1f;

            // ストップオフ
            IsGameManager.isGameEnded = false;

            PlayerPrefs.SetInt("Stage1Cleared", 1);
            PlayerPrefs.Save();

            SceneManager.LoadScene("StageSelect");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // スコア保存
            //if (scoreManager != null) scoreManager.SaveScore();

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
