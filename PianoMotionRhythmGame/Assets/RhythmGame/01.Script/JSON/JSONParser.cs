using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RhythmGame
{
    public class JSONParser : MonoBehaviour
    {
        public string fileName = "TestJson";
        public List<NoteTiming> chart;

        public void GenerateJSON(string fileName, List<NoteTiming> chart)
        {
            Wrapper<NoteTiming> data = new Wrapper<NoteTiming>();
            data.data = chart;

            string jsonData = JsonUtility.ToJson(data, true);
            string path = Path.Combine(Application.streamingAssetsPath, "JSON/" + fileName + ".json");

            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            Debug.Log(jsonData);

            File.WriteAllText(path, jsonData);
        }
        public void LoadJSON(string filename)
        {
            chart.Clear();
            chart = LoadFromJSON(filename);
        }
        public List<NoteTiming> LoadFromJSON(string filename)
        {
            var _result = new List<NoteTiming>();
            string path = Path.Combine(Application.streamingAssetsPath, "JSON/" + filename + ".json");
            string jsonData = File.ReadAllText(path);

            Debug.Log(jsonData);

            Wrapper<NoteTiming> data = new Wrapper<NoteTiming>();
            data = JsonUtility.FromJson<Wrapper<NoteTiming>>(jsonData);

            //if(data != null)
            {
                for (int i = 0; i < data.data.Count; i++)
                {
                    _result.Add(data.data[i]);
                }
            }

            return _result;
        }

        [Serializable]
        private class Wrapper<T>
        {
            public List<T> data;
        }
    }

    [System.Serializable]
    public struct NoteTiming
    {
        public float targetTime;
        public List<NoteInfo> notes;
    }

    [System.Serializable]
    public struct NoteInfo
    {
        public int laneNum;
        public KeyCode button;
    }
}
