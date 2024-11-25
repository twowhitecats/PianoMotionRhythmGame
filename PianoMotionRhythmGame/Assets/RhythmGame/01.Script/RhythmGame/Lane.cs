using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmGame
{
    public class Lane : MonoBehaviour
    {
        private Transform spawnPoint;
        private Transform hitPoint;

        private NoteManager noteManager;

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
        public void SetNoteManager(NoteManager noteManager)
        {
            this.noteManager = noteManager;
        }
        public void SpawnNote()
        {
            var go = noteManager.NotePool.Get();
            go.transform.position = this.spawnPoint.position;
            go.GetComponent<Note>().SetSpawnTime();
        }
    }
}
