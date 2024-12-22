using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;
using System.Text;
using Unity.VisualScripting;

public class ScoreManager : MonoBehaviour
{

    [Header("Instance")]
    public static ScoreManager _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public enum Judgement
    {
        Perfect,
        Great,
        Good,
        Bad,
        Miss
    }

    public float totalScore = 0;
    public float scoreUnit = 100;
    private int scoreDigit = 8;
    public int Combo = 0;
    private Judgement recentJudge;
    private float fadeDuration = 1.0f;
    private float fadeCounter = 0;


    [SerializeField, Tooltip("판정 당 가중치")]
    float[] scoreWeight = new float[4];

    [SerializeField]
    TMP_Text scoreText = null;
    [SerializeField]
    TMP_Text ComboText = null;
    [SerializeField]
    TMP_Text JudgeText = null;

    // Start is called before the first frame update

    
    
    void Start()
    {
        scoreText.text = "000000";
        ComboText.text = "";
        JudgeText.text = "";

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            NoteHit(Judgement.Perfect);
        }
        if (Input.GetKey(KeyCode.W))
        {
            NoteHit(Judgement.Great);
        }
        if (Input.GetKey(KeyCode.E))
        {
            NoteHit(Judgement.Good);
        }
        if (Input.GetKey(KeyCode.R))
        {
            NoteMiss();
        }
        if (fadeCounter >= fadeDuration)
        {
            fadeCounter = -1;

            StartCoroutine(FadeOut());
        }
        else if (fadeCounter >=0)
        {
            fadeCounter += Time.deltaTime;

        }
        
    }

    public void NoteHit(Judgement judge)
    {
        float comboMult = 1 + (Combo / 100);
        switch (judge)
        {
            case Judgement.Perfect : // perfect
                totalScore += scoreUnit * scoreWeight[0] * comboMult;
                break;
            case Judgement.Great:
                totalScore += scoreUnit * scoreWeight[1] * comboMult;
                break;
            case Judgement.Good:
                totalScore += scoreUnit * scoreWeight[2]*comboMult;
                break;
            case Judgement.Bad:
                totalScore += scoreUnit * scoreWeight[3] * comboMult;
                break;
        }
        recentJudge = judge;
        Combo++;
        UpdateScoreUI();
    }
    public void NoteMiss()
    {
        Combo = 0;
        recentJudge = Judgement.Miss;
        UpdateScoreUI();
    }

    public void UpdateScoreUI()
    {
        if (Combo == 0)
        {
            ComboText.text = "";
        }
        else
        {
            ComboText.text = string.Format($"Combo {Combo}");
        }
        int Digit =((int)totalScore).ToString().Length;
        scoreText.text = totalScore.ToString().PadLeft(scoreDigit, '0');
        JudgeText.text = recentJudge.ToString();

        switch (recentJudge)
        {
            case Judgement.Perfect:
                JudgeText.color = new Color(1.0f, 0.0f, 0.7435f, 1.0f);
                ComboText.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                break;
            case Judgement.Great:
                JudgeText.color = new Color(1.0f, 0.8235f, 0.0f, 1.0f);
                ComboText.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                break;
            case Judgement.Good:
                JudgeText.color = new Color(0.0214f, 0.9056f, 0.0f, 1.0f);
                ComboText.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                break;
            case Judgement.Bad:
                JudgeText.color = new Color(0.6037f, 0.0f, 0.0f, 1.0f);
                ComboText.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                break;
            case Judgement.Miss:
                JudgeText.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
                ComboText.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                break;
        }
        

        fadeCounter = 0;
    }

    private IEnumerator FadeOut()
    {
        while (true)
        {
            if (fadeCounter > 0)
            {
                yield break;
            }
            JudgeText.color -= new Color(0, 0, 0, 0.1f);
            ComboText.color -= new Color(0, 0, 0, 0.1f);
            if (JudgeText.color.a <= 0)
            {
                yield break;
            }
            
            yield return null;
        }
    }


}
