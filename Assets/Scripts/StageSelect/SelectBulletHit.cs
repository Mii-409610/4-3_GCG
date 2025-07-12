using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBulletHit : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 1f); // 2•bŒã‚É©“®Á–Å
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Stage1")|| other.CompareTag("Stage2"))
        {
            StageSelect zoomer = other.GetComponent<StageSelect>();
            if (zoomer != null)
            {
                zoomer.TriggerZoom();
            }
            Destroy(gameObject); // ’e‚ğíœ
        }
    }
}
