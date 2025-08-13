using UnityEditor;
using UnityEngine;

namespace _Project.Scripts.Tools
{
#if UNITY_EDITOR
    [CustomEditor(typeof(ScriptableObjectToJsonParser))]
    public class JsonParserEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var parser = target as ScriptableObjectToJsonParser;

            if (GUILayout.Button("Parse JSON")) parser.Parse();
        }
    }
#endif
}
