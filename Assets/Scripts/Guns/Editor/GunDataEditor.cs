using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GunData))]
public class GunDataEditor : Editor
{
    private ReorderableGenericList<IAction> PrimaryFireActionListGUI;
    private ReorderableGenericList<IAction> SecondaryFireActionListGUI;

    void OnEnable()
    {
        PrimaryFireActionListGUI = SetupReorderableList((target as GunData).onPrimaryFireActions, "OnPrimaryFire");
        SecondaryFireActionListGUI = SetupReorderableList((target as GunData).onSecondaryFireActions, "OnSecondaryFire");
    }

    private ReorderableGenericList<IAction> SetupReorderableList(List<IAction> target, string header)
    {
        var dropdownOptions = UnityEditor.TypeCache.GetTypesDerivedFrom<IAction>().Select(type => (StringUtil.FieldNameToLabelText(type.Name), type)).ToList();
        var list = new ReorderableGenericList<IAction>(target);
        list.headerOverride = header;
        list.addDropdownOptions = dropdownOptions;
        list.DrawElementCallback = (rect, index, isActive, isFocused) => DrawAction(rect, list[index]);
        list.GetElementHeightCallback = (index) => GetActionHeight(list[index]);
        return list;
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
        PrimaryFireActionListGUI.DrawLayout();
        SecondaryFireActionListGUI.DrawLayout();
        if (EditorGUI.EndChangeCheck())
            EditorUtility.SetDirty(target);
    }
}
