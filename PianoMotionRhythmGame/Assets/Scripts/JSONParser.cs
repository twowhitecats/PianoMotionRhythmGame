using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JSONParser : MonoBehaviour
{
    public string fileName = "TestJson.json";
    public List<NoteTiming> timings;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void GenerateJSON()
    {
        Wrapper<NoteTiming> data = new Wrapper<NoteTiming>();
        data.data = timings;

        string jsonData = JsonUtility.ToJson(data, true);
        string path = Path.Combine(Application.streamingAssetsPath, "JSON/" + fileName);

        Debug.Log(jsonData);

        File.WriteAllText(path, jsonData);
    }
    public void LoadJSON()
    {
        timings.Clear();
        string path = Path.Combine(Application.streamingAssetsPath, "JSON/" + fileName);
        string jsonData = File.ReadAllText(path);

        Debug.Log(jsonData);

        Wrapper<NoteTiming> data = new Wrapper<NoteTiming>();
        data = JsonUtility.FromJson<Wrapper<NoteTiming>>(jsonData);

        if(data != null)
        {
            for(int i = 0; i < data.data.Count; i++)
            {
                timings.Add(data.data[i]);
            }
        }
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
    public float time;
    public List<NoteInfo> notes;
}

[System.Serializable]
public struct NoteInfo
{
    public int lane;
    public string button;
}
