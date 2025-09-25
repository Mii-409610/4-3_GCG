using System.Collections;
using System.Collections.Generic;
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

    public Image[] uiImage;
    public enum Scene
    {
        Title,
        Select,
        Game,
        Result
    }
    [Header("シーン遷移先")]
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
        Out = true;
        count = 0;
    }

    public void SChange()
    {
        SF = true;
        count = 0;
    }
    public void REChange()
    {
        RE = true;
        count = 0;
    }
    public void TiChange()
    {
        title = true;
        count = 0;
    }


    void FadeIn()
    {
        count++;
        if (count <= 90)
            transform.Rotate(-1.0f, 0, 0);
        else
            In = false;

    }

    void FadeOut()
    {
        count++;
        if (count <= 90)
            transform.Rotate(1.0f, 0, 0);
        else
            switch(scene)
            {
                case Scene.Title:
                    SceneManager.LoadScene("Title Scene");
                    break;
                case Scene.Select:
                    SceneManager.LoadScene("StageSelect");
                    break;
                case Scene.Game:
                    SceneManager.LoadScene("Debug");
                    break;
                case Scene.Result:
                    SceneManager.LoadScene("Title Scene");
                    break;
                default:break;
            }
    }
    void SFade()
    {
        count++;
        if (count <= 60)
        {
            uiImage[0].transform.position += new Vector3(-16.0f, 0.0f, 0.0f);
            uiImage[1].transform.position += new Vector3(16.0f, 0.0f, 0.0f);
        }
        if (count == 60)// オプションに変更
        {
            FindObjectOfType<Ondisplay>()?.OptionChange();
            FindObjectOfType<BulletCollisionHandler>()?.CursorReset();
        }
        if (count >= 90 && count < 150)
        {
            uiImage[0].transform.position += new Vector3(16.0f, 0.0f, 0.0f);
            uiImage[1].transform.position += new Vector3(-16.0f, 0.0f, 0.0f);
        }
        if (count == 150)
            SF = false;

    }
    void REFade()
    {
        count++;
        if (count <= 90)
            transform.Rotate(1.0f, 0, 0);
        else
        {
            FindObjectOfType<PouseManager>()?.Resume();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    void TitleFade()
    {
        count++;
        if (count <= 90)
            transform.Rotate(1.0f, 0, 0);
        else
        {
            FindObjectOfType<PouseManager>()?.Resume();
            SceneManager.LoadScene("Title Scene");
        }
    }

}
