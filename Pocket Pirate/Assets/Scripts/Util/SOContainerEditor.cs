using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(SOPersistance))]
public class SOContainerEditor : Editor
{
    SerializedProperty SOContainers;

    ReorderableList list;

    private void OnEnable()
    {
        SOContainers = serializedObject.FindProperty("SOContainers");

        list = new ReorderableList(serializedObject, SOContainers, true, true, true, true);

        list.drawElementCallback = DrawListItems;
        list.drawHeaderCallback = DrawHeader;
    }

    void DrawListItems(Rect rect, int index, bool isActive, bool isFocused)
    {
        SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);

        EditorGUI.LabelField(new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight), "ID");
    }

    void DrawHeader(Rect rect)
    {
        string name = "Scriptable Object Containers";
        EditorGUI.LabelField(rect, name);
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        list.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }
}
