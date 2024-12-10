using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
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
        [SerializeField] private TMP_InputField timeInputField;

        [SerializeField] private BaselineManager baselineManager;

        private bool isPlaying;
        private bool isStarted;
        private int currentTime = 0;
        private void Start()
        {
            timeInputField.onValueChanged.AddListener(ValidateTimeInput);
            timeInputField.onEndEdit.AddListener(FormatTimeInput);
            txt_time.text = "/ " + TimeString(AudioManager._instance.GetEventLength());
        }

        private void Update()
        {
            UpdateCurrentTime();
            UpdateTimeText();
        }

        private void UpdateTimeText()
        {
            if(isPlaying)
            {
                timeInputField.text = msTimeString(currentTime);
            }
        }
        private string msTimeString(int ms)
        {
            int totalSeconds = ms / 1000;

            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;

            string remainingMilliseconds = (ms % 1000).ToString("D3").Substring(0, 3);

            return string.Format("{0:00}:{1:00}:{2}", minutes, seconds, remainingMilliseconds);
        }
        private string TimeString(int ms)
        {
            int totalSeconds = ms / 1000;

            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;

            return string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        void ValidateTimeInput(string input)
        {
            // ���Խ��� ����� �Էµ� ���� �ùٸ� �������� Ȯ��
            string validPattern = @"^\d{0,2}(:\d{0,2})?(:\d{0,3})?$";
            if (!Regex.IsMatch(input, validPattern))
            {
                // �߸��� �Է��� ������ �ؽ�Ʈ�� �ʱ�ȭ
                timeInputField.text = "";
            }
        }

        // �Է� �Ϸ� �� ���� ����ȭ
        void FormatTimeInput(string input)
        {
            if (string.IsNullOrEmpty(input)) return;

            // �Է� ������ mm, ss, fff �и�
            string[] parts = input.Split(':');
            int minutes = 0;
            int seconds = 0;
            int milliseconds = 0;

            if (parts.Length > 0 && int.TryParse(parts[0], out int parsedMinutes))
            {
                minutes = Mathf.Clamp(parsedMinutes, 0, 99); // 99�� ����
            }

            if (parts.Length > 1 && int.TryParse(parts[1], out int parsedSeconds))
            {
                seconds = Mathf.Clamp(parsedSeconds, 0, 59); // 59�� ����
            }

            if (parts.Length > 2 && int.TryParse(parts[2], out int parsedMilliseconds))
            {
                milliseconds = Mathf.Clamp(parsedMilliseconds, 0, 999); // 999ms ����
            }

            // ���� ������
            timeInputField.text = $"{minutes:00}:{seconds:00}:{milliseconds:000}";

            int position = minutes * 60*1000 + seconds * 1000 + milliseconds;
            AudioManager._instance.SetTimelinePosition(position);
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
            timeInputField.text = msTimeString(currentTime);
            isStarted = false;
            isPlaying = false;
            NoteManager.instance.ResetIndex();
            baselineManager.ResetLines();
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
