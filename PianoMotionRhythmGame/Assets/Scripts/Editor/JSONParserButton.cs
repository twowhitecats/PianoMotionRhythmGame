using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(JSONParser))]
public class JSONParserButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        JSONParser parser = (JSONParser)target;
        if(GUILayout.Button("Generate JSON"))
        {
            parser.GenerateJSON();
        }
        if(GUILayout.Button("Load JSON"))
        {
            parser.LoadJSON();
        }
    }
}
