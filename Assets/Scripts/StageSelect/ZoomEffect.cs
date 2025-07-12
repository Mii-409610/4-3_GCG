using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoomEffect : MonoBehaviour
{
    public Image speedLineImage; // 集中線
    public float displayTime = 0.4f;

    // Start is called before the first frame update
    void Start()
    {
        // 起動時に非表示にする
        if (speedLineImage != null)
        {
            speedLineImage.enabled = false;
            speedLineImage.color = new Color(
                speedLineImage.color.r,
                speedLineImage.color.g,
                speedLineImage.color.b,
                0f // ← Alphaを0にしておく
            );
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowSpeedLines()
    {
        StartCoroutine(PlaySpeedLines());// 集中線演出スタート
    }

    private System.Collections.IEnumerator PlaySpeedLines()
    {
        speedLineImage.enabled = true;// 表示開始

        // 簡単なフェードアウト
        float t = 0;
        Color originalColor = speedLineImage.color;
        // 徐々に透明にしていく処理（フェードアウト）
        while (t < displayTime)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / displayTime);
            speedLineImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);// アルファ値を下げる
            yield return null;
        }

        speedLineImage.enabled = false;
        speedLineImage.color = originalColor; // 元に戻しておく
    }
}
