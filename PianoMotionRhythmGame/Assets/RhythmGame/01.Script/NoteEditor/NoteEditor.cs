using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RhythmGame
{
    public class NoteEditor : MonoBehaviour
    {
        public List<NoteTiming> editingNotes = new List<NoteTiming>();
        List<NoteTiming> _notes = new List<NoteTiming>();
        [SerializeField] private JSONParser parser;
        [SerializeField] private TMP_InputField input_filename;

        [SerializeField] private TMP_InputField input_targetTime;

        private Note noteToEdit;
        private NoteInfo noteinfoToEdit;
        private float targetTime;
        private int laneNum;

        private string filename;

        public void SetNoteToEdit(Note note)
        {
            this.noteToEdit = note;
            this.targetTime = note.targetTime;

            this.noteinfoToEdit = new NoteInfo();
            noteinfoToEdit.laneNum = note.GetLaneNum();

            input_targetTime.text = note.targetTime.ToString();
        }
        public void SetNoteLane0()
        {
            RemoveCurrentNote();
            noteinfoToEdit.laneNum = 0;
            SetNoteTargetTime();
            ChangeNoteToEdit();

            EditList();
        }
        public void SetNoteLane1()
        {
            RemoveCurrentNote();
            noteinfoToEdit.laneNum = 1;
            SetNoteTargetTime();
            ChangeNoteToEdit();

            EditList();
        }
        public void SetNoteLane2()
        {
            RemoveCurrentNote();
            noteinfoToEdit.laneNum = 2;
            SetNoteTargetTime();
            ChangeNoteToEdit();

            EditList();
        }
        public void SetNoteLane3()
        {
            RemoveCurrentNote();
            noteinfoToEdit.laneNum = 3;
            SetNoteTargetTime();
            ChangeNoteToEdit();

            EditList();
        }
        public void SetNoteLane4()
        {
            RemoveCurrentNote();
            noteinfoToEdit.laneNum = 4;
            SetNoteTargetTime();
            ChangeNoteToEdit();

            EditList();
        }
        public void SetNoteLane5()
        {
            RemoveCurrentNote();
            noteinfoToEdit.laneNum = 5;
            SetNoteTargetTime();
            ChangeNoteToEdit();

            EditList();
        }

        private void SetNoteTargetTime()
        {
            this.targetTime = float.Parse(input_targetTime.text);
        }
        public void RemoveCurrentNote()
        {
            noteToEdit.Release();
            var result = editingNotes.Find(timing => timing.targetTime == this.targetTime && timing.notes.Contains(noteinfoToEdit));
            editingNotes.Remove(result);
        }
        private void ChangeNoteToEdit()
        {
            var go = NoteManager.instance.NotePool.Get();
            go.GetComponent<RectTransform>().anchoredPosition = new Vector2(-540+215*noteinfoToEdit.laneNum+2.5f, 480);
            go.GetComponent<Note>().targetTime = targetTime;
            go.GetComponent<Note>().SetSpawnTime();
            go.GetComponent<Note>().SetLaneNum(noteinfoToEdit.laneNum);

            noteToEdit = go.GetComponent<Note>();
        }
        private void EditList()
        {
            var noteTiming = new NoteTiming();
            noteTiming.targetTime = targetTime;
            noteTiming.notes = new List<NoteInfo>{ noteinfoToEdit };
            editingNotes.Add(noteTiming);
        }

        public void SpawnEditedNote()
        {
            RemoveCurrentNote();

            SetNoteTargetTime();
            ChangeNoteToEdit();

            EditList();
        }

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
        public void LoadFromJson()
        {
            filename = input_filename.text;
            editingNotes = parser.LoadFromJSON(filename);
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
