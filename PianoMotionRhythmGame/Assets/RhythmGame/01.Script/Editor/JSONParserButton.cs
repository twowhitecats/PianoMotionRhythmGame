using UnityEngine;
using UnityEditor;

namespace RhythmGame
{
    [CustomEditor(typeof(JSONParser))]
    public class JSONParserButton : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            JSONParser parser = (JSONParser)target;
            if (GUILayout.Button("Generate JSON"))
            {
                parser.GenerateJSON("Test.json",parser.chart);
            }
            if (GUILayout.Button("Load JSON"))
            {
                parser.LoadJSON(parser.fileName);
            }
        }
    }
}
