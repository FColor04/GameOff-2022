using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GunData))]
public class GunDataEditor : Editor
{
    private ReorderableGenericList<IAction> ActionListGUI;

    void OnEnable()
    {
        ActionListGUI = new((target as GunData).onPrimaryFireActions);
        ActionListGUI.headerOverride = "OnPrimaryFire";
        var dropdownOptions = UnityEditor.TypeCache.GetTypesDerivedFrom<IAction>().Select(type => (StringUtil.FieldNameToLabelText(type.Name), type)).ToList();
        ActionListGUI.addDropdownOptions = dropdownOptions;
        ActionListGUI.DrawElementCallback = (rect, index, isActive, isFocused) => DrawAction(rect, ActionListGUI[index]);
        ActionListGUI.GetElementHeightCallback = (index) => GetActionHeight(ActionListGUI[index]);
    }

    private void DrawAction(Rect rect, IAction action)
    {
        IActionDrawer.DrawGUI(action, rect);
    }
    private float GetActionHeight(IAction action) => IActionDrawer.GetHeight(action);

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();
        EditorGUI.BeginChangeCheck();
        ActionListGUI.DrawLayout();
        if (EditorGUI.EndChangeCheck())
            EditorUtility.SetDirty(target);
    }
}
