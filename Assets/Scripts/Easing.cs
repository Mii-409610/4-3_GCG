using UnityEngine;

public class Easing : MonoBehaviour
{
    /// <summary>
    /// リニア
    /// </summary>
    /// <param name="t">媒介変数</param>
    /// <returns>補間値</returns>
    public static float Linear(float t) => t;

    /// <summary>
    /// イーズインアウト・キュービック
    /// </summary>
    /// <param name="t">媒介変数</param>
    /// <returns>補間値</returns>
    public static float EaseInOutCubic(float t)
    {
        return t < 0.5f ?
            4 * Mathf.Pow(t, 3) :
            1 - Mathf.Pow(-2 * t + 2, 3) / 2;
    }

    /// <summary>
    /// イーズインアウト・クイント
    /// </summary>
    /// <param name="t">媒介変数</param>
    /// <returns>補間値</returns>
    public static float EaseInOutQuint(float t)
    {
        return t < 0.5f ?
            16 * Mathf.Pow(t, 5) :
            1 - Mathf.Pow(-2 * t + 2, 5) / 2;
    }

    /// <summary>
    /// イーズインアウト・サーク
    /// </summary>
    /// <param name="t">媒介変数</param>
    /// <returns>補間値</returns>
    public static float EaseInOutCirc(float t)
    {
        return t < 0.5f ?
            (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * t, 2))) / 2 :
            (Mathf.Sqrt(1 - Mathf.Pow(-2 * t + 2, 2)) + 1) / 2;
    }
}
