using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器切り替えクラス
/// </summary>
public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField,Header("切り替え対象となる武器を登録")]
    public GameObject[] weapons;

    // 現在の武器のインデックス
    private int currentWeaponIndex = 0;

    void Start()
    {
        // 初期武器をアクティブに
        SwitchWeapon(currentWeaponIndex);
    }

    void Update()
    {
        HandleWeaponSwitchInput();
    }

    /// <summary>
    /// 入力を検出し、武器の切り替えを行う
    /// </summary>
    void HandleWeaponSwitchInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SwitchWeapon(+1); // 次の武器へ
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchWeapon(-1); // 前の武器へ
        }
    }

    /// <summary>
    /// directionに応じて武器を切り替える
    /// </summary>
    /// <param name="direction">+1で次、-1で前</param>
    void SwitchWeapon(int direction)
    {
        if (weapons == null || weapons.Length == 0) return;

        // インデックスを循環させる
        currentWeaponIndex = (currentWeaponIndex + direction + weapons.Length) % weapons.Length;

        ActivateWeapon(currentWeaponIndex);
    }

    /// <summary>
    /// 指定したインデックスの武器をアクティブにし、それ以外は非アクティブにする
    /// </summary>
    /// <param name="index">アクティブにする武器のインデックス</param>
    void ActivateWeapon(int index)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            bool isActive = (i == index);
            weapons[i].SetActive(isActive);

            if (isActive)
            {
                Debug.Log("現在の武器: " + weapons[i].name);
            }
        }
    }
}
