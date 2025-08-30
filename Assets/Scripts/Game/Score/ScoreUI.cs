using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// スコアの1桁を画像のUVオフセットで表示するクラス
/// </summary>
public class ScoreUI : MonoBehaviour
{
    [SerializeField, Header("スコアUIの画像")]
    private Image image;

    [SerializeField, Header("使用するマテリアル")]
    private Material material;

    // Start is called before the first frame update
    void Start()
    {
        // Imageコンポーネントを取得
        image = GetComponent<Image>();

        if(image != null )
        {
            // マテリアルを複製して他と共有されないようにする
            image.material = new Material(image.material);
            material = image.material;
        }

        // 初期オフセットを設定
        material.mainTextureOffset = new Vector2(0.2f,0.5f);
    }

    /// <summary>
    /// スコアの1桁(0～9)に応じてUVオフセットを変更する
    /// </summary>
    /// <param name="scoreValue">0～9の数値</param>
    public void OnUpdateScore(int scoreValue)
    {
        Vector2 offset = new Vector2();

        if (scoreValue < 5)
        {
            offset.y = 0.5f;
        }
        else
        {
            offset.y = 0.0f;
        }

        // Xオフセットを設定
        offset.x = 0.2f * (scoreValue % 5);

        // Yオフセットを設定
        offset.y = (1 - (int)scoreValue / 5) * 0.5f;

        // テクスチャのUVオフセットを設定
        material.mainTextureOffset = offset;

    }
}

