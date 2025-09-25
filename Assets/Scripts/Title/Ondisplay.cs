using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ondisplay : MonoBehaviour
{
    public static bool display;
    public static bool volume;
    public static bool help;
    bool mouse;

    public GameObject Option;
    public GameObject OptionObject;
    public GameObject VolumeObject;
    public GameObject HelpObject;
    public GameObject Mouse;
    //public GameObject Cursor;
    public Image Cursor;

    int cooltime = 0;

    void Start()
    {
        display = false;
        volume = false;
        help = false;
        mouse = !CorM.Controller;
        Option.SetActive(false);
        OptionObject.SetActive(true);
        VolumeObject.SetActive(false);
        HelpObject.SetActive(false);
        Mouse.SetActive(!CorM.Controller);
        Cursor.enabled = true;
    }
    public void OptionChange()
    {
        display = !display;
        Option.SetActive(display);
    }

    public void VolumeChange()
    {
        volume = !volume;
        VolumeObject.SetActive(volume);
        OptionObject.SetActive(!volume);
        Cursor.enabled = !volume;
    }

    public void HelpChange()
    {
        help = !help;
        HelpObject.SetActive(help);
        OptionObject.SetActive(!help);
        Cursor.enabled = !help;
    }

    public void OperationChange()
    {
        mouse = !mouse;
        Mouse.SetActive(mouse);
    }

    private void Update()
    {
        if (volume == true && Input.GetKey(KeyCode.RightArrow) && cooltime == 0)
        {
            FindObjectOfType<SoundVolume>()?.SoundVolumeChange(1);
            cooltime = 25;
        }
        if (volume == true && Input.GetKey(KeyCode.LeftArrow) && cooltime == 0)
        {
            FindObjectOfType<SoundVolume>()?.SoundVolumeChange(-1);
            cooltime = 25;
        }
        if (cooltime > 0)
            cooltime--;

    }
}
