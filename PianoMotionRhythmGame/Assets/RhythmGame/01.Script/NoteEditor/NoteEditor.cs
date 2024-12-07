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

            // ��� ���
            foreach (var noteTiming in _notes)
            {
                Debug.Log($"Note ID: {noteTiming.notes[0].laneNum}, TargetTime: {noteTiming.targetTime}");
            }

            filename = input_filename.text;
            parser.GenerateJSON(filename, _notes);
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
