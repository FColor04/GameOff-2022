using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CoordinateDebugger : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    void OnGUI()
    {
        var mousePosition = Mouse.current.position.ReadValue();
        GUILayout.BeginArea(new Rect(0, 0, 100, 60));
        GUILayout.Label(new GUIContent(playerCamera.ScreenToViewportPoint(mousePosition).ToString()));
        GUILayout.EndArea();        
        }
}
