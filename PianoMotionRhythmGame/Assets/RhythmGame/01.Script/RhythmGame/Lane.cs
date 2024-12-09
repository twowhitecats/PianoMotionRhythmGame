using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RhythmGame
{
    public class Lane : MonoBehaviour
    {
        private Transform spawnPoint;
        private Transform hitPoint;

        private List<Note> notesInLane = new List<Note>();

        [SerializeField] private int laneNum;

        [SerializeField] private BoxCollider2D perfectRange;
        [SerializeField] private BoxCollider2D greatRange;
        [SerializeField] private BoxCollider2D goodRange;
        [SerializeField] private BoxCollider2D badRange;

        private void Awake()
        {
            Setup();
        }

        private void Setup()
        {
            foreach (Transform t in transform)
            {
                if (t.name.Contains("Hit"))
                {
                    this.hitPoint = t;
                }
                else if (t.name.Contains("Spawn"))
                {
                    this.spawnPoint = t;
                }
            }
        }
        private void Update()
        {
            if(NoteManager.instance.currentMode == Mode.Game)
            {
                for(int i = 0; i < notesInLane.Count; i++)
                {
                    if (notesInLane[i].CheckEnd())
                    {
                        notesInLane[i].Miss();
                        notesInLane.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        public void SpawnNote(float targetTime, KeyCode code)
        {
            var go = NoteManager.instance.NotePool.Get();

            go.transform.position = this.spawnPoint.position;
            go.GetComponent<Note>().targetTime = targetTime;
            go.GetComponent<Note>().keyToPress = code;
            go.GetComponent<Note>().SetSpawnTime();
            go.GetComponent<Note>().SetLaneNum(laneNum);

            notesInLane.Add(go.GetComponent<Note>());
        }

        public void Hit()
        {
            if(NoteManager.instance.currentMode == Mode.Editing)
            {
                NoteInfo _info = new NoteInfo();
                _info.laneNum = laneNum;
                _info.button = KeyCode.None;
                NoteManager.instance.AddNote(_info, NoteManager.instance.currentTime);
            }
            else
            {
                if(notesInLane.Count != 0)
                {
                    //Judge Timings
                    float distance = notesInLane[0].GetComponent<RectTransform>().anchoredPosition.y - hitPoint.GetComponent<RectTransform>().anchoredPosition.y;
                    Debug.Log(distance);
                    if(InRange(distance, perfectRange))
                    {
                        Debug.Log("Perfect");
                        ScoreManager._instance.NoteHit(ScoreManager.Judgement.Perfect);
                    }
                    else if(InRange(distance, greatRange))
                    {
                        Debug.Log("Great");
                        ScoreManager._instance.NoteHit(ScoreManager.Judgement.Great);
                    }
                    else if(InRange(distance, goodRange))
                    {
                        Debug.Log("Good");
                        ScoreManager._instance.NoteHit(ScoreManager.Judgement.Good);
                    }
                    else if(InRange(distance, badRange))
                    {
                        Debug.Log("Bad");
                        ScoreManager._instance.NoteHit(ScoreManager.Judgement.Bad);
                    }
                    else
                    {
                        return;
                    }
                    notesInLane[0].Pool.Release(notesInLane[0].gameObject);
                    notesInLane.RemoveAt(0);
                }
            }
            hitPoint.GetComponent<Image>().color = Color.red;
            Invoke("ReturnColor", 0.1f);
        }

        private void ReturnColor()
        {
            hitPoint.GetComponent<Image>().color = Color.white;
        }

        private bool InRange(float distance, BoxCollider2D range)
        {
            float enter = range.size.y / 2 + range.offset.y;
            float exit = -(range.size.y / 2 - range.offset.y);
            //Debug.Log(enter);
            //Debug.Log(exit);
            if(distance <= enter && distance >= exit)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
