using System.Data;
using UnityEngine;

public class MovePassManager : MonoBehaviour
{
    [SerializeField, Header("移動するオブジェクト")]
    private GameObject moveObject;

    [SerializeField, Header("パスライン描画オブジェクト")]
    private PassLineDraw passLineDraw;

    [SerializeField, Header("パス親オブジェクト")]
    private Transform passParent;

    // パスオブジェクトの配列
    private PassSetting[] passObjects;

    private GameObject fromObject;  // 移動元オブジェクト
    private GameObject toObject;    // 移動先オブジェクト

    // 現在のパスオブジェクトのインデックス
    private int currentIndex;

    private bool errorFlag; // エラーフラグ
    private bool completed; // パスの移動が完了したかどうか

    // 移動速度
    private float moveTime;

    // 使用するイージングのタイプ
    private PassSetting.Easing easingType;

    // イージングの総時間
    private float easingTotalTime;

    void Awake()
    {
        // エラー処理
        if(moveObject == null)
        {
            Debug.LogError("移動するオブジェクトが設定されていません");
            errorFlag = true;
        }

        GameObject[] passObjects = null;

        if (passParent != null)
        {
            passObjects = new GameObject[passParent.childCount];
            for(int i = 0; i < passParent.childCount; i++)
            {
                passObjects[i] = passParent.GetChild(i).gameObject;
            }
        }
        else
        {
            Debug.LogError("パスの親オブジェクトが設定されていません");
            errorFlag = true;
            return;
        }

        // パスオブジェクトの配列を初期化
        this.passObjects = new PassSetting[passObjects.Length];

        foreach(GameObject passObject in passObjects)
        {
            // 各パスオブジェクトからPassSettingコンポーネントを取得
            PassSetting passSetting = passObject.GetComponent<PassSetting>();

            if (passSetting != null)
            {
                // パスオブジェクトの配列に設定
                for(int i = 0; i < this.passObjects.Length; i++)
                {
                    if(this.passObjects[i] == null)
                    {
                        this.passObjects[i] = passSetting;
                        break;
                    }
                }
            }
            else
            {
                Debug.LogError("パスオブジェクトにPassSettingコンポーネントが見つかりません。");
                errorFlag = true;
            }
        }

        if(passLineDraw == null)
        {
            Debug.LogError("PassLineDrawコンポーネントが見つかりません。パスラインの描画が出来ません。");
            errorFlag = true;
        }

        if(this.passObjects.Length < 2)
        {
            Debug.LogWarning("パスオブジェクトが2つ以上設定されていないので、動作しません。現在の数：" + this.passObjects.Length);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (errorFlag) return; // エラーが発生している場合は処理を中断

        // ========================================
        //  初期化処理
        // ========================================
        SetFromToObject();  // 移動元と移動先のオブジェクトを設定
        SetTime();          // 移動にかかる時間を設定
        SetEasingType();    // 使用するイージングのタイプを設定

        // 移動するオブジェクトを生成
        Vector3 spawnPos = fromObject.transform.position + Vector3.up * 3.0f;
        moveObject.transform.position = spawnPos;

        foreach (var passObject in passObjects)
        {
            if(passObject != null && passLineDraw != null)
            {
                passLineDraw.AddPoint(passObject.transform.position);
            }
        }
    }

    void FixedUpdate()
    {
        // エラーが発生している場合は処理を中断
        if (errorFlag) return;

        // オブジェクトの移動処理を呼び出す
        MoveObject();
    }

    /// <summary>
    /// 移動元と移動先のオブジェクトを設定
    /// </summary>
    private void SetFromToObject()
    {
        // 現在の移動元オブジェクト
        fromObject = passObjects[currentIndex].gameObject;

        if(currentIndex == passObjects.Length - 1)
        {
            // 最後のオブジェクトの場合、移動先はなし
            toObject = null;
            return;
        }

        // 次の移動先オブジェクト
        toObject = passObjects[currentIndex + 1].gameObject;
    }

    /// <summary>
    /// 移動にかかる時間の設定
    /// </summary>
    private void SetTime()
    {
        if(currentIndex < passObjects.Length && passObjects[currentIndex] != null)
        {
            // 現在のパスオブジェクトから移動にかかる時間を取得
            moveTime = passObjects[currentIndex].MoveTime;
        }
    }

    /// <summary>
    /// イージングタイプの設定
    /// </summary>
    private void SetEasingType()
    {
        if(currentIndex < passObjects.Length && passObjects[currentIndex] != null)
        {
            // 現在のパスオブジェクトから使用するイージングのタイプを取得
            easingType = passObjects[currentIndex].EasingType;
        }
    }

    /// <summary>
    /// オブジェクトの移動処理
    /// </summary>
    private void MoveObject()
    {
        // パスの移動が完了している場合は処理を中断
        if (completed) return;

        // 移動先が設定されていない場合は処理を中断
        if (toObject == null) return;

        float yOffset = 3.0f;

        // 移動元と移動先の位置を取得
        Vector3 fromPosition = fromObject.transform.position + Vector3.up * yOffset;
        Vector3 toPosition = toObject.transform.position + Vector3.up * yOffset;

        float f = 0.0f;                                         // イージングの補間値
        float t = Mathf.Clamp01(easingTotalTime / moveTime);    // イージングの進行度を計算

        // イージングのタイプに応じて補間値を設定
        switch(easingType)
        {
            case PassSetting.Easing.Liner:          f = Easing.Linear(t);           break;  // リニア
            case PassSetting.Easing.EaseInOutCubic: f = Easing.EaseInOutCubic(t);   break;  // イーズインアウト・キュービック
            case PassSetting.Easing.EaseInOutQuint: f = Easing.EaseInOutQuint(t);   break;  // イーズインアウト・クイント
            case PassSetting.Easing.EaseInOutCirc:  f = Easing.EaseInOutCirc(t);    break;  // イーズインアウト・サーク
        }

        // オブジェクトの移動
        moveObject.transform.position = Vector3.LerpUnclamped(fromPosition, toPosition, f);

        // イージングの総時間を更新
        easingTotalTime += Time.deltaTime;

        // 移動が完了したら次のパスへ進む
        if (Vector3.Distance(moveObject.transform.position, toPosition) < 0.01f)
        {
            currentIndex++;
            if(currentIndex < passObjects.Length - 1)
            {
                // イージングの総時間をリセット
                easingTotalTime = 0.0f;

                SetFromToObject();  // 次の移動元と移動先を設定
                SetTime();          // 移動にかかる時間を設定
                SetEasingType();    // 使用するイージングのタイプを設定
            }
            else
            {
                // 最後のパスに到達した場合、イージングの総時間をリセット
                easingTotalTime = 0.0f;

                Debug.Log("全てのパスを通過しました。");

                // パスの移動が完了
                completed = true;
            }
        }
    }
}
