using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using TMPro;

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
    public int Combo = 0;
    private Judgement recentJudge;


    [SerializeField, Tooltip("판정 당 가중치")]
    float[] scoreWeight = new float[4];

    [SerializeField]
    TMP_Text scoreText = null;
    [SerializeField]
    TMP_Text ComboText = null;
    [SerializeField]
    TMP_Text Judgetext = null;

    // Start is called before the first frame update

    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //NoteHit(Judgement.Perfect);
        //if (Combo > 10)
        //{
        //    NoteMiss();
        //}
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
            ComboText.text = string.Format($"Combo {Combo}") + "\n" + recentJudge.ToString();
        }
        scoreText.text = ((int)totalScore).ToString();
    }




}
