using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SoundVolume : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI KeyText;
    public static float Volume = 4.0f;

    private void Start()
    {
        if (Volume <= 9.0f)
            KeyText.text =  " " + Volume.ToString();
        else 
            KeyText.text = Volume.ToString();

        AudioListener.volume = Volume * 0.1f;
    }
    public void SoundVolumeChange(int a)
    {
        Volume += a;
        if (Volume < 0.0f)
            Volume = 0.0f;
        if (Volume > 10.0f)
            Volume = 10.0f;

        if (Volume <= 9.0f)
            KeyText.text = " " + Volume.ToString();
        else
            KeyText.text = Volume.ToString();
        AudioListener.volume = Volume * 0.1f;
    }
}
