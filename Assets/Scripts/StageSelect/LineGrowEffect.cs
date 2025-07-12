using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineGrowEffect : MonoBehaviour
{
    public float growDuration = 1.0f;
    private Image image;
    private float timer = 0f;
    private bool growing = false;

    void Start()
    {
        image = GetComponent<Image>();
        image.fillAmount = 0f;
    }

    void Update()
    {
        if (!growing) return;

        timer += Time.unscaledDeltaTime;
        float progress = Mathf.Clamp01(timer / growDuration);
        image.fillAmount = progress;

        if (progress >= 1f)
        {
            growing = false;
        }
    }

    public void StartGrow()
    {
        timer = 0f;
        growing = true;
        if (image != null)
            image.fillAmount = 0f;
    }
}
