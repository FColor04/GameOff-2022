using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GunData))]
public class GunDataEditor : Editor
{
    private ReorderableGenericList<IAction> ActionListGUI;

    void OnEnable()
    {
        ActionListGUI = new((target as GunData).actions);
        ActionListGUI.headerOverride = "OnFire";
        ActionListGUI.addDropdownOptions = new() {
            ("Spawn Projectile", typeof(SpawnProjectile)),
            ("Raycast Shoot", typeof(RaycastShoot))
        };
        
        ActionListGUI.DrawElementCallback = (rect, index, isActive, isFocused) => DrawAction(rect, ActionListGUI[index]);
        ActionListGUI.GetElementHeightCallback = (index) => GetActionHeight(ActionListGUI[index]);
    }

    private void DrawAction(Rect rect, IAction action)
    {
        GUI.Label(new(rect.x, rect.y, rect.width, 20), action.GetType().Name);
    }
    private float GetActionHeight(IAction action)
    {
        return 0f;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();
        ActionListGUI.DrawLayout();
    }
}
