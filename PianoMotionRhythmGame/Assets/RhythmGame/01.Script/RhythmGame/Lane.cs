using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmGame
{
    public class Lane : MonoBehaviour
    {
        private Transform spawnPoint;
        private Transform hitPoint;

        private List<Note> notesInLane = new List<Note>();

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

        public void SpawnNote(float targetTime, KeyCode code)
        {
            var go = NoteManager.instance.NotePool.Get();
            go.transform.position = this.spawnPoint.position;
            go.GetComponent<Note>().targetTime = targetTime;
            go.GetComponent<Note>().keyToPress = code;
            go.GetComponent<Note>().SetSpawnTime();

            notesInLane.Add(go.GetComponent<Note>());
        }

        public void Hit()
        {
            if(notesInLane.Count != 0)
            {
                //Judge Timings
                float distance = notesInLane[0].GetComponent<RectTransform>().anchoredPosition.y - hitPoint.GetComponent<RectTransform>().anchoredPosition.y;
                Debug.Log(distance);
                if(InRange(distance, perfectRange))
                {
                    Debug.Log("Perfect");
                }
                else if(InRange(distance, greatRange))
                {
                    Debug.Log("Great");
                }
                else if(InRange(distance, goodRange))
                {
                    Debug.Log("Good");
                }
                else if(InRange(distance, badRange))
                {
                    Debug.Log("Bad");
                }
                else
                {
                    return;
                }
                notesInLane[0].Pool.Release(notesInLane[0].gameObject);
                notesInLane.RemoveAt(0);
            }
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
