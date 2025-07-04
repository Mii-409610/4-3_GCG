using UnityEngine;

public class PassLineDraw : MonoBehaviour
{
    [SerializeField, Header("線の太さ"), Range(0.0f, 1.0f)]
    private float lineSize;

    private LineRenderer passRenderer;

    void Awake()
    {
        // =========================================
        //  エラー処理
        // =========================================
        passRenderer = GetComponent<LineRenderer>();

        if(passRenderer == null)
        {
            Debug.LogError("LineRendererが見つかりません。");
            return;
        }
    }

    public void AddPoint(Vector3 point)
    {
        // LineRendererが設定
        if (passRenderer == null) return;

        int positionCount = passRenderer.positionCount;
        passRenderer.positionCount = passRenderer.positionCount + 1;    // 現在の位置数を１つ増やす
        passRenderer.SetPosition(positionCount, point);                 // 新しい位置を設定
        passRenderer.startWidth = lineSize;                             // 
        passRenderer.endWidth = lineSize;                               // 
    }
}
