using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBulletHit : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 1f); // 2秒後に削除
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Title"))
        {
            // Titleタグの場合は直接フェードアウト→シーン遷移
            FindObjectOfType<SelectFadeOut>().StartSceneTransition("Title Scene");
            Destroy(gameObject); // 弾を削除
        }
        else if (other.CompareTag("Stage2")|| other.CompareTag("Stage1"))
        {
            // Stage1,2の場合はズーム演出→フェードアウト→シーン遷移
            StageSelect zoomer = other.GetComponent<StageSelect>();
            if (zoomer != null)
            {
                zoomer.TriggerZoom();
            }
            Destroy(gameObject); // 弾を削除
        }
    }
}