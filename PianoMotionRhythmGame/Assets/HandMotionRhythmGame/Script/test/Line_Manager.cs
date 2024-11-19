using Oculus.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line_Manager : MonoBehaviour
{
    public GameObject spawn_line;
    public GameObject hit_line;
    public GameObject judgment_line;

    private List<GameObject> note_pool = new List<GameObject>();
    public int poolsize;
    public GameObject note_prefab;


    void Set_Lines()
    {
        foreach (Transform child in gameObject.transform)
        {
            if (child.name == "Hit_Line")
            {
                hit_line = child.gameObject;
            }
            if (child.name == "Spawn_Line")
            {
                spawn_line = child.gameObject;
            }
        }
        foreach (Transform child in gameObject.transform.parent)
        {
            if (child.name == "judgment_line")
            {
                judgment_line = child.gameObject;
            }
        }
    }

    void Init_Note_Pool()
    {
        for (int i = 0; i < poolsize; i++)
        {
            GameObject obj = Instantiate(note_prefab);
            obj.SetActive(false);
            obj.GetComponent<Note>().line_manager = this;
            note_pool.Add(obj);
        }
    }

    public void Creat_Note()
    {
        foreach (GameObject obj in note_pool)
        {
            if (obj.activeSelf==false)
            {
                obj.GetComponent<Note>().Init();
                break;
            }
        }
    }

    public GameObject Get_Note()
    {
        GameObject return_note=new GameObject();
        float min_y = 1000.0f;

        foreach(GameObject obj in note_pool)
        {
            if (obj.activeSelf)
            {
                if (obj.transform.position.y< min_y)
                {
                    min_y = obj.transform.position.y;
                    return_note=obj;
                }
            }
        }
        return return_note;
    }

    // Start is called before the first frame update
    void Start()
    {
        Set_Lines();
        Init_Note_Pool();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
