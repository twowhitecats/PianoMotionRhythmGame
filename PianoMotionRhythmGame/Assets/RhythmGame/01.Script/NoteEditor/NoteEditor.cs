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

            // ��� ���
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
            // targetTime �������� ����
            List<List<NoteTiming>> groupedList = new List<List<NoteTiming>>();

            // ������ ��Ʈ�� ���� (targetTime �������)
            var sortedNotes = new List<NoteTiming>(editingNotes);
            sortedNotes.Sort((a, b) => a.targetTime.CompareTo(b.targetTime));

            // �� �׷��� ã�� ���� ����
            foreach (var noteTiming in sortedNotes)
            {
                bool addedToGroup = false;

                // ���� �׷�� �߿� �߰��� �׷��� �ִ��� Ȯ��
                foreach (var group in groupedList)
                {
                    // ù ��° ��Ʈ�� targetTime���� ���̰� threshold �̸��̸� ���� �׷�
                    if (Mathf.Abs(group[0].targetTime - noteTiming.targetTime) < threshold)
                    {
                        group.Add(noteTiming);
                        addedToGroup = true;
                        break;
                    }
                }

                // �ش� �׷쿡 �߰����� ������ ���ο� �׷� ����
                if (!addedToGroup)
                {
                    groupedList.Add(new List<NoteTiming> { noteTiming });
                }
            }

            // �׷��� �ϳ��� List<NoteTiming>���� ��ġ��
            List<NoteTiming> mergedNotes = new List<NoteTiming>();
            foreach (var group in groupedList)
            {
                // �׷� �� ��� NoteTiming�� �ϳ��� ��ħ (ù ��° NoteTiming�� �������� targetTime ���)
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
