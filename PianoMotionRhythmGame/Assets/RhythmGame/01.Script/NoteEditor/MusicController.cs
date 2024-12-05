using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RhythmGame
{
    public class MusicController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI txt_PlayPause;
        [SerializeField] private TextMeshProUGUI txt_Stop;
        [SerializeField] private TextMeshProUGUI txt_time;

        private bool isPlaying;
        private bool isStarted;
        private int currentTime = 0;

        private void Update()
        {
            UpdateCurrentTime();
            UpdateTimeText();
        }

        private void UpdateTimeText()
        {
            txt_time.text = msTimeString(currentTime) + " / " + TimeString(AudioManager._instance.GetEventLength());
        }
        private string msTimeString(int ms)
        {
            int totalSeconds = ms / 1000;

            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;

            string remainingMilliseconds = (ms % 1000).ToString("D3").Substring(0, 2);

            return string.Format("{0:00}:{1:00}:{2}", minutes, seconds, remainingMilliseconds);
        }
        private string TimeString(int ms)
        {
            int totalSeconds = ms / 1000;

            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;

            return string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        private void UpdateCurrentTime()
        {
            currentTime = AudioManager._instance.GetTime();
        }

        public int GetCurrentTime() => currentTime;

        public void PlayPauseMusic()
        {
            if (isPlaying) //Pause
            {
                isPlaying = false;
                AudioManager._instance.PauseMusic();
                txt_PlayPause.text = "Play";
            }
            else //Start or Resume
            {
                if (isStarted == true) //Resume
                {
                    AudioManager._instance.Resume();
                    txt_PlayPause.text = "Pause";
                }
                else //Start
                {
                    AudioManager._instance.StartMusic();
                    txt_PlayPause.text = "Pause";
                    isStarted = true;
                }
                isPlaying = true;
            }
        }

        public void StopMusic()
        {
            AudioManager._instance.StopMusic();
            txt_PlayPause.text = "Play";
            isStarted = false;
            isPlaying = false;
            NoteManager.instance.ResetIndex();
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
