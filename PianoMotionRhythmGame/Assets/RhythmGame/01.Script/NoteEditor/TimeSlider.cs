using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RhythmGame
{
    public class TimeSlider : MonoBehaviour
    {
        public Slider timeSlider;

        private bool isDragging;
        private int musicLength;

        private void Start()
        {
            musicLength = AudioManager._instance.GetEventLength();
            InitializeSlider();
        }
        void Update()
        {
            UpdateTimeSlider();
            UpdateSliderInteractability();
        }


        private void InitializeSlider()
        {
            timeSlider.minValue = 0;
            timeSlider.maxValue = musicLength;
            timeSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        private void UpdateSliderInteractability()
        {
            if(NoteManager.instance.currentMode == Mode.Editing)
            {
                timeSlider.interactable = true;
            }
            else
            {
                timeSlider.interactable = false;
            }
        }

        private void UpdateTimeSlider()
        {
            if (!isDragging)
            {
                timeSlider.value = AudioManager._instance.GetTime();
            }
        }
        public void OnSliderValueChanged(float newValue)
        {
            if (isDragging)
            {
                if (NoteManager.instance.currentMode == Mode.Editing)
                {
                    AudioManager._instance.SetTimelinePosition((int)newValue);
                }
            }
        }
        public void OnPointerDown()
        {
            isDragging = true;
        }
        public void OnPointerUp()
        {
            isDragging = false;
            AudioManager._instance.SetTimelinePosition((int)timeSlider.value);
        }

    }
}
