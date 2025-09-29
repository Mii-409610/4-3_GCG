using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultUIManager : MonoBehaviour
{
    [SerializeField, Header("各桁のスコア表示用UI")]
    private Image[] resultDigitImages;

    [SerializeField, Header("0～9の画像")]
    private Sprite[] numberSprites;


    // Start is called before the first frame update
    void Start()
    {
        int lastScore = PlayerPrefs.GetInt("LastScore", 0);
        ScoreManager.Instance.ShowScore(lastScore, resultDigitImages, numberSprites);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
