using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RhythmGame
{
    public class NoteEditor : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI txt_PlayPause;
        [SerializeField] private TextMeshProUGUI txt_Stop;

        [SerializeField] private Slider timeSlider;

        private bool isPlaying;
        private bool isStarted;
        private int currentTime = 0;
        private int musicLength = 0;


        void Start()
        {
            musicLength = AudioManager._instance.GetEventLength();
            InitializeSlider();
        }

        void Update()
        {
            UpdateCurrentTime();
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
            if (!timeSlider.IsInteractable())
            {
                timeSlider.value = AudioManager._instance.GetTime();
            }
        }

        private void OnSliderValueChanged(float newValue)
        {
            AudioManager._instance.SetTimelinePosition((int)newValue);
        }

        public int GetCurrentTime() => currentTime;

        public void PlayPauseMusic()
        {
            if (isPlaying)
            {
                isPlaying = false;
                AudioManager._instance.PauseMusic();
                txt_PlayPause.text = "Play";
            }
            else
            {
                isPlaying = true;
                if (isStarted == true)
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
            AudioManager._instance.StopMusic();
            txt_PlayPause.text = "Play";
            isStarted = false;
        }

        public void Rewind()
        {
            AudioManager._instance.SetTimelinePosition(currentTime - 100);
        }
        public void Skip()
        {
            AudioManager._instance.SetTimelinePosition(currentTime + 100);
        }
    }
}
