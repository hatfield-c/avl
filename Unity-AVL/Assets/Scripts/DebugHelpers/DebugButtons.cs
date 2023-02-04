using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DebugManager))]
public class DebugButtons : Editor
{
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        DebugManager debug = (DebugManager)this.target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Assignment-1 Debugging", EditorStyles.boldLabel);

        if (GUILayout.Button("Activate Boat")) {
            debug.ActivateBoat();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Assignment-2 Debugging", EditorStyles.boldLabel);

        if (GUILayout.Button("Randomize Empty Parking Spot (All Positions)")) {
            debug.RandomEmptySpace();
        }

        if (GUILayout.Button("Randomize Empty Parking Spot (First Row Only)")) {
            debug.RandomFirstRow();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Assignment-3 Debugging", EditorStyles.boldLabel);

        if (GUILayout.Button("Randomize Empty Parking Spot (All Positions)")) {
            debug.RandomEmptySpace();
        }

        if (GUILayout.Button("Randomize Empty Parking Spot (First Row Only)")) {
            debug.RandomFirstRow();
        }
    }
}
