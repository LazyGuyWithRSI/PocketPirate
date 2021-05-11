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
    SerializedProperty EnemyPrefabContainers;

    ReorderableList listSOContainers;
    ReorderableList listEnemyPrefabContainers;

    private void OnEnable()
    {
        SOContainers = serializedObject.FindProperty("SOContainers");

        listSOContainers = new ReorderableList(serializedObject, SOContainers, true, true, true, true);

        listSOContainers.elementHeight = EditorGUIUtility.singleLineHeight * 2 + 12;
        listSOContainers.drawElementCallback = DrawListItems;
        listSOContainers.drawHeaderCallback = DrawHeader;

        EnemyPrefabContainers = serializedObject.FindProperty("EnemyPrefabContainers");

        listEnemyPrefabContainers = new ReorderableList(serializedObject, EnemyPrefabContainers, true, true, true, true);

        listEnemyPrefabContainers.elementHeight = EditorGUIUtility.singleLineHeight * 2 + 12;
        listEnemyPrefabContainers.drawElementCallback = DrawListItemsEnemy;
        listEnemyPrefabContainers.drawHeaderCallback = DrawHeaderEnemy;
    }

    void DrawListItems(Rect rect, int index, bool isActive, bool isFocused)
    {
        SerializedProperty element = listSOContainers.serializedProperty.GetArrayElementAtIndex(index);

        EditorGUI.LabelField(new Rect(rect.x, rect.y, 50, EditorGUIUtility.singleLineHeight), "ID");
        EditorGUI.PropertyField(new Rect(rect.x + 120, rect.y, 200, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("ID"), GUIContent.none);
        EditorGUI.LabelField(new Rect(rect.x, rect.y + 2 + EditorGUIUtility.singleLineHeight, 100, EditorGUIUtility.singleLineHeight), "Scriptable Object");
        EditorGUI.PropertyField(new Rect(rect.x + 120, rect.y + 2 + EditorGUIUtility.singleLineHeight, 200, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("Object"), GUIContent.none);
    }

    void DrawHeader(Rect rect)
    {
        string name = "Scriptable Object Containers";
        EditorGUI.LabelField(rect, name);
    }

    void DrawHeaderEnemy(Rect rect)
    {
        string name = "Enemy Prefab Containers";
        EditorGUI.LabelField(rect, name);
    }

    void DrawListItemsEnemy(Rect rect, int index, bool isActive, bool isFocused)
    {
        SerializedProperty element = listEnemyPrefabContainers.serializedProperty.GetArrayElementAtIndex(index);

        EditorGUI.LabelField(new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight), "Enemy ID");
        EditorGUI.PropertyField(new Rect(rect.x + 120, rect.y, 200, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("ID"), GUIContent.none);
        EditorGUI.LabelField(new Rect(rect.x, rect.y + 2 + EditorGUIUtility.singleLineHeight, 100, EditorGUIUtility.singleLineHeight), "Prebab");
        EditorGUI.PropertyField(new Rect(rect.x + 120, rect.y + 2 + EditorGUIUtility.singleLineHeight, 200, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("Object"), GUIContent.none);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        listSOContainers.DoLayoutList();
        listEnemyPrefabContainers.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }
}
