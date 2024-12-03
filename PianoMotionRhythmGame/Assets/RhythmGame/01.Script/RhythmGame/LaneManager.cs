using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmGame
{
    public class LaneManager : MonoBehaviour
    {
        [SerializeField] private GameObject prefab_Lane;
        private List<Lane> lanes = new List<Lane>();

        private float startOffset = -540;
        private float padding = 215;
        private int laneNum = 6;

        private void Awake()
        {
            //SetupLane();
            InitializeLanes();
        }

        private void Update()
        {
            if(Input.GetKey(KeyCode.S))
            {
                Hit(0);
            }
            if(Input.GetKey(KeyCode.D))
            {
                Hit(1);
            }
            if(Input.GetKey(KeyCode.F))
            {
                Hit(2);
            }
            if(Input.GetKey(KeyCode.J))
            {
                Hit(3);
            }
            if(Input.GetKey(KeyCode.K))
            {
                Hit(4);
            }
            if(Input.GetKey(KeyCode.L))
            {
                Hit(5);
            }
        }

        private void InitializeLanes()
        {
            foreach(Transform t in transform)
            {
                lanes.Add(t.GetComponent<Lane>());
            }
        }

        private void SetupLane()
        {
            for (int i = 0; i < laneNum; i++)
            {
                var _go = Instantiate(prefab_Lane, transform);
                _go.name = "Lane" + i;
                _go.GetComponent<RectTransform>().anchoredPosition = new Vector2(startOffset + padding * i, 0);
                lanes.Add(_go.GetComponent<Lane>());
            }
        }

        public void Hit(int laneNum)
        {
            lanes[laneNum].Hit();
        }

        public void SpawnNote(int laneNum)
        {
            lanes[laneNum].SpawnNote(5, KeyCode.Space);
        }
        public void SpawnNote(int laneNum, float targetTime)
        {
            lanes[laneNum].SpawnNote(targetTime, KeyCode.Space);
        }
        public void SpawnNote(NoteInfo info, float targetTime)
        {
            lanes[info.laneNum].SpawnNote(targetTime, info.button);
        }
    }
}
