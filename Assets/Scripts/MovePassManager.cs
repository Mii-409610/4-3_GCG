using System.Data;
using UnityEngine;

public class MovePassManager : MonoBehaviour
{
    [SerializeField, Header("移動するオブジェクト")]
    private GameObject moveObject;

    [SerializeField, Header("パスオブジェクト")]
    private PassSetting[] passObjects;

    // 移動するオブジェクトのインスタンス
    private GameObject moveObjectInstance;

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
        if(moveObject==null)
        {
            Debug.LogError("移動するオブジェクトが設定されていません");
            errorFlag = true;
        }


        if (passObjects == null || passObjects.Length == 0)
        {
            Debug.LogError("パスオブジェクトが設定されていません");
            errorFlag = true; // エラーフラグを立てる
        }

        if (passObjects.Length < 2)
        {
            Debug.LogWarning("パスオブジェクトが2つ以上設定されていないので、動作しません。現在の数: " + passObjects.Length);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (errorFlag) return; // エラーが発生している場合は処理を中断

        // ========================================
        //  初期化処理
        // ========================================
        currentIndex = 0;       // 
        completed = false;      // 
        easingTotalTime = 0.0f; // 
        moveTime = 1.0f;        // 

        // 
        SetFromToObject();

        // 
        SetTime();

        // 
        SetEasingType();

        // 移動するオブジェクトのインスタンスを生成
        moveObjectInstance = Instantiate(moveObject, fromObject.transform.position, Quaternion.identity);
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

        if(currentIndex==passObjects.Length-1)
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
            // 現在のパsオブジェクトから使用するイージングのタイプを取得
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

        // 移動元と移動先の位置を取得
        Vector3 fromPosition = fromObject.transform.position;
        Vector3 toPosition = toObject.transform.position;

        // イージングの補間値
        float f = 0.0f;

        // イージングのタイプに応じて補間値を設定
        switch(easingType)
        {
            case PassSetting.Easing.Liner:
                f = Easing.Linear(Mathf.Clamp01(easingTotalTime / moveTime)); // リニア
                break;
            case PassSetting.Easing.EaseInOutCubic:
                f = Easing.EaseInOutCubic(Mathf.Clamp01(easingTotalTime / moveTime)); // イーズインアウト・キュービック
                break;
            case PassSetting.Easing.EaseInOutQuint:
                f = Easing.EaseInOutQuint(Mathf.Clamp01(easingTotalTime / moveTime)); // イーズインアウト・クイント
                break;
            case PassSetting.Easing.EaseInOutCirc:
                f = Easing.EaseInOutCirc(Mathf.Clamp01(easingTotalTime / moveTime)); // イーズインアウト・サーク
                break;
        }

        // オブジェクトの移動
        moveObjectInstance.transform.position = Vector3.LerpUnclamped(fromPosition, toPosition, f);

        // イージングの総時間を更新
        easingTotalTime += Time.deltaTime;

        // 移動が完了したら次のパスへ進む
        if (Vector3.Distance(moveObjectInstance.transform.position, toPosition)<0.01f)
        {
            currentIndex++;
            if(currentIndex < passObjects.Length - 1)
            {
                // イージングの総時間をリセット
                easingTotalTime = 0.0f;

                // 次の移動元と移動先を設定
                SetFromToObject();

                // 移動にかかる時間を設定
                SetTime();

                // 使用するイージングのタイプを設定
                SetEasingType();
            }
            else
            {
                // 最後のパスに到達した場合、イージングの総時間をリセット
                easingTotalTime = 0.0f;

                // パスの移動が完了
                completed = true;
            }
        }
    }
}
