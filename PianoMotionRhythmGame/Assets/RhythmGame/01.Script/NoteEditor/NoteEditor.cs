using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoteEditor : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txt_PlayPause;
    [SerializeField] private TextMeshProUGUI txt_Stop;

    [SerializeField] private Slider timeSlider;

    private bool isPlaying;
    private bool isStarted;
    private float currentTime = 0;
    private int musicLength = 0;


    void Start()
    {
        musicLength = AudioManager._instance.GetEventLength();
        InitializeSlider();
    }

    void Update()
    {
        //UpdateCurrentTime();
        UpdateTimeSlider();
    }

    private void InitializeSlider()
    {
        timeSlider.minValue = 0;
        timeSlider.maxValue = musicLength;
        timeSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void UpdateCurrentTime()
    {
        currentTime = AudioManager._instance.GetTime();
    }

    private void UpdateTimeSlider()
    {
        if(!timeSlider.IsInteractable())
        {
            timeSlider.value = AudioManager._instance.GetTime();
        }
    }

    private void OnSliderValueChanged(float newValue)
    {
        AudioManager._instance.SetTimelinePosition((int)newValue);
    }

    public void PlayPauseMusic()
    {
        if(isPlaying)
        {
            isPlaying = false;
            AudioManager._instance.PauseMusic();
            txt_PlayPause.text = "Play";
        }
        else
        {
            isPlaying = true;
            if(isStarted == true)
            {
                AudioManager._instance.Resume();
                txt_PlayPause.text = "Pause";
            }
            else
            {
                AudioManager._instance.StartMusic();
                txt_PlayPause.text = "Pause";
                isStarted = true;
            }
        }
    }

    public void StopMusic()
    {
        if(isPlaying)
        {
            AudioManager._instance.StopMusic();
            txt_PlayPause.text = "Play";
            isStarted = false;
        }
    }
}
