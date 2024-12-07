using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

namespace RhythmGame
{
    public class NoteEditor : MonoBehaviour
    {
        public List<NoteTiming> editingNotes = new List<NoteTiming>();
        List<NoteTiming> _notes = new List<NoteTiming>();
        [SerializeField] private JSONParser parser;
        [SerializeField] private TMP_InputField input_filename;

        private string filename;

        public void SaveToJson()
        {
            _notes = MergeGroupedNotes(editingNotes, 0.05f);

            // 결과 출력
            foreach (var noteTiming in _notes)
            {
                Debug.Log($"Note ID: {noteTiming.notes[0].laneNum}, TargetTime: {noteTiming.targetTime}");
            }

            filename = input_filename.text;
            parser.GenerateJSON(filename, _notes);
        }

        List<NoteTiming> MergeGroupedNotes(List<NoteTiming> editingNotes, float threshold)
        {
            // targetTime 기준으로 묶기
            List<List<NoteTiming>> groupedList = new List<List<NoteTiming>>();

            // 편집된 노트를 정렬 (targetTime 순서대로)
            var sortedNotes = new List<NoteTiming>(editingNotes);
            sortedNotes.Sort((a, b) => a.targetTime.CompareTo(b.targetTime));

            // 각 그룹을 찾기 위한 변수
            foreach (var noteTiming in sortedNotes)
            {
                bool addedToGroup = false;

                // 기존 그룹들 중에 추가할 그룹이 있는지 확인
                foreach (var group in groupedList)
                {
                    // 첫 번째 노트의 targetTime과의 차이가 threshold 미만이면 같은 그룹
                    if (Mathf.Abs(group[0].targetTime - noteTiming.targetTime) < threshold)
                    {
                        group.Add(noteTiming);
                        addedToGroup = true;
                        break;
                    }
                }

                // 해당 그룹에 추가되지 않으면 새로운 그룹 생성
                if (!addedToGroup)
                {
                    groupedList.Add(new List<NoteTiming> { noteTiming });
                }
            }

            // 그룹을 하나의 List<NoteTiming>으로 합치기
            List<NoteTiming> mergedNotes = new List<NoteTiming>();
            foreach (var group in groupedList)
            {
                // 그룹 내 모든 NoteTiming을 하나로 합침 (첫 번째 NoteTiming을 기준으로 targetTime 사용)
                NoteTiming mergedNoteTiming = new NoteTiming
                {
                    targetTime = group[0].targetTime,
                    notes = new List<NoteInfo>()
                };

                foreach (var noteTiming in group)
                {
                    mergedNoteTiming.notes.AddRange(noteTiming.notes);
                }

                mergedNotes.Add(mergedNoteTiming);
            }

            return mergedNotes;
        }
    }
}
