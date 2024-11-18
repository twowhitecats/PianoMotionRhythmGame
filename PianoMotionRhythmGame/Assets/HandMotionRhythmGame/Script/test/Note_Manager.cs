using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Note_Manager : MonoBehaviour
{
    public GameObject Notes;
    public List<GameObject> spawn_lines = new List<GameObject>();

    void Set_Lines()
    {
        foreach (Transform lane in Notes.transform)
        {
            foreach (Transform child in lane)
            {
                if (child.name == "Spawn_Line")
                {
                    spawn_lines.Add(child.gameObject);
                }
            }
        }
    }

    void Create_Note(GameObject spawn_line)
    {
        GameObject note_lane = spawn_line.transform.parent.gameObject;
        if (note_lane != null)
        {
            note_lane.GetComponent<Line_Manager>().Creat_Note();
        }
        else
        {
            Debug.Log("no lane");
        }
    }

    void Random_Create()
    {
        int ind = Random.Range(0, spawn_lines.Count);
        Create_Note(spawn_lines[ind]);
    }

    // Start is called before the first frame update
    void Start()
    {
        Set_Lines();
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
        {
            Random_Create();
        }
    }
}
