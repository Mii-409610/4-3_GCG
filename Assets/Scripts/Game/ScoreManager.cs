using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// スコア表示用のUIの制御を行うマネージャークラス
/// </summary>
public class ScoreManager : MonoBehaviour
{
    // 子オブジェクトにアタッチされたScoreUIを格納する配列
    private ScoreUI[] score;

     void Start()
    {
        // 子オブジェクトの数に合わせて配列のサイズを調整
        Array.Resize(ref score, transform.childCount);

        // ScoreUIを取得して配列に格納
        for(int i = 0; i < transform.childCount; i++)
        {
            score[i] = transform.GetChild(i).GetComponent<ScoreUI>();
        }
    }

    /// <summary>
    /// スコアの数値を渡して、各行ごとにUIを更新する処理
    /// </summary>
    /// <param name="scoreValue">スコアの数値</param>
    public void OnUpdateScore(int scoreValue)
    {
        int digit = scoreValue;

        // 各行の値を取り出して、対応するScoreUIに渡す
        for (int i = 0; i < transform.childCount; i++)
        {
            // 1の位、10の位、100の位...の順に処理
            score[i].OnUpdateScore(digit % 10);
            digit = digit / 10;
        }
    }
    void Update()
    {
        // 毎フレーム固定スコアを表示
        OnUpdateScore(357);
    }
}


//using System;
//using UnityEngine;
//using UnityEngine.UI;

//public class ScoreManager : MonoBehaviour
//{
//    // 各桁のスコア表示用UI Image（10個用意）
//    public Image[] digitImages = new Image[10];

//    // 数字（0〜9）のスプライト画像（10個）
//    public Sprite[] numberSprites = new Sprite[10];

//    // 現在のスコア値（整数）
//    public int score = 0;

//    // 他のスクリプトからスコアを加算できるようにする関数（未実装）
//    internal static void AddScore(int v)
//    {
//        throw new NotImplementedException(); // あとで実装予定
//    }

//    // ゲーム開始時にスコアを一度表示する
//    void Start()
//    {
//        UpdateScoreDisplay();
//    }

//    // 毎フレーム呼ばれる（スペースキーでデバッグ表示）
//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.Space))
//        {
//            UpdateScoreDisplay(); // スコア表示を更新
//        }
//    }

//    // スコア値に応じてImageを更新する処理
//    void UpdateScoreDisplay()
//    {
//        // スコアを10桁文字列に変換（例：0000000123）
//        string scoreStr = score.ToString().PadLeft(10, '0');

//        // 各桁ごとにスプライトを設定
//        for (int i = 0; i < 10; i++)
//        {
//            int digit = int.Parse(scoreStr[i].ToString()); // 文字を数値に変換
//            digitImages[i].sprite = numberSprites[digit];  // 該当する数字のスプライトを設定
//        }
//    }
//}