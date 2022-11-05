using Rooms;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Exit))]
public class ExitEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("GenerateAnotherRoom"))
        {
            ((Exit) target).controller.PropagateExit((Exit) target, ((Exit) target).controller.pool.Random(), out _);
        }

        GUI.enabled = false;
        GUILayout.Label(((Exit) target).transform.eulerAngles.y.ToString());
        GUI.enabled = true;
        base.OnInspectorGUI();
    }
}