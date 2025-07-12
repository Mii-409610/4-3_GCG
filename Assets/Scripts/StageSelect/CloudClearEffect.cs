using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloudClearEffect : MonoBehaviour
{
    public GameObject leftCloud;
    public GameObject rightCloud;

    private Image leftImage;
    private Image rightImage;
    [Header("演出設定")]
    public float moveDistance = 300f;  // 雲の移動距離
    public float moveTime = 1f;        // 雲が移動する時間
    public float fadeTime = 1f;        // 雲がフェードアウトする時間
    [Header("初期位置設定（ローカルポジション）")]
    public Vector3 leftCloudStartPos = new Vector3(-20f, 0f, -80f);
    public Vector3 rightCloudStartPos = new Vector3(20f, 0f, -80f);

    public void ResetClouds()
    {
        leftCloud.SetActive(true);
        rightCloud.SetActive(true);

        // インスペクターで設定した位置を反映
        leftCloud.transform.localPosition = leftCloudStartPos;
        rightCloud.transform.localPosition = rightCloudStartPos;

        if (leftImage == null) leftImage = leftCloud.GetComponent<Image>();
        if (rightImage == null) rightImage = rightCloud.GetComponent<Image>();

        if (leftImage) leftImage.color = new Color(1f, 1f, 1f, 1f);
        if (rightImage) rightImage.color = new Color(1f, 1f, 1f, 1f);
    }

    public void StartClearEffect()
    {
        StartCoroutine(ClearClouds());
    }

    IEnumerator ClearClouds()
    {
        if (leftImage == null) leftImage = leftCloud.GetComponent<Image>();
        if (rightImage == null) rightImage = rightCloud.GetComponent<Image>();

        Vector3 leftStart = leftCloud.transform.localPosition;
        Vector3 rightStart = rightCloud.transform.localPosition;

        Vector3 leftTarget = leftStart + Vector3.left * moveDistance;
        Vector3 rightTarget = rightStart + Vector3.right * moveDistance;

        float timer = 0f;

        // 移動アニメーション
        while (timer < moveTime)
        {
            float t = timer / moveTime;
            leftCloud.transform.localPosition = Vector3.Lerp(leftStart, leftTarget, t);
            rightCloud.transform.localPosition = Vector3.Lerp(rightStart, rightTarget, t);
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0f;

        // フェードアウトアニメーション
        while (timer < fadeTime)
        {
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeTime);
            if (leftImage) leftImage.color = new Color(1f, 1f, 1f, alpha);
            if (rightImage) rightImage.color = new Color(1f, 1f, 1f, alpha);
            timer += Time.deltaTime;
            yield return null;
        }

        leftCloud.SetActive(false);
        rightCloud.SetActive(false);
    }
}
