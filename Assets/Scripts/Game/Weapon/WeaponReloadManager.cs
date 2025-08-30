using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 武器のリロードを管理するクラス
/// </summary>
public class WeaponReloadManager : MonoBehaviour
{
    /// <summary>
    /// 武器ごとのリロード情報
    /// </summary>
    private class ReloadInfo
    {
        public bool isReloading = false;        // リロード中フラグ
        public float timer = 0f;                // 経過時間
        public float duration = 0f;             // リロード所要時間
        public System.Action onReloadComplete;  // リロード完了時のコールバック
        public Image reloadGauge;               // リロードゲージのUI
    }

    // 武器名をキーにしたリロード情報のテーブル(key = 武器名)
    private static Dictionary<string, ReloadInfo> reloadTable = new Dictionary<string, ReloadInfo>();

    /// <summary>
    /// リロード開始
    /// </summary>
    /// <param name="weaponName">武器名</param>
    /// <param name="duration">リロードにかかる時間</param>
    /// <param name="onReloadComplete">リロード完了時に呼ばれる処理</param>
    /// <param name="reloadGauge">リロード進捗を表示するUIゲージ</param>
    public static void StartReload(string weaponName, float duration, System.Action onReloadComplete, Image reloadGauge)
    {
        // 既にリロード中なら無視
        if (!reloadTable.ContainsKey(weaponName))
            reloadTable[weaponName] = new ReloadInfo();
        var info = reloadTable[weaponName];
        info.isReloading = true;                    // リロード中フラグを立てる
        info.timer = 0f;                            // 経過時間リセット
        info.duration = duration;                   // リロード所要時間セット
        info.onReloadComplete = onReloadComplete;   // 完了時の処理
        info.reloadGauge = reloadGauge;             // UIゲージセット
        // リロード開始時にゲージを空にする
        if (info.reloadGauge != null)
            info.reloadGauge.fillAmount = 0.0f;
    }

    /// <summary>
    /// 指定武器がリロード中かどうか
    /// </summary>
    public static bool IsReloading(string weaponName)
    {
        if (!reloadTable.ContainsKey(weaponName)) return false;
        return reloadTable[weaponName].isReloading;
    }

    /// <summary>
    /// 指定武器のリロード進捗(0.0～1.0)を返す
    /// </summary>
    public static float GetReloadProgress(string weaponName)
    {
        if (!reloadTable.ContainsKey(weaponName)) return 1f;    // 未登録なら満タン
        var info = reloadTable[weaponName];
        return Mathf.Clamp01(info.timer / info.duration);
    }

    void Update()
    {
        foreach (var pair in reloadTable)
        {
            var info = pair.Value;

            if (info.isReloading)
            {
                // 経過時間を加算
                info.timer += Time.deltaTime;

                // UIゲージ進捗もここで反映
                if (info.reloadGauge != null)
                    info.reloadGauge.fillAmount = Mathf.Clamp01(info.timer / info.duration);

                // リロードが完了したら
                if (info.timer >= info.duration)
                {
                    info.isReloading = false;
                    info.onReloadComplete?.Invoke(); // 弾数回復などの処理を実行

                    // 完了時ゲージ満タン
                    if (info.reloadGauge != null)
                        info.reloadGauge.fillAmount = 1.0f;
                }
            }
        }
    }
}