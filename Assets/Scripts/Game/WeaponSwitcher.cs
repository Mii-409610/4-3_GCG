using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField,Header("切り替え対象となる武器を登録")]
    public GameObject[] weapons;

    // 現在の武器のインデックス
    private int currentWeaponIndex = 0;

    void Start()
    {
        SwitchWeapon(currentWeaponIndex);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            int nextIndex = (currentWeaponIndex + 1) % weapons.Length;
            SwitchWeapon(nextIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            int nextIndex = (currentWeaponIndex - 1 + weapons.Length) % weapons.Length;
            SwitchWeapon(nextIndex);
        }
    }

    void SwitchWeapon(int index)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            // モデル表示切り替え（オプション）
            Transform model = weapons[i].transform.Find("Weapon_Model");
            if (model != null)
                model.gameObject.SetActive(i == index);

            // インターフェースから制御
            IWeaponControl weaponScript = weapons[i].GetComponent<IWeaponControl>();
            if (weaponScript != null)
                weaponScript.SetWeaponActive(i == index);
        }

        currentWeaponIndex = index;
    }
}
