using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(JSONParser))]
public class JSON2Note : MonoBehaviour
{
    JSONParser parser;

    public float currentTime;
    public bool running;

    private int timingIndex;

    public List<GameObject> lanes;
    void Start()
    {
        currentTime = 0;
        timingIndex = 0;
        parser = GetComponent<JSONParser>();
    }

    void Update()
    {
        if(running)
        {
            currentTime += Time.deltaTime;
            if(currentTime > parser.timings[timingIndex].time)
            {
                string _time = parser.timings[timingIndex].time.ToString();
                var _noteinfos = parser.timings[timingIndex].notes;
                string _notes = "Spawn ";
                for(int i = 0; i < _noteinfos.Count; i++)
                {
                    _notes += _noteinfos[i].button + " in " + _noteinfos[i].lane + ", ";
                    //lanes[_noteinfos[i].lane].SpawnNote(button=_noteinfos[i].button);
                }

                Debug.Log(_time + ": " + _notes);
                if (timingIndex < parser.timings.Count - 1)
                {
                    timingIndex = timingIndex + 1;
                }
            }
        }
    }
}
