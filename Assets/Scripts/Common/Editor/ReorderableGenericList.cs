using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditorInternal;
using UnityEditor;
using System;

public class ReorderableGenericList<T>
{
    private ReorderableList list;    
    public Action<Rect, int, bool, bool> DrawElementCallback;
    public Func<int, float> GetElementHeightCallback;
    public string headerOverride;    
    public List<T> Elements
    {
        get => (List<T>)list.list;
        set => list.list = value;
    }

    public List<(string name, Type type)> addDropdownOptions
    {
        set
        {
            list.onAddDropdownCallback = null;
            if (value != null)
                if (value.Count > 0)
                    list.onAddDropdownCallback = (Rect buttonRect, ReorderableList l) =>
                    {
                        var menu = new GenericMenu();
                        foreach (var item in value)
                            menu.AddItem(new GUIContent(item.name), false, () => l.list.Add(Activator.CreateInstance(item.type)));
                        menu.ShowAsContext();
                    };
        }
    }

    public T this[int index] => (T)list.list[index];

    public ReorderableGenericList() : this(new List<T>()) { }
    public ReorderableGenericList(IList<T> elements)
    {
        list = new ReorderableList((IList)elements, typeof(T));        
        list.drawElementCallback = (rect, index, isActive, isFocused) => DrawElementCallback?.Invoke(rect, index, isActive, isFocused);
        list.elementHeightCallback = (index) => GetElementHeightCallback?.Invoke(index) ?? 0f;
        list.drawHeaderCallback = DrawHeader;
    }

    private void DrawHeader(Rect rect) { GUI.Label(rect, headerOverride ?? $"{typeof(T).Name}"); }

    public IList<T> DrawLayout() { list.DoLayoutList(); return (IList<T>)list.list; }
    public IList<T> DrawRect(Rect rect) { list.DoList(rect); return (IList<T>)list.list; }

    public static implicit operator ReorderableGenericList<T>(List<T> list)
    {
        return new ReorderableGenericList<T>(list);
    }
    public static implicit operator List<T>(ReorderableGenericList<T> list)
    {
        return (List<T>)list.list.list;
    }
}
