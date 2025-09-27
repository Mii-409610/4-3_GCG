using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// スコア表示用のUIの制御を行うクラス
/// </summary>
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; } // シングルトンインスタンス

    [SerializeField, Header("各桁のスコア表示用UI")] 
    public Image[] digitImages; // 右端から左端へ

    [SerializeField, Header("0～9の画像")]
    public Sprite[] numberSprites;

    private int targetScore = 0;    // 最終的なスコア
    private int displayScore = 0;   // 表示用スコア

    private Coroutine scoreCoroutine; // スコア加算演出用コルーチン

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (digitImages == null || digitImages.Length == 0)
        {
            Debug.LogError("ScoreManager: digitImagesが設定されていません。Imageを割り当ててください。");
        }
        if(numberSprites == null || numberSprites.Length != 10)
        {
            Debug.LogError("ScoreManager: numberSpritesが設定されていません。Spriteを割り当ててください。");
        }
    }

    /// <summary>
    /// スコア加算処理
    /// </summary>
    public void AddScore(int value)
    {
        targetScore += value;

        // スコア加算演出
        if(scoreCoroutine != null)
        {
            StopCoroutine(scoreCoroutine);
        }
        scoreCoroutine = StartCoroutine(ScoreCountUp(0.5f));
    }

    /// <summary>
    /// スコア加算演出用コルーチン
    /// </summary>
    private IEnumerator ScoreCountUp(float duration)
    {
        float elapsed = 0f;
        int startScore = displayScore;
        int diff = targetScore - startScore;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            displayScore = startScore + Mathf.RoundToInt(diff + t);
            ShowScore(displayScore);
            yield return null;
        }
        displayScore = targetScore; // 最終値に合わせる
        ShowScore(displayScore);    // 最終表示
        scoreCoroutine = null;
    }

    void Update()
    {
        ShowScore(displayScore);
    }

    /// <summary>
    /// スコア表示処理
    /// </summary>
    public void ShowScore(int value)
    {
        displayScore = Mathf.Max(0, value); // スコア値を0以上に補正

        // 全桁を非表示
        foreach(var image in digitImages)
        {
            image.enabled = false;
        }

        // スコアが0の場合は一桁目に0表示
        if (value == 0)
        {
            digitImages[0].sprite = numberSprites[0];
            digitImages[0].enabled = true;
            return;
        }

        int tempScore = displayScore;   // 表示用の一時変数
        int digitsUsed = 0;             // 使った桁数(右端から)

        // スコアの桁が上がったら次の桁表示
        do
        {
            int digit = tempScore % 10; // 1の位から順番に取得

            // 該当画像をセット
            digitImages[digitsUsed].sprite = numberSprites[digit];

            digitImages[digitsUsed].enabled = true; // 桁を表示
            tempScore /= 10;                        // 次の桁へ
            digitsUsed++;                           // 桁数カウント 
        }
        while (tempScore > 0 && digitsUsed < digitImages.Length);
    }

    public int GetScore()
    {
        return targetScore;
    }
}
