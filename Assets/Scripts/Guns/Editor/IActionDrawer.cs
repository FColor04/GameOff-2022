using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class IActionDrawer
{
    private static Dictionary<List<IAction>, ReorderableGenericList<IAction>> listsDict = new();

    private static ReorderableGenericList<IAction> GetOrCreateReorderableList(List<IAction> actions)
    {
        if (listsDict.ContainsKey(actions))
            return listsDict[actions];
        var list = new ReorderableGenericList<IAction>(actions);        
        var dropdownOptions = UnityEditor.TypeCache.GetTypesDerivedFrom<IAction>().Select(type => (StringUtil.FieldNameToLabelText(type.Name), type)).ToList();
        list.addDropdownOptions = dropdownOptions;
        list.DrawElementCallback = (rect, index, isActive, isFocused) => DrawGUI(list[index], rect);
        list.GetElementHeightCallback = (index) => GetHeight(list[index]);
        listsDict[actions] = list;
        return listsDict[actions];
    }

    public static float GetHeight(IAction action)
    {
        var actionType = action.GetType();
        var fields = actionType.GetFields().Where(f => f.IsPublic);
        var y = EditorGUIUtility.singleLineHeight;
        foreach (var field in fields)
        {
            if (field.FieldType == typeof(int))
            {
                y += EditorGUIUtility.singleLineHeight;
                continue;
            }
            if (field.FieldType == typeof(float))
            {
                y += EditorGUIUtility.singleLineHeight;
                continue;
            }
            if (field.FieldType == typeof(string))
            {
                y += EditorGUIUtility.singleLineHeight;
                continue;
            }
            if (field.FieldType.IsSubclassOf(typeof(UnityEngine.Object)))
            {
                y += EditorGUIUtility.singleLineHeight;
                continue;
            }
            if (field.FieldType == typeof(List<IAction>))
            {
                var actions = (List<IAction>)field.GetValue(action) ?? new();                
                y += EditorGUIUtility.singleLineHeight * 4;
                for (int i = 0; i < actions.Count; i++)
                    y += GetOrCreateReorderableList(actions).GetElementHeightCallback.Invoke(i);
                continue;
            }
        }
        return y;
    }

    public static void DrawGUI(IAction action, Rect rect)
    {
        var actionType = action.GetType();
        var fields = actionType.GetFields().Where(f => f.IsPublic);
        rect.height = EditorGUIUtility.singleLineHeight;
        GUI.Label(rect, StringUtil.FieldNameToLabelText(action.GetType().Name));
        rect.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.indentLevel = 1;
        rect = EditorGUI.IndentedRect(rect);
        EditorGUI.indentLevel = 0;
        foreach (var field in fields)
        {
            if (field.FieldType == typeof(int))
            {
                rect.height = EditorGUIUtility.singleLineHeight;
                field.SetValue(action, EditorGUI.IntField(rect, new GUIContent(StringUtil.FieldNameToLabelText(field.Name)), (int)field.GetValue(action)));
                rect.y += EditorGUIUtility.singleLineHeight;
                continue;
            }
            if (field.FieldType == typeof(float))
            {
                rect.height = EditorGUIUtility.singleLineHeight;
                field.SetValue(action, EditorGUI.FloatField(rect, new GUIContent(StringUtil.FieldNameToLabelText(field.Name)), (float)field.GetValue(action)));
                rect.y += EditorGUIUtility.singleLineHeight;
                continue;
            }
            if (field.FieldType == typeof(string))
            {
                rect.height = EditorGUIUtility.singleLineHeight;
                field.SetValue(action, EditorGUI.TextField(rect, new GUIContent(StringUtil.FieldNameToLabelText(field.Name)), (string)field.GetValue(action)));
                rect.y += EditorGUIUtility.singleLineHeight;
                continue;
            }
            if (field.FieldType.IsSubclassOf(typeof(UnityEngine.Object)))
            {
                rect.height = EditorGUIUtility.singleLineHeight;
                field.SetValue(action, EditorGUI.ObjectField(rect, new GUIContent(StringUtil.FieldNameToLabelText(field.Name)), (UnityEngine.Object)field.GetValue(action), field.FieldType, allowSceneObjects: false));
                rect.y += EditorGUIUtility.singleLineHeight;
                continue;
            }
            if (field.FieldType == typeof(List<IAction>))
            {
                var actions = (List<IAction>)field.GetValue(action) ?? new();
                var list = GetOrCreateReorderableList(actions);
                list.headerOverride = $"Actions ({actions.Count})";
                list.DrawRect(rect);
                field.SetValue(action, list.Elements);
                rect.y += EditorGUIUtility.singleLineHeight * 4;
                for (int i = 0; i < actions.Count; i++)
                    rect.y += list.GetElementHeightCallback.Invoke(i);
                continue;
            }
        }
    }


}
