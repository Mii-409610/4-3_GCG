using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 武器切り替え管理クラス
/// </summary>
public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField, Header("切り替え対象となる武器を登録")]
    public GameObject[] weapons;

    // 現在の武器のインデックス
    private int currentWeaponIndex = 1;

    void Start()
    {
        // 初期武器をアクティブに
        SetWeaponActive(currentWeaponIndex);
    }

    void Update()
    {
        // 武器切り替え入力
        HandleWeaponSwitchInput();
    }

    /// <summary>
    /// 入力を検知し武器切り替え
    /// </summary>
    void HandleWeaponSwitchInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // 次の武器へ
            SwitchWeapon(+1);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            // 前の武器へ
            SwitchWeapon(-1);
        }
    }

    /// <summary>
    /// derectionに応じて武器を切り替え
    /// </summary>
    /// <param name="direction">+1:次の武器、-1:前の武器、配列範囲内で循環する</param>
    void SwitchWeapon(int direction)
    {
        // 武器が登録されていなければ何もしない
        if (weapons == null || weapons.Length == 0) return;

        // インデックスを循環させる
        int nextIndex = (currentWeaponIndex + direction + weapons.Length) % weapons.Length;
        SetWeaponActive(nextIndex);
    }

    /// <summary>
    /// 指定インデックスの武器をアクティブ化し、他を非アクティブ化
    /// </summary>
    /// <param name="index">アクティブにする武器のインデックス</param>
    void SetWeaponActive(int index)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            // 指定インデックスの武器だけアクティブ化
            bool isActive = (i == index);
            weapons[i].SetActive(isActive);

            // 必要に応じてWeapon_Modelやエフェクトもここで切り替え可

            // 切り替えた武器名をデバック出力
            if (isActive)
                Debug.Log("現在の武器: " + weapons[i].name);
        }

        // 現在の武器インデックスを更新
        currentWeaponIndex = index;
    }
}