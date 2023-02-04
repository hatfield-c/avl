using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DebugManager))]
public class DebugButtons : Editor
{
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Assignment-1 Debugging", EditorStyles.boldLabel);

        if (GUILayout.Button("Activate Boat")) {
            DebugManager debug = (DebugManager)this.target;
            debug.ActivateBoat();
        }
    }
}
