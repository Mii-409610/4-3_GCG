using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    int count = 0;
    bool Out = false;
    bool In = false;
    bool SF = false;
    bool RE = false;
    bool title = false;

    public GameObject canvas;
    public GameObject[] uiImage;
    public enum Scene
    {
        Title,
        StageSelect,
        Game1,
        Game2,
        Result
    }
    [Header("ÉVÅ[ÉìëJà⁄êÊ")]
    public Scene scene;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 120;
        Out = false;
        In = true;
        SF = false;
        RE = false;
        title = false;
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        count++;
        if (In)
        {
            FadeIn();
        }

        if (Out)
        {
            FadeOut();
        }
        if (SF)
        {
            SFade();
        }
        if (RE)
        {
            REFade();
        }
        if (title)
        {
            TitleFade();
        }
    }

    public void Tchange()
    {
        if (!In && !Out && !SF && !RE && !title)
        {
            Out = true;
            count = 0;
        }
    }

    public void SChange()
    {
        if (!In && !Out && !SF && !RE && !title)
        {
            SF = true;
            count = 0;
        }
    }
    public void REChange()
    {
        if (!In && !Out && !SF && !RE && !title)
        {
            RE = true;
            count = 0;
        }
    }
    public void TiChange()
    {
        if (!In && !Out && !SF && !RE && !title)
        {
            title = true;
            count = 0;
        }
    }


    void FadeIn()
    {
        switch (count)
        {
            case 30: uiImage[4].SetActive(false);
                canvas.transform.position = new Vector3(923.0f, 524.0f, -614.0f); break;
            case 45: uiImage[3].SetActive(false);
                canvas.transform.position = new Vector3(923.0f, 524.0f, -616.2f); break;
            case 60: uiImage[2].SetActive(false);
                canvas.transform.position = new Vector3(923.0f, 524.0f, -618.0f); break;
            case 75: uiImage[1].SetActive(false); break;
            case 90: uiImage[0].SetActive(false); In = false; break;
            default: break;
        }

    }

    void FadeOut()
    {
        switch (count)
        {
            case 20:
                uiImage[0].SetActive(true);
                canvas.transform.position = new Vector3(923.0f, 524.0f, -618.0f); break;
            case 35:
                uiImage[1].SetActive(true);
                canvas.transform.position = new Vector3(923.0f, 524.0f, -616.2f); break;
            case 50:
                uiImage[2].SetActive(true);
                canvas.transform.position = new Vector3(923.0f, 524.0f, -614.0f); break;
            case 65:
                uiImage[3].SetActive(true);
                canvas.transform.position = new Vector3(923.0f, 524.0f, -610.0f); break;
            case 80:
                uiImage[4].SetActive(true);
                switch (scene)
                {
                    case Scene.Title:
                        SceneManager.LoadScene("Title Scene");
                        break;
                    case Scene.StageSelect:
                        SceneManager.LoadScene("StageSelect");
                        break;
                    case Scene.Game1:
                        SceneManager.LoadScene("Stage1");
                        break;
                    case Scene.Game2:
                        SceneManager.LoadScene("Stage1");
                        break;
                    case Scene.Result:
                        SceneManager.LoadScene("Title Scene");
                        break;
                    default: break;
                }
                break;
            default: break;
        }
    }
    void SFade()
    {
        switch (count)
        {
            case 20: uiImage[0].SetActive(true);
                canvas.transform.position = new Vector3(923.0f,524.0f,-618.0f); break;
            case 35: uiImage[1].SetActive(true);
                canvas.transform.position = new Vector3(923.0f, 524.0f, -616.2f); break;
            case 50: uiImage[2].SetActive(true);
                canvas.transform.position = new Vector3(923.0f, 524.0f, -614.0f); break;
            case 65: uiImage[3].SetActive(true);
                canvas.transform.position = new Vector3(923.0f, 524.0f, -610.0f); break;
            case 80: uiImage[4].SetActive(true); break;
            case 120: 
                FindObjectOfType<Ondisplay>()?.OptionChange();
                FindObjectOfType<BulletCollisionHandler>()?.CursorReset();break;
            case 135: uiImage[4].SetActive(false);
                canvas.transform.position = new Vector3(923.0f, 524.0f, -614.0f); break;
            case 150: uiImage[3].SetActive(false);
                canvas.transform.position = new Vector3(923.0f, 524.0f, -616.2f); break;
            case 165: uiImage[2].SetActive(false);
                canvas.transform.position = new Vector3(923.0f, 524.0f, -618.0f); break;
            case 180: uiImage[1].SetActive(false); break;
            case 195: uiImage[0].SetActive(false); SF = false; break;
            default: break;
        }

    }
    void REFade()
    {
        switch (count)
        {
            case 20: uiImage[0].SetActive(true);
                canvas.transform.position = new Vector3(923.0f, 524.0f, -618.0f); break;
            case 35: uiImage[1].SetActive(true);
                canvas.transform.position = new Vector3(923.0f, 524.0f, -616.2f); break;
            case 50: uiImage[2].SetActive(true);
                canvas.transform.position = new Vector3(923.0f, 524.0f, -614.0f); break;
            case 65: uiImage[3].SetActive(true);
                canvas.transform.position = new Vector3(923.0f, 524.0f, -610.0f); break;
            case 80: uiImage[4].SetActive(true); break;
            case 110: FindObjectOfType<PouseManager>()?.Resume();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name); break;
            default: break;
        }
    }
    void TitleFade()
    {
        switch (count)
        {
            case 20: uiImage[0].SetActive(true);
                canvas.transform.position = new Vector3(923.0f, 524.0f, -618.0f); break;
            case 35: uiImage[1].SetActive(true);
                canvas.transform.position = new Vector3(923.0f, 524.0f, -616.2f); break;
            case 50: uiImage[2].SetActive(true);
                canvas.transform.position = new Vector3(923.0f, 524.0f, -614.0f); break;
            case 65: uiImage[3].SetActive(true);
                canvas.transform.position = new Vector3(923.0f, 524.0f, -610.0f); break;
            case 80: uiImage[4].SetActive(true); break;
            case 110: FindObjectOfType<PouseManager>()?.Resume();
            SceneManager.LoadScene("Title Scene");break;
            default: break;
        }
    }

}
