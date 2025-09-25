using UnityEngine;

/// <summary>
/// パス設置するクラス
/// </summary>
public class PassSetting : MonoBehaviour
{
    /// <summary>
    /// イージングのタイプを定義する列挙型
    /// </summary>
    public enum Easing
    {
        Liner,          // リニア
        EaseInOutCubic, // イーズインアウト・キュービック
        EaseInOutQuint, // イーズインアウト・クイント
        EaseInOutCirc   // イーズインアウト・サーク
    }

    [SerializeField, Header("次のパスまでかかる時間")]
    private float moveTime = 1.0f;      // 次のパスまでかかる時間(デフォルト値)

    public float MoveTime => moveTime;  // 外部からアクセル可能なプロパティ(読み取り専用)

    [SerializeField, Header("使用するイージング")]
    private Easing easingType = Easing.Liner;   // 使用するイージングのタイプ(デフォルトはリニア)

    public Easing EasingType => easingType;     // 外部からアクセス可能なプロパティ(読み取り専用)

    // Start is called before the first frame update
    void Start()
    {
        // 実行時には非アクティブにする
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
